using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class RecItem
{
    public string Name;
    public int Level;
    public float Critical;
}

public sealed class ItemIO
{
    public static void Write(List<RecItem> ItemList, string filePath)
    {
        //xml 문서 생성
        XmlDocument Document = new XmlDocument();
        //새로운 요소 생성
        XmlElement ItemListElement = Document.CreateElement("ItemList0101");
        //Document의 첨부
        Document.AppendChild(ItemListElement);

        foreach (RecItem Item in ItemList)
        {
            XmlElement ItemElement = Document.CreateElement("Item");
            ItemElement.SetAttribute("Name", Item.Name);
            ItemElement.SetAttribute("Level", Item.Level.ToString());
            ItemElement.SetAttribute("Critical", Item.Critical.ToString());
            ItemListElement.AppendChild(ItemElement);
        }
        //xml데이터 저장
        Document.Save(filePath);
    }

    public static List<RecItem> Read(string filePath)
    {
        XmlDocument Document = new XmlDocument();
        Document.Load(filePath);
        XmlElement ItemListElement = Document["ItemList"];

        List<RecItem> ItemList = new List<RecItem>();

        foreach (XmlElement ItemElement in ItemListElement.ChildNodes)
        {
            RecItem Item = new RecItem();
            Item.Name = ItemElement.GetAttribute("Name");
            Item.Level = System.Convert.ToInt32(ItemElement.GetAttribute("Level"));
            Item.Critical = System.Convert.ToSingle(ItemElement.GetAttribute("Critical"));
            ItemList.Add(Item);
        }
        return ItemList;
    }
}