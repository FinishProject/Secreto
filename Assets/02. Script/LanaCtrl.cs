using UnityEngine;
using System.Collections;

public class LanaCtrl : MonoBehaviour {

    private Animator anim;
    public Transform playerTr;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}

    void Update()
    {
        float dis = (playerTr.position - transform.position).sqrMagnitude;

        anim.SetFloat("Distance", dis);

        anim.SetBool("Complete", ScriptMgr.instance.isQuest);
    }

    public void StartSpeak()
    {
        StartCoroutine(Speak());
    }
	
	IEnumerator Speak()
    {
        while (true)
        {
            anim.SetBool("Speak", ScriptMgr.instance.bgUi.activeSelf);
            yield return null;
        }
    }
}
