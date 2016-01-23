using UnityEngine;
using System.Collections;

public class HorizonHold : MonoBehaviour {

    private Transform tr;
    private Transform playerTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f;
    public float length = 8f;
    
	void Start () {
        tr = GetComponent<Transform>();
        maxLengthPos.x = tr.position.x + length;
        originPos.x = tr.position.x;
    }
	
	void Update () {
        if(tr.position.x >= maxLengthPos.x && speed >= 1) { speed *= -1;}
        else if (tr.position.x <= originPos.x && speed <= -1) { speed *= -1;}

        tr.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerTr = coll.GetComponent<Transform>();
            playerTr.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        playerTr = null;
    }
}
