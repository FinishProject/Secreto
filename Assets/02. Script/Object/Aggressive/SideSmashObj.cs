using UnityEngine;
using System.Collections;

public class SideSmashObj : MonoBehaviour {
    [System.Serializable]
    public struct ObjInfo {
        public Transform topObj;
        public Transform botObj;
        public Vector3 topOriginPos, botOriginPos;
    };

    private float speed = 0f, speed2 = 0f;
    public float length = 5f;
    public float waitTime = 1.5f; // 충돌 후 대기 시간
    public float distance = 80f;
    public Transform Point;

    private bool isReverse = false;
    private bool isSecondUP = false;

    public ObjInfo[] objInfo;
    private Vector3[] finishPos;
    private Vector3[] botTarget, topTarget;
    private Collider[] childColl;

    delegate void MoveDelegate();
    MoveDelegate moveDelegate; 

    void Start()
    {
        if (!transform.parent)
        {
            childColl = GetComponentsInChildren<Collider>();

            finishPos = new Vector3[objInfo.Length];
            botTarget = new Vector3[objInfo.Length];
            topTarget = new Vector3[objInfo.Length];

            if (objInfo[1].botObj != null)
            {
                BothSideInit();
            }
            else {
                OnceSideInit();
            }

            StartCoroutine(ActiveUpdate());
        }
    }

    void BothSideInit()
    {
        for(int i = 0; i < objInfo.Length; i++)
        {
            // 시작 위치(초기 위치) 저장
            objInfo[i].topOriginPos = objInfo[i].topObj.position;     
            objInfo[i].botOriginPos = objInfo[i].botObj.position;
   
            // 두 오브젝트 사이의 목표 지점을 구함
            finishPos[i] = objInfo[i].topObj.position - objInfo[i].botObj.position;
            finishPos[i].y *= 0.32f;
        }
        moveDelegate = FirstMove;
        moveDelegate += SeconddMove;
    }

    void OnceSideInit()
    {
        // 시작 위치(초기 위치) 저장
        for (int i = 0; i < objInfo.Length; i++)
        {
            // 시작 위치(초기 위치) 저장
            objInfo[i].topOriginPos = objInfo[i].topObj.position;

            // 두 오브젝트 사이의 목표 지점을 구함
            finishPos[i] = objInfo[i].topOriginPos;
            finishPos[i].y += length;
        }
        moveDelegate = OneSideMove;
    }

    IEnumerator ActiveUpdate()
    {
        while (true)
        {
            //distance = (PlayerCtrl.instance.transform.position - Point.position).sqrMagnitude;
            moveDelegate();
            yield return null;
        }
    }

    void FirstMove()
    {
        for (int i = 0; i < objInfo.Length; i += 2)
        {
            // 목표지점까지 내려감
            if (!isReverse)
            {
                speed += Time.deltaTime * 0.4f;

                // 목표 좌표를 가져옴
                topTarget[i] = new Vector3(objInfo[i].topOriginPos.x, objInfo[i].topOriginPos.y - finishPos[i].y, objInfo[i].topOriginPos.z);
                botTarget[i] = new Vector3(objInfo[i].botOriginPos.x, objInfo[i].botOriginPos.y + finishPos[i].y, objInfo[i].botOriginPos.z);

                if (objInfo[i].topObj.position.Equals(topTarget[i]))
                {
                    
                    isReverse = true;
                    speed = 0f;
                    StartCoroutine(WaitForTime());
                }
            }
            // 원위치로 올라감
            else
            {
                speed = Time.deltaTime;

                // 원 위치 좌표를 가져옴
                topTarget[i] = objInfo[i].topOriginPos;
                botTarget[i] = objInfo[i].botOriginPos;

                if (objInfo[i].topObj.position.y >= objInfo[i].topOriginPos.y - 0.2f)
                {
                    isReverse = false;
                }
            }

            // 이동
            objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, topTarget[i], speed);
            objInfo[i].botObj.position = Vector3.Lerp(objInfo[i].botObj.position, botTarget[i], speed);
        }
    }

    void SeconddMove()
    {
        for (int i = 1; i < objInfo.Length; i += 2)
        {
            // 목표지점까지 내려감
            if (isSecondUP)
            {
                speed2 += Time.deltaTime * 0.4f;

                topTarget[i] = new Vector3(objInfo[i].topOriginPos.x, objInfo[i].topOriginPos.y - finishPos[i].y, objInfo[i].topOriginPos.z);
                botTarget[i] = new Vector3(objInfo[i].botOriginPos.x, objInfo[i].botOriginPos.y + finishPos[i].y, objInfo[i].botOriginPos.z);

                if (objInfo[i].topObj.position.Equals(topTarget[i]))
                {
                    speed2 = 0f;
                    isSecondUP = false;
                }
            }
            // 원위치로 올라감
            else
            {
                speed2 = Time.deltaTime;
                // 원 위치 좌표를 가져옴
                topTarget[i] = objInfo[i].topOriginPos;
                botTarget[i] = objInfo[i].botOriginPos;
            }
            // 이동
            objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, topTarget[i], speed2);
            objInfo[i].botObj.position = Vector3.Lerp(objInfo[i].botObj.position, botTarget[i], speed2);
        }
    }

    void OneSideMove()
    {
        for (int i = 0; i < objInfo.Length; i += 2)
        {
            // 목표지점까지 내려감
            if (!isReverse)
            {
                speed += Time.deltaTime * 0.4f;

                // 목표 좌표를 가져옴
                topTarget[i] = new Vector3(objInfo[i].topOriginPos.x, objInfo[i].topOriginPos.y - finishPos[i].y, objInfo[i].topOriginPos.z);

                if (objInfo[i].topObj.position.Equals(topTarget[i]))
                {
                    if (distance < 500f)
                    {
                        //StartCoroutine(CameraCtrl_4.instance.Shake(40f, 5, 0.01f));
                    }

                    isReverse = true;
                    speed = 0f;
                    StartCoroutine(WaitForTime());
                }
            }
            // 원위치로 올라감
            else
            {
                speed = Time.deltaTime;

                // 원 위치 좌표를 가져옴
                topTarget[i] = objInfo[i].topOriginPos;

                if (objInfo[i].topObj.position.y >= objInfo[i].topOriginPos.y - 0.2f)
                {
                    isReverse = false;
                }
            }

            objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, topTarget[i], speed);
        }

        for (int i = 1; i < objInfo.Length; i += 2)
        {
            // 목표지점까지 내려감
            if (isSecondUP)
            {
                speed2 += Time.deltaTime * 0.4f;

                topTarget[i] = new Vector3(objInfo[i].topOriginPos.x, objInfo[i].topOriginPos.y - finishPos[i].y, objInfo[i].topOriginPos.z);

                if (objInfo[i].topObj.position.Equals(topTarget[i]))
                {
                    speed2 = 0f;
                    isSecondUP = false;
                }
            }
            // 원위치로 올라감
            else
            {
                speed2 = Time.deltaTime;

                // 원 위치 좌표를 가져옴
                topTarget[i] = objInfo[i].topOriginPos;
            }
            // 이동
            objInfo[i].topObj.position = Vector3.Lerp(objInfo[i].topObj.position, topTarget[i], speed2);
        }
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(0.9f);
        isSecondUP = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerCtrl.instance.PlayerDie();
        }
    }
}
