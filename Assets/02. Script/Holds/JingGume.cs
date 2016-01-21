using UnityEngine;
using System.Collections;

public class JingGume : MonoBehaviour {

    private Renderer[] chRender;
    private Transform[] chTr;
    private float distance;

    public Transform playerTr;

    private int cnt = 0;
    

	void Start () {
        chRender = GetComponentsInChildren<Renderer>();
        chTr = GetComponentsInChildren<Transform>();
        for (int i = 1; i <= 2; i++)
            chRender[i].enabled = false;
	}
	
	void Update () {
        distance = Vector3.Distance(playerTr.position, chTr[cnt].position);
        if(distance <= 1.1f) {
            chRender[cnt].enabled = true;
            if(cnt < chRender.Length-1)
                cnt++;
        }
    }
}
