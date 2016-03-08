using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public sealed class DataMgr {
//대화 완료한 NPC이름 저장
    public static void Save(List<string> name)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement scriptElement = doc.CreateElement("Script");
        doc.AppendChild(scriptElement);

        XmlElement scriptSpeak = doc.CreateElement("SpeakNPC");
        for (int i = 0; i < name.Count; i++)
        {
            scriptSpeak.SetAttribute("Speak_"+name[i].ToString(), name[i].ToString());
        }
        scriptElement.AppendChild(scriptSpeak);
        
        doc.Save(Application.dataPath + "/Resources/ScriptSpeak.xml");
    }
}