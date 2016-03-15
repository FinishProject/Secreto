using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Data
{
    public Vector3 pPosition;
}

public class PlayerData {

    public static void Save()
    {
        Data data = new Data();
        //xml 생성
        XmlDocument doc = new XmlDocument();
        //요소 생성
        XmlElement posElement = doc.CreateElement("PlayerPosition");
        doc.AppendChild(posElement);
        //캐릭터 위치값 저장
        XmlElement posDataElement = doc.CreateElement("Position");
        posDataElement.SetAttribute("x", data.pPosition.x.ToString());
        posDataElement.SetAttribute("y", data.pPosition.y.ToString());
        posDataElement.SetAttribute("z", data.pPosition.y.ToString());
        posElement.AppendChild(posDataElement);
        //데이터 저장
        doc.Save(Application.dataPath + "/Resources/Player_Data.xml");
    }

    public static Data Load()
    {
        XmlDocument xmlDoc = new XmlDocument();
        //해당 경로의 XMl문서 불러오기
        xmlDoc.Load(Application.dataPath + "/Resources/Player_Data.xml");
        XmlElement posElement = xmlDoc["PlayerPosition"];

        float posX, posY, posZ;
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
    
}