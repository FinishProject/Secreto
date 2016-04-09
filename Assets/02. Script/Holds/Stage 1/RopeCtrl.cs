using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    로프.. 쥬긴다

    사용방법 :
    


*************************************************************/


public class RopeCtrl : MonoBehaviour {
    [Tooltip("로프 구슬 프리펩")]
    public GameObject prefab;
    [Tooltip("로프 구슬 개수")]
    public int ropeCnt;
    [Tooltip("각도 제한")]
    public float angleLimit = 100f;             // 각도 제한
    [System.NonSerialized]
    public bool isCtrlAuthority = false;        // 조작권한 (로프 조종)

    private Transform[] lowRopes;
    private float ropeScale;

    private float L = 0;                        // 실 길이(m)
    private float stepSize = 0.1f;              // 스텝사이즈
    private float resist = -0.5f;               // 저항
    private float theta = 50 * Mathf.Deg2Rad;   // 각도
    private float theta1 = 0;
    private float theta2 = 0;
    private float pre_theta1 = 0;
    private float pre_theta2 = 0;

    private float addPower = 70.0f;
    private bool isLimited = false;             // 힘의 제한
    private bool isLeft;
    private int playerIdx;                      // 플레이어의 인덱스 위치

    void Start()
    {
        lowRopes = new Transform[ropeCnt];
        playerIdx = ropeCnt - 1;

        CreateRope();
        changeRopeRange(playerIdx);
        InitState(40.0f);

    }

