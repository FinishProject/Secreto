using UnityEngine;
using System.Collections;

public class BouncingMushroom : MonoBehaviour {

    public float jumpHight = 10f;
    Vector3 moveDir = Vector3.zero;

    void OnTriggerEnter(Collider coll)
    {
        PlayerCtrl.moveDir.y = jumpHight;
        PlayerCtrl.controller.Move(moveDir * 10f * Time.deltaTime);
    }

}
