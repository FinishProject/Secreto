using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogoCtrl : MonoBehaviour {

    public float speed = 0.1f;

    private bool isChange = true;
    private int index = 0;

    public Image[] logoImg;
    private Color c;

    // Use this for initialization
    void Start () {
       
        for(int i=0; i<logoImg.Length; i++)
        {
            c = logoImg[i].color;
            c.a = 0;
            logoImg[i].color = c;
        }
        StartCoroutine(SchoolLogo());
	}

    IEnumerator SchoolLogo()
    {
        while (true)
        {
            if (logoImg[index].color.a >= 1)
            {
                //yield return new WaitForSeconds(1f);
                speed *= -1f;
            }
            c.a += speed * Time.deltaTime;
            logoImg[index].color = c;

            if (logoImg[index].color.a <= 0)
            {
                c.a = 0;
                speed *= -1f;
                index++;

                if (index >= logoImg.Length)
                {
                    Application.LoadLevel("MainScene");
                }
            }
            yield return null;
        }
    }
}
