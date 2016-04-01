using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    public enum MoveType { IDLE, TRACE, WALL, };
    public static MoveType moveType = MoveType.IDLE;

    public float speed; // 이동속도
    private bool isFush; // 오브젝트 밀고 당기기 체크
    //public float maxTime = 5f; // 최대 이동 시간
    private  bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    private bool isWall = false; // 벽에 충돌했는지 여부

    public Transform playerTr; // 플레이어 위치
    private Vector3 moveDir, camPos; // 이동 벡터, 카메라 벡터
    private GameObject targetObj = null; //인력, 척력 대상 오브젝트

    void FixedUpdate()
    {
        //플레이어와 고래의 거리 차이 구함
        float distance = Vector3.Distance(transform.position, playerTr.position);
        if (distance >= 3.7f && !isWall)
        {
            moveType = MoveType.TRACE;
        }
        else if (!isWall) moveType = MoveType.IDLE;

        MovementType();

        if (moveType == MoveType.TRACE)
        {
            if (PlayerCtrl.inputAxis < 0f && isFocusRight) { TurnFocus(); }
            else if (PlayerCtrl.inputAxis > 0f && !isFocusRight) { TurnFocus(); }
        }

        //키 입력에 따른 척력 인력 실행
        if (Input.GetKey(KeyCode.V)) { FullFushObject(); isFush = true; }
        else if (Input.GetKey(KeyCode.C)) { FullFushObject(); isFush = false; }
        else { StopCoroutine("GrabObject"); targetObj = null; } // 잡기 중지
    }

    void TurnFocus()
    {
        isFocusRight = !isFocusRight;
        transform.Rotate(new Vector3(0, 0, 1), 180f);
        //Vector3 scale = transform.localScale;
        //scale.y *= -1f;
        //transform.localScale = scale;
    }

    //고래 이동 타입
    void MovementType()
    {
        switch (moveType) {
            case MoveType.IDLE:
                break;
            case MoveType.TRACE: // 플레이어 추격
                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 0.5f) + (playerTr.up * 3.3f),
                    speed * Time.deltaTime);
                break;
            case MoveType.WALL:
                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * -2f) + (playerTr.up * 3.3f),
                    speed * Time.deltaTime);
                StartCoroutine(ResetType());
                break;
        }
    }

    IEnumerator ResetType()
    {
        yield return new WaitForSeconds(1f);
        isWall = false;
        StopCoroutine(ResetType());
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

    //인력, 척력
    void FullFushObject()
    {
        //주위 오브젝트 탐색
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 3f);
        int i = 0;
        while (i < hitCollider.Length)
        {
            if (hitCollider[i].tag == "OBJECT" || hitCollider[i].tag == "MONSTER" && targetObj == null)
            {
                StartCoroutine("GrabObject", hitCollider[i].gameObject);
                break;
            }
            i++;
        }
    }
    // 인력, 척력 유지
    IEnumerator GrabObject(GameObject target)
    {
        targetObj = target;
        while (true)
        {
            if (isFush)
                targetObj.SendMessage("FullObject");
            else if (!isFush)
                targetObj.SendMessage("FushObject");
            yield return null;
        }
    }
}
