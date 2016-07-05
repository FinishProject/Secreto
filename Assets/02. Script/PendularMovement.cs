using UnityEngine;
using System.Collections;

public class PendularMovement : MonoBehaviour {

    public float speed = 1.5f;
    public float moveLength = 5f;
    public float widhtLenth = 3f;

    private float angle = 0f;
    private float moveSpeed = 0f;
    private float upMoveSpeed = 0f;
    private float rotMoveSpeed = 0f;
    private float startTime = 0f;

    Quaternion qStart, qEnd;
    Vector3 vStart, vEnd;
    Vector3 targetPos;

    void Start()
    {
        //vStart = transform.position;
        vEnd = transform.position - (Vector3.forward * moveLength);

        angle = (Mathf.Atan2(vEnd.z, transform.position.z) * Mathf.Rad2Deg);

        qStart = Quaternion.AngleAxis(angle, Vector3.right);
        qEnd = Quaternion.AngleAxis(-angle, Vector3.right);

        targetPos = vEnd;
    }
    void Update()
    {
        startTime += Time.deltaTime;
        
        moveSpeed = (Mathf.Sin(startTime * speed) * moveLength);
        //upMoveSpeed = (Mathf.Sin(startTime) * widhtLenth);
        rotMoveSpeed = (Mathf.Sin(startTime * speed + Mathf.PI * 0.5f) + 1.0f) * 0.5f;

        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(qStart, qEnd, rotMoveSpeed);


        //moveSpeed = (Mathf.Sin(startTime * 1.5f) + 1.0f);
        //Vector3 center = (transform.position + vEnd) * 0.5f;
        //center += new Vector3(0, 1, 1);
        //transform.position = Vector3.Slerp(transform.position - center, targetPos - center,
        //    moveSpeed);
        //transform.position += center;



        //if (moveSpeed <= 0.2f && isChange)
        //{
        //    isChange = false;
        //    if (isRight)
        //    {
        //        targetPos = vStart;
        //        isRight = false;
        //    }
        //    else
        //    {
        //        targetPos = vEnd;
        //        isRight = true;
        //    }
        //}
        //else if(moveSpeed >= 0.7f && !isChange)
        //    isChange = true;
    }
}
