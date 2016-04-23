using UnityEngine;
using System.Collections;

public class TeleportGate : MonoBehaviour {

    public GameObject exitGate; // 출구 오브젝트

    private bool isTravel = true; // 이동 가능 여부
    private TeleportGate telepGate; // 출구 오브젝트의 스크립트를 담을 변수
    private Transform olaTr, camTr;

    void Start()
    {
        olaTr = GameObject.FindGameObjectWithTag("WAHLE").transform;
        camTr = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

	void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Player") && isTravel)
        {
            // 반대편에 도착 시 잠시 이동 불가능하게 만듬
            telepGate = exitGate.GetComponent<TeleportGate>();
            telepGate.Block();

            // 출구 위치로 이동
            coll.transform.position = exitGate.transform.position;
            camTr.position = new Vector3(exitGate.transform.position.x, exitGate.transform.position.y, camTr.position.z);
            olaTr.position = exitGate.transform.position; 
        }
    }

    public void Block()
    {
        StartCoroutine(BlockTravel());
    }

    // 잠시 이동 이동 불가능하게 만듬
    IEnumerator BlockTravel()
    {
        isTravel = false;
        yield return new WaitForSeconds(2f);
        isTravel = true;
        StopCoroutine(BlockTravel());
    }
}
