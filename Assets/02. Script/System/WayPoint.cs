using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

    public delegate void SaveSystem();
    public static event SaveSystem OnSave;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            OnSave();
            InGameUI_2.instance.AvtiveSave();
        }
    }
}
