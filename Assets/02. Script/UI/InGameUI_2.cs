using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUI_2 : MonoBehaviour {

    public GameObject gameEnd;
    public GameObject pauseUI;
    public static InGameUI_2 instance;

    public Image saveImg;
    public Image saveCircleImg;

    [System.Serializable]
    public struct SaveInfo
    {
        public int angle;
        public int speed;
    }
    public SaveInfo saveInfo;

    void Awake()
    {
        instance = this;
        pauseUI.SetActive(false);

        saveImg.enabled = false;
        saveCircleImg.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseUI.activeSelf)
        {
            pauseUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseUI.activeSelf)
        {
            // 일시정지 UI에서도 ESC입력 처리를 따로 해야 하므로 pauseUI.SetActive(false) 사용 안함 
            pauseUI.GetComponent<PauseUI_2>().ClosePauseUI();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AvtiveSave();
        }
    }

    public void GameEnd()
    {
        gameEnd.SetActive(true);
    }

    public void AvtiveSave()
    {
        StartCoroutine(Saved());
    }

    public IEnumerator Saved()
    {
        saveImg.enabled = true;
        saveCircleImg.enabled = true;
        float round = 0;

        while(true)
        {
            if (round > saveInfo.angle)
                break;

            saveCircleImg.transform.Rotate(new Vector3(0, 0, -saveInfo.speed));

            round += saveInfo.speed;
            yield return new WaitForSeconds(0.01f);
        }
        

        saveImg.enabled = false;
        saveCircleImg.enabled = false;
    }
}
