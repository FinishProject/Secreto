using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    서로 다른 위치에 있는 문 오브젝트를 이동할 수 있는 클래스

    사용방법 :

    출구가 될 오브젝트를 eixtGate에 삽입하여 사용해야한다.
        
*************************************************************/

public class TeleportGate : MonoBehaviour {

    public Transform exitGate;
    private TeleportGate exitTelpo;
    private Transform boxTr;

    private bool isBox = false;

    void Start()
    {
        exitTelpo = exitGate.GetComponent<TeleportGate>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            FadeInOut.instance.StartFadeInOut(1f, 1.8f, 1f);
            StartCoroutine(MoveGate());
        }
        else if (col.CompareTag("OBJECT"))
        {
            isBox = true;
            boxTr = col.transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("OBJECT"))
        {
            isBox = false;
        }
    }

    IEnumerator MoveGate()
    {
        Vector3 exitPoint = exitGate.position;
        exitPoint -= Vector3.right * 3f;
        exitPoint -= Vector3.up * 3.8f;

        yield return new WaitForSeconds(1f);

        if (isBox && boxTr != null)
        {
            boxTr.position = new Vector3(exitPoint.x - 3f, exitPoint.y, exitPoint.z);
        }

        PlayerCtrl.instance.transform.position = exitPoint;
        PlayerCtrl.instance.TurnPlayer();
    }
}

