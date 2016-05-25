using UnityEngine;
using System.Collections;

public class CameraCtrl_2 : MonoBehaviour, Sensorable
{
    bool isLeftSide = false;
    GameObject inLine_L, inLine_R;
    Transform tr, playerTr, inLine_L_Tr, inLine_R_Tr;
    float speed;
    float speed_InLine = 100f;
    float speed_OutLine = 10f;
    float heightGap;
    float delay;
    public bool ActiveSensor(int index)
    {
        switch(index)
        {
            case 1:
                isLeftSide = true;
                speed = speed_InLine;
                break;
            case 2:
                isLeftSide = false;
                speed = speed_InLine;
                break;
            case 3:
                isLeftSide = false;
                StartCoroutine(SetActived(inLine_L));
                speed = speed_OutLine;
                break;
            case 4:
                isLeftSide = true;
                StartCoroutine(SetActived(inLine_R));
                speed = speed_OutLine;
                break;
        }

        return false;
    }


    // Use this for initialization
    void Start () {
        playerTr = PlayerCtrl.instance.transform;
        inLine_L = GameObject.Find("inLine_L");
        inLine_R = GameObject.Find("inLine_R");
        inLine_L_Tr = inLine_L.transform;
        inLine_R_Tr = inLine_R.transform;
        heightGap = transform.position.y - playerTr.position.y;
        delay = speed_OutLine * 0.1f;
    }

	// Update is called once per frame
	void LateUpdate() {
        MoveInsideLine();
    }

    void MoveInsideLine()
    {
        Vector3 tempPos = transform.position;
        
        if (isLeftSide)
        {
            if(playerTr.position.x > inLine_L_Tr.position.x)
            {
                tempPos.x = playerTr.position.x + (tempPos.x - inLine_L_Tr.position.x);
            }
        }
        else
        {
            if (playerTr.position.x < inLine_R_Tr.position.x)
            {
                tempPos.x = playerTr.position.x - (inLine_R_Tr.position.x - tempPos.x);
            }
        }
        tempPos.y = playerTr.position.y + heightGap;
        transform.position = Vector3.MoveTowards(transform.position, tempPos, speed * Time.deltaTime);
    }

    IEnumerator SetActived(GameObject tempObject)
    {
        tempObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        tempObject.SetActive(true);
    }
}
