using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LoadingScreen : MonoBehaviour
{
    public Text text;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {

        AsyncOperation async = Application.LoadLevelAsync("0601");

        while (!async.isDone)
        {
            float progress = async.progress * 100.0f;
            int pRounded = Mathf.RoundToInt(progress);
            text.text = "Loading…"+pRounded.ToString() + "%";

            yield return true;
        }
    }

}