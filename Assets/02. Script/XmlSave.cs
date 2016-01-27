using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XmlSave : MonoBehaviour {

	// Use this for initialization
	void Start () {
        List<RecItem> itemLists = new List<RecItem>();

        for(int i=0; i<100; ++i)
        {
            
            RecItem item = new RecItem();
            item.Name = "아이템";
            item.Level = 2;
            item.Critical = Random.Range(0.1f, 1.0f);
            itemLists.Add(item);
        }
        ItemIO.Write(itemLists, Application.dataPath + "/Resources/ItemList_Attributes.xml");
    }
	
}
