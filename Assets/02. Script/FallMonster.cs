using UnityEngine;
using System.Collections;

public class FallMonster : MonoBehaviour {

    public GameObject coll;

	// Use this for initialization
	void Start () {
       

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("OBJECT"))
        {
            PlayerCtrl.instance.SetStopMove();
            StartCoroutine(RotateObj(col));
            
        }
    }

    IEnumerator RotateObj(Collider col)
    {
        StartCoroutine(End());
        
        while (true)
        {
            col.transform.Translate(Vector3.right * 0.8f * Time.deltaTime);
            col.transform.RotateAround(Vector3.forward, -0.5f * Time.deltaTime);           
            yield return null;
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        FadeInOut.instance.StartFadeInOut(1f, 1.8f, 1f);
        yield return new WaitForSeconds(1f);
        Application.LoadLevel("MainScene 1");
    }
}