    void InitState(float getAngle)
    {
        stepSize = 0.025f;

        theta = getAngle * Mathf.Deg2Rad;
        theta1 = 0;
        theta2 = 0;
        pre_theta1 = 0;
        pre_theta2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCtrlAuthority)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                changeRopeRange(--playerIdx);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                changeRopeRange(++playerIdx);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isLimited && isLeft)
            {
                if (Mathf.Abs(theta1) < addPower)
                    theta1 += addPower * Mathf.Deg2Rad;
                pre_theta1 = 0;
                pre_theta2 = 0;
                isLimited = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isLimited && !isLeft)
            {
                if (Mathf.Abs(theta1) < addPower)
                    theta1 -= addPower * Mathf.Deg2Rad;
                pre_theta1 = 0;
                pre_theta2 = 0;
                isLimited = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCtrlAuthority = false;
                PlayerCtrl.instance.GetCtrlAuthorityByRope();  // 플레이어에게 조작 권한을 돌려줌
            }
        }
    }

    void FixedUpdate()
    {
        PendulumMove();
    }

    // 진자 운동
    void PendulumMove()
    {
        // 진자운동 구현
        theta2 = (Physics.gravity.y / L) * Mathf.Sin(theta);                               // 가속도
        theta1 = theta1 + (pre_theta2 * stepSize)                                          // 속도
             + ((theta2 - pre_theta2) * stepSize * 0.5f)
             - ((theta2 - pre_theta2) * resist);
        theta = theta + (theta1 * stepSize) + ((theta1 - pre_theta1) * stepSize * 0.5f);   // 변위
        pre_theta2 = theta2;
        pre_theta1 = theta1;

        // 각도 제한
        if (theta < -angleLimit * Mathf.Deg2Rad)
        {

            theta = -angleLimit * Mathf.Deg2Rad;
            pre_theta1 = 0;
            pre_theta2 = 0;
        }
        else if (theta > angleLimit * Mathf.Deg2Rad)
        {
            theta = angleLimit * Mathf.Deg2Rad;
            pre_theta1 = 0;
            pre_theta2 = 0;
        }


        // 방향에 따른 리미트 해제
        if (Mathf.Sin(theta) < 0f && !isLeft || Mathf.Sin(theta) >= 0f && isLeft)
            isLimited = false;


        // 줄 회전
        /*
        ChangeRot(0, playerIdx, theta / Mathf.Deg2Rad);
        if (playerIdx != ropeCnt - 1)
            ChangeRot(playerIdx, ropeCnt - 1, ((theta - theta) - (theta * 0.15f)) / Mathf.Deg2Rad);
        */

        if (!isCtrlAuthority)
        {
            playerIdx = ropeCnt - 1;
            ChangeRot_2(0 , playerIdx, -(theta / Mathf.Deg2Rad));
        }
        else
        {
            ChangeRot(0, playerIdx, theta / Mathf.Deg2Rad);
            if (playerIdx != ropeCnt - 1)
                ChangeRot_2(playerIdx, ropeCnt - 1, theta / Mathf.Deg2Rad);
        }

       

        if (Mathf.Sin(theta) < 0f)
            isLeft = true;
        else
            isLeft = false;
    }

    // 플레이어 위치에 따른 줄의 길이 (진자의 위치)
    void changeRopeRange(int idx)
    {
        if (idx >= ropeCnt)
        {
            playerIdx = ropeCnt - 1;
            return;
        }
        else if (idx <= 0)
        {
            playerIdx = 0;
            // 로프의 끝
            return;
        }

        playerIdx = idx;
        L = Vector3.Distance(lowRopes[0].transform.position, lowRopes[playerIdx].transform.position);

    }

    // 각도 조절 (시작, 각도)
    void ChangeRot(int index, float rot)
    {
        Quaternion temp;
        for (int i = index; i < ropeCnt; i++)
        {
            temp = lowRopes[i].transform.rotation;
            temp.eulerAngles = new Vector3(0, 0, rot);
            lowRopes[i].rotation = temp;
            if (i == 0)
                continue;
            MovePos(i, rot);
        }
    }

    // 각도 조절 (시작, 끝, 각도)
    void ChangeRot(int startIdex, int endIdx, float rot)
    {
        Quaternion temp;
        for (int i = startIdex; i <= endIdx; i++)
        {
            temp = lowRopes[i].transform.rotation;
            temp.eulerAngles = new Vector3(0, 0, rot);
            lowRopes[i].rotation = temp;
            if (i == 0)
                continue;
            MovePos(i, rot);
        }
    }

    void ChangeRot_2(int startIdex, int endIdx, float rot)
    {
        Quaternion temp;
        float tempRot = rot;
        for (int i = startIdex; i <= endIdx; i++)
        {
            temp = lowRopes[i].transform.rotation;
            tempRot += tempRot / 40;
            temp.eulerAngles = new Vector3(0, 0, rot - tempRot);
            lowRopes[i].rotation = temp;
            if (i == 0)
                continue;
            MovePos(i, rot - tempRot);
        }
    }

    // 로프 움직이기 (각도에 맞춰서 꼬리를 물어야 하므로)
    void MovePos(int index, float rot)
    {
        Vector3 temp = lowRopes[index - 1].transform.position;
        temp.x += Mathf.Sin(rot * Mathf.Deg2Rad) * (ropeScale / 2);
        temp.y -= Mathf.Cos(rot * Mathf.Deg2Rad) * (ropeScale / 2);

        lowRopes[index].transform.position = temp;
    }


    // 로프 생성
    void CreateRope()
    {
        for (int i = 0; i < ropeCnt; i++)
        {
            GameObject lowRope = Instantiate(prefab) as GameObject;
            lowRope.name = i.ToString();
            lowRope.tag = "Rope";
            lowRope.transform.parent = gameObject.transform;

            lowRopes[i] = lowRope.transform;

            Vector3 temp = gameObject.transform.position;
            if (i == 0)
            {
                temp.y = gameObject.transform.position.y - gameObject.transform.localScale.y / 2;
                lowRopes[i].position = temp;
                ropeScale = lowRopes[i].localScale.y;
            }
            else
            {
                temp.y = lowRopes[i - 1].position.y - ropeScale / 2;
                lowRopes[i].position = temp;
            }
        }
    }

    //플레이어에게 조작권한을 넘겨줌
    public void setPlayerAuthority(int playerIdx)
    {
        this.playerIdx = playerIdx;
        isCtrlAuthority = true;
        playerIdx = ropeCnt - 1;
    }

    public Transform getLowRopeTransform()
    {
        if(playerIdx.Equals(0))
            return lowRopes[playerIdx].transform;
        else
            return lowRopes[playerIdx-1].transform;
    }

    public float getRadian()
    {
        if (playerIdx.Equals(0))
            return lowRopes[playerIdx].transform.eulerAngles.z;
        else
            return lowRopes[playerIdx - 1].transform.eulerAngles.z;
    }

    public float getSpeed()
    {
        return theta;
    }


}

