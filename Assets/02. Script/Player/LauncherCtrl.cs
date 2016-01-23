using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 5f;

    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

	void Update () {
        
        tr.Translate(Vector3.right * speed * Time.deltaTime);
	}

    void OnCollisionEnter(Collision coll)
    {
        gameObject.SetActive(false);
    }
}
