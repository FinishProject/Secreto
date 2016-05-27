using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
    //public bool isType = true; // 아래에서 위 또는 위에서 아래로 가는지 체크
    public float speed; // 이동 속도
    public float maxLength = 10f; // 마지막 위치 길이
    public float waitTime = 3f;

    private bool isActive = false; // 작동 여부
    private bool isStep = false; // 플레이어가 밟고 있는지 여부
    private Vector3 originPos, finishPos; // 시작 위치, 최종 위치
    private Vector3 targetPos;
    private Transform playerTr; // 플레이어, 올라 트랜스폼

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        // 시작 위치 및 최종 위치 구함
        originPos = this.transform.position;
        finishPos = new Vector3(originPos.x, originPos.y + maxLength, originPos.z);

        targetPos = finishPos;    
    }

    IEnumerator OnActive()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (transform.position.y >= finishPos.y - 0.2f && !isStep)
            {
                StartCoroutine(WaitMove());
            }
            else if(transform.position.y <= originPos.y && !isStep)
            {
                isActive = false;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !isActive)
        {
            isActive = true;
            isStep = true;
            targetPos = finishPos;
            StartCoroutine(OnActive());
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            WahleCtrl.curState = WahleCtrl.instance.StepHold();
        }
    }

    void OnTriggerExit(Collider col)
    {
        WahleCtrl.curState = WahleCtrl.instance.Move();
        isStep = false;
    }

    // 플레이어 벗어난 후 초기 위치로 돌아가기 위한 카운트 다운
    IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(waitTime);
        targetPos = originPos;
    }
}
