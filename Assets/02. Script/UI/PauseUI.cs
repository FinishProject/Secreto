using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePopup;
    public GameObject exitPopup;

    // Use this for initialization
    void Start()
    {
        ClossExitPopup();
        ClossTitlePopup();
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
