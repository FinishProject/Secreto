using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

// 스크립트 정보들
public class Script
{
    public string name;
    public string context;
    public int scriptType;
    public int speaker;
    public string questType;
    public string questTarget;
    public int completeNum;
}

enum ScriptState
{
    basic, answer, refuse, end,
}

public class ScriptMgr : MonoBehaviour {

    public Text[] txtUi; // 대사 텍스트 출력 UI
    public GameObject[] bgUi; // 대사 출력 배경 UI
    
    public bool isQuest = false; // 퀘스트 완료 여부
    public static int curIndex = 0; // 현재 보여줄 대사 인덱스

    private List<Script> scriptData = new List<Script>(); //XML 데이터 저장
    public List<Script> curScript = new List<Script>(); //현재 NPC의 대사를 저장
    private List<string> spokeNpc = new List<string>(); // 만난 NPC이름 저장
    private List<QuestInfo> questInfo = new List<QuestInfo>(); // 퀘스트 정보 저장

    public static ScriptMgr instance;

    public bool isAnimQuest = false;

    void Awake()
    {
        instance = this;
        for(int i=0; i<bgUi.Length; i++)
        {
            bgUi[i].SetActive(false);
        }
        scriptData =  PlayerData.LoadScript(); // 대사 XML 문서 불러오기
        spokeNpc = PlayerData.LoadNpcName(); // 이미 대화한 NPC 이름 불러오기
    }

    // NPC 이름에 해당하는 대사들과 퀘스트 정보를 가져옴
    public void GetScript(string name)
    {
        // 이전에 대화를 하지 않는 NPC일 경우 대화를 위해 정보들을 가져옴
        if (!SpeakName(name))
        {
            for (int i = 0; i < scriptData.Count; i++)
            {
                if (scriptData[i].name == name)
                {
                    // 대사 정보들을 저장
                    curScript.Add(new Script
                    {
                        name = name,
                        context = scriptData[i].context,
                        scriptType = scriptData[i].scriptType,
                        speaker = scriptData[i].speaker,
                    });
                    // 퀘스트 정보를 저장
                    questInfo.Add(new QuestInfo
                    {
                        questType = scriptData[0].questType,
                        targetName = scriptData[0].questTarget,
                        completNum = scriptData[0].completeNum,
                    });
                }
            }
            spokeNpc.Add(name);
            QuestMgr.instance.GetQuestInfo(questInfo[0]);
            StartCoroutine(SpeakingNPC());
        }
        // 퀘스트 수락 후 완료 시
        else if (SpeakName(name) && isQuest)
        {
            for (int i = 0; i < scriptData.Count; i++)
            {
                if (scriptData[i].name == name && scriptData[i].scriptType == 1)
                {
                    // 대사 정보들을 저장
                    curScript.Add(new Script
                    {
                        name = name,
                        context = scriptData[i].context,
                        scriptType = scriptData[i].scriptType,
                        speaker = scriptData[i].speaker,
                    });
                }
            }
            StartCoroutine(SpeakingNPC());
        }
        else if (SpeakName(name) && !isQuest) 
        {
            Debug.Log("END SPEAK");
        }
    }
    // NPC와 대화
    IEnumerator SpeakingNPC()
    {
        ShowUI();
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(curIndex < curScript.Count)
                    curIndex++;
                Debug.Log(curIndex);
                // 퀘스트 미완료 시
                if (curScript[curIndex].scriptType == 1 && isQuest)
                {
                    ShowUI();
                }
                else if (curScript[curIndex].scriptType == 0 && !isQuest)
                {
                    ShowUI();
                }
                else
                {
                    curScript.Clear();
                    PlayerCtrl.instance.isMove = true;
                    curIndex = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        bgUi[i].SetActive(false);
                    }
                    break;
                }


                //}
                //// 퀘스트 완료 시
                //else if (isQuest)
                //{
                //    if (curScript[curIndex].scriptType == 1 && isQuest)
                //    {
                //        ShowUI();
                //    }
                //    else
                //    {
                //        Debug.Log("End");
                //        curScript.Clear();
                //        PlayerCtrl.instance.isMove = true;
                //        curIndex = 0;
                //        isQuest = false;
                //        for (int i = 0; i < 2; i++)
                //        {
                //            bgUi[i].SetActive(false);
                //        }
                //        break;
                //    }
                //}
            }
            yield return null;
        }
    }

    void ShowUI()
    {
        if (curScript[curIndex].speaker == 0)
        {
            bgUi[1].SetActive(false);
            bgUi[0].SetActive(true);
            txtUi[0].text = curScript[curIndex].context;
        }
        else if(curScript[curIndex].speaker == 1)
        {
            bgUi[0].SetActive(false);
            bgUi[1].SetActive(true);
            txtUi[1].text = curScript[curIndex].context;
        }
        
    }
  
    //이미 대화한 NPC인지 확인
    public bool SpeakName(string name)
    {
        for (int i = 0; i < spokeNpc.Count; i++)
        {
            if (spokeNpc[i] == name)
                return true;
        }
        return false;
    }

    // 대화한 NPC 이름 저장
    void SaveNpcName()
    {
        PlayerData.SaveNpcName(spokeNpc);
    }

    void OnEnable()
    {
        WayPoint.OnSave += SaveNpcName;
    }
}
