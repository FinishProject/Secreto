using UnityEngine;
using System.Collections;

public class Seesaw : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 dir;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnTriggerStay(Collider coll)
    {
        
        dir = coll.ClosestPointOnBounds(coll.gameObject.transform.position);
        rb.AddForceAtPosition(-Vector3.up * 10f * Time.deltaTime, dir);
    }
}
