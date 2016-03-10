using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    void FullObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;

        Vector3 relativePos = wahle.position - transform.position;
        transform.Translate(relativePos.normalized * 10f * Time.deltaTime);
    }

    void FushObject()
    {
        Transform wahle = GameObject.FindWithTag("Wahle").transform;

        Vector3 relativePos = wahle.position - transform.position;
        transform.Translate(-relativePos.normalized * 10f * Time.deltaTime);
    }
}
