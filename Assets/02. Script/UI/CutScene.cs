using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutScene : MonoBehaviour {
    public Image[] cutImages;

    private int index;
    private int imgCnt;

	void Start () {
        index = 0;

        imgCnt = cutImages.Length;

        cutImages[0].enabled = true;
        for (int i = 1; i < imgCnt; i++)
            cutImages[i].enabled = false;
    }
	
	void Update () {
        
	    if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ViewImage(false);
        }
        else if (Input.anyKeyDown)
        {
            ViewImage(true);
        }

	}

    void ViewImage(bool isNext)
    {
        if(isNext)
        {
            if (imgCnt <= index + 1)
            {
                Application.LoadLevel("MainScene");
                return;
            }

            cutImages[index++].enabled = false;
            cutImages[index].enabled = true;
        }
        
        else if (index > 0)
        {
            cutImages[index--].enabled = false;
            cutImages[index].enabled = true;
        }
    }
}
