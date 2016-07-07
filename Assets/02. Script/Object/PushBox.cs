﻿using UnityEngine;
using System.Collections;

public class PushBox : MonoBehaviour {

    public float speed = 1f;

    private Vector3 moveDir = Vector3.zero;

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerCtrl.instance.SetPushAnim(true);
            moveDir = col.transform.forward;
            transform.position += moveDir * speed * Time.deltaTime;
        }
        //else if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    PlayerCtrl.instance.SetPushAnim(false);
        //}
    }

   void OnTriggerExit(Collider col)
    {
        PlayerCtrl.instance.SetPushAnim(false);
    }
}