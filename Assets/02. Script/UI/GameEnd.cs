using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameEnd : MonoBehaviour {

    Image BlackImg;
    float alpha;

    void OnEnable()
    {
        Debug.Log(3);
        BlackImg = gameObject.GetComponent<Image>();
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {

        while (alpha < 1)
        {
            alpha += 0.5f * Time.deltaTime;
            BlackImg.color = new Color(0, 0, 0, alpha);
            Debug.Log(alpha);
            yield return true;
        }


        Application.LoadLevel("MainScene");


    }
}
