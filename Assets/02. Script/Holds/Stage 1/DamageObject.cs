﻿using UnityEngine;
using System.Collections;

public class DamageObject : MonoBehaviour {

    public float damage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("damaged");
            //col.GetComponent<PlayerCtrl>().getDamage(damage);
        }
    }
}
