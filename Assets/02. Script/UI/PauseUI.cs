using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePopup;
    public GameObject exitPopup;
    public GameObject selectButton;
    public GameObject menu;
    int curSelectIdx;
    Transform[] menuButtons;

    // Use this for initialization
    void Start()
    {
        curSelectIdx = 1;
        menuButtons = menu.GetComponentsInChildren<Transform>();
        ClossExitPopup();
        ClossTitlePopup();

        StartCoroutine(SelectMenu());
    }

    IEnumerator SelectMenu()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && curSelectIdx > 1)
                curSelectIdx--;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && curSelectIdx < 5)
                curSelectIdx++;
            else if (Input.GetKeyUp(KeyCode.Space))
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
            case 1: break;
            case 2: break;
            case 3: break;
            case 4: OpenTitlePopup(); break;
            case 5: OpenExitPopup(); break;

        }
    }


    void OnEnable()
    {
        Time.timeScale = 0;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ClossExitPopup()
    {
        exitPopup.SetActive(false);
    }

    public void OpenExitPopup()
    {
        exitPopup.SetActive(true);
    }

    public void ToTitleScene()
    {
        Application.LoadLevel("MainScene");
    }

    public void ClossTitlePopup()
    {
        pausePopup.SetActive(false);
    }

    public void OpenTitlePopup()
    {
        pausePopup.SetActive(true);
    }
}
