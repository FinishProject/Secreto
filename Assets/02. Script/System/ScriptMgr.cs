using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

[System.Serializable]
class Script
{
    public string name;
    public string context;
}

public class SciptSave{
    void Save()
    {
        Debug.Log("Script");
    }
}

public class ScriptMgr : MonoBehaviour {

    public Text txt;
    public GameObject scriptUi;

    private int curIndex = 0; // 현재 보여줄 대사 인덱스
    private XmlDocument xmldoc;
    private List<Script> script = new List<Script>(); //XML 데이터 저장
    private List<string> scriptInfo = new List<string>(); //현재 NPC의 대사를 저장
    private List<string> speakNpc = new List<string>(); // 만난 NPC이름 저장
   
    public static ScriptMgr instance;

    void Awake()
    {
        instance = this;
        //XML 생성
        xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Resources/script.xml");
        XmlNodeList nodes = xmldoc.SelectNodes("UniSet/info");
        //XML데이터를 Script클래스 리스트의 옮겨 담음
        for (int i = 0; i < nodes.Count; i++) {
            string m_Name, m_Context;
            m_Name = nodes[i].SelectSingleNode("name").InnerText;
            m_Context = nodes[i].SelectSingleNode("context").InnerText;
            script.Add(new Script { name = m_Name, context = m_Context });
        }
        scriptUi.SetActive(false); 
    }

    public bool GetScript(string name)
    {
        //대화할 NPC의 이름을 XML 데이터에서 찾아 대사를 getInfo에 저장
        if (!SpeakName(name)) {
            for (int i = 0; i < script.Count; i++) {
                if (script[i].name == name) {
                    scriptInfo.Add(script[i].context);
                }
            }
            //NPC 이름 저장, 대사 길이, UI 실행
            speakNpc.Add(name);
            curIndex = 0;
            scriptUi.SetActive(true);
        }
        //대화 종료 후 초기화
        if (curIndex >= scriptInfo.Count) {
            scriptInfo.Clear();
            scriptUi.SetActive(false);
            return false;
        }
        //다음 대화가 있을 경우 대화 출력
        else {
            txt.text = scriptInfo[curIndex];
            curIndex++;
            return true;
        }
    }

    //이미 대화한 NPC인지 확인
    bool SpeakName(string name)
    {
        for(int i=0; i< speakNpc.Count; i++) { 
            if(speakNpc[i] == name) return true;
        }
        return false;
    }

    //대화 완료한 NPC이름 저장
    public void SpeakNpcSave()
    {
        List<string> name = new List<string>();
        name = speakNpc;

        XmlDocument doc = new XmlDocument();
        XmlElement scriptElement = doc.CreateElement("Script");
        doc.AppendChild(scriptElement);

        XmlElement scriptSpeak = doc.CreateElement("SpeakNPC");
        for (int i = 0; i < name.Count; i++)
        {
            scriptSpeak.SetAttribute("Speak_" + name[i].ToString(), name[i].ToString());
        }
        scriptElement.AppendChild(scriptSpeak);

        doc.Save(Application.dataPath + "/Resources/ScriptSpeak.xml");
    }
}
