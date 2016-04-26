using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public Transform targetTr;
    public float speed = 40f;

    void Update()
    {

        transform.RotateAround(targetTr.position, Vector3.right, speed * Time.deltaTime);
    }
}
