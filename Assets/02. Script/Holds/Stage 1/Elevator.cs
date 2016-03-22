using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    public bool isType = true; // 아래에서 위 또는 위에서 아래로 가는지 체크
    public float speed = 5f; // 이동 속도
    public float maxLength = 10f; // 마지막 위치 길이

    private bool isActive = true; // 작동 여부
    private bool isTrample = false; // 플레이어가 밟고 있는지 여부
    private Vector3 originPos, finishPos; // 시작 위치, 최종 위치
    private Transform playerTr; // 플레이어 트랜스폼

    void Start()
    {
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

    void Move()
    {
        switch (isType)
        {
            case true: // 아래에서 시작 시 이동
                //최고 높이까지 이동
                if (transform.position.y >= finishPos.y && speed > -1f) {
                    isActive = false;
                    speed *= -1f; // 이동 방향 변경    
                }
                else if (isActive && isTrample)
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                //초기 위치까지 이동
                if (transform.position.y <= originPos.y && speed < 1f) {
                    isActive = true;
                    speed *= -1;
                }
                else if (!isActive && !isTrample)
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;

            case false: // 위에서 시작 시 이동
                //최저 높이까지 이동
                if (transform.position.y <= originPos.y && speed < 1f) {
                    isActive = false;
                    speed *= -1f;
                }
                if(isActive && isTrample)
                    transform.Translate(Vector3.up * speed * Time.deltaTime);
                //최고 높이까지 이동
                if (transform.position.y >= finishPos.y && speed > -1) {
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
        if(col.gameObject.tag == "Player")
        {
            StopCoroutine("CountDown");
            if(isActive) // 오브젝트가 작동 중이고 플레이어 밟고 있을 시 true
                isTrample = true;
            //플레이어 이동
            playerTr = col.GetComponent<Transform>();
            playerTr.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider col)
    {
        StartCoroutine("CountDown");
        //playerTr = null;
    }
    // 플레이어 벗어난 후 초기 위치로 돌아가기 위한 카운트 다운
    IEnumerator CountDown()
    {
        float time = 0f;
        while (true) {
            time += Time.deltaTime;
            if(time >= 3f) {
                isTrample = false;
                break;
            }
            yield return null;
        }
        StopCoroutine("TimeDown");
    }

}
