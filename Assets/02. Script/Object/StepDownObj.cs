using UnityEngine;
using System.Collections;

public class StepDownObj : MonoBehaviour {

    public float downLenth = 0.3f;
    public float speed = 4f;

    private bool isActive = false;

    private Vector3 targetPos, originPos;

    void Start()
    {
        originPos = transform.position;
        targetPos = new Vector3(originPos.x, originPos.y - downLenth, originPos.z);
    }

    void OnTriggerEnter(Collider col)
    {
        isActive = false;
    }

    void OnTriggerStay(Collider col)
    {
        //StopCoroutine(BackOriginPos());
        // 목표 위치까지 아래로 내려감
        if (col.CompareTag("Player") && !transform.position.y.Equals(targetPos.y)){
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("Player"))
            StartCoroutine(BackOriginPos());
    }

    // 초기 위치로 돌아감
    IEnumerator BackOriginPos()
    {
        while (!transform.position.y.Equals(originPos.y))
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, speed * Time.deltaTime);

            yield return null;
        }
    }
}
