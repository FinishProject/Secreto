using UnityEngine;
using System.Collections;

public class Balance : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 dir;

    public float power = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision coll)
    {
        power += Time.deltaTime;
        transform.Rotate(new Vector3(0f, 0f, 1f), power);
    }
}
