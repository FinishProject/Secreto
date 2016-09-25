using UnityEngine;
using System.Collections;

public class ShotRazorObj : MonoBehaviour {

    public float maxLength = 30f;
    private float interValue = 0.115f;

    public GameObject startObj;
    public GameObject lazerObj;

    private Vector3 shotPoint;

    void Start()
    {
        //startObj.SetActive(true);

        shotPoint = this.transform.position;
        shotPoint.x += 0.5f;
        startObj.transform.position = shotPoint;
    }

	void Update () {
        ShotRay();
	}

    void ShotRay()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.right);

        int layer = (1 << LayerMask.NameToLayer("CAM"));

        if (Physics.Raycast(this.transform.position, forward, out hit, maxLength))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerCtrl.instance.PlayerDie();
            }

            Vector3 scale = lazerObj.transform.localScale;   
            scale.x = hit.distance * interValue;
            lazerObj.transform.localScale = scale;
            startObj.transform.position = shotPoint;
        }
    }
}
