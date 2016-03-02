using UnityEngine;
using System.Collections;

public class WahleCtrl : MonoBehaviour {

    private Transform tr;
    public Transform playerTr;
    public float speed;
    
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        tr.position = Vector3.Lerp(tr.position,
            playerTr.position - (playerTr.forward * 1.0f) + (playerTr.up * 1.5f),
            speed * Time.deltaTime);

        //중점을 중심으로 회전
        //tr.RotateAround(playerTr.position, Vector3.up, Time.deltaTime * 20f);
        //tr.LookAt(playerTr.position);

        //목표를 봐라보도록 회전시킨다. LookAt과 비슷하다.
        //Vector3 dir = targetTr.position - tr.position;
        //Quaternion drot = Quaternion.LookRotation(dir);

        //Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * 20f);
        //transform.rotation = rot;

    }
}
