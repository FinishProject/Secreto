﻿using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    private float pixelX, pixelY;
    private float rndPointX = 0f, rndPointY = 0f;
    private float distance = 0f;
    private float initSpeed = 0f;
    private bool isAttack = false;

    private Transform targetPoint;
    private Vector3 relativePos; // 상대적 위치값
    private Quaternion lookRot;
    private Vector3 rightRayPos, leftRayPos; // 레이캐스트 발사 위치
    private GameObject targetObj;

    public Transform playerTr;
    private Transform camTr;
    private Animator anim;

    private IEnumerator curState;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        camTr = GameObject.FindGameObjectWithTag("MainCamera").transform;
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = new GameObject().transform;

        curState = Idle();
        StartCoroutine(CoroutineUpdate());
        StartCoroutine(SearchEnemy());
    }

    IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            if(!curState.Equals(null) && curState.MoveNext())
            {
                yield return curState.Current;
            }
            else
                yield return null;
        }
    }
    // 대기
    IEnumerator Idle()
    {
        anim.SetBool("Move", true);
        SetRandomPos();
        while (true)
        {
            ShotRay();
            // 이동으로 전환
            if (PlayerCtrl.inputAxis >= 1f || PlayerCtrl.inputAxis <= -1f)
            {
                if(CheckOutCamera())
                    curState = Move();
            }
            // 타겟 위치를 랜덤한 위치로 재배치
            if(distance <= 4f)
            {
                SetRandomPos();
            }

            relativePos = targetPoint.position - transform.position;
            distance = relativePos.sqrMagnitude;
            lookRot = Quaternion.LookRotation(relativePos);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 1f * Time.deltaTime);
            transform.Translate(0f, 0f, 2f * Time.deltaTime);

            yield return null;
        }
    }
    // 이동
    IEnumerator Move()
    {
        initSpeed = 0f;
        float changeTime = 0f;
        float sinSpeed = 0f, delTime = 0f;
        while (true)
        {
            relativePos = playerTr.position - transform.position;
            lookRot = Quaternion.LookRotation(relativePos);
            if (PlayerCtrl.inputAxis == 0f)
            {
                changeTime += Time.deltaTime;
                if (changeTime >= 4f)
                {
                    curState = Idle();
                }
                sinSpeed = Mathf.Cos(delTime += Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 1.5f * Time.deltaTime);
                transform.Translate(0, 0f, 2f * Time.deltaTime);
            }
            else {
                initSpeed = IncrementSpeed(initSpeed, 10f, 10f);

                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 2f * Time.deltaTime);

                if (initSpeed < 10f)
                    transform.Translate(0, 0, initSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.Lerp(transform.position, playerTr.position + (playerTr.forward),
                            3f * Time.deltaTime);
            }


            yield return null;
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        float speed = 3f;

        Vector3 fromPointVec = playerTr.position + ((playerTr.forward * 2f ) + (playerTr.up * 0.5f));
        Vector3 enemyRelativePos;
        while (true)
        {
            if (CheckOutCamera() || !targetObj.activeSelf)
            {
                isAttack = false;
                curState = Move();
            }

            enemyRelativePos = targetObj.transform.position - transform.position;
            lookRot = Quaternion.LookRotation(enemyRelativePos);

            distance = relativePos.sqrMagnitude; // 플레이어와 거리차
            //float targetDis = enemyRelativePos.sqrMagnitude; // 몬스터와 거리차

            speed += Time.deltaTime * 5f;
            // 각도 제한
            if (transform.localRotation.x <= 0.24f)
                lookRot.x = 0f;

            // 타겟을 봐라보도록 회전
            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
            // 지정 된 위치로 이동
            Vector3 center = (fromPointVec + transform.position) * 0.55f;
            center -= new Vector3(0, 1, 1);
            transform.position = Vector3.Slerp(transform.position - center, fromPointVec - center, speed * Time.deltaTime);
            transform.position += center;

            yield return null;
        }
    }

   // 주변 몬스터 탐색
   IEnumerator SearchEnemy()
    {
        while (true)
        {
            Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 5f);
            if (!isAttack)
            {
                for (int i = 0; i < hitCollider.Length; i++)
                {
                    if (hitCollider[i].CompareTag("MONSTER"))
                    {
                        targetObj = hitCollider[i].gameObject;
                        curState = Attack();
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

    void SetRandomPos()
    {
        rndPointX = Random.Range(camTr.position.x - 7f, camTr.position.x + 7f);
        rndPointY = Random.Range(playerTr.position.y - 1f, camTr.position.y + 3f);

        targetPoint.transform.position = new Vector3(rndPointX, rndPointY, playerTr.position.z);
    }

    // 레이캐스트 발사
    void ShotRay()
    {
        // 양측 레이 발사 위치
        rightRayPos = transform.position + (transform.right * 0.3f);
        leftRayPos = transform.position - (transform.right * 0.3f);

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        // 우측 레이캐스트
        if (Physics.Raycast(rightRayPos, forward, out hit, 3f) || Physics.Raycast(leftRayPos, forward, out hit, 3f))
        {
            relativePos += hit.normal * 20f;
            //if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("WALL"))
            //{
            //    Debug.Log("Hit");
            //    relativePos += hit.normal * 20f;
            //}
        }
    }

    bool CheckOutCamera()
    {
        Vector3 camVec = Camera.main.WorldToScreenPoint(transform.position);
        if (camVec.x >= Camera.main.pixelWidth || camVec.x <= -1f)
        {
            return true;
        }
        else
            return false;
        //else if (camVec.y >= Camera.main.pixelHeight) moveType = Type.trace;
    }


    //private float initSpeed = 0f; // 초기 이동속도
    //public float maxSpeed; // 최대 이동속도
    //public float accel; // 가속도

    //private float lookSpeed = 1f; // 봐라보도록 회전하는 속도
    //private float distance = 0f; // 플레이어와 고래의 거리 차이
    //private bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    //private bool isWait = false; // 코루틴 실행 중인지 확인
    //private float focusDir = 1f; // 현재 봐라보고 있는 방향 변수
    //private float delTime = 0f; // 누적 되는 deltaTime 값 저장
    //private float sinSpeed = 0f; // sin함수로 생성되는 값 저장
    //private int stateValue = 0; // 상태 값
    //private bool isAttack = false; // 현재 상태가 공격인지 확인

    //public static IEnumerator curState; // 현재 실행 중인 코루틴 함수를 담음

    //public Transform playerTr; // 플레이어 위치

    //private  GameObject targetObj; // 적 위치값
    //private Vector3 relativePos; // 상대적 위치값
    //private Quaternion lookRot;
    //private Animator anim;
    //private Vector3 rightRayPos, leftRayPos; // 레이캐스트 발사 위치

    //public static WahleCtrl instance;

    //void Start()
    //{
    //    instance = this;
    //    anim = gameObject.GetComponentInChildren<Animator>();

    //    curState = Idle();
    //    StartCoroutine(StateUpdate());
    //    StartCoroutine(SearchEnemy());
    //}

    //// curState가 null이 아니면 curState에 있는 코루틴을 실행함
    //IEnumerator StateUpdate()
    //{
    //    while (true)
    //    {
    //        relativePos = ShotRay();
    //        if (!curState.Equals(null) && curState.MoveNext())
    //        {
    //            yield return curState.Current;
    //        }
    //        else
    //            yield return null;
    //    }
    //}

    //// 대기
    //IEnumerator Idle()
    //{
    //    float[] value = { 100, 0 };
    //    stateValue = GetRandomValue(value);

    //    anim.SetBool("Move", true);

    //    float cosSpeed = 0f;

    //    while (true)
    //    {
    //        // Move 상태로 변경
    //        if (distance > 6f && !PlayerCtrl.inputAxis.Equals(0f))
    //        {
    //            curState = Move();
    //            break;
    //        }

    //        //if (!isWait)
    //        //    StartCoroutine(WaitRandom(3f, value));

    //        distance = relativePos.sqrMagnitude;
    //        lookRot = Quaternion.LookRotation(relativePos);

    //        //Debug.Log(Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg);

    //        switch (stateValue)
    //        {
    //            //case 0:
    //            //    //transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 2f * Time.deltaTime);
    //            //    sinSpeed = Mathf.Cos(delTime += Time.deltaTime) * 0.3f;
    //            //    cosSpeed = Mathf.Cos(delTime) * 0.1f;
    //            //    transform.Translate(cosSpeed * Time.deltaTime, sinSpeed * Time.deltaTime, 0f);
    //                //break;
    //            case 0: // 플레이어 주위를 선회
    //                sinSpeed = Mathf.Cos(delTime += Time.deltaTime) * 0.5f;

    //                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, lookSpeed * Time.deltaTime);
    //                transform.Translate(0, sinSpeed * Time.deltaTime, 3f * Time.deltaTime);
    //                break;
    //            case 2: // 360도 회전   

    //                transform.RotateAround(transform.position, Vector3.forward, 100f * Time.deltaTime);
    //                transform.Translate(new Vector3(0f, 5f * Time.deltaTime, 0f));
    //                break;
    //        }
    //        yield return null;
    //    }
    //}

    //// 이동
    //public IEnumerator Move()
    //{
    //    float[] value = { 60, 40 };
    //    float repeatSpeed = 1f; // 
    //    float rotSpeed = 7f;
    //    anim.SetBool("Move", true);
    //    initSpeed = 0f;

    //    while (true)
    //    {
    //        // Idle 상태로 변경
    //        if (distance <= 6f && PlayerCtrl.inputAxis.Equals(0f))
    //        {
    //            curState = Idle();
    //            break;
    //        }

    //        distance = relativePos.sqrMagnitude; // 거리 차
    //        lookRot = Quaternion.LookRotation(relativePos);

    //        // 이동속도 증가
    //        initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);

    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, rotSpeed * Time.deltaTime);
    //        // 선형 보간을 사용한 캐릭터 추격
    //        transform.position = Vector3.Lerp(transform.position, playerTr.position - (playerTr.forward),
    //                initSpeed * Time.deltaTime);

    //        if (initSpeed.Equals(maxSpeed))
    //        {
    //            // 앞뒤로 반복 이동을 위한 속도 값
    //            sinSpeed = (Mathf.Sin(delTime += Time.deltaTime) - 0.3f) * 2f;

    //            if (repeatSpeed <= 5f) {
    //                repeatSpeed += (Time.deltaTime * 2f);
    //            }
    //            transform.Translate(new Vector3(0f, 0f, (sinSpeed * repeatSpeed) * Time.deltaTime));
    //        }

    //        yield return null;
    //    }
    //}
    //// 공격
    //IEnumerator Attack()
    //{
    //    //anim.SetBool("Move", false);
    //    float targetDis = 0f;
    //    float speed = 0f;

    //    Vector3 fromPointVec = playerTr.position + ((playerTr.forward * 2f * focusDir) + (playerTr.up * 0.5f));
    //    Vector3 enemyRelativePos;

    //    while (true)
    //    {
    //        if (!targetObj.activeSelf || distance >= 20f)
    //        {
    //            curState = Move();
    //            isAttack = false;
    //        }

    //        enemyRelativePos = targetObj.transform.position - transform.position;
    //        lookRot = Quaternion.LookRotation(enemyRelativePos);

    //        distance = relativePos.sqrMagnitude; // 플레이어와 거리차
    //        targetDis = enemyRelativePos.sqrMagnitude; // 몬스터와 거리차

    //        speed += Time.deltaTime * 5f;
    //        // 각도 제한
    //        if (transform.localRotation.x <= 0.24f)
    //            lookRot.x = 0f;

    //        // 타겟을 봐라보도록 회전
    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
    //        // 지정 된 위치로 이동
    //        Vector3 center = (fromPointVec + transform.position) * 0.55f;
    //        center -= new Vector3(0, 1, 1);
    //        transform.position = Vector3.Slerp(transform.position - center, fromPointVec - center, speed * Time.deltaTime);
    //        transform.position += center;

    //        yield return null;
    //    }
    //}

    //public IEnumerator StepHold()
    //{
    //    while (true)
    //    {
    //        //relativePos = (playerTr.position - transform.position); // 두 객체간의 거리 차
    //        lookRot = Quaternion.LookRotation(relativePos);

    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
    //        // 선형 보간을 사용한 캐릭터 추격
    //        transform.position = Vector3.Lerp(transform.position, playerTr.position - playerTr.forward,
    //            10f * Time.deltaTime);

    //        yield return null;
    //    }
    //}

    //// 주변 몬스터 탐색
    //IEnumerator SearchEnemy()
    //{
    //    while (true)
    //    {
    //        Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 5f);
    //        if (!isAttack && PlayerCtrl.controller.isGrounded)
    //        {
    //            for (int i = 0; i < hitCollider.Length; i++)
    //            {
    //                if (hitCollider[i].CompareTag("MONSTER"))
    //                {
    //                    targetObj = hitCollider[i].gameObject;
    //                    curState = Attack();
    //                    isAttack = true;
    //                    break;
    //                }
    //            }
    //        }

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //IEnumerator WaitRandom(float waitTime, float[] value)
    //{
    //    isWait = true;
    //    yield return new WaitForSeconds(waitTime);
    //    stateValue = GetRandomValue(value);
    //    isWait = false;
    //}

    //// 랜덤 값 생성
    //int GetRandomValue(float[] values)
    //{
    //    float total = 0;
    //    // 전체 합을 구함
    //    for (int i = 0; i < values.Length; i++) {
    //        total += values[i];
    //    }
    //    // 전체 합의 임이의 0~1의 변수를 곱함
    //    float randomPoint = Random.value * total;

    //    for (int i = 0; i < values.Length; i++)
    //    {
    //        if (randomPoint < values[i]) {
    //            return i;
    //        }
    //        else {
    //            randomPoint -= values[i];
    //        }
    //    }
    //    return values.Length - 1;
    //}

    //// 이동 속도 증가
    //public float IncrementSpeed(float initSpeed, float maxSpeed, float accel)
    //{
    //    if (initSpeed.Equals(maxSpeed))
    //        return initSpeed;
    //    else {
    //        initSpeed += accel * Time.deltaTime;
    //        // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
    //        return (Mathf.Sign(maxSpeed - initSpeed).Equals(1)) ? initSpeed : maxSpeed;
    //    }
    //}
    //// 이동 속도 감소
    //float DecreaseSpeed(float initSpeed, float minSpeed, float accel)
    //{
    //    if (initSpeed.Equals(minSpeed))
    //        return initSpeed;
    //    else
    //    {
    //        initSpeed -= accel * Time.deltaTime;
    //        return (Mathf.Sign(initSpeed - minSpeed).Equals(1)) ? initSpeed : minSpeed;
    //    }
    //}
    //// 레이캐스트 발사
    //Vector3 ShotRay()
    //{
    //    // 양측 레이 발사 위치
    //    rightRayPos = transform.position + (transform.right * 0.3f);
    //    leftRayPos = transform.position - (transform.right * 0.3f);
    //    relativePos = (playerTr.position - transform.position);

    //    RaycastHit hit;
    //    Vector3 forward = transform.TransformDirection(Vector3.forward);
    //    // 우측 레이캐스트
    //    if (Physics.Raycast(rightRayPos, forward, out hit, 3f) || Physics.Raycast(leftRayPos, forward, out hit, 3f))
    //    {
    //        if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("WALL"))
    //        {
    //            // 레이캐스트 충돌 시 회피
    //            if (!PlayerCtrl.isFocusRight)
    //            {
    //                lookSpeed = 1.5f;
    //                return new Vector3(relativePos.x - hit.normal.x, relativePos.y + hit.normal.y * 20f, relativePos.z - hit.normal.z);
    //            }
    //            else
    //                return relativePos += hit.normal * 20f;
    //        }
    //        else
    //            lookSpeed = 1f;
    //    }
    //    return relativePos;
    //}
}