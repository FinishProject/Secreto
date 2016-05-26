using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
    public bool isType = true; // 아래에서 위 또는 위에서 아래로 가는지 체크
    public float speed; // 이동 속도
    public float maxLength = 10f; // 마지막 위치 길이

    private bool isActive = true; // 작동 여부
    private bool isStep = false; // 플레이어가 밟고 있는지 여부
    private Vector3 originPos, finishPos; // 시작 위치, 최종 위치
    private Vector3 targetPos;
    private Transform playerTr; // 플레이어, 올라 트랜스폼

    private Vector3 velocity = Vector3.up;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        // 시작 위치 및 최종 위치 구함
        originPos = this.transform.position;
        finishPos = new Vector3(originPos.x, originPos.y + maxLength, originPos.z);
        //finishPos = originPos;
        //finishPos.y += 15f;

        // 위에서 시작할 시 초기 값 설정
        if (!isType) {
            transform.position = finishPos;
            speed *= -1;
        }

        targetPos = finishPos;
        //speed = 0f;
    }

    IEnumerator OnActive()
    {
        while (true)
        {
            if (isStep)
            {
               // speed += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            }

            if (transform.position.y >= finishPos.y - 0.2f)
            {
                isActive = false;
                targetPos = originPos;
                speed *= -1f;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void FixedUpdate()
    {
        
        //Move();
    }

    //void Move()
    //{
    //    switch (isType)
    //    {
    //        case true: // 아래에서 시작 시 이동
    //            //최고 높이까지 이동
    //            if (transform.position.y >= finishPos.y && speed > -1f)
    //            {
    //                isActive = false;
    //                speed *= -1f; // 이동 방향 변경    
    //            }
    //            else if (isActive && isStep)
    //            {
    //                transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //            }

    //            //초기 위치까지 이동
    //            if (transform.position.y <= originPos.y && speed < 1f)
    //            {
    //                isActive = true;
    //                speed *= -1;
    //            }
    //            else if (!isActive && !isStep)
    //            {
    //                transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //            }
    //            break;

    //        case false: // 위에서 시작 시 이동
    //            //최저 높이까지 이동
    //            if (transform.position.y <= originPos.y && speed < 1f)
    //            {
    //                isActive = false;
    //                speed *= -1f;
    //            }
    //            else if (isActive && isStep)
    //                transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //            //최고 높이까지 이동
    //            if (transform.position.y >= finishPos.y && speed > -1)
    //            {
    //                isActive = true;
    //                speed *= -1;
    //            }
    //            else if (!isActive && !isStep)
    //                transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //            break;
    //    }
    //}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(OnActive());
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StopCoroutine(WaitMove());
            // 오브젝트가 작동 중이고 플레이어 밟고 있을 시 true
            if (isActive)
            {
                WahleCtrl.curState = WahleCtrl.instance.StepHold();
                isStep = true;
                //플레이어 이동
                playerTr.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        StartCoroutine(WaitMove());
        WahleCtrl.curState = WahleCtrl.instance.Move();
    }

    // 플레이어 벗어난 후 초기 위치로 돌아가기 위한 카운트 다운
    IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(3f);
        isStep = false;
    }

}
