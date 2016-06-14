using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WahleCtrl : MonoBehaviour {

    public float maxSpeed = 10f; // 최대 속도
    public float accel = 5f; // 가속도

    protected float initSpeed = 0f; // 초기 속도
    protected float distance = 0f; // 거리 차
    private bool isSearch = false; // 탐색 여부

    protected Transform targetPoint; // 대기 상태시 추격할 포인트 위치
    protected Vector3 relativePos; // 상대적 위치값
    protected Quaternion lookRot; // 봐라볼 방향

    public Transform playerTr;
    private Transform camTr;
    protected Animator anim;

    public IEnumerator curState;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        camTr = GameObject.FindGameObjectWithTag("MainCamera").transform;
        fsmBase = GetComponent<FSMBase>();
        
        targetPoint = new GameObject().transform;
        targetPoint.name = "TargetPoint";

        curState = Idle();
        StartCoroutine(CoroutineUpdate());
        StartCoroutine(SearchEnemy());
    }


    public IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            if (!curState.Equals(null) && curState.MoveNext())
            {
                yield return curState.Current;
            }
            else
                yield return null;
        }
    }

    //대기
    public IEnumerator Idle()
    {
        anim.SetBool("Move", true);
        SetRandomPos();

        while (true)
        {
            ShotRay();
            // 플레이어가 움직이거나 고래가 화면 밖으로 나갈 시 이동으로 전환
            if (PlayerCtrl.inputAxis >= 1f || PlayerCtrl.inputAxis <= -1f)
            {
                if (CheckOutCamera())
                    curState = Move();
            }
            // 타겟 위치를 임의의 위치로 배치
            if (distance <= 4f)
            {
                targetPoint.position = SetRandomPos();
            }

            relativePos = targetPoint.position - transform.position;
            distance = relativePos.sqrMagnitude;
            lookRot = Quaternion.LookRotation(relativePos);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 0.8f * Time.deltaTime);
            transform.Translate(0f, 0f, 2f * Time.deltaTime);

            yield return null;
        }
    }

    //이동
    IEnumerator Move()
    {
        initSpeed = 0f;
        float changeTime = 0f; // 대기로 전환 전 대기 시간
        float focusDir = 1f; // 봐라보고 있는 방향 오른쪽 : 1, 왼쪽 : -1

        while (true)
        {
            focusDir = Mathf.Sign(PlayerCtrl.inputAxis);
            relativePos = playerTr.position - transform.position;
            lookRot = Quaternion.LookRotation(relativePos);
            distance = relativePos.sqrMagnitude;

            // 플레이어가 멈췄을 시
            if (PlayerCtrl.inputAxis == 0f && distance <= 3f)
            {
                //initSpeed = 3f;

                initSpeed = DecreaseSpeed(initSpeed, 2f, 20f);
                // 플레이어 주위를 선회
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 1.5f * Time.deltaTime);
                transform.Translate(Vector3.forward * initSpeed * Time.deltaTime);

                // 4초이상 움직임이 없을 시 대기 상태로 전환
                changeTime += Time.deltaTime;
                if (changeTime >= 4f)
                {
                    initSpeed = 0f;
                    curState = Idle();
                }
            }
            // 플레이어가 이동중일 시
            else {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                       new Quaternion(lookRot.x, lookRot.y, lookRot.z, lookRot.w), 4f * Time.deltaTime);

                // 이동속도 증가
                initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);
                transform.position = Vector3.Lerp(transform.position, playerTr.position - (playerTr.forward),
                        initSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    IEnumerator Attack(GameObject targetObj)
    {
        isSearch = true;
        initSpeed = 3f;

        // 플레이어 정면에 위치
        Vector3 fromPointVec = playerTr.position + ((playerTr.forward * 2f ) + (playerTr.up * 0.5f));
        while (true)
        {
            Debug.Log(targetObj.GetComponent<FSMBase>()._curState);
            
            // 화면 밖으로 나가거나 타겟이 사라졌을 시 이동으로 전환
            if (CheckOutCamera())
            {
                isSearch = false;
                curState = Move();
            }

            Vector3 enemyRelativePos = targetObj.transform.position - transform.position;
            lookRot = Quaternion.LookRotation(enemyRelativePos);

            initSpeed += Time.deltaTime * 5f;
            // 각도 제한
            if (transform.localRotation.x <= 0.24f)
                lookRot.x = 0f;

            // 타겟을 봐라보도록 회전
            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
            // 지정 된 위치로 포물선을 그리며 이동
            Vector3 center = (fromPointVec + transform.position) * 0.55f;
            center -= new Vector3(0, 1, 1);
            transform.position = Vector3.Slerp(transform.position - center, fromPointVec - center, initSpeed * Time.deltaTime);
            transform.position += center;

            yield return null;
        }
    }
    // NPC 주위 선회
    IEnumerator TurningNpc(Transform npcPos)
    {
        isSearch = true;
        Vector3 lookVec = npcPos.position;
        lookVec.y += 1.5f;
        initSpeed = maxSpeed;
        while (true)
        {
            relativePos = lookVec - transform.position;
            lookRot = Quaternion.LookRotation(relativePos);
            
            float npcDis = relativePos.sqrMagnitude;

            if (npcDis <= 5f)
                initSpeed = 2f;
            
            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, Time.deltaTime);
            transform.Translate(0f, 0f, initSpeed * Time.deltaTime);

            yield return null;
        }
    }

    // 주변 몬스터 탐색
    IEnumerator SearchEnemy()
    {
        float monsterDis = 0f;
        while (true)
        {
            Collider[] searchColl = Physics.OverlapSphere(playerTr.position, 20f);
            if (!isSearch)
            {
                for (int i = 0; i < searchColl.Length; i++)
                {
                    // 몬스터 탐색
                    if (searchColl[i].CompareTag("MONSTER"))
                    {
                        // 몬스터가 일정거리 안에 들어올 시 공격으로 전환
                        monsterDis = (searchColl[i].transform.position - playerTr.position).sqrMagnitude;
                        if (monsterDis <= 28f)
                        {
                            curState = Attack(searchColl[i].gameObject);
                            break;
                        }
                    }
                    // NPC 탐색
                    else if (searchColl[i].CompareTag("NPC"))
                    {
                        curState = TurningNpc(searchColl[i].transform);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 이동 속도 증가
    public float IncrementSpeed(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed.Equals(maxSpeed))
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
            return (Mathf.Sign(maxSpeed - initSpeed).Equals(1)) ? initSpeed : maxSpeed;
        }
    }
    float DecreaseSpeed(float initSpeed, float minSpeed, float accel)
    {
        //if (initSpeed.Equals(minSpeed))
        //    return initSpeed;
        //else {
            initSpeed -= accel * Time.deltaTime;
        // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
        
            return (Mathf.Sign(minSpeed - initSpeed).Equals(1)) ? minSpeed : initSpeed;
        //}
    }
    // 대기 상태 시 랜덤으로 포인트 위치 이동
    protected virtual Vector3 SetRandomPos()
    {
        float rndPointX = Random.Range(camTr.position.x - 4f, camTr.position.x + 4f);
        float rndPointY = Random.Range(playerTr.position.y - 1f, camTr.position.y + 2f);

        return new Vector3(rndPointX, rndPointY, playerTr.position.z);
    }

    // 레이캐스트 발사
    protected virtual void ShotRay()
    {
        // 양측 레이 발사 위치
        Vector3 rightRayPos = transform.position + (transform.right * 0.3f);
        Vector3 leftRayPos = transform.position - (transform.right * 0.3f);

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        // 우측 레이캐스트
        if (Physics.Raycast(rightRayPos, forward, out hit, 3f) || Physics.Raycast(leftRayPos, forward, out hit, 3f))
        {
            // 플레이어, 벽, 땅이 있을 시 우회
            if (hit.collider.CompareTag("WALL") || hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Player"))
            {
                relativePos += hit.normal * 50f;
            }
        }
    }
    // 카메라 밖으로 나갔는지 확인
    protected virtual bool CheckOutCamera()
    {
        Vector3 camVec = Camera.main.WorldToScreenPoint(transform.position);
        // 화면 밖으로 나갔을 시 true (오른쪽 || 왼쪽) 
        if (camVec.x >= Camera.main.pixelWidth - 130f || camVec.x <= 130f)
            return true;
        else
            return false;
    }

    // 랜덤 값 생성
    protected virtual int GetRandomValue(float[] values)
    {
        float total = 0f;
        // 전체 합을 구함
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        // 전체 합의 임이의 0~1의 변수를 곱함
        float randomPoint = Random.value * total;

        for (int i = 0; i < values.Length; i++)
        {
            if (randomPoint < values[i])
            {
                return i;
            }
            else {
                randomPoint -= values[i];
            }
        }
        return values.Length - 1;
    }
}