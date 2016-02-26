using UnityEngine;
using System.Collections;

public class Seesaw : MonoBehaviour {

    private Rigidbody rb;
    public float power = 10f;
    public float slope = 0.09f;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnTriggerStay(Collider coll)
    {
        if (transform.rotation.y <= slope)
        {
            rb.constraints = RigidbodyConstraints.None;
            //플레이어와 접촉 되어 있는 부분에서 아래로 힘을 가함
            Vector3 dir = coll.ClosestPointOnBounds(coll.gameObject.transform.position);
            rb.AddForceAtPosition(-Vector3.up * power * Time.deltaTime, dir);
        }
        else { rb.constraints = RigidbodyConstraints.FreezeRotation; }
    }
    void OnTriggerExit(Collider coll)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
