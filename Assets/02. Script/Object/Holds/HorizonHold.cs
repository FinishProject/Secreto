﻿using UnityEngine;
using System.Collections;

public class HorizonHold : MonoBehaviour
{
    private Transform playerTr, olaTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f; // 발판 이동 속도
    private float pSpeed; // 플레이어 이동 속도
    public float length = 8f; // 발판이 이동할 길이
    private bool isFocus; // 플레이어가 왼쪽을 봐라보는지
    float focusDir = 1f;
    private Vector3 targetPos;

    void Start()
    {
        olaTr = GameObject.FindGameObjectWithTag("WAHLE").transform;
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        originPos = this.transform.position; //초기 이동 길이(좌측)
        maxLengthPos = originPos;
        maxLengthPos.x = originPos.x + length; //최대 이동 길이(우측)
        
        targetPos = maxLengthPos;

        pSpeed = speed;
    }

    void FixedUpdate()
    {
        //최대 위치 또는 초기 위치 도착시 방향 전환


        //if (transform.position.x >= maxLengthPos.x - 0.2f)
        //{
        //    targetPos = originPos;
        //}
        //else if (transform.position.x <= targetPos.x + 0.2f) {
        //    targetPos = maxLengthPos;
        //}

        //transform.position = Vector3.Lerp(transform.position, 
        //    new Vector3(targetPos.x * focusDir, targetPos.y, targetPos.z), Time.deltaTime);


        // 최대 위치 또는 초기 위치 도착시 방향 전환
        if (transform.position.x >= maxLengthPos.x && speed >= 1) { speed *= -1; }
        else if (transform.position.x <= originPos.x && speed <= -1) { speed *= -1; }

        transform.Translate(Vector3.right * -speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider col)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (col.CompareTag("Player"))
        {
            WahleCtrl.curState = WahleCtrl.instance.StepHold();
            playerTr.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        WahleCtrl.curState = WahleCtrl.instance.Move();
    }
}