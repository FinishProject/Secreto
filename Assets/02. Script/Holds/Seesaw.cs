﻿using UnityEngine;
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
        Debug.Log("Push");
        dir = coll.ClosestPointOnBounds(coll.gameObject.transform.position);
        rb.AddForceAtPosition(-Vector3.up * power * Time.deltaTime, dir);
    }
}
