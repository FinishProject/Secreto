﻿using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float speed; // 이동 속도
    public float maxLength = 10f; // 마지막 위치 길이
    public float waitTime = 3f; // 목표 타겟을 변경하기 전 대기시간

    private Vector3 pMoveVector;
    private bool isActive = false; // 작동 여부
    private bool isStep = false; // 플레이어가 밟고 있는지 여부
    private Vector3 originPos, finishPos; // 시작 위치, 최종 위치
    private Vector3 targetPos; // 이동할 목표 위치
    private Transform playerTr; // 플레이어, 올라 트랜스폼

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        // 시작 위치 및 최종 위치 구함
        originPos = this.transform.position;
        finishPos = new Vector3(originPos.x, originPos.y + maxLength, originPos.z);

        targetPos = finishPos;
        pMoveVector = Vector3.up;
    }

    IEnumerator OnActive()
    {
        while (true)
        {
            // 목표지점까지 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // 목표지점 도착
            if (transform.position.y.Equals(finishPos.y))
            {
                // 목표 위치 도착 후 캐릭터가 아직 머물고 있을 시
                if (isStep)
                    CameraCtrl_4.instance.ChangeCamSpeed(0f);
                // 캐릭터가 발판을 떠났을 시
                else if (!isStep)
                    StartCoroutine(ChangeTargetPos());
            }
            // 초기 위치로 돌아왔을 시 코루틴 종료
            else if (transform.position.y.Equals(originPos.y))
            {
                isActive = false;
                pMoveVector = Vector3.up;
                break;
            }
            // 플레이어가 발판 위에 있을 시 플레이어 이동
            else if (isStep)
            {
                PlayerCtrl.instance.transform.Translate(pMoveVector * speed * Time.deltaTime);
            }

            yield return null;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !isActive)
        {
            CameraCtrl_4.instance.ChangeCamSpeed(100f);
            isActive = true;
            targetPos = finishPos;
            WahleCtrl.curState = WahleCtrl.instance.StepHold();
            StartCoroutine(OnActive());
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isStep = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        CameraCtrl_4.instance.ResetCameSpeed();
        WahleCtrl.instance.ChangeState(WahleState.MOVE);
        isStep = false;
    }

    // 플레이어 벗어난 후 초기 위치로 돌아가기 위한 카운트 다운
    IEnumerator ChangeTargetPos()
    {
        yield return new WaitForSeconds(waitTime);
        pMoveVector = Vector3.down;
        targetPos = originPos;
    }
}
