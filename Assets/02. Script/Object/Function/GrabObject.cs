using UnityEngine;
using System.Collections;

public class GrabObject : MonoBehaviour {

    private Transform playerTr; // 플레이어 위치

	void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            // E 키 입력 시 오브젝트 잡기
            if (Input.GetKey(KeyCode.E))
            {
                // 플레이어의 자식 객체로 들어감
                playerTr = col.GetComponent<Transform>();
                this.transform.parent = playerTr.parent;
                // 오브젝트 이동
                Vector3 relativePos = playerTr.position - transform.position;
                transform.position = Vector3.Lerp(transform.position, 
                    new Vector3(playerTr.position.x + 1f, playerTr.position.y + 3f, 0f),
                    100f * Time.deltaTime);
            }
            // 플레이어의 자식 객체에서 나옴
            else if (Input.GetKeyUp(KeyCode.E)) {
                this.transform.parent = null;
            }
        }
    }
}
