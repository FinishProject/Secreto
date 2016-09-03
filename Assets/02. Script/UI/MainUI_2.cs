﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUI_2 : MonoBehaviour
{

    public Image pressAnyKey;
    public GameObject selectButton;
    public GameObject menu;
    int curSelectIdx;
    Transform[] menuButtons;

    // Use this for initialization
    void Start()
    {
        curSelectIdx = 1;
        menuButtons = menu.GetComponentsInChildren<Transform>();
        selectButton.SetActive(false);
        menu.SetActive(false);
        StartCoroutine(PressAnyKey());
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
            case 1: StartNewGame(); break;
            case 2: ExitGame(); break;

        }
    }

    public void StartNewGame()
    {
        Application.LoadLevel("LoadingScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
