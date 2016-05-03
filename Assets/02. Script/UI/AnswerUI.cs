using UnityEngine;
using System.Collections;

public class AnswerUI : MonoBehaviour {

    public GameObject[] image;
    public static int Length = 0;
    private int index = 0;
    private int curIndex = 0;

    private Vector3 originScale;

    void Awake()
    {
        originScale = image[0].transform.localScale;
    }

    void OnEnable()
    {
        StartCoroutine(StartUi());
    }

    IEnumerator StartUi()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ChoiceAnswer());
    }

    IEnumerator ChoiceAnswer()
    {
        while (true)
        {
            // 위 아래로 선택지를 고를 수 있음
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (index > 0)
                    index--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index < image.Length - 1)
                    index++;
            }
            // 선택지 선택
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                // 거절 시
                if (index == 1)
                {
                    ScriptMgr.curIndex = Length - 2;
                    ScriptMgr.instance.txt.text = ScriptMgr.instance.scriptInfo[ScriptMgr.curIndex];
                }
                // 수락 시
                else {
                    ScriptMgr.curIndex += index + 1;
                }
                gameObject.SetActive(false);
            }
            // 현재 선택중인 선택지의 효과를 줌
            CurChoiceAnswer();

            yield return null;
        }
    }
    // 현재 선택중인 선택지의 효과를 줌
    void CurChoiceAnswer()
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (i == index)
            {
                image[index].transform.localScale = new Vector3(0.15f, 0.3f, 1f);
            }
            else
            {
                image[i].transform.localScale = originScale;
            }
        }
    }
}
