using UnityEngine;
using System.Collections;


public class WayPoint : MonoBehaviour {

    ScriptMgr sData = new ScriptMgr();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Test(this.gameObject.name);
            PlayerData.Save();
            sData.SpeakNpcSave();
            //ScriptMgr.instance.SpeakNpcSave();
            //PlayerCtrl.instance.Save();
        }
    }

   public void Test(string o)
    {
        Debug.Log(o);
    }
}
