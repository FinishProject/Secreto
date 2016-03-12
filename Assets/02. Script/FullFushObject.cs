using UnityEngine;
using System.Collections;

public class FullFushObject : MonoBehaviour {

    private float speed = 20f;

    void FullObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;

        Vector3 relativePos = wahle.position - transform.position;
        transform.position = Vector3.Lerp(transform.position, wahle.position, speed * Time.deltaTime);
        //transform.Translate(relativePos.normalized * 20f * Time.deltaTime);
    }

    void FushObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;

        Vector3 relativePos = wahle.position - transform.position;
        transform.position = Vector3.Lerp(transform.position, -wahle.position, speed * Time.deltaTime);
        //transform.Translate(-relativePos.normalized * 20f * Time.deltaTime);
    }
}
