using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    아이템 매니저

*************************************************************/

public partial class ItemMgr : MonoBehaviour
{
    public ItemPool[] items;

    public GameObject HpRecoveryItemPrefab;
    public int HpRecoveryItemNumber;
    public GameObject EXPItemPrefab;
    public int EXPItemNumber;

    public void ItemPoolInit()
    {
        items = new ItemPool[2];
        items[(int)ItemFunction.HpRecovery] = new ItemPool(HpRecoveryItemPrefab, HpRecoveryItemNumber);
        items[(int)ItemFunction.Exp]        = new ItemPool(EXPItemPrefab, EXPItemNumber);

    }

    public ItemPool GetItem(ItemFunction itemType)
    {
        return items[(int)itemType];
    }
}

public class ItemStruct
{
    public int id;
    public string name;
    public int function;
    public int value;

    public void SetData(int id, string name, int function, int value)
    {
        this.id = id;
        this.name = name;
        this.function = function;
        this.value = value;
    }
}

public enum ItemFunction
{
    HpRecovery = 0,
    Exp,
    LearnSkill,
    Interaction,
    Collection,
}

// 싱글톤
public partial class ItemMgr : MonoBehaviour
{
    ItemStruct temp;
    public static ItemMgr instance;
    private static GameObject ItemPool;
    public static CSVParser csvParser;
    
    void Awake()
    {
        instance = this;

        temp = new ItemStruct();
        csvParser = new CSVParser("Item");
        ItemPoolInit();
//        csvParser.Load();
//        csvParser.ParseByID(temp, 0);
    }

    public CSVParser GetItem()
    {
        return csvParser;
    }

}