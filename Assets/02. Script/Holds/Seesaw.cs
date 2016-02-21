using UnityEngine;
using System.Collections;

public class Seesaw : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 dir;
    public float power = 10f;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnTriggerStay(Collider coll)
    {
        //플레이어와 접촉 되어 있는 부분에서 아래로 힘을 가함
        dir = coll.ClosestPointOnBounds(coll.gameObject.transform.position);
        rb.AddForceAtPosition(-Vector3.up * power * Time.deltaTime, dir);
    }
}
