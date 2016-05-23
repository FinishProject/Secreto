using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    특정 오브젝트를 스위치 위에 올리면 문이 열리게하는 클래스

    사용방법 :
            
*************************************************************/

public class SwitchGate : MonoBehaviour {

    public Transform leftGate, rightGate; // 양쪽 문 오브젝트
    public float speed;
    public float maxLength; // 최대 이동 거리

    private Vector3 finishPos; // 최종 위치

    void Start()
    {
        // 최종 위치를 구함
        //finishPos = rightGate.position;
        //finishPos.x += maxLength;
    }

	void OnTriggerEnter(Collider col)
    {
        // 특정 오브젝트와 충돌시(위에 올려졌을 시)
        if(col.CompareTag("OBJECT"))
        {
            StartCoroutine(OpenGate());
        }
    }

    IEnumerator OpenGate()
    {
        while (true)
        {
            // 최종 위치 이상 도달 시 종료
            if (rightGate.position.x >= finishPos.x)
                break;
            // 양쪽 문 오브젝트 이동
            leftGate.Translate(Vector3.forward * -speed * Time.deltaTime);
            //rightGate.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }
    }
}
