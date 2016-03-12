using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 5f;
    private float m_Time = 0f;

    private static Transform targetTr;

    void Update ()
    {
        if (targetTr == null) { transform.Translate(Vector3.right * speed * Time.deltaTime); }
        else
        {
            Vector3 relativePos = targetTr.position - transform.position;
            //transform.position = Vector3.Lerp(transform.position, relativePos, speed * Time.deltaTime);
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
        }

        m_Time += Time.deltaTime;
        if(m_Time >= 4f) { gameObject.SetActive(false); }
	}

    void OnCollisionEnter(Collision coll)
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        m_Time = 0f;
    }

    public static void GetTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
    }
}
