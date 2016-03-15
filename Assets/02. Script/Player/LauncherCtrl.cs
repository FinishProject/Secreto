using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 5f;
    private float m_Time = 0f;

    private Transform targetTr;

    void Update ()
    {
        //타겟 없을 시
        if (targetTr == null) { transform.Translate(Vector3.right * speed * Time.deltaTime); }
        //타겟 있을 시
        else {
            Vector3 relativePos = this.targetTr.position - transform.position;
            //transform.position = Vector3.Lerp(transform.position, relativePos, speed * Time.deltaTime);
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
        }
        m_Time += Time.deltaTime;
        if(m_Time >= 3f) { gameObject.SetActive(false); }
	}
    void OnTriggerEnter(Collider coll)
    {
        this.targetTr = null;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        m_Time = 0f;
    }

    void GetTarget(Transform _targetTr)
    {
        this.targetTr = _targetTr;
    }
}
