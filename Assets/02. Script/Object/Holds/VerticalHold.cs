using UnityEngine;
using System.Collections;

public class VerticalHold : MonoBehaviour {

    private Transform tr;
    private Transform playerTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f;
    public float length = 8f;

    Vector3 moveDir = Vector3.zero;

    void Start()
    {
        tr = GetComponent<Transform>();
        maxLengthPos.y = tr.position.y + length; //최대 이동 길이(상)
        originPos.y = tr.position.y; // 기본 이동 길이(하)
    }

    void FixedUpdate()
    {
        if (tr.position.y >= maxLengthPos.y && speed >= 1) { speed *= -1; }
        else if (tr.position.y <= originPos.y && speed <= -1) { speed *= -1; }
        moveDir = Vector3.up * speed;
        moveDir = transform.TransformDirection(moveDir);
        tr.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerStay(Collider coll)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (coll.gameObject.tag == "Player")
        {
            playerTr = coll.GetComponent<Transform>();
            playerTr.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        playerTr = null;
    }
}
