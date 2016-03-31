using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    List<ItemStruct> ownItems = new List<ItemStruct>();

	void Start () {
	
	}
	
	void Update () {
	
	}

    void Insert(ItemStruct item)
    {
        ownItems.Add(item);
    }

    // 아이디값과 일치하는 데이터 삭제
    void Delete(int tempid)
    {
        ownItems.RemoveAt( ownItems.FindIndex( x => x.id == tempid));
    }
}
