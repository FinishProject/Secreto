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
            // 선택지 선택

            // 거절 시
            if (Input.GetKeyDown(KeyCode.X))
            {
                ScriptMgr.curIndex = Length - 2;
                ScriptMgr.instance.txt.text = ScriptMgr.instance.scriptInfo[ScriptMgr.curIndex];
                gameObject.SetActive(false);
            }
            // 수락 시
            else if(Input.GetKeyDown(KeyCode.Z)){
                ScriptMgr.curIndex += index + 1;
                gameObject.SetActive(false);
            }
            

            // 현재 선택중인 선택지의 효과를 줌
            //CurChoiceAnswer();

            yield return null;
        }
    }
    // 현재 선택중인 선택지의 효과를 줌
    //void CurChoiceAnswer()
    //{
    //    for (int i = 0; i < image.Length; i++)
    //    {
    //        if (i == index)
    //        {
    //            image[index].transform.localScale = new Vector3(1.0f, 0.3f, 1f);
    //        }
    //        else
    //        {
    //            image[i].transform.localScale = originScale;
    //        }
    //    }
    //}
}
