using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour {

    public Image shiftImg;
    private float fadeSpeed = 2f;
    private bool isActive = true;

    public float alpha = 0f;
    public static ShowUI instanace;

    void Awake()
    {
        instanace = this;
    }

    public void OnImage(float fadeDir)
    {
        if (fadeDir == -1)
            StartCoroutine(OffImage());

        Color imgColor = shiftImg.material.color;
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        imgColor.a = alpha;

        shiftImg.material.color = imgColor;
    }

    IEnumerator OffImage()
    {
        while (alpha > 0f)
        {
            Color imgColor = shiftImg.material.color;
            alpha -= fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            Debug.Log(alpha);
            imgColor.a = alpha;

            shiftImg.material.color = imgColor;

            yield return null;
        }
        Debug.Log("End");
    }
}
