using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutScene : MonoBehaviour {
    public float normalTransTime;

    [System.Serializable]
    public struct ImgInfo
    {
        public Image cutImg;
        public float transTime;
        public bool playFadeOut;
    }
    public ImgInfo[] imgInfo; 

    private int index;
    private int imgCnt;

	void Start () {
        index = 0;
        
        imgCnt = imgInfo.Length;
        
        imgInfo[0].cutImg.enabled = true;
        for (int i = 1; i < imgCnt; i++)
            imgInfo[i].cutImg.enabled = false;

        StartCoroutine(SlideShow());
    }


    IEnumerator SlideShow()
    {
        while (true)
        {
            Debug.Log(index + 1);
            

            if (imgInfo[index].transTime > 0)
            {
                yield return new WaitForSeconds(imgInfo[index].transTime);
            }
            else
            {
                yield return new WaitForSeconds(normalTransTime);
            }

            if (imgInfo[index].playFadeOut)
            {
                FadeInOut.instance.StartFadeInOut(0.5f, 0.3f, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }

            if (imgCnt <= index + 1)
            {
                Application.LoadLevel("MainScene");
                yield break;
            }

            imgInfo[index++].cutImg.enabled = false;
            imgInfo[index].cutImg.enabled = true;
        }
    }
}
