using UnityEngine;
using System.Collections;

public class VerticalHold : MonoBehaviour {

    private Transform tr;
    private Rigidbody playerTr;
    public float speed = 3f;

    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine("Change");
    }

    void Update()
    {
        tr.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator Change()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            speed *= -1;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerTr = coll.GetComponent<Rigidbody>();
            playerTr.isKinematic = false;
            //playerTr.Translate(Vector3.up * speed * Time.deltaTime);
            //playerTr.parent = tr;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        playerTr.isKinematic = true;
        //playerTr.parent = null;
    }
}
