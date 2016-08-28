using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogoCtrl : MonoBehaviour {

    public float fadeSpeed = 0.1f;

    private bool isChange = true;
    private int imgCount = 0;

    public Image[] logoImg;
    private Color colorValue;

    // Use this for initialization
    void Start () {
       
        for(int i=0; i<logoImg.Length; i++)
        {
            colorValue = logoImg[i].color;
            colorValue.a = 0;
            logoImg[i].color = colorValue;
        }
	}

    void Update()
    {
        // 아무 키나 입력 시 다음 로고 이미지로 바꿈
        if (Input.anyKeyDown && imgCount < logoImg.Length)
        {
            colorValue.a = 0f;
            logoImg[imgCount].color = colorValue;
            imgCount++;
        }
        // 카운트가 이미지 갯수 초과시 씬 전환
        else if (imgCount >= logoImg.Length)
        {
            Application.LoadLevel("MainScene");
        }

        colorValue.a += fadeSpeed * Time.deltaTime;
        logoImg[imgCount].color = colorValue;

        // alpha 증가
        if (logoImg[imgCount].color.a >= 1)
        {
            fadeSpeed *= -1f;
        }

        // alpha 감소
        else if (logoImg[imgCount].color.a <= 0)
        {
            colorValue.a = 0;
            fadeSpeed *= -1f;
            imgCount++;
        }
    }
}
