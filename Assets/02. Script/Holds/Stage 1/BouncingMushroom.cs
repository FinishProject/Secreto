using UnityEngine;
using System.Collections;

public class BouncingMushroom : MonoBehaviour {

    public float jumpHight = 10f;
    Vector3 moveDir = Vector3.zero;

    void OnTriggerEnter(Collider coll)
    {
        coll.GetComponent<PlayerCtrl>().moveDir.y = jumpHight;
        coll.GetComponent<PlayerCtrl>().controller.Move(moveDir * Time.deltaTime);
    }

}
