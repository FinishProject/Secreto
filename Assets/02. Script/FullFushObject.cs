using UnityEngine;
using System.Collections;

public class FullFushObject : MonoBehaviour {

    private float speed = 50f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FullObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;
        //rb.isKinematic = true;
        Vector3 relativePos = wahle.position - transform.position;
        transform.position = Vector3.Lerp(transform.position, wahle.position, speed * Time.deltaTime);
        //transform.Translate(relativePos.normalized * 20f * Time.deltaTime);
    }

    void FushObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;
        Vector3 relativePos = wahle.position - transform.position;
        //transform.position = Vector3.Lerp(transform.position, -wahle.position, speed * Time.deltaTime);
        transform.Translate(new Vector3(-relativePos.normalized.x * 20f * Time.deltaTime, -relativePos.normalized.y * 20f * Time.deltaTime, 0f));
    }
}
