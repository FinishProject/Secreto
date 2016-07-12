using UnityEngine;
using System.Collections;

public class StepDownObj : MonoBehaviour {

    public float downLenth = 0.3f;
    public float speed = 4f;

    private bool isActive = true;

    private Vector3 targetPos, originPos;

    void Start()
    {
        originPos = transform.position;
        targetPos = new Vector3(originPos.x, originPos.y - downLenth, originPos.z);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isActive = true;
            StartCoroutine(MoveDown());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isActive = false;
            StartCoroutine(BackOriginPos());
        }
    }

    // 아래로 이동
    IEnumerator MoveDown()
    {
        while (transform.position.y >= (targetPos.y + 0.1f) && isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                targetPos, speed * Time.deltaTime);

            yield return null;
        }
    }

    // 초기 위치로 돌아감
    IEnumerator BackOriginPos()
    {
        while (transform.position.y <= (originPos.y - 0.1f) && !isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                originPos, speed * Time.deltaTime);

            yield return null;
        }
    }
}
