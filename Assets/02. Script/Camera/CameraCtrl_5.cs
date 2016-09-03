using UnityEngine;
using System.Collections;

public class CameraCtrl_5 : MonoBehaviour, Sensorable_Return
{
    public float camSpeed = 5f;

    bool isNearByWall_L = false;
    bool isNearByWall_R = false;

    public Vector3 NearWallDistance;

    private float originCamSpeed;

    float player_L_EndPos;
    float player_R_EndPos;

    Transform playerTr;
    Transform tr;
    Transform sensorArea, sensorWall_L, sensorWall_R;
    Vector3 baseCamPos;

    public static CameraCtrl_5 instance;

    void Start()
    {
        instance = this;
        playerTr = PlayerCtrl.instance.transform;

        sensorArea = GameObject.Find("Sensor_Area").transform;
        sensorWall_L = GameObject.Find("Sensor_Wall_L").transform;
        sensorWall_R = GameObject.Find("Sensor_Wall_R").transform;

        tr = gameObject.transform;
        baseCamPos = tr.position - playerTr.position;

        originCamSpeed = camSpeed;
    }

    void Update()
    {
        // 거리가 급격히 멀어 졌을 때 (죽었을 때)
        if (Vector3.Distance(tr.position, playerTr.position) > 30f)
            tr.position = playerTr.position;

        // 벽과 충돌이 있을때
        if (isNearByWall_L || isNearByWall_R)
        {
            NearByWall();
        }
        // 벽과 충돌이 없을 때
        else
        {
            tr.position = Vector3.Lerp(tr.position, playerTr.position + baseCamPos, camSpeed * Time.deltaTime);
        }
    }


    void NearByWall()
    {
        // 양쪽 두 벽과 충돌이 있을 때

        if (isNearByWall_L && isNearByWall_R)
        {
            Vector3 tempPos = tr.position;
            tempPos.y = playerTr.position.y + baseCamPos.y;
            tr.position = Vector3.Lerp(tr.position, tempPos, camSpeed * Time.deltaTime);
        }

        // 한쪽 벽과 충돌이 있을 때
        else if (isNearByWall_L || isNearByWall_R)
        {
            if (isNearByWall_L && playerTr.position.x >= player_L_EndPos + baseCamPos.x)
            {
                tr.position = Vector3.Lerp(tr.position, playerTr.position + NearWallDistance, camSpeed * Time.deltaTime);
            }
            else if (isNearByWall_R && playerTr.position.x <= player_R_EndPos - baseCamPos.x)
            {
                tr.position = Vector3.Lerp(tr.position, playerTr.position + NearWallDistance, camSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tempPos = tr.position;
                tempPos.y = playerTr.position.y + baseCamPos.y;
                tr.position = Vector3.Lerp(tr.position, tempPos, camSpeed * Time.deltaTime);
            }
        }

    }


    public void ActiveSensor_Retuen(int index, GameObject returnObjet)
    {
        switch (index)
        {

            case 2:
                if (returnObjet == null)
                {
                    isNearByWall_L = false;
                }
                else
                {
                    isNearByWall_L = true;
                    NearWallDistance = baseCamPos;

                    float sensor_L_EndPos = tr.position.x - (tr.position.x - sensorWall_L.transform.position.x) - (sensorWall_L.transform.localScale.x * 0.5f);
                    float wall_L_EndPos = returnObjet.transform.position.x + (returnObjet.transform.localScale.x * 0.5f);
                    NearWallDistance.x = wall_L_EndPos - sensor_L_EndPos;
                    player_L_EndPos = playerTr.position.x;
                }

                break;

            case 3:
                if (returnObjet == null)
                {
                    isNearByWall_R = false;
                }
                else
                {
                    isNearByWall_R = true;
                    NearWallDistance = baseCamPos;

                    float sensor_R_EndPos = tr.position.x + (sensorWall_R.transform.position.x - tr.position.x) + (sensorWall_R.transform.localScale.x * 0.5f);
                    float wall_R_EndPos = returnObjet.transform.position.x - (returnObjet.transform.localScale.x * 0.5f);
                    NearWallDistance.x = -(sensor_R_EndPos - wall_R_EndPos);
                    player_R_EndPos = playerTr.position.x;
                }
                break;
        }
    }

    // 외부에서 카메라 속도 변경
    public void ChangeCamSpeed(float speed)
    {
        camSpeed = speed;
    }
    // 카메라 속도를 초기 설정한 속도로 되돌림
    public void ResetCameSpeed()
    {
        camSpeed = originCamSpeed;
    }
}
