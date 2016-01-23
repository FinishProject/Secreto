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
        maxLengthPos.y = tr.position.y + length;
        originPos.y = tr.position.y;
    }

    void Update()
    {
        if (tr.position.y >= maxLengthPos.y && speed >= 1) { speed *= -1; }
        else if (tr.position.y <= originPos.y && speed <= -1) { speed *= -1; }
        moveDir = Vector3.up * speed;
        moveDir = transform.TransformDirection(moveDir);
        tr.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    void OnTriggerStay(Collider coll)
    {
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
