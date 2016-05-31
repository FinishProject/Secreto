using UnityEngine;
using System.Collections;

public class LanaCtrl : MonoBehaviour {

    private float diatance = 0f;

    public Transform target; // 포물선을 그리며 이동시 도착 지점
    private Animator anim;
    private Transform playerTr;
   
    void Start () {
        anim = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(CheckDistance());
	}
    // 플레이어와의 거리 체크
    IEnumerator CheckDistance()
    {
        while (true)
        {
            diatance = (playerTr.position - transform.position).sqrMagnitude;
            // 움직임 시작
            if (diatance <= 55f)
            {
                StartCoroutine(MoveUpdate());
                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
    // 움직임을 위한 코루틴
    IEnumerator MoveUpdate()
    {
        while (true)
        {
            if (diatance <= 55f)
                AppearNpc();

            yield return null;
        }
    }

    // 포물선을 그리며 플레이어 앞으로 등장 함수
    void AppearNpc()
    {
        anim.SetBool("Appear", true);

        Vector3 center = (target.position + this.transform.position) * 0.5f;
        center -= new Vector3(0, 1, 1);
        Vector3 fromRelCenter = this.transform.position - center;
        Vector3 toRelCenter = target.position - center;
        transform.position = Vector3.Slerp(fromRelCenter, toRelCenter, 3f * Time.deltaTime);
        transform.position += center;

        Vector3 lookTarget = new Vector3(0f, playerTr.position.y, playerTr.position.z);

        transform.LookAt(lookTarget);
    }
}
