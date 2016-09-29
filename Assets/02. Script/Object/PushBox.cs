﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PushBox : MonoBehaviour {

    public float speed = 1f;
    private Vector3 moveDir = Vector3.zero;
    public Image shiftImg;

    bool isRight = true;
    bool isActive = false;

    public void PushObject(Transform playerTr, bool isDir)
    {
        isRight = isDir;
        if(!isActive)
            StartCoroutine(Push(playerTr));
        //moveDir = playerTr.forward;
        //transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            ShowUI.instanace.OnImage(1);
            //ShowUI.instanace.SetPosition(this.transform.position);
            Vector3 imgPosition = this.transform.position;
            imgPosition.y += 1f;
            shiftImg.transform.position = imgPosition;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            ShowUI.instanace.OnImage(-1);
        }
    }

    IEnumerator Push(Transform playerTr)
    {
        PlayerCtrl.instance.SetPushAnim(true);
        isActive = true;
        
        while (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyUp(KeyCode.LeftShift) || PlayerCtrl.inputAxis == 0f
                || transform.position.y < playerTr.position.y || isRight != PlayerCtrl.isFocusRight)
            {
                break;
            }

            moveDir = playerTr.forward;
            transform.position += moveDir * speed * Time.deltaTime;

            yield return null;
        }
        PlayerCtrl.instance.SetPushAnim(false);
        isActive = false;
    }
}
