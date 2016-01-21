using UnityEngine;
using System.Collections;

public class HorizonHold : MonoBehaviour {

    private Transform tr;
    private Transform playerTr;
    private Vector3 rightPos, leftPos;

    public float speed = 3f;
    public float length = 8f;
    
	void Start () {
        tr = GetComponent<Transform>();
        rightPos.x = tr.position.x + length;
        leftPos.x = tr.position.x;
    }
	
	void Update () {
        if(tr.position.x >= rightPos.x && speed >= 1) { speed *= -1;}
        else if (tr.position.x <= leftPos.x && speed <= -1) { speed *= -1;}

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
