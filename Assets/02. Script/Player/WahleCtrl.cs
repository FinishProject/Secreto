using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    public Transform playerTr;
    public float speed;

    public static bool isChange = true;
    private Vector3 moveDir = Vector3.zero;
    Vector3 realtivePos = Vector3.zero;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { isChange = !isChange; }

        if (isChange)
        {
            transform.position = Vector3.Lerp(transform.position,
                playerTr.position - (playerTr.forward * 1.0f) + (playerTr.up * 1.5f),
                speed * Time.deltaTime);
        }
        else if (!isChange)
        {
            //moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            //transform.Translate(moveDir * (speed * 10f) * Time.deltaTime);

            if (Input.GetMouseButton(1))
            {
                GetMousePos();
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(moveDir.x, moveDir.y, 0),
                2f * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.V)) { FullFushObject(true); }
        else if(Input.GetKey(KeyCode.C)) { FullFushObject(false); }

        //중점을 중심으로 회전
        //tr.RotateAround(playerTr.position, Vector3.up, Time.deltaTime * 20f);
        //tr.LookAt(playerTr.position);

        //목표를 봐라보도록 회전시킨다. LookAt과 비슷하다.
        //Vector3 dir = targetTr.position - tr.position;
        //Quaternion drot = Quaternion.LookRotation(dir);

        //Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * 20f);
        //transform.rotation = rot;

    }

    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500f)) { moveDir = hit.point; }
    }

    void FullFushObject(bool isFush)
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 3f);
        int i = 0;
        while (i < hitCollider.Length)
        {
            if (hitCollider[i].tag == "Object")
            {
                if (isFush)
                    hitCollider[i].SendMessage("FullObject");
                else if (!isFush)
                    hitCollider[i].SendMessage("FushObject");
            }
            i++;
        }
    }
}
