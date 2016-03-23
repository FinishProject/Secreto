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
    private bool isCtrlAuthority = true; // 조작권한 (로프 조종)

    private SwitchObject switchState;

    void Awake()
    {
        switchState = gameObject.AddComponent<SwitchObject>();
        switchState.IsCanUseSwitch = false;

        // 옵저버 등록
        worldData = WorldCtrl.GetInstance().RetrunThis();
        worldData.registerObserver(this);
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

    public void updateObserver(WeatherState state, float value)
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.name == "Leaf")
        {
            Destroy(coll.gameObject);
            isUsingLeaf = true;
            carryItemName = coll.name;
            StartCoroutine(LeafDestroy());
        }

        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
            switchState = coll.GetComponent<SwitchObject>();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = false;
            switchState = gameObject.AddComponent<SwitchObject>();
        }
    }

    // 임시
    IEnumerator LeafDestroy()
    {
        yield return new WaitForSeconds(LeafTimer);
        carryItemName = null;
        isUsingLeaf = false;
    }
}
