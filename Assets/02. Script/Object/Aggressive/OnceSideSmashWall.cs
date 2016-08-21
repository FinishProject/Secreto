using UnityEngine;
using System.Collections;

public class OnceSideSmashWall : MonoBehaviour {

    public float downLength = 3; // 내려갈 위치
    public float downSpeed = 0.4f; // 하강 속도
    public float upSpeed = 1f; // 상승 속도
    public float waitTime = 1f; // 하강 후 대기 시간

    private bool isActive = true;
    private float moveSpeed = 0f; // 이동 속도
    private bool isFirstDown = true; // 첫번째 그룹 벽 하강 체크
    private bool isSecondDown = false; // 두번째 그룹 벽 하강 체크

    public GameObject[] walls;
    private Vector3[] vStart, vEnd; // 시작, 마지막 위치 벡터
    private Vector3 targetPos; // 이동할 목표 위치 벡트

    private delegate IEnumerator MoveDelegate();
    MoveDelegate moveDel;

    void Start()
    {
        vStart = new Vector3[walls.Length];
        vEnd = new Vector3[walls.Length];

        // 초기, 목표 위치 초기화
        for (int i = 0; i < walls.Length; i++)
        {
            vStart[i] = walls[i].transform.position;
            vEnd[i] = vStart[i];
            vEnd[i].y -= downLength;
        }

        StartCoroutine(FirstWallMovement());
        StartCoroutine(SecondWallMovement());
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isActive = true;
            StartCoroutine(FirstWallMovement());
            StartCoroutine(SecondWallMovement());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isActive = false;
        }
    }
    // 첫번째 그룹 벽 이동
    IEnumerator FirstWallMovement()
    {
        while (isActive)
        {
            moveSpeed += downLength * Time.deltaTime;

            for (int i = 0; i < walls.Length; i += 2)
            {
                // 하강
                if (isFirstDown)
                {
                    // 이동 속도 변수
                    targetPos = vEnd[i];
                    // 목표 도달 시 일정 시간 후 위로 올라가도록 변경
                    if (walls[0].transform.position.y - 0.1f <= targetPos.y)
                    {
                        StartCoroutine(CameraCtrl_4.instance.Shake(2f, 1, 10f));
                        yield return new WaitForSeconds(waitTime);
                        isFirstDown = false;
                        isSecondDown = true; // 두번째 그룹 하강 가능하도록 변경
                        moveSpeed = 0f;
                    }
                }
                // 상승
                else
                {
                    moveSpeed = upSpeed * Time.deltaTime;
                    targetPos = vStart[i];
                    if (walls[i].transform.position.y + 0.1f >= targetPos.y)
                    {
                        yield return new WaitForSeconds(waitTime);
                        isFirstDown = true;
                        moveSpeed = 0f;
                    }
                }
                // 이동
                MovementObject(i);
                //walls[i].transform.position = Vector3.Lerp(walls[i].transform.position, targetPos, moveSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    // 두번째 그룹 벽 이동
    IEnumerator SecondWallMovement()
    {
        while (isActive)
        {
            moveSpeed += downLength * Time.deltaTime;
            for (int i = 1; i < walls.Length; i += 2)
            {
                if (isSecondDown)
                {
                    targetPos = vEnd[i];

                    if (walls[i].transform.position.y - 0.1f <= targetPos.y)
                    {
                        yield return new WaitForSeconds(waitTime);
                        isSecondDown = false;
                        moveSpeed = 0f;
                    }
                }
                else
                {
                    moveSpeed = upSpeed * Time.deltaTime;
                    targetPos = vStart[i];
                    if (walls[i].transform.position.y + 0.1f >= targetPos.y)
                    {
                        yield return new WaitForSeconds(waitTime);
                        moveSpeed = 0f;
                    }
                }
                MovementObject(i);
                //walls[i].transform.position = Vector3.Lerp(walls[i].transform.position, targetPos, moveSpeed);
            }

            yield return null;
        }
    }    

    void MovementObject(int index)
    {
        walls[index].transform.position = 
            Vector3.Lerp(walls[index].transform.position, targetPos, moveSpeed);
    }
}
