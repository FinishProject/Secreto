using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    Image BlackImg;
    float alpha;
    bool trigger;

    public static FadeInOut instance;
    void Start()
    {
        instance = this;
        BlackImg = gameObject.GetComponent<Image>();
        trigger = false;
    }

    public void StartFadeInOut(float fadeInTime, float waitTime, float fadeOutTime)
    {
        if(!trigger)
            StartCoroutine(Load(fadeInTime, waitTime, fadeOutTime));
    }

    IEnumerator Load(float fadeInTime, float waitTime, float fadeOutTime)
    {
        trigger = true;
        while (alpha < 1)
        {
            alpha += (1 / fadeInTime) * Time.deltaTime;
            BlackImg.color = new Color(0, 0, 0, alpha);
            yield return true;
        }

        yield return new WaitForSeconds(waitTime);

        while (alpha > 0)
        {
            alpha -= (1 / fadeOutTime) * Time.deltaTime;
            BlackImg.color = new Color(0, 0, 0, alpha);
            yield return true;
        }
        trigger = false;
    }
}
