using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    public float speed; // 이동속도
    public static bool isChange = true; // 스위칭 체크
    private bool isType = false; // 추격 방법 체크
    private bool isFush; // 척력 인력 구분 체크

    public Transform playerTr;
    private Vector3 moveDir;
    private GameObject targetObj = null;

    void FixedUpdate()
    {
        //이동 방식 스위칭
        if (Input.GetKeyDown(KeyCode.Tab)) { isChange = !isChange; }

        //캐릭터 조종
        if (isChange)
        {
            float distance = Vector3.Distance(new Vector3(moveDir.x, moveDir.y, 0f), 
                new Vector3(transform.position.x, transform.position.y, 0f));
            //캐릭터를 추격함 (클릭한 위치이고 플레이어가 이동 시)
            if (distance <= 3f && PlayerCtrl.instance.inputAxis != 0.0f) { isType = false; }
            //클릭한 마우스 위치로 이동함
            if (Input.GetMouseButton(1)) {
                isType = true;
                GetMousePos();
            }
            MoveType(); //이동
        }
        //고래 조종
        else if (!isChange)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.Translate(moveDir * (speed * 10f) * Time.deltaTime);
        }
        //키 입려에 따른 척력 인력 실행
        if (Input.GetKey(KeyCode.V)) { FullFushObject(); isFush = true; }
        else if(Input.GetKey(KeyCode.C)) { FullFushObject(); isFush = false; }
        else { StopCoroutine("GrabObject"); targetObj = null; } // 잡기 중지
    }
    //고래 이동 타입
    void MoveType()
    {
        //플레이어 추격
        if (!isType) {
            transform.position = Vector3.Lerp(transform.position,
                    playerTr.position - (playerTr.forward * 1.0f) + (playerTr.up * 1.5f),
                    speed * Time.deltaTime);
        }
        //마우스 위치 추격
        else if (isType) {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(moveDir.x, moveDir.y, 0), speed * Time.deltaTime);
        }
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
        while (i < hitCollider.Length) {
            if (hitCollider[i].tag == "OBJECT" && targetObj == null) {
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
