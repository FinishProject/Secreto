using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
    public Material red;
    public Material blue;
    public bool isRed; 
	// Use this for initialization
	void Start () {
        if(isRed)
            gameObject.GetComponent<MeshRenderer>().material = red;
        else
            gameObject.GetComponent<MeshRenderer>().material = blue;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Player"))
        {
            if (isRed) col.GetComponent<PlayerCtrl>().isRed = true;
            else col.GetComponent<PlayerCtrl>().isRed = false;
            gameObject.SetActive(false);
        }
    }
}
