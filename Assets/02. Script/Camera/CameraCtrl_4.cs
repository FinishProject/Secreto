using UnityEngine;
using System.Collections;

public class CameraCtrl_4 : MonoBehaviour, Sensorable_Return, Sensorable_Something
{
    public Vector3 playerDistance;
    public float camSpeed = 5f;

    bool isNearByWall_L = false;
    bool isNearByWall_R = false;

    Vector3 originalDistance;
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

        originalDistance = playerDistance;
        playerTr = PlayerCtrl.instance.transform;
    }
	
	void Update () {
        if (isNearByWall_L || isNearByWall_R)
        {
            if ((isNearByWall_L && Input.GetKey(KeyCode.RightArrow) ||
                isNearByWall_R && Input.GetKey(KeyCode.LeftArrow)) && !(isNearByWall_L && isNearByWall_R) ) 
            {
                tr.position = Vector3.Lerp(tr.position, playerTr.position + playerDistance, camSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tempPos = tr.position;
                tempPos.y = playerTr.position.y + playerDistance.y;
                tr.position = Vector3.Lerp(tr.position, tempPos, camSpeed * Time.deltaTime);

            }
        }
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

    public void ActiveSensor_Retuen(GameObject returnObjet)
    {
        if (returnObjet == null)
        {
            return;
        }

        cameraArea = returnObjet.GetComponent<CameraArea>();
        playerDistance = cameraArea.playerDistance;
        
    }
    public bool ActiveSensor_Something(int index)
    {
        switch (index)
        {
            case 2:
                isNearByWall_L = true;
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
