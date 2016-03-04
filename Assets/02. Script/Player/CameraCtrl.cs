using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public Transform playerTr; // 플레이어의 위치값
    public float speed = 10f; // 카메라의 속도

    private Vector3 relCameraPos;

    void Start()
    {
        relCameraPos = transform.position - playerTr.position;

    }
    void FixedUpdate()
    {
        Vector3 standardPos = playerTr.position + relCameraPos;
        transform.position = Vector3.Lerp(transform.position, standardPos, speed * Time.deltaTime);

    }

    //void Start()
    //{
    //    tr = GetComponent<Transform>();
    //    offset = tr.position - playerTr.position;
    //}

    //void FixedUpdate()
    //{
    //    tr.position = playerTr.position + new Vector3(offset.x+ horizontal, offset.y+y, offset.z+ zoom);
    //    //타겟 추적
    //    tr.position = Vector3.Lerp(tr.position,
    //            playerTr.position - (playerTr.right * zoom) + (playerTr.up * vertical) + (playerTr.forward * horizontal),
    //           speed * Time.deltaTime);

    //    //타겟을 봐라봄(LookAt과 같음)
    //    Vector3 dir = playerTr.position - tr.position;
    //    Quaternion drot = Quaternion.LookRotation(dir);
    //    Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * speed);
    //    transform.rotation = rot;
    //}
}
