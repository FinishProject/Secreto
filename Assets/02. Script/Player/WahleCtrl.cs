using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    public enum Type { trace, mouse, keybord, };
    public static Type moveType = Type.trace;

    public float speed; // 이동속도
    private bool isFush;
    private float countTime = 0f; 
    public float maxTime = 5f;
    public float distance = 0f;

    public Transform playerTr;
    private Vector3 moveDir, camPos;
    private GameObject targetObj = null;

    void FixedUpdate()
    {
        ////이동 방식 스위칭
        //if (Input.GetKeyDown(KeyCode.Tab)) {
        //    if (moveType != Type.keybord) { moveType = Type.keybord;}
        //    else if (moveType == Type.keybord) { moveType = Type.trace; }
        //}
        //else if (Input.GetMouseButton(1) && moveType != Type.keybord) {
        //    moveType = Type.mouse;
        //    GetMousePos();
        //}
        //CheckOutCamera();

        distance = Vector3.Distance(transform.position, playerTr.position);
        //if (distance >= 4.2f) 
        MoveType();

        //키 입력에 따른 척력 인력 실행
        if (Input.GetKey(KeyCode.V)) { FullFushObject(); isFush = true; }
        else if (Input.GetKey(KeyCode.C)) { FullFushObject(); isFush = false; }
        else { StopCoroutine("GrabObject"); targetObj = null; } // 잡기 중지
    }

    //고래 이동 타입
    void MoveType()
    {
        switch (moveType)
        {
            case Type.trace: // 플레이어 추격
                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 1.3f) + (playerTr.up * 1.3f),
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

    //void LookTarger()
    //{
    //중점을 중심으로 회전
    //tr.RotateAround(playerTr.position, Vector3.up, Time.deltaTime * 20f);
    //tr.LookAt(playerTr.position);

    //목표를 봐라보도록 회전시킨다. LookAt과 비슷하다.
    //Vector3 dir = targetTr.position - tr.position;
    //Quaternion drot = Quaternion.LookRotation(dir);

    //Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * 20f);
    //transform.rotation = rot;
    //}
}
