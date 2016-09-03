using UnityEngine;
using System.Collections;

public class OnceSideSmashWall : MonoBehaviour {

    public float downLength; // 내려갈 위치
    public float downSpeed; // 하강 속도
    public float upSpeed; // 상승 속도
    public float waitTime; // 하강 후 대기 시간

    private bool isActive = true; // 코루틴 작동여부
    //private float curDirSpeed = 0f; // 움직일 방향의 속도
    private bool isFirstDown = true; // 첫번째 그룹 벽 하강 체크
    private bool isSecondDown = false; // 두번째 그룹 벽 하강 체크

    public GameObject[] walls; // 벽 오브젝트들
    private Vector3[] originPos, arrivePos; // 시작, 도착 위치 벡터
    private Vector3 targetPos; // 이동할 목표 위치 벡트

    void Start()
    {
        // 배열 크기 초기화
        originPos = new Vector3[walls.Length];
        arrivePos = new Vector3[walls.Length];

        // 초기, 목표 위치 초기화
        for (int i = 0; i < walls.Length; i++)
        {
            originPos[i] = walls[i].transform.position;
            arrivePos[i] = originPos[i];
            arrivePos[i].y -= downLength;
        }
        //curDirSpeed = downSpeed;

        //StartCoroutine(FirstWallMovement());
        //StartCoroutine(SecondWallMovement());
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
        float curDirSpeed = downSpeed;
        float moveSpeed = 0f;
        while (isActive)
        {
            moveSpeed = curDirSpeed * Time.deltaTime;

            for (int i = 0; i < walls.Length; i += 2)
            {
                // 하강 또는 상승 시 목표 위치
                if (isFirstDown)
                    targetPos = arrivePos[i];
                else
                    targetPos = originPos[i];

                // 도착지점의 도착했을 시
                if (walls[i].transform.position.y.Equals(targetPos.y))
                {
                    // 하강이었을 때
                    if (isFirstDown)
                    {
                        // 카메라 흔들림
//                        StartCoroutine(CameraCtrl_4.instance.Shake(2f, 1, 10f));
                        yield return new WaitForSeconds(waitTime);
                        isSecondDown = true; // 두번째 그룹 하강하도록 변경
                        isFirstDown = false; // 첫번째 그룹 상승하도록 변경
                        curDirSpeed = upSpeed; // 상승 속도로 변경
                        moveSpeed = 0f;
                    }
                    // 상승이었을 때
                    else
                    {
                        yield return new WaitForSeconds(waitTime);
                        isFirstDown = true;
                        curDirSpeed = downSpeed; // 하강 속도로 변경
                        moveSpeed = 0f;
                    }
                }
                // 이동
                MovementObject(i, targetPos, moveSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    // 두번째 그룹 벽 이동
    IEnumerator SecondWallMovement()
    {
        float curDirSpeed = downSpeed;
        float moveSpeed = 0f;
        while (isActive)
        {
            moveSpeed = curDirSpeed * Time.deltaTime;

            for (int i = 1; i < walls.Length; i += 2)
            {
                if (isSecondDown)
                    targetPos = arrivePos[i];
                else
                    targetPos = originPos[i];

                // 목표 지점에 도착했을 시
                if (walls[i].transform.position.y.Equals(targetPos.y))
                {
                    // 하강이었을 시
                    if (isSecondDown)
                    {
//                        StartCoroutine(CameraCtrl_4.instance.Shake(2f, 1, 10f));
                        yield return new WaitForSeconds(waitTime - 0.5f);
                        
                        isSecondDown = false;
                        
                        curDirSpeed = upSpeed;
                        moveSpeed = 0f;
                    }
                    // 상승이었을 시
                    else
                    {
                        yield return new WaitForSeconds(waitTime);
                        curDirSpeed = downSpeed;
                        moveSpeed = 0f;
                    }
                }
                MovementObject(i, targetPos, moveSpeed);
            }
            yield return null;
        }
    }    

    // 벽 오브젝트 이동
    void MovementObject(int index, Vector3 moveDir, float speed)
    {
        walls[index].transform.position = 
            Vector3.MoveTowards(walls[index].transform.position, moveDir, speed);
    }
}
