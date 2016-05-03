using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    private enum MoveType { IDLE, TRACE, WALL, ATTACK};
    private MoveType moveType = MoveType.IDLE;

    private float initSpeed; // 초기 이동속도
    public float maxSpeed; // 최대 이동속도
    public float accel; // 가속도
    private float distance; // 플레이어와 고래의 거리 차이
    private bool isFush; // 오브젝트 밀고 당기기 체크
    private  bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    private bool isWall = false; // 벽에 충돌했는지 여부
    private float fowardValue = 1f;

    public Transform playerTr; // 플레이어 위치
    private Vector3 moveDir; // 이동 벡터, 카메라 벡터
    private Transform monTr; // 몬스터 위치
    private GameObject targetObj = null; //인력, 척력 대상 오브젝트

    private bool isMon = false;

    void Start()
    {
        //StartCoroutine(FindMonster());
    }

    void FixedUpdate()
    {
        // 플레이어와 고래의 거리 차이 구함
        distance = (transform.position - playerTr.position).sqrMagnitude;
        //transform.Rotate(Vector3.up, 70f * Time.deltaTime, Space.Self);
        MovementType();
    }
    // 고래 회전
    void TurnFocus()
    {
        isFocusRight = !isFocusRight;
        Vector3 scale = transform.localScale;
        scale.y *= -1f;
        transform.localScale = scale;
        fowardValue *= -1f;
        moveType = MoveType.IDLE;
        //transform.Rotate(new Vector3(0, 0, 1), 180f);
        initSpeed = 0f;
    }

    //고래 이동 타입
    void MovementType()
    {
        switch (moveType) {
            case MoveType.IDLE:
                initSpeed = 0f;
                if (distance >= 4.1f)
                    moveType = MoveType.TRACE;
                break;
            case MoveType.TRACE: // 플레이어 추격
                if (distance <= 4f)
                    moveType = MoveType.IDLE;
                // 플레이어가 고래의 좌우 어느쪽에 있는지 체크함.
                float curSide = Mathf.Sign(playerTr.position.x - transform.position.x);

                if (PlayerCtrl.inputAxis < -0.1f && isFocusRight && curSide <= -1f) { TurnFocus(); }
                else if (PlayerCtrl.inputAxis > 0.1f && !isFocusRight && curSide >= 1f) { TurnFocus(); }

                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position + (playerTr.up * 1.7f) - (playerTr.forward * fowardValue), initSpeed * Time.deltaTime);

                //transform.position = Vector3.Lerp(transform.position,
                //    playerTr.position - (playerTr.forward * 0.5f) + (playerTr.up * 1.5f ),
                //    initSpeed * Time.deltaTime);
                // 가속도
                initSpeed = IncrementToWards(initSpeed, maxSpeed, accel);
                break;
            //case MoveType.ATTACK:
            //    if (isMon)
            //    {
            //        transform.position = Vector3.Lerp(transform.position,
            //            playerTr.position - (playerTr.up * 2f),
            //            initSpeed * Time.deltaTime);
            //    }
                //Vector3 lookPos = new Vector3(monTr.position.x, monTr.position.y + 20f, 0f);
                //transform.LookAt(lookPos);
                //break;
        }
    }

    IEnumerator FindMonster()
    {
        while (true)
        {
            if (!isMon)
            {
                Collider[] hitColl = Physics.OverlapSphere(playerTr.position, 10f);
                for (int i = 0; i < hitColl.Length; i++)
                {
                    if (hitColl[i].tag == "MONSTER")
                    {
                        monTr = hitColl[i].transform;
                        moveType = MoveType.ATTACK;
                        isMon = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void OnTriggerStay(Collider coll)
    {
        // 벽과 겹쳐지지 않도록 이동
        if (coll.tag == "WALL") {
            // 충돌 방향 구함
            //Quaternion targetRotate = Quaternion.LookRotation(coll.transform.position - transform.position, Vector3.up);
            moveType = MoveType.WALL;
            isWall = true;
        }
    }
    IEnumerator ResetType()
    {
        yield return new WaitForSeconds(1f);
        isWall = false;
        StopCoroutine(ResetType());
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
