using UnityEngine;
using System.Collections;

public class WindObj : MonoBehaviour {

    public float speed = 3f;
    private Vector3 moveDir;

    CameraCtrl_5 cam;

    void Start()
    {
        cam = GetComponent<CameraCtrl_5>();
    }

	void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            if(cam != null)
                cam.ChangeCamSpeed(200f);

            moveDir = PlayerCtrl.moveDir;
            moveDir.x -= speed;
            PlayerCtrl.controller.Move(moveDir * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(cam != null)
                cam.ResetCameSpeed();
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
