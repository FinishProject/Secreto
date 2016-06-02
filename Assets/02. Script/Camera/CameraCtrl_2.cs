using UnityEngine;
using System.Collections;

public class CameraCtrl_2 : MonoBehaviour, Sensorable_Player, Sensorable_Something
{
    bool isLeftSide = false;
    bool isZoom = false;

    GameObject inLine_L, inLine_R, Outside_L, Outside_R;
    Transform tr, playerTr, inLine_L_Tr, inLine_R_Tr;
    Vector3 originPos;
    public Transform rayPos;

    float speed;
    float speed_InLine = 8f;
    float speed_OutLine = 9f;
    float heightGap;

    public float zoomStartRangePercent = 0.1f;
    float zoomAreaX;                    // 줌 구역 시작 X좌표 위치
    float zoomAreaSize;                 // 줌 구역 길이
    float zoomDeep;                     // 줌 깊이
    float zoomHeight;                   // 줌 높이

    public bool ActiveSensor_Player(int index)
    {
        switch(index)
        {
            case 1:
                isLeftSide = true;
                inLine_L.SetActive(true);
                speed = speed_InLine;
                break;
            case 2:
                isLeftSide = false;
                inLine_R.SetActive(true);
                speed = speed_InLine;
                break;
            case 3:
                isLeftSide = false;
                ActiveLine();
                inLine_L.SetActive(false);     
                speed = speed_OutLine;
                break;
            case 4:
                isLeftSide = true;
                ActiveLine();
                inLine_R.SetActive(false);
                speed = speed_OutLine;
                break;
        }

        return false;
    }

    public bool ActiveSensor_Something(int index)
    {
        switch (index)
        {
            case 3:
                inLine_L.SetActive(false);
                inLine_R.SetActive(false);
                Outside_L.SetActive(false);
                speed = 0;
                break;
            case 4:
                inLine_L.SetActive(false);
                inLine_R.SetActive(false);
                Outside_R.SetActive(false);
                speed = 0;
                break;
        }

        return false;
    }

    void Zoomed()
    {
        Vector3 tempPos = transform.position;
        if (isZoom)
        {
            float zoomRange = transform.position.x - zoomAreaX;
            float zoomPercent = Mathf.Ceil(zoomRange / zoomAreaSize * 100) * 0.01f;
            
            if (zoomPercent < zoomStartRangePercent)
            {
                tempPos.y = originPos.y + (zoomHeight * (zoomPercent / zoomStartRangePercent));
                tempPos.z = originPos.z - (zoomDeep * (zoomPercent / zoomStartRangePercent));
            }
            else if(zoomPercent <= (1 - zoomStartRangePercent))
            {
                tempPos.y = originPos.y + zoomHeight;
                tempPos.z = originPos.z - zoomDeep;
            }
            else
            {
                tempPos.y = originPos.y + (zoomHeight * ((1 - zoomPercent) / zoomStartRangePercent));
                tempPos.z = originPos.z - (zoomDeep * ((1 - zoomPercent) / zoomStartRangePercent));
            }
        }
        else
        {
            tempPos.y = originPos.y;
            tempPos.z = originPos.z;
        }
        transform.position = Vector3.Lerp(transform.position, tempPos, speed * Time.deltaTime);
    }

    public void ActiveLine()
    {
        inLine_L.SetActive(true);
        inLine_R.SetActive(true);
        Outside_L.SetActive(true);
        Outside_R.SetActive(true);
    }


    // Use this for initialization
    void Start () {
        playerTr = PlayerCtrl.instance.transform;
        inLine_L = GameObject.Find("inLine_L");
        inLine_R = GameObject.Find("inLine_R");
        Outside_L = GameObject.Find("Outside_L");
        Outside_R = GameObject.Find("Outside_R");

        inLine_L_Tr = inLine_L.transform;
        inLine_R_Tr = inLine_R.transform;

        heightGap = transform.position.y - playerTr.position.y;
        originPos = transform.position;
        originPos.y -= playerTr.position.y;
    }


    void Update()
    {
        ShotRay();
        
    }

	// Update is called once per frame
	void LateUpdate() {
        
        MoveInsideLine();
//        Zoomed();
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

    
    RaycastHit hit;
    void ShotRay()
    {
        Debug.DrawRay(transform.position, -(transform.position - rayPos.position), Color.yellow);
        if (Physics.Raycast(transform.position, -(transform.position - rayPos.position),out hit))
        {
            if (!isZoom && hit.transform.CompareTag("ZoomArea"))
            {
                isZoom = true;
                zoomAreaSize = hit.transform.localScale.x;
                zoomAreaX = hit.transform.position.x - (zoomAreaSize * 0.5f);
                zoomDeep = hit.transform.GetComponent<ZoomArea>().zoomDeep;
                zoomHeight = hit.transform.GetComponent<ZoomArea>().zoomHeight;
            }
        }
        else
        {
            isZoom = false;
            Debug.Log("없음");
        }
    }
}
