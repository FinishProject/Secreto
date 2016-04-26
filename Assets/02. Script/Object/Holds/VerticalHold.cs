using UnityEngine;
using System.Collections;

public class VerticalHold : MonoBehaviour {

    private Transform playerTr, olaTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f;
    public float length = 8f;

    Vector3 moveDir = Vector3.zero;

    void Start()
    {
        olaTr = GameObject.FindGameObjectWithTag("WAHLE").transform;
        maxLengthPos.y = transform.position.y + length; //최대 이동 길이(상)
        originPos.y = transform.position.y; // 기본 이동 길이(하)
    }

    void FixedUpdate()
    {
        if (transform.position.y >= maxLengthPos.y && speed >= 1) { speed *= -1; }
        else if (transform.position.y <= originPos.y && speed <= -1) { speed *= -1; }
        moveDir = Vector3.up * speed;
        moveDir = transform.TransformDirection(moveDir);
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerStay(Collider coll)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (coll.CompareTag("Player"))
        {
            playerTr = coll.GetComponent<Transform>();
            playerTr.Translate(Vector3.up * speed * Time.deltaTime);
            olaTr.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        playerTr = null;
    }
}
