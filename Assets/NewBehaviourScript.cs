﻿using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    Transform tr;
	// Use this for initialization
	void Start () {
        tr = gameObject.transform;
	}

    // Update is called once per frame
    void Update()
    {
        ChackGround2();

    }

    GameObject oldGround = null;
    RaycastHit[] hits;
    RaycastHit hit;
    Vector3 groundPos;
    void ChackGround1()
    {
        int mask = 1 << LayerMask.NameToLayer("Floor");
        Debug.DrawRay(tr.position, -Vector3.up * 10, Color.yellow);
        if (Physics.Raycast(tr.position, - Vector3.up * 10, out hit))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.gameObject && hit.transform.CompareTag("Land"))
            {
                groundPos = hit.point;
                Debug.Log(hit.point  + "  " + hit.transform.name);

            }
        }
    }
    Ray ray;
    void ChackGround2()
    {
        Debug.DrawRay(tr.position, - Vector3.up * 10 , Color.yellow);
        hits = Physics.RaycastAll(tr.position, - Vector3.up, 10);

        Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Land"))
            {
                
            }
        }

    }


    
    
}
