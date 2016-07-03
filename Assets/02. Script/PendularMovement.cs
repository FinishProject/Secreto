using UnityEngine;
using System.Collections;

public class PendularMovement : MonoBehaviour {

    private float angle = 0f;
    public float speed = 1.5f;
    private float moveSpeed = 0f;
    private float startTime = 0f;
    private bool isRight = true;
    bool isChange = true;

    public bool activateR = true;
    public bool activateL;

    Quaternion qStart, qEnd;
    Vector3 vStart, vEnd;
    Vector3 targetPos;

    void Start()
    {

        vStart = transform.position;
        vEnd = transform.position + (Vector3.right * 5f);

        angle = (Mathf.Atan2(vEnd.x, vStart.x) * Mathf.Rad2Deg);

        qStart = Quaternion.AngleAxis(angle, Vector3.forward);
        qEnd = Quaternion.AngleAxis(-angle, Vector3.forward);

        targetPos = vEnd;
    }
    void Update()
    {
        startTime += Time.deltaTime;
        moveSpeed = (Mathf.Sin(startTime * 1.5f) + 1.0f);

        Vector3 center = (transform.position + vEnd) * 0.5f;
        center += new Vector3(0, 1, 1);
        transform.position = Vector3.Slerp(transform.position - center, targetPos - center,
            moveSpeed * Time.deltaTime);
        transform.position += center;

        //transform.rotation = Quaternion.Lerp(qStart, qEnd, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);

        if (moveSpeed <= 0.2f && isChange)
        {
            isChange = false;
            if (isRight)
            {
                targetPos = vStart;
                isRight = false;
            }
            else
            {
                targetPos = vEnd;
                isRight = true;
            }
        }
        else if(moveSpeed >= 0.7f && !isChange)
            isChange = true;
    }
}
