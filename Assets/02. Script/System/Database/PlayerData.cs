using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Data
{
    public Vector3 pPosition;
    public float hp;
}

public class PlayerData {

    public static void Save(Data data)
    {
        //xml 생성
        XmlDocument doc = new XmlDocument();
        //요소 생성
        XmlElement posElement = doc.CreateElement("PlayerPosition");
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
        //posElement.AppendChild(infoDataElement);
        //데이터 저장
        doc.Save(Application.dataPath + "/Resources/Player_Data.xml");
    }

    public static Data Load()
    {
        XmlDocument xmlDoc = new XmlDocument();
        //해당 경로의 XMl문서 불러오기
        xmlDoc.Load(Application.dataPath + "/Resources/Player_Data.xml");
        XmlElement posElement = xmlDoc["PlayerPosition"];

        float posX, posY, posZ, pHp;
        Data data = new Data();

        foreach (XmlElement PosElement in posElement.ChildNodes)
        {
            posX = System.Convert.ToSingle(PosElement.GetAttribute("x"));
            posY = System.Convert.ToSingle(PosElement.GetAttribute("y"));
            posZ = System.Convert.ToSingle(PosElement.GetAttribute("z"));
          
            Vector3 initVec = new Vector3(posX, posY, posZ);
            data.pPosition = initVec;
        }
        return data;
    }

    //NPC 대사 XML 문서 불러오기
    public static List<Script> LoadScript()
    {
        List<Script> scriptData = new List<Script>();
        //XML 생성
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(ScriptMgr.instance.scirptFile.text);
        //xmldoc.Load(scirpt.ToString());
        XmlNodeList nodes = xmldoc.SelectNodes("UniSet/info");
        //XML데이터를 Script클래스 리스트의 옮겨 담음
        for (int i = 0; i < nodes.Count; i++)
        {
            string m_Name, m_Context, m_TargetName;
            int m_scriptType, m_QuestType, m_CompletNum;
            int mYes, mNo;

            m_Name = nodes[i].SelectSingleNode("name").InnerText;
            m_Context = nodes[i].SelectSingleNode("context").InnerText;
            m_scriptType = System.Convert.ToInt32(nodes[i].SelectSingleNode("type").InnerText);
         
            m_QuestType = System.Convert.ToInt32(nodes[i].SelectSingleNode("questType").InnerText);
            m_CompletNum = System.Convert.ToInt32(nodes[i].SelectSingleNode("completINum").InnerText);
            m_TargetName = nodes[i].SelectSingleNode("targetName").InnerText;

            mYes = System.Convert.ToInt32(nodes[i].SelectSingleNode("yes").InnerText);
            mNo = System.Convert.ToInt32(nodes[i].SelectSingleNode("no").InnerText);

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
            });
        }
        return scriptData;

        //// 대화한 NPC 이름 XML 데이터 불러오기
        //XmlDocument xmlDocName = new XmlDocument();
        //xmlDocName.Load(Application.dataPath + "/Resources/ScriptSpeak.xml");
        //XmlElement posElemnet = xmlDocName["Script"];

        //string name;
        //foreach (XmlElement PosElement in posElemnet.ChildNodes)
        //{
        //    name = System.Convert.ToString(PosElement.GetAttribute("Speak_NPC"));
        //    spokeNpc.Add(name);
        //}
    }

}