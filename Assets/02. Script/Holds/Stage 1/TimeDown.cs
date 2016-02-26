using UnityEngine;
using System.Collections;

public class TimeDown : MonoBehaviour {

    private bool bCntDown = false;
    public float cnt = 3f;

	void Update () {
        if (bCntDown)
        {
            cnt -= Time.deltaTime;
            if (cnt <= 0f) { this.gameObject.SetActive(false); }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            bCntDown = true;
        }
    }

    void OnDisable()
    {
        GameObject ob = this.gameObject;
    }
}
