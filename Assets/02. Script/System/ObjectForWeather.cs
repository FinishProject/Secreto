using UnityEngine;
using System.Collections;

public class ObjectForWeather : MonoBehaviour, WorldObserver
{
    WorldSubject worldData;
    WeatherState weatherState;
    float weatherValue;

    private Transform tr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();

        // 옵저버 등록
        worldData = WorldCtrl.GetInstance().RetrunThis();
        worldData.registerObserver(this);
    }
	
	// Update is called once per frame
	void FixedUpdate() {

        if ((WeatherState.NONE & weatherState) == WeatherState.NONE)
        {

        }
        if ((WeatherState.WIND & weatherState) == WeatherState.WIND)
        {
            tr.position = Vector3.Lerp(tr.position, tr.position - (Vector3.right * weatherValue), Time.deltaTime);
        }
        if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN)
        {

        }
    }

    public void updateObserver(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
        this.weatherValue = weatherValue;
    }
}
