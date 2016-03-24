using UnityEngine;
using System.Collections;
using System;

public class PlayerWeather : MonoBehaviour, WorldObserver
{
    WorldSubject worldData;
    WeatherState weatherState;

    private bool isUsingLeaf = false; // 나뭇잎 쓰고 있니?
    public float LeafTimer = 10.0f;
    private string carryItemName = null;

    //private SwitchObject switchState;

    void Awake()
    {
        //switchState = gameObject.AddComponent<SwitchObject>();
        //switchState.IsCanUseSwitch = false;

        // 옵저버 등록
        worldData = WorldCtrl.GetInstance().RetrunThis();
        worldData.registerObserver(this);
    }

    void Update()
    {
        WeatherType();
    }
    // 날씨 종류
    void WeatherType()
    {
        //if ((WeatherState.NONE & weatherState) == WeatherState.NONE)
        //{
        //    Debug.Log("11");
        //}
        // 바람 영향 받을 시
        if ((WeatherState.WIND_LR & weatherState) == WeatherState.WIND_LR)
        {
            PlayerCtrl.instance.controller.Move(-Vector3.right * 8f * Time.deltaTime);
        }
        // 나뭇잎 습득
        else if ((WeatherState.WIND_UD & weatherState) == WeatherState.WIND_UD && isUsingLeaf)
        {
            Debug.Log("Get Leaf");
        }
        // 비 영향 받을 시 
        else if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN && !isUsingLeaf)
        {
            Debug.Log("Rain && Not Leaf");
        }
    }

    public void updateObserver(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
    }

    // 임시
    IEnumerator LeafDestroy()
    {
        yield return new WaitForSeconds(LeafTimer);
        carryItemName = null;
        isUsingLeaf = false;
    }


    void OnTriggerEnter(Collider coll)
    {
        //나뭇잎 습득
        if (coll.name == "Leaf")
        {
            Destroy(coll.gameObject);
            isUsingLeaf = true;
            carryItemName = coll.name;
            StartCoroutine(LeafDestroy());
        }

        //if (coll.name == "Switch")
        //{
        //    coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
        //    switchState = coll.GetComponent<SwitchObject>();
        //}
    }

    //void OnTriggerExit(Collider coll)
    //{
    //    if (coll.name == "Switch")
    //    {
    //        coll.GetComponent<SwitchObject>().IsCanUseSwitch = false;
    //        switchState = gameObject.AddComponent<SwitchObject>();
    //    }
    //}

    void OnGUI()
    {
        string tempText;
        tempText = "바람 : ";
        if ((WeatherState.WIND_LR & weatherState) == WeatherState.WIND_LR)
            tempText += "L/R  ";
        else if ((WeatherState.WIND_UD & weatherState) == WeatherState.WIND_UD)
            tempText += "U/D  ";
        else
            tempText += "OFF  ";

        tempText += "비  : ";
        if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN)
            tempText += "ON   ";
        else
            tempText += "OFF  ";

        if (isUsingLeaf)
            tempText += "나뭇잎 사용 O";
        else
            tempText += "나뭇잎 사용 X";

        GUI.TextField(new Rect(0, 0, 300.0f, 30.0f), tempText);
    }

}
