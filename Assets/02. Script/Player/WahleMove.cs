using UnityEngine;
using System.Collections;

public class WahleMove : WahleCtrl {

    private float focusDir = 1f;
    float changeTime = 0f;

    protected  IEnumerator Move()
    {
        while (true)
        {
            focusDir = Mathf.Sign(PlayerCtrl.inputAxis);
            relativePos = playerTr.position - transform.position;
            lookRot = Quaternion.LookRotation(relativePos);
            // 플레이어가 멈췄을  시
            if (PlayerCtrl.inputAxis == 0f)
            {
                WaitForMove();
            }
            // 플레이어가 이동중일 시
            else {
                initSpeed = IncrementSpeed(initSpeed, maxSpeed, accel);

                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                       lookRot, 5f * Time.deltaTime);

                if (initSpeed < 10f)
                    transform.Translate(0, 0, initSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.Lerp(transform.position, playerTr.position + (playerTr.forward) * focusDir,
                            3f * Time.deltaTime);
            }
            yield return null;
        }
    }

    void WaitForMove()
    {
        initSpeed = 0f;
        // 4초이상 움직임이 없을 시 대기 상태로 전환
        changeTime += Time.deltaTime;
        if (changeTime >= 4f)
        {
            changeTime = 0f;
            curState = Idle();
        }
        // 플레이어 주위를 선회
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            new Quaternion(lookRot.x, lookRot.y * focusDir, lookRot.z, lookRot.w), 1.5f * Time.deltaTime);
        transform.Translate(0, 0f, 2f * Time.deltaTime);
    }
}
