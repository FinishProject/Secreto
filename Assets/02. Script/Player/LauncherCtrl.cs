using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    private float speed = 14f;
    private Transform targetTr;
    private Vector3 focusVec;

    void FixedUpdate ()
    {
        //타겟 없을 시
        if (targetTr == null) { transform.Translate(focusVec * speed * Time.deltaTime); }
        //타겟 있을 시
        else {
            Vector3 relativePos = this.targetTr.position - this.transform.position;
            //transform.position = Vector3.Lerp(transform.position, relativePos, speed * Time.deltaTime);
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
        } 
	}
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "MONSTER")
        {
            this.targetTr = null;
            gameObject.SetActive(false);
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    void GetFocusVector(Vector3 _focusVec)
    {
        focusVec = _focusVec;
    }

    void GetTarget(Transform _targetTr)
    {
        this.targetTr = _targetTr;
    }
}
