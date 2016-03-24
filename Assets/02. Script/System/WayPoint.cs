using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

    PlayerData pData = new PlayerData();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerData.Save();
//            ScriptMgr.instance.SpokenNpcSave();
//            PlayerCtrl.instance.Save();
        }
    }
}
