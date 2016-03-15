using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomHolds : MonoBehaviour {

    public GameObject holds;

    private Transform[] points = new Transform[5];
    private List<GameObject> gaHold = new List<GameObject>();
    private List<int> beforeNum = new List<int>();

    private int index = 0;
    public int spawnHoldNum = 3;

    void Start () {
        points = GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < 3; i++) {
            gaHold.Add((GameObject)Instantiate(holds, transform.position, Quaternion.identity));
            gaHold[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            Init();
            StartCoroutine("SpwanHolds");
        }
    }

    IEnumerator SpwanHolds()
    {
        int rndNum = 0;
        while(beforeNum.Count < spawnHoldNum)
        {
            rndNum = Random.Range(1, points.Length - 1);
            
            if (beforeNum.Contains(rndNum)) { rndNum = Random.Range(1, points.Length - 1); }
            else {
                beforeNum.Add(rndNum);
                gaHold[index].SetActive(true);
                gaHold[index].transform.position = points[rndNum].position;
                index++;
            }
            yield return null;
        }
    }

    void Init()
    {
        for(int i=0; i < gaHold.Count; i++) {
            gaHold[i].SetActive(false);
        }
        index = 0;
        beforeNum.RemoveRange(0, beforeNum.Count);
    }
}
