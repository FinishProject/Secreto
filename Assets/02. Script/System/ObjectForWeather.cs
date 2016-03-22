using UnityEngine;
using System.Collections;

public class ObjectForWeather : MonoBehaviour, WorldObserver
{
    public bool isInfluence_Wind;
    public bool isInfluence_Rain;

    private WorldSubject worldData;
    private WeatherState weatherState;
    private float weatherValue;
    private Transform tr;

    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();

        // 옵저버 등록
        worldData = WorldCtrl.GetInstance().RetrunThis();
        worldData.registerObserver(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 날씨 없음
        if ((WeatherState.NONE & weatherState) != 0)
        {

        }

        // 바람 _ 좌우
        if ((WeatherState.WIND_LR & weatherState) != 0 && isInfluence_Wind)
        {
            tr.position = Vector3.Lerp(tr.position, tr.position - (Vector3.right * weatherValue), Time.deltaTime);
        }

        // 비
        if ((WeatherState.RAIN & weatherState) != 0 && isInfluence_Rain)
        {
            Vector3 tempScale = tr.transform.localScale;
            Vector3 tempPos = tr.transform.position;
            tempScale.y += 0.02f;
            tr.transform.localScale = tempScale;
        }
    }

    public void updateObserver(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
        this.weatherValue = weatherValue;
    }
}
