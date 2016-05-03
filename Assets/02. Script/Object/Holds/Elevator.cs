using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
    public bool isType = true; // 아래에서 위 또는 위에서 아래로 가는지 체크
    public float speed; // 이동 속도
    public float maxLength = 10f; // 마지막 위치 길이

    private bool isActive = true; // 작동 여부
    private bool isTrample = false; // 플레이어가 밟고 있는지 여부
    private Vector3 originPos, finishPos; // 시작 위치, 최종 위치
    private Transform playerTr, olaTr; // 플레이어, 올라 트랜스폼

    void Start()
    {
        olaTr = GameObject.FindGameObjectWithTag("WAHLE").transform;

        // 시작 위치 및 최종 위치 구함
        originPos = this.transform.position;
        finishPos = new Vector3(originPos.x, originPos.y + maxLength, originPos.z);

        // 위에서 시작할 시 초기 값 설정
        if (!isType) {
            transform.position = finishPos;
            speed *= -1;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    //void Move()
    //{
    //    // 아래 목표 위치 도달 시
    //    if (transform.position.y - 0.1f <= originPos.y)
    //    {
    //        isActive = true;
    //        StartCoroutine(WaitMove());
    //        Debug.Log("Origin");
    //    }
    //    // 위 목표 위치 도달 시
    //    else if (transform.position.y + 0.1f >= finishPos.y)
    //    {
    //        isActive = false;
    //        StartCoroutine(WaitMove());
    //        Debug.Log("Finish");
    //    }
    //    if(isActive && isTrample)
    //        transform.position = Vector3.Lerp(transform.position, finishPos, speed * Time.deltaTime);
    //    else if(!isActive && !isTrample)
    //        transform.position = Vector3.Lerp(transform.position, originPos, speed * Time.deltaTime);
    //}

    void Move()
    {
        switch (isType)
        {
            case true: // 아래에서 시작 시 이동
                //최고 높이까지 이동
                if (transform.position.y >= finishPos.y && speed > -1f)
                {
                    isActive = false;
                    speed *= -1f; // 이동 방향 변경    
                }
                else if (isActive && isTrample)
                {
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                }

                //초기 위치까지 이동
                if (transform.position.y <= originPos.y && speed < 1f)
                {
                    isActive = true;
                    speed *= -1;
                }
                else if (!isActive && !isTrample)
                {
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                }
                break;

            case false: // 위에서 시작 시 이동
                //최저 높이까지 이동
                if (transform.position.y <= originPos.y && speed < 1f)
                {
                    isActive = false;
                    speed *= -1f;
                }
                if (isActive && isTrample)
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                //최고 높이까지 이동
                if (transform.position.y >= finishPos.y && speed > -1)
                {
                    isActive = true;
                    speed *= -1;
                }
                else if (!isActive && !isTrample)
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            StopCoroutine(WaitMove());
            playerTr = col.GetComponent<Transform>();
            // 오브젝트가 작동 중이고 플레이어 밟고 있을 시 true
            if (isActive)
            {
                isTrample = true;
                //플레이어 이동
                playerTr.Translate(Vector3.up * speed * Time.deltaTime);
                olaTr.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        StartCoroutine(WaitMove());
        playerTr = null;
    }

    // 이동 속도 가속도
    float IncrementToWards(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed == maxSpeed)
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            Debug.Log(initSpeed);
            // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
            return (1 == Mathf.Sign(maxSpeed - initSpeed)) ? initSpeed : maxSpeed;
        }
    }

    // 플레이어 벗어난 후 초기 위치로 돌아가기 위한 카운트 다운
    IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(3f);
        isTrample = false;
    }

}
