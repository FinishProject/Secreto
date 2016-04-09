using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

class Script
{
    public string name;
    public string context;
}

public class ScriptMgr : MonoBehaviour {

    public Text txt;
    public GameObject scriptUi;

    private int curIndex = 0; // 현재 보여줄 대사 인덱스
    private List<Script> scriptData = new List<Script>(); //XML 데이터 저장
    private List<string> scriptInfo = new List<string>(); //현재 NPC의 대사를 저장
    private List<string> spokeNpc = new List<string>(); // 만난 NPC이름 저장

    public static ScriptMgr instance;

    void Awake()
    {
        instance = this;
        LoadScript();
    }

    public bool GetScript(string name)
    {
        Debug.Log("sc");
        //대화할 NPC의 이름을 XML 데이터에서 찾아 대사를 getInfo에 저장
        if (!SpeakName(name))
        {
            for (int i = 0; i < scriptData.Count; i++)
            {
                if (scriptData[i].name == name)
                {
                    scriptInfo.Add(scriptData[i].context);
                }
            }
            //NPC 이름 저장, 대사 길이, UI 실행
            spokeNpc.Add(name);
            curIndex = 0;
            scriptUi.SetActive(true);
        }
        //대화 종료 후 초기화
        if (curIndex >= scriptInfo.Count)
        {
            scriptInfo.Clear();
            scriptUi.SetActive(false);
            return true;
        }
        //다음 대화가 있을 경우 대화 출력
        else {
            txt.text = scriptInfo[curIndex];
            curIndex++;
            return false;
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

    void LoadScript()
    {
        // NPC 대사 XML 문서 불러오기
        //XML 생성
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Resources/script.xml");
        XmlNodeList nodes = xmldoc.SelectNodes("UniSet/info");
        //XML데이터를 Script클래스 리스트의 옮겨 담음
        for (int i = 0; i < nodes.Count; i++)
        {
            string m_Name, m_Context;
            m_Name = nodes[i].SelectSingleNode("name").InnerText;
            m_Context = nodes[i].SelectSingleNode("context").InnerText;
            scriptData.Add(new Script { name = m_Name, context = m_Context });
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
        scriptUi.SetActive(false);
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
