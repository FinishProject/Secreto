using UnityEngine;
using System.Collections;

public class FollowPoint : MonoBehaviour {

    public Transform pointTr;

    private Vector3 relativePos;
    private Quaternion lookRot;

    void Start()
    {
        relativePos = pointTr.position - transform.position;
    }

    void Update()
    {
        relativePos = pointTr.position - transform.position;
        lookRot = Quaternion.LookRotation(relativePos);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, relativePos, 3f * Time.deltaTime);
    }
}
