using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    public Transform playerTr;
    public float speed;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
            playerTr.position - (playerTr.forward * 1.0f) + (playerTr.up * 1.5f),
            speed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.V))
        {
            FullFushObject(true);
            
        }
        else if(Input.GetKey(KeyCode.C))
        {
            FullFushObject(false);
        }

        //중점을 중심으로 회전
        //tr.RotateAround(playerTr.position, Vector3.up, Time.deltaTime * 20f);
        //tr.LookAt(playerTr.position);

        //목표를 봐라보도록 회전시킨다. LookAt과 비슷하다.
        //Vector3 dir = targetTr.position - tr.position;
        //Quaternion drot = Quaternion.LookRotation(dir);

        //Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * 20f);
        //transform.rotation = rot;

    }

    void FullFushObject(bool bFush)
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 3f);
        int i = 0;
        while (i < hitCollider.Length)
        {
            if (hitCollider[i].tag == "Object")
            {
                if (bFush)
                    hitCollider[i].SendMessage("FullObject");
                else if (!bFush)
                    hitCollider[i].SendMessage("FushObject");
            }
            i++;
        }
    }
}
