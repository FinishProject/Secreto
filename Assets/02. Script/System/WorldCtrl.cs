using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeatherState
{
    NONE = 0x1, WIND_LR = 0x2, WIND_UD = 0x4, RAIN = 0x8,
}

public interface WorldObserver
{
    void updateObserver(WeatherState state, float value);
}

public interface WorldSubject
{
    void registerObserver(WorldObserver o);
    void removeObserver(WorldObserver o);
    void notifyObservers();
}

public partial class WorldCtrl : MonoBehaviour, WorldSubject
{
    List<WorldObserver> observers;
    private WeatherState weatherState;
    private float weatherValue;
    bool isWinding = false;

    // 옵저버 추가
    public void registerObserver(WorldObserver o)
    {
        observers.Add(o);
    }

    // 옵저버 삭제
    public void removeObserver(WorldObserver o)
    {
        if (observers.Count >= 0)
            observers.Remove(o);
    }

    // 옵저버들에게 통지
    public void notifyObservers()
    {
        WorldObserver observer;

        for (int i = 0; i < observers.Count; i++)
        {
            observer = observers[i];
            observer.updateObserver(weatherState, weatherValue);
        }
    }

    // 날씨 입력
    public void WeatherCtrl(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
        this.weatherValue = weatherValue;
        changeWeather();
    }

    // 날씨 변경
    public void changeWeather()
    {
        notifyObservers();
    }

    // 날씨 효과 초기화
    IEnumerator WeatherReset(float timer)
    {
        yield return new WaitForSeconds(timer);
        WeatherCtrl(WeatherState.NONE, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeatherCtrl(WeatherState.WIND_LR, 4);
            StartCoroutine(WeatherReset(3.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeatherCtrl(WeatherState.RAIN, 4);
            StartCoroutine(WeatherReset(4.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeatherCtrl(WeatherState.WIND_LR | WeatherState.RAIN, 4);
            StartCoroutine(WeatherReset(4.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WeatherCtrl(WeatherState.WIND_UD,4);
            StartCoroutine(WeatherReset(8.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            WeatherCtrl(WeatherState.NONE, 0);
        }
    }
}

// 싱글톤 / 초기화
public partial class WorldCtrl : MonoBehaviour, WorldSubject
{
    private static WorldCtrl instance;
    private static GameObject container;
    public static WorldCtrl GetInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "WorldCtrl";
            instance = container.AddComponent(typeof(WorldCtrl)) as WorldCtrl;

        }
        return instance;
    }

    public WorldCtrl()
    {
        observers = new List<WorldObserver>();
    }

    public WorldCtrl RetrunThis()
    {
        return this;
    }
}