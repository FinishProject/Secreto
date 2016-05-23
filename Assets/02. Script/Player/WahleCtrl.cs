﻿using UnityEngine;
using System.Collections;

public enum WahleState { IDLE, MOVE, WALL, ATTACK };

public class WahleCtrl : MonoBehaviour {

    private float initSpeed = 0f; // 초기 이동속도
    public float maxSpeed; // 최대 이동속도
    public float accel; // 가속도

    private float distance = 0f; // 플레이어와 고래의 거리 차이
    private bool isFocusRight = true; // 오른쪽을 봐라보는지 확인
    private bool isWait = false; // 코루틴 실행 중인지 확인
    private float focusDir = 1f; // 현재 봐라보고 있는 방향 변수
    private float delTime = 0f; // 누적 되는 deltaTime 값 저장
    private float sinSpeed = 0f; // sin함수로 생성되는 값 저장
    private int stateValue = 0; // 상태 값

    public Transform playerTr; // 플레이어 위치
    private  Transform targetTr; // 적 위치값
    private Vector3 relativePos;
    private Quaternion lookRot;

    public static WahleCtrl instance;

    void Start()
    {
        instance = this;
        StartCoroutine(Idel());
    }
    
    IEnumerator Idel()
    {
        initSpeed = 0f;

        float[] value = { 0, 100 };
        stateValue = GetRandomValue(value);

        while (true)
        {
            if (distance > 6f && PlayerCtrl.inputAxis != 0f)
            {
                StartCoroutine(Move());
                break;
            }

            if (!isWait)
                StartCoroutine(WaitRandom(3f, value));

            relativePos = playerTr.position - transform.position;
            distance = relativePos.sqrMagnitude;
            lookRot = Quaternion.LookRotation(relativePos);

            switch (stateValue)
            {
                case 0: // 플레이어 주위를 선회
                    sinSpeed = Mathf.Sin(delTime += Time.deltaTime);

                    transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, Time.deltaTime);
                    transform.Translate(0, (sinSpeed * 0.5f) * Time.deltaTime, 3f * Time.deltaTime);
                    break;
                case 1: // 360도 회전   
                    transform.RotateAround(transform.position, Vector3.forward, 100f * Time.deltaTime);
                    transform.Translate(new Vector3(0f, 2f * Time.deltaTime, 2f * Time.deltaTime));
                    break;
            }

            yield return null;
        }
    }

    // 이동
    IEnumerator Move()
    {
        float[] value = { 60, 40 };
        float speed = 1f;
        while (true)
        {
            if (distance <= 6f && PlayerCtrl.inputAxis == 0f)
            {
                StartCoroutine(Idel());
                break;
            }

            relativePos = (playerTr.position - transform.position); // 두 객체간의 거리 차
            distance = relativePos.sqrMagnitude; // 거리 차
            lookRot = Quaternion.LookRotation(relativePos);

            // 캐릭터 회전시 겹쳐짐을 방지하기 위해 좌우 방향에 따른 양수 또는 음수를 줌
            focusDir = Mathf.Sign(PlayerCtrl.inputAxis);
            // -1 ~ 1까지 반복 되는 수
            sinSpeed = Mathf.Sin(delTime += Time.deltaTime);

            // 이동속도 증가
            initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);

            // 캐릭터를 향해 회전
            if (initSpeed < maxSpeed)
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
            // 선형 보간을 사용한 캐릭터 추격
            transform.position = Vector3.Lerp(transform.position, playerTr.position - (playerTr.forward * focusDir),
                initSpeed * Time.deltaTime);

            if (initSpeed >= maxSpeed)
            {
                if (speed <= 5f)
                    speed += (Time.deltaTime * 2f);
                transform.Translate(new Vector3(0f, (sinSpeed * speed) * Time.deltaTime, 0f));
                transform.Translate(new Vector3(0f, 0f, (sinSpeed * speed * 2f) * Time.deltaTime));
            }

            //float rotValue += transform.rotation.y;
            //if (rotValue <= -3f)
            //{
            //transform.RotateAround(transform.position, Vector3.right * sinSpeed, 50f * Time.deltaTime);
            //}

            yield return null;
        }
    }


    //// 공격
    //void Attack()
    //{
    //    if (targetTr != null)
    //    {
    //        float dis = (playerTr.position - transform.position).sqrMagnitude;
    //        if (dis >= 5f)
    //        {
    //            SetState(WahleState.MOVE);
    //        }

    //        relativePos = (targetTr.position - transform.position);
    //        lookRot = Quaternion.LookRotation(relativePos);
    //        focusDir = Mathf.Sign(targetTr.position.x - transform.position.x);

    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 5f * Time.deltaTime);
    //        transform.position = Vector3.Lerp(transform.position, targetTr.position -
    //            ((targetTr.forward * -focusDir * 3f) - (targetTr.up)), 7f * Time.deltaTime);
    //    }
    //    else
    //    {
    //        SetState(WahleState.IDLE);
    //    }
    //}

    // 주변 몬스터 탐색
    IEnumerator SearchEnemy()
    {
        while (true)
        {
            Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 5f);
            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (hitCollider[i].CompareTag("MONSTER"))
                {
                    targetTr = hitCollider[i].transform;
                    
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator WaitRandom(float waitTime, float[] value)
    {
        isWait = true;
        yield return new WaitForSeconds(waitTime);
        stateValue = GetRandomValue(value);
        isWait = false;
    }


    // 랜덤 값 생성
    int GetRandomValue(float[] values)
    {
        float total = 0;

        for (int i = 0; i < values.Length; i++) {
            total += values[i];
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < values.Length; i++)
        {
            if (randomPoint < values[i]) {
                return i;
            }
            else {
                randomPoint -= values[i];
            }
        }
        return values.Length - 1;
    }

    // 이동 속도 증가
    float IncrementSpeed(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed == maxSpeed)
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
            return (1 == Mathf.Sign(maxSpeed - initSpeed)) ? initSpeed : maxSpeed;
        }
    }
}