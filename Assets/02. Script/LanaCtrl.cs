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


        if (ScriptMgr.instance.bgUi.activeSelf)
        {
            transform.LookAt(playerTr);
        }
       
        anim.SetFloat("Distance", dis);
        anim.SetBool("Speak", ScriptMgr.instance.bgUi.activeSelf);
        anim.SetBool("Complete", ScriptMgr.instance.isAnimQuest);
    }
}
