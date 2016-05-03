using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

    PlayerData pData = new PlayerData();

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerCtrl.instance.Save();
            //ScriptMgr.instance.SpokenNpcSave();
        }
    }
}
