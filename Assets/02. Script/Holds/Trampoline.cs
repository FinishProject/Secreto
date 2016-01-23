using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

    public float jumpSpeed = 10f;
    private Transform tr;
    Vector3 moveDir = Vector3.zero;

    void OnTriggerEnter(Collider coll)
    {
        coll.GetComponent<PlayerCtrl>().moveDir.y = jumpSpeed;
        coll.GetComponent<PlayerCtrl>().controller.Move(moveDir * Time.deltaTime);
    }

}
