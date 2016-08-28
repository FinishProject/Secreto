using UnityEngine;
using System.Collections;

public class OnceSideSmashWall : MonoBehaviour {

    public float downLength; // 내려갈 위치
    public float downSpeed; // 하강 속도
    public float upSpeed; // 상승 속도
    public float waitTime; // 하강 후 대기 시간

    private bool isActive = true;
    private float curDirSpeed = 0f;
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
        curDirSpeed = downSpeed;

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
        float moveSpeed = 0f;
        while (isActive)
        {
            moveSpeed += curDirSpeed * Time.deltaTime;

            for (int i = 0; i < walls.Length; i += 2)
            {
                if (isFirstDown)
                    targetPos = vEnd[i];
                else
                    targetPos = vStart[i];

                if (walls[i].transform.position.y.Equals(targetPos.y))
                {
                    if (isFirstDown)
                    {
                        StartCoroutine(CameraCtrl_4.instance.Shake(2f, 1, 10f));
                        yield return new WaitForSeconds(waitTime);

                        isFirstDown = false;
                        isSecondDown = true; // 두번째 그룹 하강 가능하도록 변경

                        curDirSpeed = upSpeed;
                        moveSpeed = 0f;
                    }
                    else
                    {
                        yield return new WaitForSeconds(waitTime);
                        //isFirstDown = true;
                        curDirSpeed = downSpeed;
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
        float moveSpeed = 0f;
        while (isActive)
        {
            moveSpeed += curDirSpeed * Time.deltaTime;

            for (int i = 1; i < walls.Length; i += 2)
            {

                if (isSecondDown)
                    targetPos = vEnd[i];
                else
                    targetPos = vStart[i];

                if (walls[i].transform.position.y.Equals(targetPos.y))
                {
                    if (isSecondDown)
                    {
                        StartCoroutine(CameraCtrl_4.instance.Shake(2f, 1, 10f));
                        yield return new WaitForSeconds(waitTime);
                        isSecondDown = false;
                        isFirstDown = true;
                        curDirSpeed = upSpeed;
                        moveSpeed = 0f;
                    }
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
