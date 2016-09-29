using UnityEngine;
using System.Collections;

public class CameraCtrl_7 : MonoBehaviour {

    Transform tr;   // 현재 Transform
    Transform playerTr;   // 현재 Transform
    public Transform prevNode, curNode, nextNode;  // Node들의 Transform정보 각각 이전,현재,다음
    Vector3 unitVector;
    Vector3 camAddPos;
    float curRange, totalRange, ratio;
    // Use this for initialization
    void Start () {
        tr = transform;
        unitVector = nextNode.position - curNode.position;
        playerTr = PlayerCtrl.instance.transform;
        camAddPos = tr.position - playerTr.position;
    }

    // Update is called once per frame
    void Update () {

        //totalRange = Vector3.Distance(curNode.position, nextNode.position);
        //curRange = Vector3.Distance(playerTr.position + camAddPos, nextNode.position);
        //ratio = 1 - (curRange / totalRange);
        totalRange = Mathf.Round((nextNode.position.x - curNode.position.x) * 100)/100;
        curRange = Mathf.Round((nextNode.position.x - tr.position.x) * 100) / 100;
        Debug.Log(totalRange);
        Debug.Log(curRange);
        ratio =  1 - (curRange / totalRange);
        //        Debug.Log(ratio);
        unitVector.x = 0;
        tr.position = playerTr.position + camAddPos + (unitVector * ratio);
//         tr.position = Vector3.Lerp(tr.position , playerTr.position + camAddPos, Time.deltaTime);

    }

   
}
