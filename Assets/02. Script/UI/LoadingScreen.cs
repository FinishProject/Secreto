using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LoadingScreen : MonoBehaviour
{
    public Text text;

    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {

        AsyncOperation async = Application.LoadLevelAsync("0613_copy");

        while (!async.isDone)
        {
            float progress = async.progress * 100.0f;
            int pRounded = Mathf.RoundToInt(progress);
//            text.text = "Loading…"+pRounded.ToString() + "%";

            yield return true;
        }
    }

}