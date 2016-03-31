using UnityEngine;
using System.Collections;

public class PlayerFunc : MonoBehaviour {

    public static PlayerFunc instance;

    void Awake()
    {
        instance = this;
    }

	public void FindObject()
    {
        Collider[] hitColl = Physics.OverlapSphere(this.transform.position, 5f);
        for(int i = 0; i< hitColl.Length; i++) {
            if(hitColl[i].tag == "OBJECT") {
                hitColl[i].SendMessage("GetImpact");
            }
        }
    }

    public void SetPowerDamage()
    {
        Collider[] hitColl = Physics.OverlapSphere(this.transform.position, 10f);
        for(int i = 0; i < hitColl.Length; i++)
        {
            hitColl[i].SendMessage("GetDamage");
        }
    }
}
