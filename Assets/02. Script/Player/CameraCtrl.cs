using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    public Transform playerTr; // 플레이어의 위치값
    public float speed = 10f; // 카메라의 속도

    private Vector3[] relCameraPos = new Vector3[2];
    private Vector3[] standardPos = new Vector3[2];

    private int index = 0;
    Vector3 curVec;
    bool focusRight = true;

    void Awake()
    {
        relCameraPos[0] = transform.position - playerTr.position;
        curVec = playerTr.position;
    }

    void FixedUpdate()
    {
        standardPos[0] = FocusPlayerVec() + relCameraPos[0];
        //standardPos[0] = playerTr.position + relCameraPos[0];
        transform.position = Vector3.Lerp(transform.position, standardPos[index], speed * Time.deltaTime);
    }

    Vector3 FocusPlayerVec()
    {
        float focusDir = PlayerCtrl.inputAxis;
        if (focusDir >= 0.1f)
        {
            curVec = playerTr.position;
            return curVec;
        }
        else if (focusDir <= -0.8f)
        {
            curVec = new Vector3(playerTr.position.x - 3f, playerTr.position.y, playerTr.position.z);
            return curVec;
        }
        else {
            return curVec;
        }
    }
}
