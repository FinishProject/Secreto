using UnityEngine;
using System.Collections;

public class SideSmashObj : MonoBehaviour {
    [System.Serializable]
    public struct ObjInfo {
        public Transform topObj;
        public Transform botObj;
        public Vector3 topOriginPos, botOriginPos;
    };

    public ObjInfo[] objInfo;
    private Vector3[] finishPos;

    public float speed;
    public float length;
    public float waitTime = 1.5f; // 충돌 후 대기 시간
    private bool isFirstUP = false; // 오른쪽에 위치한 오브젝트인지 확인
    private bool isSecondUP = false;

    private bool isSmash = false; // 충돌 했는 지
    private bool isMove = false; // 대기 시간 지난 후 이동 체크
    private Vector3 upOrigin, downOrigin;
    private Vector3 upTarget, downTarget;

    Vector3 center;

    bool isSecond = false;

    void Start()
    {
        for(int i=0; i<objInfo.Length; i++)
        {
            objInfo[i].topOriginPos = objInfo[i].topObj.position;
            if(objInfo[i].botObj != null)
                objInfo[i].botOriginPos = objInfo[i].botObj.position;
        }

        finishPos = new Vector3[objInfo.Length];
        if (objInfo[0].botObj != null)
        {
            for (int i = 0; i < finishPos.Length; i++)
            {
                finishPos[i] = (objInfo[i].topOriginPos - objInfo[i].botOriginPos) * 0.5f;
            }
        }
        else
        {
            for (int i = 0; i < finishPos.Length; i++)
            {
                finishPos[i] = (objInfo[i].topOriginPos);
                finishPos[i].y = (objInfo[i].topOriginPos.y - 5f);
            }
        }
    }

    void Update()
    {
        if (objInfo[0].botObj != null)
        {
            SideMove();
        }
        else
        {
            SoloMove();
        }
    }

    void SideMove()
    {
        if (objInfo[0].topObj.position.y >= finishPos[0].y - 0.2f)
        {
            isFirstUP = true;
            isSecond = true;
        }
        else if (objInfo[0].topObj.position.y <= objInfo[0].topOriginPos.y + 0.2f)
        {
            isFirstUP = false;
        }

        if (objInfo[1].topObj.position.y >= finishPos[1].y - 0.2f)
        {
            isSecondUP = true;
        }
        else if (objInfo[1].topObj.position.y <= objInfo[1].topOriginPos.y + 0.2f)
        {
            isSecondUP = false;
        }

        for (int i = 0; i < objInfo.Length; i += 2)
        {
            if (i <= objInfo.Length)
            {
                if (isFirstUP)
                {
                    objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, new Vector3(objInfo[i].topObj.position.x, objInfo[i].topOriginPos.y, objInfo[i].topObj.position.z), Time.deltaTime);
                    objInfo[i].botObj.position = Vector3.Lerp(objInfo[i].botObj.position, new Vector3(objInfo[i].botObj.position.x, objInfo[i].botOriginPos.y, objInfo[i].botObj.position.z), Time.deltaTime);
                }
                else if (!isFirstUP)
                {
                    objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, new Vector3(objInfo[i].topObj.position.x, finishPos[i].y, objInfo[i].topObj.position.z), Time.deltaTime);
                    objInfo[i].botObj.position = Vector3.Lerp(objInfo[i].botObj.position, new Vector3(objInfo[i].botObj.position.x, finishPos[i].y, objInfo[i].botObj.position.z), Time.deltaTime);
                }
            }
        }
        if (isSecond)
        {
            for (int j = 1; j < objInfo.Length; j += 2)
            {
                if (j <= objInfo.Length)
                {
                    if (isSecondUP)
                    {
                        objInfo[j].topObj.position = Vector3.Lerp(objInfo[j].topObj.position, new Vector3(objInfo[j].topObj.position.x, objInfo[j].topOriginPos.y, objInfo[j].topObj.position.z), Time.deltaTime);
                        objInfo[j].botObj.position = Vector3.Lerp(objInfo[j].botObj.position, new Vector3(objInfo[j].botObj.position.x, objInfo[j].botOriginPos.y, objInfo[j].botObj.position.z), Time.deltaTime);
                    }
                    else if (!isSecondUP)
                    {
                        objInfo[j].topObj.position = Vector3.Lerp(objInfo[j].topObj.position, new Vector3(objInfo[j].topObj.position.x, finishPos[j].y, objInfo[j].topObj.position.z), Time.deltaTime);
                        objInfo[j].botObj.position = Vector3.Lerp(objInfo[j].botObj.position, new Vector3(objInfo[j].botObj.position.x, finishPos[j].y, objInfo[j].botObj.position.z), Time.deltaTime);
                    }
                }
            }
        }
    }

    void SoloMove()
    {
        if (objInfo[0].topObj.position.y <= finishPos[0].y + 0.2f)
        {
            isFirstUP = true;
            isSecond = true;
        }
        else if (objInfo[0].topObj.position.y >= objInfo[0].topOriginPos.y - 0.2f)
        {
            isFirstUP = false;
        }

        if (objInfo[1].topObj.position.y <= finishPos[1].y + 0.2f)
        {
            isSecondUP = true;
        }
        else if (objInfo[1].topObj.position.y >= objInfo[1].topOriginPos.y - 0.2f)
        {
            isSecondUP = false;
        }

        for (int i = 0; i < objInfo.Length; i += 2)
        {
            if (i <= objInfo.Length)
            {
                if (isFirstUP)
                {
                    objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, new Vector3(objInfo[i].topObj.position.x, objInfo[i].topOriginPos.y, objInfo[i].topObj.position.z), Time.deltaTime);
                }
                else if (!isFirstUP)
                {
                    objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, new Vector3(objInfo[i].topObj.position.x, finishPos[i].y, objInfo[i].topObj.position.z), Time.deltaTime);
                }
            }
        }
        if (isSecond)
        {
            for (int j = 1; j < objInfo.Length; j += 2)
            {
                if (j <= objInfo.Length)
                {
                    if (isSecondUP)
                    {
                        objInfo[j].topObj.position = Vector3.Lerp(objInfo[j].topObj.position, new Vector3(objInfo[j].topObj.position.x, objInfo[j].topOriginPos.y, objInfo[j].topObj.position.z), Time.deltaTime);
                    }
                    else if (!isSecondUP)
                    {
                        objInfo[j].topObj.position = Vector3.Lerp(objInfo[j].topObj.position, new Vector3(objInfo[j].topObj.position.x, finishPos[j].y, objInfo[j].topObj.position.z), Time.deltaTime);
                    }
                }
            }
        }
    }
}
