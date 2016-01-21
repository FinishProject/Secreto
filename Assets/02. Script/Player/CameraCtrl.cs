using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public Transform playerTr; // 플레이어의 위치값
    private float speed = 200f; // 카메라의 속도

    public float zoom = 0f;
    public float vertical = 0f;
    public float horizontal = 0f;
    public float y = 0f;

    private Transform tr;

    Vector3 offset;

    void Start()
    {
        tr = GetComponent<Transform>();
        offset = tr.position - playerTr.position;
    }

    void FixedUpdate()
    {
        tr.position = playerTr.position + new Vector3(offset.x+ horizontal, offset.y+y, offset.z+ zoom);
        //타겟 추적
        tr.position = Vector3.Lerp(tr.position,
                playerTr.position - (playerTr.right * zoom) + (playerTr.up * vertical) + (playerTr.forward * horizontal),
               speed * Time.deltaTime);

        //타겟을 봐라봄(LookAt과 같음)
        Vector3 dir = playerTr.position - tr.position;
        Quaternion drot = Quaternion.LookRotation(dir);
        Quaternion rot = Quaternion.Slerp(tr.rotation, drot, Time.deltaTime * speed);
        transform.rotation = rot;
    }
}
