using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player") {
            ScriptMgr.instance.SpeakNpcSave();
            PlayerCtrl.instance.Save();
        }
    }
}
