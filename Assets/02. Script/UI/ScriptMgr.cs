using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

// 스크립트 정보들
class Script
{
    public string name;
    public string context;
    public int answer;
    public int choice;
    public int quesetType;
    public string targetName;
    public string completeItem;
    public int completNum; 
}

enum ScriptSate
{
    sTrue, sFalse, sReturn, sNull,
}

public class ScriptMgr : MonoBehaviour {

    QuestInfo info = new QuestInfo();
    // 선택지에 대한 상태
    ScriptSate choiceState = ScriptSate.sFalse;

    public Text txt; // 대사 텍스트 출력 UI
    public GameObject bgUi; // 대사 출력 배경 UI
    public GameObject answerUi; // 선택지 UI

    public TextAsset scirpt;

    public bool isQuest = false; // 퀘스트 완료 여부

    public static int curIndex = 0; // 현재 보여줄 대사 인덱스

    private List<Script> scriptData = new List<Script>(); //XML 데이터 저장
    public List<string> scriptInfo = new List<string>(); //현재 NPC의 대사를 저장
    private List<string> spokeNpc = new List<string>(); // 만난 NPC이름 저장

    private List<int> answerScript = new List<int>(); // 답변 상태 여부 저장
    private List<int> choiceScript = new List<int>(); // 선택지 상태 여부 저장
    
    public static ScriptMgr instance;



    // UI를 배치 했을때 해상도
    private float baseWidth = 600;
    private float baseHeight = 450;
    // 현재 해상도와 UI를 배치했을때 해상도의 비율
    private float proportionWidth;
    private float proportionHeight;
    public GameObject text;
    private Vector3 textPos;

    void Awake()
    {
        instance = this;

        bgUi.SetActive(false);
        answerUi.SetActive(false);

        LoadScript();

        textPos = text.transform.position;
        proportionWidth = baseWidth / Screen.width;
        proportionHeight = baseHeight / Screen.height;
        ChangeUIByResolution(text, textPos);
    }

    void ChangeUIByResolution(GameObject changeUI, Vector3 basePos)
    {
        Vector3 tempSize = new Vector3();
        Vector3 tempPos = new Vector3();

        // 크기 비율 맞춤
        tempSize.x = 1 / proportionWidth;
        tempSize.y = 1 / proportionHeight;

        // 위치 비율 맞춤
        tempPos.x = basePos.x / proportionWidth;
        tempPos.y = basePos.y / proportionHeight;

        // 적용
        changeUI.transform.position = tempPos;
        changeUI.transform.localScale = tempSize;

    }

    public bool GetScript(string name)
    {
        //대화할 NPC의 이름을 XML 데이터에서 찾아 대사를 getInfo에 저장
        if (!SpeakName(name))
        {
            string comItemName;
            for (int i = 0; i < scriptData.Count; i++)
            {
                if (scriptData[i].name == name)
                {
                    scriptInfo.Add(scriptData[i].context);
                    choiceScript.Add(scriptData[i].choice);
                    // 퀘스트 정보를 받아옴.
                    info.targetName = scriptData[0].targetName;
                    comItemName = scriptData[0].completeItem;
                    info.questType = scriptData[0].quesetType;
                    info.completNum = scriptData[0].completNum - 1;
                }
            }
            spokeNpc.Add(name); // 대화를 한 NPC 이름을 저장함
            curIndex = 0;
            bgUi.SetActive(true);
            QuestMgr.instance.GetQuestInfo(info); // 퀘스트 정보를 넘김
            AnswerUI.Length = scriptData.Count; // 현재 대사의 최대 길이(량)을 넘겨줌
        }
        
        //다음 대화가 있을 경우 대화 출력
        if(curIndex < scriptInfo.Count-2)
        {
            txt.text = scriptInfo[curIndex];
            choiceState = (ScriptSate)choiceScript[curIndex];
            // 거절 시 처음으로 돌아감
            if (choiceState == ScriptSate.sReturn)
            {
                curIndex = 0;
                bgUi.SetActive(false);
                return true;
            }
            // 선택지 찬성 시
            else if (choiceState == ScriptSate.sTrue)
            {
                answerUi.SetActive(true);
            }
            else {
                curIndex++;
            }
            return false;
        }
        //대화 종료 후 초기화
        else
        {
            // 퀘스트 완료 후 마지막 대사 출력
            if (isQuest)
            {
                txt.text = scriptInfo[scriptInfo.Count-1];
                bgUi.SetActive(true);
                isQuest = false;
                scriptInfo.Clear();
            }
            // 대사 UI 종료
            else if (!isQuest)
            {
                bgUi.SetActive(false);
            }
            return true;
        }
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

    //NPC 대사 XML 문서 불러오기
    void LoadScript()
    {
        //XML 생성
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(scirpt.text);
        //xmldoc.Load(scirpt.ToString());
        XmlNodeList nodes = xmldoc.SelectNodes("UniSet/info");
        //XML데이터를 Script클래스 리스트의 옮겨 담음
        for (int i = 0; i < nodes.Count; i++)
        {
            string m_Name, m_Context, m_TargetName, m_CompleteItem;
            int m_Choice, m_Answer, m_Type, m_CompletNum;

            m_Name = nodes[i].SelectSingleNode("name").InnerText;
            m_Context = nodes[i].SelectSingleNode("context").InnerText;
            m_Answer = System.Convert.ToInt32(nodes[i].SelectSingleNode("answer").InnerText);
            m_Choice = System.Convert.ToInt32(nodes[i].SelectSingleNode("choice").InnerText);

            m_Type = System.Convert.ToInt32(nodes[0].SelectSingleNode("questType").InnerText);
            m_CompletNum = System.Convert.ToInt32(nodes[0].SelectSingleNode("completINum").InnerText);
            m_TargetName = nodes[0].SelectSingleNode("targetName").InnerText;
            m_CompleteItem = nodes[0].SelectSingleNode("completItem").InnerText;

            scriptData.Add(new Script { name = m_Name, context = m_Context, answer = m_Answer, choice = m_Choice,
            quesetType = m_Type, targetName = m_TargetName, completeItem = m_CompleteItem, completNum = m_CompletNum});
        }

        // 대화한 NPC 이름 XML 데이터 불러오기
        XmlDocument xmlDocName = new XmlDocument();
        xmlDocName.Load(Application.dataPath + "/Resources/ScriptSpeak.xml");
        XmlElement posElemnet = xmlDocName["Script"];

        string name;
        foreach (XmlElement PosElement in posElemnet.ChildNodes)
        {
            name = System.Convert.ToString(PosElement.GetAttribute("Speak_NPC"));
            spokeNpc.Add(name);
        }
    }

    //대화 완료한 NPC이름 저장
    public void SpokenNpcSave()
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
