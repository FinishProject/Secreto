using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    private enum Type { trace, mouse, keybord };

    private Type moveType = Type.trace;

    public float speed; // 이동속도
    public static bool isChange = true; // 스위칭 체크
    private bool isFush;

    private float countTime = 0f; 
    public float maxTime = 5f; 

    public Transform playerTr;
    private Vector3 moveDir;

    private GameObject targetObj = null;
    private Vector3 camVec;

    void FixedUpdate()
    {
        Debug.Log(countTime);
        //이동 방식 스위칭
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isChange = !isChange;
            countTime = 0f;
            if (!isChange) { moveType = Type.keybord;}
            else if (isChange) { moveType = Type.trace; }
        }
        else if (Input.GetMouseButton(1) && isChange) {
            moveType = Type.mouse;
            GetMousePos();
        }
        CheckOutCamera();
        MoveType();

        //키 입려에 따른 척력 인력 실행
        if (Input.GetKey(KeyCode.V)) { FullFushObject(); isFush = true; }
        else if (Input.GetKey(KeyCode.C)) { FullFushObject(); isFush = false; }
        else { StopCoroutine("GrabObject"); targetObj = null; } // 잡기 중지
    }
    //카메라 밖 체크
    void CheckOutCamera()
    {
        camVec = Camera.main.WorldToScreenPoint(transform.position);

        if (camVec.x >= Camera.main.pixelWidth || camVec.x <= -1f) moveType = Type.trace;
        else if (camVec.y >= Camera.main.pixelHeight) moveType = Type.trace;
    }
    //고래 이동 타입
    void MoveType()
    {
        switch (moveType)
        {
            case Type.trace:
                transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 1.0f) + (playerTr.up * 1.5f),
                    speed * Time.deltaTime);
                break;
            case Type.mouse:
                transform.position = Vector3.Lerp(transform.position,
                        new Vector3(moveDir.x, moveDir.y, 0), speed * Time.deltaTime);
                CountDonw();
                break;
            case Type.keybord:
                moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                transform.Translate(moveDir * (speed * 10f) * Time.deltaTime);
                CountDonw();
                break;
        }
    }

    void CountDonw()
    {
        countTime += Time.deltaTime;
        if (countTime >= maxTime) { moveType = Type.trace; isChange = true; }
    }

    //마우스 좌표값 구하기
    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f)) { moveDir = hit.point; }
    }

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
