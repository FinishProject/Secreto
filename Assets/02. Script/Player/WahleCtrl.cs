using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {
    //고래 이동 타입
    public enum MoveType { idle, trace, mouse, keybord, };
    public static MoveType moveType = MoveType.idle;

    public float speed; // 이동속도
    private bool isFush; // 오브젝트 밀고 당기기 체크
    private float countTime = 0f; 
    public float maxTime = 5f; // 최대 이동 시간
    private  bool isFocusRight = true; // 오른쪽을 봐라보는지 확인

    public Transform playerTr;
    private Vector3 moveDir, camPos; // 이동 벡터, 카메라 벡터
    private GameObject targetObj = null; //인력, 척력 대상 오브젝트

    void FixedUpdate()
    {
        //플레이어와 고래의 거리 차이 구함
        float distance = Vector3.Distance(transform.position, playerTr.position);
        if (distance >= 3.7f) moveType = MoveType.trace;
        else moveType = MoveType.idle;

        MovementType();

        if (moveType == MoveType.trace)
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
        //transform.Rotate(new Vector3(0, 0, 1), 180f);
        Vector3 scale = transform.localScale;
        scale.y *= -1f;
        transform.localScale = scale;
    }

    //고래 이동 타입
    void MovementType()
    {
        switch (moveType) {
            case MoveType.idle:
                break;
            case MoveType.trace: // 플레이어 추격
                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 0.5f) + (playerTr.up * 3.3f),
                    speed * Time.deltaTime);
                break;
            //case Type.mouse: // 마우스 이동
            //    transform.position = Vector3.Lerp(transform.position,
            //            new Vector3(moveDir.x, moveDir.y, 0), speed * Time.deltaTime);
            //    CountDown();
            //    break;
            //case Type.keybord: // 키보드 이동
            //    moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            //    transform.Translate(moveDir * (speed * 10f) * Time.deltaTime);
            //    //CheckDistance();
            //    CountDown();
            //    break;
        }
    }
    //카메라 밖 체크
    //void CheckOutCamera()
    //{
    //    camPos = Camera.main.WorldToScreenPoint(transform.position);

    //    if (camPos.x >= Camera.main.pixelWidth || camPos.x <= -1f ||
    //        camPos.y >= Camera.main.pixelHeight) { moveType = Type.trace; }
    //}
    ////거리 체크
    //void CheckDistance()
    //{
    //    float distance = Vector3.Distance(transform.position, playerTr.position);

    //    if(distance > 14f) { moveType = Type.trace; }
    //}
    ////시간 제한 체크
    //void CountDown()
    //{
    //    countTime += Time.deltaTime;
    //    if (countTime >= maxTime) { moveType = Type.trace; countTime = 0f; }
    //}
    ////마우스 좌표값 구하기
    //void GetMousePos()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 500f)) { moveDir = hit.point; }
    //}

    //인력, 척력
    void FullFushObject()
    {
        //주위 오브젝트 탐색
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 3f);
        int i = 0;
        while (i < hitCollider.Length)
        {
            if (hitCollider[i].tag == "OBJECT" && targetObj == null)
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
