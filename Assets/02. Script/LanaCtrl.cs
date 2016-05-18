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


            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);

            if (camPos.x >= Camera.main.pixelWidth || camPos.x <= -1f ||
                camPos.y >= Camera.main.pixelHeight) { Debug.Log("11"); }
       
        anim.SetFloat("Distance", dis);
        anim.SetBool("Speak", ScriptMgr.instance.bgUi.activeSelf);
        anim.SetBool("Complete", ScriptMgr.instance.isAnimQuest);
    }
}
