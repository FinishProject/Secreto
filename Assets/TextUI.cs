using UnityEngine;
using System.Collections;

public class TextUI : MonoBehaviour {

    float i = 10f;
    float j = 1.5f;

    void Update()
    {
        PlayerCtrl.instance.speed = i;
        PlayerCtrl.instance.jumpHight = j;
    }

	void OnGUI()
    {
        GUI.TextArea(new Rect(0, 1f, 100f, 25f), "이동 속도");
        i = float.Parse(GUI.TextArea(new Rect(0, 25, 100, 20), i.ToString("0.00")));
        GUI.TextField(new Rect(0, 50f, 100f, 25f), "점프 높이");
        j = float.Parse(GUI.TextArea(new Rect(0, 75f, 100, 20), j.ToString("0.00")));
    }

    /*
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
*/
}
