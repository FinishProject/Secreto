using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Data
{
    public Vector3 pPosition;
    public float hp;
    public float enhance;
}

public abstract class PlayerData {

    public static void Save(Data data)
    {
        //xml 생성
        XmlDocument doc = new XmlDocument();
        //요소 생성
        XmlElement posElement = doc.CreateElement("PlayerData");
        doc.AppendChild(posElement);
        //캐릭터 위치값 저장
        XmlElement posDataElement = doc.CreateElement("Position");
        posDataElement.SetAttribute("x", data.pPosition.x.ToString());
        posDataElement.SetAttribute("y", data.pPosition.y.ToString());
        posDataElement.SetAttribute("z", data.pPosition.z.ToString());
        posElement.AppendChild(posDataElement);
        //캐릭터 정보 저장
        //XmlElement infoDataElement = doc.CreateElement("Info");
        //infoDataElement.SetAttribute("hp", data.hp.ToString());
        //infoDataElement.SetAttribute("enhance", data.enhance.ToString());
        //posElement.AppendChild(infoDataElement);

        //데이터 저장
        doc.Save(Application.dataPath + "/Resources/Player_Data.xml");
    }

    public static Data Load()
    {
        XmlDocument xmlDoc = new XmlDocument();
        //해당 경로의 XMl문서 불러오기
        xmlDoc.Load(Application.dataPath + "/Resources/Player_Data.xml");
        XmlElement posElement = xmlDoc["PlayerData"];

        float posX = 0f, posY = 0f, posZ = 0f;
        Data loadData = new Data();
        foreach (XmlElement PosElement in posElement.ChildNodes)
        {
            posX = System.Convert.ToSingle(PosElement.GetAttribute("x"));
            posY = System.Convert.ToSingle(PosElement.GetAttribute("y"));
            posZ = System.Convert.ToSingle(PosElement.GetAttribute("z"));
            //pHp = System.Convert.ToSingle(posElement.GetAttribute("hp"));
            //pEnhance = System.Convert.ToSingle(posElement.GetAttribute("enhance"));

            Vector3 initVec = new Vector3(posX, posY, posZ);
            loadData.pPosition = initVec;
            //loadData.hp = pHp;
            //loadData.enhance = pEnhance;
        }
        return loadData;
    }

    //NPC 대사 XML 문서 불러오기
    public static List<Script> LoadScript()
    {
        List<Script> scriptData = new List<Script>();
        //XML 생성
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(Application.dataPath + "/Resources/Script_Data.xml"); // XML 파일 불러오기
        //xmldoc.LoadXml(ScriptMgr.instance.scirptFile.text);
        XmlNodeList nodes = xmldoc.SelectNodes("UniSet/info");
        //XML데이터를 Script클래스 리스트의 옮겨 담음
        for (int i = 0; i < nodes.Count; i++)
        {
            string m_Name, m_Context, m_TargetName;
            int m_scriptType, m_QuestType, m_CompletNum;
            int mYes, mNo, mSpeaker;

            m_Name = nodes[i].SelectSingleNode("name").InnerText;
            m_Context = nodes[i].SelectSingleNode("context").InnerText;
            m_scriptType = System.Convert.ToInt32(nodes[i].SelectSingleNode("type").InnerText);
         
            m_QuestType = System.Convert.ToInt32(nodes[i].SelectSingleNode("questType").InnerText);
            m_CompletNum = System.Convert.ToInt32(nodes[i].SelectSingleNode("completINum").InnerText);
            m_TargetName = nodes[i].SelectSingleNode("targetName").InnerText;

            mYes = System.Convert.ToInt32(nodes[i].SelectSingleNode("yes").InnerText);
            mNo = System.Convert.ToInt32(nodes[i].SelectSingleNode("no").InnerText);
            mSpeaker = System.Convert.ToInt32(nodes[i].SelectSingleNode("speaker").InnerText);

            scriptData.Add(new Script
            {
                name = m_Name,
                context = m_Context,
                scriptType = m_scriptType,
                quesetType = m_QuestType,
                targetName = m_TargetName,
                completNum = m_CompletNum,
                yes = mYes,
                no = mNo,
                speaker = mSpeaker,
            });
        }
        return scriptData;
    }

    //대화 완료한 NPC이름 저장
    public static void SaveNpcName(List<string> npcName)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement scriptElement = doc.CreateElement("Script");
        doc.AppendChild(scriptElement);

        XmlElement scriptSpeak = doc.CreateElement("SpokeNPC");
        for (int i = 0; i < npcName.Count; i++)
        {
            scriptSpeak.SetAttribute("Spoke_NPC", npcName[i].ToString());
        }
        scriptElement.AppendChild(scriptSpeak);

        doc.Save(Application.dataPath + "/Resources/SpokeNpcName.xml");
    }

    public static List<string> LoadNpcName()
    {
        // 대화한 NPC 이름 XML 데이터 불러오기
        XmlDocument xmlDocName = new XmlDocument();
        xmlDocName.Load(Application.dataPath + "/Resources/SpokeNpcName.xml");
        XmlElement NameElemnet = xmlDocName["Script"];

        List<string> npcName = new List<string>();

        foreach (XmlElement nameElemnet in NameElemnet.ChildNodes)
        {
            npcName.Add(System.Convert.ToString(nameElemnet.GetAttribute("Speak_NPC")));
        }

        return npcName;
    }

}