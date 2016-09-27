using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    서로 다른 위치에 있는 문 오브젝트를 이동할 수 있는 클래스

    사용방법 :

    출구가 될 오브젝트를 eixtGate에 삽입하여 사용해야한다.
        
*************************************************************/

public class TeleportGate : MonoBehaviour {

    public Transform exitGate;
    public float lenth = 5f;

    private TeleportGate exitTelpo;

    private bool isBox = false;


    void Start()
    {
        exitTelpo = exitGate.GetComponent<TeleportGate>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Vector3 exitPoint = exitGate.position;
            exitPoint -= Vector3.forward * lenth;

           
            PlayerCtrl.instance.transform.position = exitPoint;

            Transform playerTr = PlayerCtrl.instance.transform;
        }
    }
}

