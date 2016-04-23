using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "BULLET")
        {
            gameObject.SetActive(false);
        }
    }
}
