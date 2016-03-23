using UnityEngine;
using System.Collections;

public class GrabObject : MonoBehaviour {

    private Transform playerTr;

	void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                playerTr = col.GetComponent<Transform>();
                Vector3 relativePos = playerTr.position - transform.position;
                transform.position = Vector3.Lerp(transform.position, 
                    new Vector3(playerTr.position.x + 1f, playerTr.position.y + 3f, 0f),
                    50f * Time.deltaTime);
            }
        }
    }
}
