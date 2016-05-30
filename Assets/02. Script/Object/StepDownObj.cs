using UnityEngine;
using System.Collections;

public class StepDownObj : MonoBehaviour {

    private float delTime = 0f;
    private bool isActive = true;
    private float speed = 4f;

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (isActive)
            {
                if (delTime >= 0.3f)
                {
                    isActive = false;
                }

                delTime += Time.deltaTime * 5f;
                transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(RollBack());
            isActive = true;
        }
    }

    IEnumerator RollBack()
    {
        while (true)
        {
            if(delTime <= 0f)
            {
                break;
            }

            delTime -= Time.deltaTime * 5f;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            yield return null;
        }
    }
}
