using UnityEngine;
using System.Collections;

public class BulletObject_Parabola : MonoBehaviour {


    public void Moving(Vector3 startPos)
    {
        StartCoroutine(ParabolaMoving(startPos));
    }

    IEnumerator ParabolaMoving(Vector3 startPos)
    {
        transform.position = startPos;
        float angle = 70f;

        var targetPos = PlayerCtrl.instance.transform.position;
        targetPos.x += Random.Range(-1.5f, 1.5f);
        targetPos.y += PlayerCtrl.instance.transform.localScale.y;
        var dir = targetPos - transform.position;  // 방향 벡터
        var h = dir.y;                             // 높이 차이
        dir.y = -2;                                 // 수평방향만 유지 하기 위해 (높이 조절 가능)
        var dist = dir.magnitude;                  // 수평 거리
        var a = angle * Mathf.Deg2Rad;             // 각도를 라디안으로 변환
        dir.y = dist * Mathf.Tan(a);               // 방향을 앙각으로 맞춤( 지평면과 포신 방향이 이루는 각)
        dist += h / Mathf.Tan(a);                  // 정확한 높이 차이를 위해             
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)); // 속도를 계산
        var ballistic = vel * dir.normalized;

        while (true)
        {
            ballistic += Physics.gravity * Time.deltaTime;
            transform.position += ballistic * Time.deltaTime;
            yield return null;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
            PlayerCtrl.instance.getDamage(10);
        else if (col.tag.Equals("BULLET") || col.tag.Equals("MONSTER"))
            return;

        gameObject.SetActive(false);
    }
}
