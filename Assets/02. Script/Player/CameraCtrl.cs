using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public Transform playerTr; // 플레이어의 위치값
    public Transform whaleTr;
    
    private Vector3[] relCameraPos = new Vector3[2];
    private Vector3[] standardPos = new Vector3[2];

    public float speed = 10f; // 카메라의 속도
    private int index = 0;

    void Awake()
    {
        relCameraPos[0] = transform.position - playerTr.position; // 플레이어
        relCameraPos[1] = transform.position - whaleTr.position; // 고래
    }

    void FixedUpdate()
    {
        if (WahleCtrl.isChange) { index = 0; }
        else { index = 1; }

        GetTarget();
        transform.position = Vector3.Lerp(transform.position, standardPos[index], speed * Time.deltaTime);
    }

    void GetTarget()
    {
        switch (index)
        {
            case 0:
                standardPos[0] = playerTr.position + relCameraPos[0];
                break;
            case 1:
                standardPos[1] = whaleTr.position + relCameraPos[1];
                break;
        }
    }
}
