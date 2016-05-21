using UnityEngine;
using System.Collections;

public enum WahleState { IDLE, MOVE, WALL, ATTACK };

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    //public static WahleState state = WahleState.IDLE;
    //private WahleState curState = state;

    private float initSpeed = 0f; // 초기 이동속도
    public float maxSpeed; // 최대 이동속도
    public float accel; // 가속도

    private float distance = 0f; // 플레이어와 고래의 거리 차이
    private bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    private bool isWait = false; 
    private float focusDir = 0f;
    private float delTime = 0f;
    private float sinSpeed = 0f;
    private int stateValue = 0;

    public Transform playerTr; // 플레이어 위치
    private  Transform targetTr; // 적 위치값
    private Vector3 relativePos;
    private Quaternion lookRot;

    public static WahleCtrl instance;

    void Start()
    {
        instance = this;
        StartCoroutine(Idel());
    }

    //void Update()
    //{
    //    distance = (transform.position - playerTr.position).sqrMagnitude;
    //    relativePos = (playerTr.position - transform.position);
    //    lookRot = Quaternion.LookRotation(relativePos);

    //    SearchEnemy();

    //    switch (state)
    //    {
    //        case WahleState.IDLE:
    //            Idel();
    //            break;
    //        case WahleState.MOVE:
    //            Move();
    //            break;
    //        case WahleState.ATTACK:
    //            Attack();
    //            break;
    //    }
    //}

    IEnumerator Idel()
    {
        initSpeed = 0f;

        float[] value = { 50, 50 };
        //int num = GetRandomValue(value);

        while (true)
        {
            if (distance > 6f && PlayerCtrl.inputAxis != 0f)
            {
                StartCoroutine(Move());
                break;
            }

            if (!isWait)
                StartCoroutine(WaitRandom(3f, value));

            relativePos = playerTr.position - transform.position;
            distance = relativePos.sqrMagnitude;
            lookRot = Quaternion.LookRotation(relativePos);

            initSpeed = IncrementSpeed(initSpeed, 3f, 0.2f);

            if (stateValue == 1)
            {
                transform.RotateAround(transform.position, Vector3.forward, 100f * Time.deltaTime);
                transform.Translate(new Vector3(0f, 2f * Time.deltaTime, 2f * Time.deltaTime));
            }
            // 플레이어 주위 돌기
            else {
                sinSpeed = Mathf.Sin(delTime += Time.deltaTime);

                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, Time.deltaTime);
                transform.Translate(0, (sinSpeed * 0.5f) * Time.deltaTime, initSpeed * Time.deltaTime);
        }
        yield return null;
        }
    }

    // 이동
    IEnumerator Move()
    {
        while (true)
        {
            if (distance <= 6f && PlayerCtrl.inputAxis == 0f)
            {
                StartCoroutine(Idel());
                break;
            }
            // 이동속도 증가
            initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);

            delTime += Time.deltaTime;
            sinSpeed = Mathf.Sin(delTime);

            distance = (transform.position - playerTr.position).sqrMagnitude;
            relativePos = (playerTr.position - transform.position);
            lookRot = Quaternion.LookRotation(relativePos);

            //moveTime += Time.deltaTime;
            //if (moveTime >= 1.5f)
            //{
            //    transform.RotateAround(transform.position, Vector3.right, 200f * Time.deltaTime);
            //    float rotValue += transform.rotation.y;
            //    if (rotValue <= -3f)
            //    {
            //        moveTime = 0f;
            //    }
            //}
            //else
            //{
            //    transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
            //}

            // 캐릭터 회전시 겹쳐짐을 방지하기 위해 좌우 방향에 따른 양수 또는 음수를 줌
            focusDir = Mathf.Sign(PlayerCtrl.inputAxis);

            transform.Translate(new Vector3(0f, (sinSpeed * 3f) * Time.deltaTime, 0f));

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 2f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, playerTr.position - (playerTr.forward * focusDir),
                initSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator WaitRandom(float waitTime, float[] value)
    {
        isWait = true;
        yield return new WaitForSeconds(waitTime);
        stateValue = GetRandomValue(value);
        isWait = false;
    }

    //// 공격
    //void Attack()
    //{
    //    if (targetTr != null)
    //    {
    //        float dis = (playerTr.position - transform.position).sqrMagnitude;
    //        if (dis >= 5f)
    //        {
    //            SetState(WahleState.MOVE);
    //        }

    //        relativePos = (targetTr.position - transform.position);
    //        lookRot = Quaternion.LookRotation(relativePos);
    //        focusDir = Mathf.Sign(targetTr.position.x - transform.position.x);

    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
    //        transform.position = Vector3.Lerp(transform.position, targetTr.position -
    //            ((targetTr.forward * -focusDir * 3f) - (targetTr.up)), 7f * Time.deltaTime);
    //    }
    //    else
    //    {
    //        SetState(WahleState.IDLE);
    //    }
    //}
    // 주변 몬스터 탐색
    //void SearchEnemy()
    //{
    //    if (state != WahleState.ATTACK && !isDie)
    //    {
    //        Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 5f);
    //        for (int i = 0; i < hitCollider.Length; i++)
    //        {
    //            if (hitCollider[i].CompareTag("MONSTER"))
    //            {
    //                targetTr = hitCollider[i].transform;
    //                state = WahleState.ATTACK;
    //            }
    //        }
    //    }
    //}

    // 랜덤 값 생성
    int GetRandomValue(float[] values)
    {
        float total = 0;

        for (int i = 0; i < values.Length; i++) {
            total += values[i];
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < values.Length; i++)
        {
            if (randomPoint < values[i]) {
                return i;
            }
            else {
                randomPoint -= values[i];
            }
        }
        return values.Length - 1;
    }

    // 이동 속도 증가
    float IncrementSpeed(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed == maxSpeed)
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
            return (1 == Mathf.Sign(maxSpeed - initSpeed)) ? initSpeed : maxSpeed;
        }
    }
}