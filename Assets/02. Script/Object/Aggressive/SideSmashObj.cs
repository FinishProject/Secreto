using UnityEngine;
using System.Collections;

public class SideSmashObj : MonoBehaviour {

    public float speed;
    public float waitTime = 1.5f; // 충돌 후 대기 시간
    public bool isRight = true; // 오른쪽에 위치한 오브젝트인지 확인
  
    private bool isSmash = false; // 충돌 했는 지
    private bool isMove = false; // 대기 시간 지난 후 이동 체크
    private Vector3 originPos; // 초기 위치

    void Start()
    {
        originPos = this.transform.position;
    }

    void Update()
    {
        // 충돌 할때까지 이동
        if (!isSmash)
        {
            if (isRight) // 오른쪽에 위치한 오브젝트시 이동
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else // 왼쪽에 위치한 오브젝트시 이동
                transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        // 충돌 했을 시 초기 위치까지 되돌아감
        else if(isSmash && isMove)
        {
            if (isRight) // 오른쪽에 위치한 오브젝트시 이동
            {
                if (transform.position.x <= originPos.x)
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                // 초기 위치 도달 시 초기화
                else if (transform.position.x >= originPos.x)
                {
                    isMove = false;
                    StartCoroutine(WaitMove(true));
                }
            }
            else // 왼쪽에 위치한 오브젝트시 이동
            {
                if (transform.position.x >= originPos.x)
                    transform.Translate(Vector3.left * speed * Time.deltaTime);

                else if (transform.position.x <= originPos.x)
                {
                    isMove = false;
                    StartCoroutine(WaitMove(true));
                }
            }
        }
        
    }
    // 충돌 시 이동 중지
    void OnCollisionEnter(Collision col)
    {
        isSmash = true;
        StartCoroutine(WaitMove(false));
    }
    // 잠시 대기 후 다시 이동
    IEnumerator WaitMove(bool isType)
    {
        yield return new WaitForSeconds(waitTime);
        if (isType) // 충돌 하기 위해 이동
        {
            isSmash = false;
        }
        else { // 돌아가기 위해 이동
            isMove = true;
        }
        StopCoroutine("WaitMove");
    }
}
