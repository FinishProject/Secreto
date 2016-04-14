using UnityEngine;
using System.Collections;

public class JingGume_Trigger : MonoBehaviour {
    
    public  GameObject[] gObjects = null;
    public  int[]        requisiteCnts = null;     // 필요 조건 (개수)
    private int          Cnt = 0;

    JingGume_Trigger parent;

    void Start()
    {
        //발판 끄기
        for (int i = 1; i < gObjects.Length; i++)
        {
            gObjects[i].SetActive(false);
        }
        if (transform.parent)
            parent = transform.parent.GetComponentInParent<JingGume_Trigger>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Cnt++;
            parent.OnHolds();
        }
    }

    void OnHolds()
    {
        for (int i = 1; i < gObjects.Length; i++)
        {
            if(requisiteCnts[i] <= gObjects[i-1].GetComponent<JingGume_Trigger>().Cnt)
            {
                gObjects[i].SetActive(true);
            }
        }
    }

}
