using UnityEngine;
using System.Collections;

public class CameraCtrl_4 : MonoBehaviour, Sensorable_Return, Sensorable_Something
{
    public Vector3 playerDistance;
    public float camSpeed = 5f;

    bool isNearByWall_L = false;
    bool isNearByWall_R = false;
    bool isCinematicView = false;

    public Vector3 NearWallDistance;

    public GameObject giz;
    Transform playerTr;
    Transform tr;

    Transform sensorArea, sensorWall_L, sensorWall_R;
    CameraArea cameraArea;
    Camera cam;

    void Start ()
    {
        tr = GetComponent<Transform>();
        cam = GetComponent<Camera>();

        sensorArea   = GameObject.Find("Sensor_Area"  ).transform;
        sensorWall_L = GameObject.Find("Sensor_Wall_L").transform;
        sensorWall_R = GameObject.Find("Sensor_Wall_R").transform;

        playerTr = PlayerCtrl.instance.transform;
    }
	
	void Update () {
        Debug.Log("왼쪽 : "+ isNearByWall_L);
        Debug.Log("오른 : " + isNearByWall_R);

        if (Vector3.Distance(tr.position, playerTr.position) > 30f)
            tr.position = playerTr.position;
        
        // 양쪽 두 벽과 충돌이 있을 때
        if (isNearByWall_L && isNearByWall_R)
        {
            Vector3 tempPos = tr.position;
            tempPos.y = playerTr.position.y + playerDistance.y;
            tr.position = Vector3.Lerp(tr.position, tempPos, camSpeed * Time.deltaTime);
        }
        // 한쪽 벽과 충돌이 있을 때
        else if(isNearByWall_L || isNearByWall_R)
        {
            /*
            Vector3 tmpPos = tr.position;
            tmpPos.x = player_L_EndPos + playerDistance.x;
            giz.transform.position = tmpPos;
            */

            if ( isNearByWall_L && playerTr.position.x >= player_L_EndPos + playerDistance.x )
            {
                tr.position = Vector3.Lerp(tr.position, playerTr.position + NearWallDistance, camSpeed * Time.deltaTime);
            }
            else if ( isNearByWall_R && playerTr.position.x <= player_R_EndPos - playerDistance.x )
            {
                tr.position = Vector3.Lerp(tr.position, playerTr.position + NearWallDistance, camSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tempPos = tr.position;
                tempPos.y = playerTr.position.y + playerDistance.y;
                tr.position = Vector3.Lerp(tr.position, tempPos, camSpeed * Time.deltaTime);
            }

        }
        // 벽과 충돌이 없을 때
        else
        {
            tr.position = Vector3.Lerp(tr.position, playerTr.position + playerDistance, camSpeed * Time.deltaTime);
        }


        if (cameraArea.hasFocus)
        {
            Vector3 pos = cameraArea.focusTr.position - tr.position;
            Quaternion newRot = Quaternion.LookRotation(pos);
            tr.rotation = Quaternion.Lerp(tr.rotation, newRot, camSpeed * Time.deltaTime);
        }
        else
        {
            tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.identity, camSpeed * Time.deltaTime);
        }


        

        CensorRotZero();
    }


    
    void CensorRotZero()
    {
        sensorWall_L.rotation = Quaternion.Lerp(sensorWall_L.rotation, Quaternion.identity, camSpeed * Time.deltaTime);
        sensorWall_R.rotation = Quaternion.Lerp(sensorWall_R.rotation, Quaternion.identity, camSpeed * Time.deltaTime);
        sensorArea.rotation   = Quaternion.Lerp(sensorArea.rotation, Quaternion.identity, camSpeed * Time.deltaTime);
    }

    float player_L_EndPos;
    float player_R_EndPos;
    public void ActiveSensor_Retuen(int index, GameObject returnObjet)
    {
        switch (index)
        {
            case 1:
                if (returnObjet != null)
                {
                    cameraArea = returnObjet.GetComponent<CameraArea>();
                    playerDistance = cameraArea.playerDistance;
                }
                break;

            case 2:
                if (returnObjet == null)
                {
                    isNearByWall_L = false;
                }
                else
                {
                    isNearByWall_L = true;
                    NearWallDistance = playerDistance;
                    
                    float sensor_L_EndPos = tr.position.x - (tr.position.x - sensorWall_L.transform.position.x) - (sensorWall_L.transform.localScale.x * 0.5f);
                    float wall_L_EndPos = returnObjet.transform.position.x + (returnObjet.transform.localScale.x * 0.5f);
                    NearWallDistance.x = wall_L_EndPos - sensor_L_EndPos;
                    player_L_EndPos = playerTr.position.x;

                    /*
                    NearWallDistance.x +=
                        (returnObjet.transform.position.x - returnObjet.transform.localScale.x * 0.5f)
                        - (sensorWall_R.position.x - sensorWall_R.localScale.x * 0.5f);
                    */
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
                    NearWallDistance = playerDistance;

                    float sensor_R_EndPos = tr.position.x + (sensorWall_R.transform.position.x - tr.position.x) + (sensorWall_R.transform.localScale.x * 0.5f);
                    float wall_R_EndPos = returnObjet.transform.position.x - (returnObjet.transform.localScale.x * 0.5f);
                    NearWallDistance.x    = -(sensor_R_EndPos - wall_R_EndPos);
                    player_R_EndPos = playerTr.position.x;
                    /*
                    NearWallDistance.x += 
                        (returnObjet.transform.position.x - returnObjet.transform.localScale.x * 0.5f)
                        -(sensorWall_R.position.x + sensorWall_R.localScale.x * 0.5f);
                    */
                }
                break;
        }

        
        
    }
    public bool ActiveSensor_Something(int index)
    {
        switch (index)
        {
            case 2:
                isNearByWall_L = true;
                NearWallDistance = playerDistance;
                break;

            case 3:
                isNearByWall_R = true;
                break;

            case 102:
                isNearByWall_L = false;
                break;

            case 103:
                isNearByWall_R = false;
                break;
        }

        return false;
    }

}
