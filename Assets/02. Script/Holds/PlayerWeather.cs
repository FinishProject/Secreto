using UnityEngine;
using System.Collections;
using System;

public class PlayerWeather : MonoBehaviour, WorldObserver
{
    WorldSubject worldData;
    WeatherState weatherState;
    private float weatherValue;

    private bool isUsingLeaf = false; // 나뭇잎 쓰고 있니?
    public float LeafTimer = 10.0f;
    private string carryItemName = null;
    private bool isLocked_WIND_UD = false;    // 데이터 한번만 보내야할때 잠금 역할
    private bool isLocked_NONE = false;    // 데이터 한번만 보내야할때 잠금 역할

    //private SwitchObject switchState;

    void Awake()
    {
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
        if ((WeatherState.NONE & weatherState) == WeatherState.NONE)
        {
            if(!isLocked_NONE)
            {
                PlayerCtrl.instance.jumpState = JumpType.IDLE;
                PlayerCtrl.instance.moveResistant = 0;
                isLocked_WIND_UD = false;
                isLocked_NONE = true;
            }
        }
        else
        {
            isLocked_NONE = false;
        }

        // 바람 영향 받을 시(좌우)
        if ((WeatherState.WIND_LR & weatherState) == WeatherState.WIND_LR)
        {
            PlayerCtrl.instance.controller.Move(-Vector3.right * weatherValue * Time.deltaTime);
        }
        // 바람 영향 받을 시(상하) 나뭇잎 보유했을때
        if ((WeatherState.WIND_UD & weatherState) == WeatherState.WIND_UD && isUsingLeaf)
        {
            if(!isLocked_WIND_UD)
            {
                PlayerCtrl.instance.jumpState = JumpType.FLY_IDLE;
                isLocked_WIND_UD = true;
            }
        }
        // 비 영향 받을 시 
        if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN && !isUsingLeaf)
        {
            PlayerCtrl.instance.moveResistant = weatherValue;
        }
    }

    public void updateObserver(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
        this.weatherValue = weatherValue;
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
    }

    

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
