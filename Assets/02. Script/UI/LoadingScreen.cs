﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LoadingScreen : MonoBehaviour
{
    public Text text;
    private string str0 = "Loading.";
    private string str1 = "Loading..";
    private string str2 = "Loading...";

    public float speed = 0.2f;

    void Start()
    {
        StartCoroutine(ShowText());
        AsyncOperation async = Application.LoadLevelAsync("0613_copy");
    }

    IEnumerator ShowText()
    {
        while (true)
        {
            text.text = str0;
            yield return new WaitForSeconds(speed);
            text.text = str1;
            yield return new WaitForSeconds(speed);
            text.text = str2;
            yield return new WaitForSeconds(speed);

            yield return null;
        }
    }

}