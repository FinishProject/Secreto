using UnityEngine;
using System.Collections;

public class HorizonHold : MonoBehaviour
{

    private Transform playerTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f; // 발판 이동 속도
    private float pSpeed = 3f; // 플레이어 이동 속도
    public float length = 8f; // 발판이 이동할 길이
    private bool isFocus; // 플레이어가 왼쪽을 봐라보는지

    void Start()
    {
        maxLengthPos.x = transform.position.x + length; //최대 이동 길이(우측)
        originPos.x = transform.position.x; //초기 이동 길이(좌측)
    }

    void FixedUpdate()
    {
        //최대 위치 또는 초기 위치 도착시 방향 전환
        if (transform.position.x >= maxLengthPos.x && speed >= 1) { speed *= -1; }
        else if (transform.position.x <= originPos.x && speed <= -1) { speed *= -1; }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider coll)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (coll.gameObject.tag == "Player")
        {
            playerTr = coll.GetComponent<Transform>();
            isFocus = PlayerCtrl.isFocusRight;
            pSpeed = speed;
            if (!isFocus) { pSpeed *= -1; }
            //발판 위에 있을 시 플레이어 이동
            playerTr.Translate(Vector3.forward * pSpeed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        playerTr = null;
    }
}