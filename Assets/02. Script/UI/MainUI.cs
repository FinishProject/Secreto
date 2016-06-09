using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUI : MonoBehaviour {

    public Image pressAnyKey;
    public GameObject menu;
    public GameObject exitPopup;

    // Use this for initialization
    void Start () {
        menu.SetActive(false);
        ClossExitPopup();
        StartCoroutine(PressAnyKey());
    }

    IEnumerator PressAnyKey()
    {
        while(true)
        {
            if (Input.anyKeyDown)
            {
                menu.SetActive(true);
                pressAnyKey.enabled = false;
                break;
            }

            yield return null;
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

    public void ClossExitPopup()
    {
        exitPopup.SetActive(false);
    }

    public void OpenExitPopup()
    {
        exitPopup.SetActive(true);
    }
}
