using UnityEngine;
using System.Collections;

public class JingGume : MonoBehaviour {

    public GameObject[] gObject;
    private int cnt = 1;
    private bool isStep = false; // 밟은 여부

    private JingGume parent;

    void Start () {
        //발판 오브젝트 끄기
        for (int i = 1; i < gObject.Length; i++)
        {
            gObject[i].SetActive(false);
        }
        if(transform.parent)
            parent = transform.parent.GetComponentInParent<JingGume>();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !isStep)
        {
            Debug.Log("11");
            this.isStep = true;
            parent.OnHolds();
        }
    }

    void OnHolds()
    {
        if(cnt < gObject.Length)
            gObject[cnt++].SetActive(true);
    }
}
