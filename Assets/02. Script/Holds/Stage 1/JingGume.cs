using UnityEngine;
using System.Collections;

public class JingGume : MonoBehaviour {

    public GameObject[] gObject;
    private int cnt = 1;

    JingGume parent;

    void Start () {
        //발판 끄기
        for (int i = 1; i < gObject.Length; i++)
        {
            gObject[i].SetActive(false);
        }
        parent = transform.parent.GetComponentInParent<JingGume>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            parent.OnHolds();
        }
    }

    void OnHolds()
    {
        if(cnt < gObject.Length)
            gObject[cnt++].SetActive(true);
    }
}
