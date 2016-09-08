using UnityEngine;
using System.Collections;

public class CameraCtrl_6 : MonoBehaviour {


    public float traceYpos = 3f;    // 추격 시작할 Y차이 (땅과 캐릭터)

    Transform tr, playerTr;     
    Vector3 groundPos;              // 플레이어가 위에 있는 땅 pos
    Vector3 baseCamPos;             // 카메라의 기본 위치 (x,z값은 유지, y 값은 지형 높이에 따라 변화 줌)
    float groundToCamYgap;
    RaycastHit hit;
   
    void Start () {
        tr = transform;
        playerTr = PlayerCtrl.instance.transform;
        baseCamPos = tr.position - playerTr.position;
        groundToCamYgap = baseCamPos.y;

    }

    
    void Update ()
    {
        
    }

    void LateUpdate()
    {

        Vector3 temp = tr.position;
        temp = Vector3.Lerp(tr.position, new Vector3(playerTr.position.x, 0, playerTr.position.z) + baseCamPos, 5 * Time.deltaTime);
        ChackGround();
        Debug.Log(playerTr.position.y + " : "+ (traceYpos + groundPos.y));
        if (playerTr.position.y > traceYpos + groundPos.y)
            temp.y = Mathf.Lerp(tr.position.y, traceYpos + groundPos.y + baseCamPos.y, 3f * 0.5f * Time.deltaTime);
        else
            temp.y = Mathf.Lerp(tr.position.y, baseCamPos.y, 3f * 0.8f * Time.deltaTime);

        tr.position = temp;
    }

    GameObject oldGround = null;
    void ChackGround()
    {
        Debug.DrawLine(tr.position, tr.position - Vector3.up * 20, Color.yellow);
        if (Physics.Raycast(tr.position, tr.position - Vector3.up * 20, out hit, 10))
        {
            if (hit.collider.gameObject && hit.transform.CompareTag("Untagged"))
            { 
                groundPos = hit.point;
                Debug.Log(hit.point);
//                baseCamPos.y = groundPos.y + groundToCamYgap;
//                Debug.Log(hit.point);

            }
        }
    }
}
