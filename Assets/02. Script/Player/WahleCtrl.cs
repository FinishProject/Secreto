using UnityEngine;
using System.Collections;

public enum WahleState { IDLE, MOVE, WALL, ATTACK };

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    public static WahleState state = WahleState.IDLE;
    private WahleState curState = state;

    private float initSpeed = 0f; // 초기 이동속도
    public float maxSpeed; // 최대 이동속도
    public float accel; // 가속도

    private float distance = 0f; // 플레이어와 고래의 거리 차이
    private bool isFocusRight = true; // 오른쪽을 봐라보는지 확인

    private float moveTime = 0f;
    private float rotValue = 0f;
    private float focusDir = 0f;

    public Transform playerTr; // 플레이어 위치

    public  Transform targetTr; // 적 위치값
    private Vector3 relativePos;
    private Quaternion lookRot;

    public bool isDie = false;

    public static WahleCtrl instance;

    void Awake()
    {
        instance = this;
    }

    //void Start()
    //{
    //    StartCoroutine(SearchEnemy());
    //}

    void FixedUpdate()
    {
        distance = (transform.position - playerTr.position).sqrMagnitude;
        relativePos = (playerTr.position - transform.position);
        lookRot = Quaternion.LookRotation(relativePos);
        SearchEnemy();
        switch (state)
        {
            case WahleState.IDLE:
                Idel();
                break;
            case WahleState.MOVE:
                Move();
                break;
            case WahleState.ATTACK:
                Attack();
                break;
        }
    }

    void Idel()
    {
        if (distance > 6f && PlayerCtrl.inputAxis != 0f)
        {
            state = WahleState.MOVE;
        }
        // 플레이어 주위 돌기
        // 부드럽게 타겟을 향해 회전함
        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, Time.deltaTime);
        transform.Translate(0, 0f, 3f * Time.deltaTime);
    }

    void Move()
    {
        if (distance <= 6f && PlayerCtrl.inputAxis == 0f)
        {
            state = WahleState.IDLE;
            initSpeed = 0f;
            moveTime = 0f;
            rotValue = 0f;
        }
        // 이동속도 증가
        initSpeed = IncrementToWards(initSpeed, maxSpeed, accel);

        moveTime += Time.deltaTime;
        if (moveTime >= 1.5f)
        {
            transform.RotateAround(transform.position, Vector3.right, 200f * Time.deltaTime);
            rotValue += transform.rotation.y;
            if (rotValue <= -3f)
            {
                moveTime = 0f;
            }
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
        }

        // 플레이어를 추격
        focusDir = Mathf.Sign(PlayerCtrl.inputAxis);
        transform.position = Vector3.Lerp(transform.position, playerTr.position - (playerTr.forward * focusDir), initSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (targetTr != null)
        {
            float dis = (playerTr.position - transform.position).sqrMagnitude;
            if (dis >= 5f)
            {
                state = WahleState.MOVE;
            }

            relativePos = (targetTr.position - transform.position);
            lookRot = Quaternion.LookRotation(relativePos);
            focusDir = Mathf.Sign(targetTr.position.x - transform.position.x);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetTr.position -
                ((targetTr.forward * -focusDir * 3f) - (targetTr.up)), 7f * Time.deltaTime);
        }
        else
        {
            state = WahleState.IDLE;
        }

        
    }
    // 주변 몬스터 탐색
    void SearchEnemy()
    {
        if (state != WahleState.ATTACK && !isDie)
        {
            Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 5f);
            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (hitCollider[i].CompareTag("MONSTER"))
                {
                    targetTr = hitCollider[i].transform;
                    state = WahleState.ATTACK;
                }
            }
        }
    }

    // 이동 속도 가속도
    float IncrementToWards(float initSpeed, float maxSpeed, float accel)
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