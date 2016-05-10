using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public Transform playerTr; // 플레이어의 위치값
    public float speed = 10f; // 카메라의 속도

    private Vector3[] relCameraPos = new Vector3[2];
    Vector3[] standardPos = new Vector3[2];

    private int index = 0;

    void Start()
    {
        relCameraPos[0] = transform.position - playerTr.position;
        //relCameraPos[1] = transform.position - whaleTr.position;
    }

    void FixedUpdate()
    {
        standardPos[0] = playerTr.position + relCameraPos[0];
        transform.position = Vector3.Lerp(transform.position, standardPos[index], speed * Time.deltaTime);

    }
}
