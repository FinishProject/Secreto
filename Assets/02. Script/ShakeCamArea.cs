using UnityEngine;
using System.Collections;

public class ShakeCamArea : MonoBehaviour {

	void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(CameraCtrl_4.instance.Shake(4f, 3, 0.01f));
        }
    }
}
