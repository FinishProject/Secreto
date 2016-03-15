using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 5f;
    private float m_Time = 0f;

    private Transform targetTr;

    void Update ()
    {
        if (targetTr == null) { transform.Translate(Vector3.right * speed * Time.deltaTime); }
        else
        {
            Vector3 relativePos = targetTr.position - transform.position;
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
        }

        m_Time += Time.deltaTime;
        if(m_Time >= 4f) { gameObject.SetActive(false); }
	}

    void OnCollisionEnter(Collision coll)
    {
        Destroy(coll.gameObject);
        gameObject.SetActive(false);      
    }

    void OnEnable()
    {
        m_Time = 0f;
//        targetTr = GameObject.FindGameObjectWithTag("Monster").transform;
    }
}
