using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainUI_2 : MonoBehaviour
{
    [System.Serializable]
    public struct Cloudes
    {
        public Transform cloudeTr;
        public float speed;
        public float length;
    };
    [System.Serializable]
    public struct OlaInfo
    {
        public Transform olaTr;
        public float speed;
        public float length;
    }

    public Cloudes[] cloude = new Cloudes[3];
    private RectTransform rectTr;

    public OlaInfo ola;

    public Image pressAnyKey;
    public GameObject selectButton;
    public GameObject menu;
    int curSelectIdx;
    Transform[] menuButtons;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();

        curSelectIdx = 1;
        menuButtons = menu.GetComponentsInChildren<Transform>();
        selectButton.SetActive(false);
        menu.SetActive(false);
        StartCoroutine(PressAnyKey());
    }

    void Update()
    {
        CloudeMove();
        OlaMove();
    }

    void CloudeMove()
    {
        for(int i=0; i<cloude.Length; i++)
        {
            cloude[i].cloudeTr.Translate(Vector3.right * cloude[i].speed * Time.deltaTime);

            if(cloude[i].cloudeTr.position.x >= rectTr.rect.width + cloude[i].length)
            {
                cloude[i].cloudeTr.position = 
                    new Vector3(-rectTr.rect.width - cloude[i].length, cloude[i].cloudeTr.position.y, cloude[i].cloudeTr.position.z);
            }
        }
    }

    void OlaMove()
    {
        float moveSpeed = Mathf.Sin(Time.time * ola.speed) * ola.length;

        ola.olaTr.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    IEnumerator PressAnyKey()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                menu.SetActive(true);
                selectButton.SetActive(true);
                pressAnyKey.enabled = false;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SelectMenu());
                break;
            }
            yield return null;
        }
    }

    IEnumerator SelectMenu()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && curSelectIdx > 1)
                curSelectIdx--;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && curSelectIdx < 2)
                curSelectIdx++;
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                ConnectFunction(curSelectIdx);

            else
            {
                Vector3 tempPos = selectButton.transform.position;
                tempPos.y = menuButtons[curSelectIdx].position.y;
                selectButton.transform.position = tempPos;
            }

            yield return null;
        }
    }

    void ConnectFunction(int curIdx)
    {
        switch (curIdx)
        {
            case 1: StartCoroutine(StartNewGame()); break;
            case 2: ExitGame(); break;
        }
    }

    IEnumerator StartNewGame()
    {
        while (true)
        {
            float fadeAlpha = GetComponent<Fade>().BeginFade(1);

            if (fadeAlpha.Equals(1))
            {
                Application.LoadLevel(Application.loadedLevel + 1);
            }

            yield return null;
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
