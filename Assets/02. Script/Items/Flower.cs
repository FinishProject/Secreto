using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
    public Material red;
    public Material blue;
    public bool isRed; 
	// Use this for initialization
	void Start () {
        if(isRed)
            gameObject.GetComponentInChildren<MeshRenderer>().material = red;
        else
            gameObject.GetComponentInChildren<MeshRenderer>().material = blue;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Player"))
        {
            if (isRed) SkillCtrl.instance.ChangeAttribute(AttributeState.red);
            else SkillCtrl.instance.ChangeAttribute(AttributeState.blue);
            gameObject.SetActive(false);
        }
    }
}
