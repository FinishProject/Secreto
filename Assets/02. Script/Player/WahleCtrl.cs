using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    private enum MoveType { IDLE, TRACE, WALL, ATTACK};
    private MoveType moveType = MoveType.IDLE;

    public float initSpeed; // 이동속도
    public float maxSpeed;
    public float accel; // 가속도
    private float distance;
    private bool isFush; // 오브젝트 밀고 당기기 체크
    private  bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    private bool isWall = false; // 벽에 충돌했는지 여부

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

    void TurnFocus()
    {
        isFocusRight = !isFocusRight;
        transform.Rotate(new Vector3(0, 0, 1), 180f);
    }

    //고래 이동 타입
    void MovementType()
    {
        switch (moveType) {
            case MoveType.IDLE:
                initSpeed = 2f;
                if (distance >= 2f)
                    moveType = MoveType.TRACE;
                break;
            case MoveType.TRACE: // 플레이어 추격
                if (distance <= 1.1f && PlayerCtrl.inputAxis == 0f)
                    moveType = MoveType.IDLE;

                if (PlayerCtrl.inputAxis < 0f && isFocusRight) { TurnFocus(); }
                else if (PlayerCtrl.inputAxis > 0f && !isFocusRight) { TurnFocus(); }

                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 0.5f) + (playerTr.up * 0.59f ),
                    initSpeed * Time.deltaTime);

                initSpeed = IncrementToWards(initSpeed, maxSpeed, accel);
                break;
            case MoveType.ATTACK:

                if (isMon)
                {
                    transform.position = Vector3.Lerp(transform.position,
                        playerTr.position - (playerTr.up * 2f),
                        initSpeed * Time.deltaTime);
                }
                //Vector3 lookPos = new Vector3(monTr.position.x, monTr.position.y + 20f, 0f);
                //transform.LookAt(lookPos);
                break;
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

    float IncrementToWards(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed == maxSpeed)
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            return (1 == Mathf.Sign(maxSpeed - initSpeed)) ? initSpeed : maxSpeed;
        }
    }
}
