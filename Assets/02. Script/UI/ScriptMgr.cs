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
    public int quesetType;
    public string targetName;
    public int completNum;
    public int yes, no;
}

enum ScriptState
{
    basic, answer, refuse, end,
}

public class ScriptMgr : MonoBehaviour {

    public Text txtUi; // 대사 텍스트 출력 UI
    public GameObject bgUi; // 대사 출력 배경 UI
    public GameObject answerUi; // 선택지 UI
    
    public bool isQuest = false; // 퀘스트 완료 여부
    public static int curIndex = -1; // 현재 보여줄 대사 인덱스
    private bool isAnswer = false; // 현재 선택지 출력중 여부

    private List<Script> scriptData = new List<Script>(); //XML 데이터 저장
    public List<Script> scriptInfo = new List<Script>(); //현재 NPC의 대사를 저장
    private List<string> spokeNpc = new List<string>(); // 만난 NPC이름 저장
    private List<QuestInfo> questInfo = new List<QuestInfo>(); // 퀘스트 정보 저장

    public static ScriptMgr instance;

    public bool isAnimQuest = false;

    void Awake()
    {
        instance = this;
        bgUi.SetActive(false);
        answerUi.SetActive(false);
        scriptData =  PlayerData.LoadScript(); // 대사 XML 문서 불러오기
        spokeNpc = PlayerData.LoadSpokeNpc(); // 이미 대화한 NPC 이름 불러오기
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
                    scriptInfo.Add(new Script
                    {
                        name = name,
                        context = scriptData[i].context,
                        scriptType = scriptData[i].scriptType,
                        yes = scriptData[i].yes,
                        no = scriptData[i].no
                    });
                    // 퀘스트 정보를 저장
                    questInfo.Add(new QuestInfo
                    {
                        questType = scriptData[i].quesetType,
                        targetName = scriptData[i].targetName,
                        completNum = scriptData[i].completNum,
                    });
                }
            }
            PlayerCtrl.instance.isMove = false;
            StartCoroutine(SpeakingNPC());
        }
        // 퀘스트 수락 후 완료 시
        else if(SpeakName(name) && isQuest)
        {
            StartCoroutine(SpeakingNPC());
        }
        else
        {
            Debug.Log("END SPEAK");
        }
    }
    // NPC와 대화
    IEnumerator SpeakingNPC()
    {
        bgUi.SetActive(true);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return) && !isAnswer)
            {
                if (!isQuest)
                {
                    curIndex++;
                    // 종료
                    if (curIndex >= scriptInfo.Count - 1)
                    {
                        bgUi.SetActive(false);
                        PlayerCtrl.instance.isMove = true;
                        curIndex = -1;
                        break;
                    }
                    // 거절
                    if (scriptInfo[curIndex].scriptType == (int)ScriptState.refuse)
                    {
                        curIndex = scriptInfo.Count - 1;
                    }
                    // 기본
                    else if (scriptInfo[curIndex].scriptType <= (int)ScriptState.answer)
                    {
                        txtUi.text = scriptInfo[curIndex].context;
                        // 선택지가 있음
                        if (scriptInfo[curIndex].scriptType == (int)ScriptState.answer)
                        {
                            StartCoroutine(Answer());
                        }
                    }
                }

                else if (isQuest)
                {
                    isAnimQuest = true;
                    curIndex = scriptInfo.Count - 1;
                    txtUi.text = scriptInfo[curIndex].context;
                    isQuest = false;
                }
            }
            
            yield return null;
        }
    }
    // 선택지 출력 
    IEnumerator Answer()
    {
        answerUi.SetActive(true);
        isAnswer = true;
        while (isAnswer)
        {
            // 수락
            if (Input.GetKeyDown(KeyCode.Z))
            {
                curIndex = (int)scriptInfo[curIndex].yes;
                if(scriptInfo[curIndex].quesetType >= 0)
                {
                    QuestMgr.instance.GetQuestInfo(questInfo[curIndex]); // 퀘스트 정보를 건내줌
                    spokeNpc.Add(scriptInfo[curIndex].name); // 대화를 한 NPC 이름을 저장함
                }
                isAnswer = false;
            }
            // 거절
            else if (Input.GetKeyDown(KeyCode.X))
            {
                curIndex = scriptInfo[curIndex].no;
                isAnswer = false;
            }
            yield return null;
        }
        answerUi.SetActive(false);
        txtUi.text = scriptInfo[curIndex].context;
    }

    //이미 대화한 NPC인지 확인
    bool SpeakName(string name)
    {
        for (int i = 0; i < spokeNpc.Count; i++)
        {
            if (spokeNpc[i] == name) return true;
        }
        return false;
    }

    //대화 완료한 NPC이름 저장
    public void SaveSpokenNpc()
    {
        XmlDocument doc = new XmlDocument();
        XmlElement scriptElement = doc.CreateElement("Script");
        doc.AppendChild(scriptElement);

        XmlElement scriptSpeak = doc.CreateElement("SpokeNPC");
        for (int i = 0; i < spokeNpc.Count; i++) {
            scriptSpeak.SetAttribute("Spoke_NPC", spokeNpc[i].ToString());
        }
        scriptElement.AppendChild(scriptSpeak);

        doc.Save(Application.dataPath + "/Resources/SpokeNpcName.xml");
    }
}
