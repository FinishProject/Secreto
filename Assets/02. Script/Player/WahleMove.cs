using UnityEngine;
using System.Collections;

public class WahleMove : WahleCtrl
{
    public static WahleMove instance = new WahleMove();


    public void Move()
    {
        initSpeed = 0f;
        float changeTime = 0f; // 대기로 전환 전 대기 시간
        float focusDir = 1f; // 봐라보고 있는 방향 오른쪽 : 1, 왼쪽 : -1
        Debug.Log("11");
        //while (true)
        //{
            focusDir = Mathf.Sign(PlayerCtrl.inputAxis);
            relativePos = playerTr.position - transform.position;
            lookRot = Quaternion.LookRotation(relativePos);
            distance = relativePos.sqrMagnitude;

            // 플레이어와의 거리가 벌어졌을 시 최대 속도로 전환
            if (distance >= 25f)
            {
                initSpeed = 10f;
            }

            // 플레이어가 멈췄을 시
            if (PlayerCtrl.inputAxis == 0f && distance <= 3f)
            {
                // 플레이어 주위를 선회
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 1.5f * Time.deltaTime);
                transform.Translate(Vector3.forward * initSpeed * Time.deltaTime);

                // 4초이상 움직임이 없을 시 대기 상태로 전환
                changeTime += Time.deltaTime;
                if (changeTime >= 4f)
                {
                    initSpeed = 0f;
                    
                    //curState = Idle();
                }
            }
            // 플레이어가 이동중일 시
            else {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                       lookRot, 5f * Time.deltaTime);

                // 최고 속도에 도달했을 시
                if (initSpeed < 10f)
                    transform.Translate(Vector3.forward * initSpeed * Time.deltaTime);
                else {
                    // 이동속도 증가
                    initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);
                    transform.position = Vector3.Lerp(transform.position, playerTr.position + (playerTr.forward * focusDir),
                            3f * Time.deltaTime);
                }
            }
            //yield return null;
        //}
    }
}