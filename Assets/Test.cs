using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
    {
        //print(coll.bounds);
        print(coll.contactOffset);
    }
}
