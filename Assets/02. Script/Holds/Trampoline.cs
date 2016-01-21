using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

    public float jumpSpeed = 10f;

    void OnTriggerEnter(Collider coll)
    {
        coll.GetComponent<PlayerCtrl>().moveDir.y = jumpSpeed;
    }

}
