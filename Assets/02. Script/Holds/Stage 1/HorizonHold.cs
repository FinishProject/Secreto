using UnityEngine;
using System.Collections;

public class HorizonHold : MonoBehaviour {

    private Transform tr;
    private Transform playerTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f;
    public float length = 8f; // 발판이 이동할 길이
    
	void Start () {
        tr = GetComponent<Transform>();
        maxLengthPos.x = tr.position.x + length; //최대 이동 길이(우측)
        originPos.x = tr.position.x; //초기 이동 길이(좌측)
    }
	
	void FixedUpdate () {
        if(tr.position.x >= maxLengthPos.x && speed >= 1) { speed *= -1;}
        else if (tr.position.x <= originPos.x && speed <= -1) { speed *= -1;}

        tr.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider coll)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (coll.gameObject.tag == "Player")
        {
            playerTr = coll.GetComponent<Transform>();
            playerTr.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    //void OnTriggerExit(Collider coll)
    //{
    //    playerTr = null;
    //}
}
