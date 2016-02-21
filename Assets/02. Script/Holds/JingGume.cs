using UnityEngine;
using System.Collections;

public class JingGume : MonoBehaviour {

    private Renderer[] chRender;
    private Transform[] chTr;
    private Transform playerTr;

    private float distance;
    private int cnt = 0;

	void Start () {
        chRender = GetComponentsInChildren<Renderer>();
        chTr = GetComponentsInChildren<Transform>();
        //발판 렌더링 끄기
        for (int i = 1; i <= 2; i++)
            chRender[i].enabled = false;
	}
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerTr = col.gameObject.transform;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (playerTr != null)
        {
            //플레이어와 발판간의 거리 체크
            distance = Vector3.Distance(playerTr.position, chTr[cnt].position);
            if (distance <= 1.1f)
            {
                chRender[cnt].enabled = true;
                if (cnt < chRender.Length - 1)
                    cnt++;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        playerTr = null;
    }
}
