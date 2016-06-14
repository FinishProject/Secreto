using UnityEngine;
using System.Collections;

public class WahleIdle : WahleCtrl
{

    public static WahleIdle instance = new WahleIdle();

    // 대기
    public IEnumerator Idle()
    {
        anim.SetBool("Move", true);
        SetRandomPos();

        while (true)
        {
            ShotRay();
            // 플레이어가 움직이거나 고래가 화면 밖으로 나갈 시 이동으로 전환
            if (PlayerCtrl.inputAxis >= 1f || PlayerCtrl.inputAxis <= -1f)
            {
                //if (CheckOutCamera())
                //    OnUpate = WahleMove.instance.Move;
                // ChangeSate(WahleMove.instance.Move());
            }
            // 타겟 위치를 임의의 위치로 배치
            if (distance <= 4f)
            {
                targetPoint.position = SetRandomPos();
            }

            relativePos = targetPoint.position - transform.position;
            distance = relativePos.sqrMagnitude;
            lookRot = Quaternion.LookRotation(relativePos);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 0.5f * Time.deltaTime);
            transform.Translate(0f, 0f, 1f * Time.deltaTime);

            yield return null;
        }
    }
}
