using UnityEngine;
using System.Collections;

public class HorizonHold_Switch : MonoBehaviour
{
    public GameObject switchObject;
    public Transform TargetPos;

    private Transform playerTr;
    private Vector3 maxLengthPos, originPos;

    public float speed = 3f; // 발판 이동 속도
    private float pSpeed = 3f; // 플레이어 이동 속도
    
    private bool isFocus; // 플레이어가 왼쪽을 봐라보는지
    private bool isOnPlayer;
    void Start()
    {
        maxLengthPos.x = TargetPos.transform.position.x; //최대 이동 길이(우측)
        originPos.x = transform.position.x; //초기 이동 길이(좌측)
    }

    void FixedUpdate()
    {
        if (switchObject.GetComponent<SwitchObject>().IsSwitchOn)
        {
            //최대 위치 또는 초기 위치 도착시 방향 전환
            if (transform.position.x >= maxLengthPos.x && speed >= 1) { switchObject.GetComponent<SwitchObject>().IsSwitchOn = false; speed *= -1; }
            else if (transform.position.x <= originPos.x && speed <= -1) { switchObject.GetComponent<SwitchObject>().IsSwitchOn = false; speed *= -1; }

            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider coll)
    {
        // 플레이어가 발판 위에 있을 시 발판과 같이 이동
        if (coll.gameObject.tag == "Player" && switchObject.GetComponent<SwitchObject>().IsSwitchOn)
        {
            Debug.Log(gameObject.name);
            playerTr = coll.GetComponent<Transform>();
            isFocus = PlayerCtrl.isFocusRight;
            pSpeed = speed;
            if (!isFocus) { pSpeed *= -1; }
            //발판 위에 있을 시 플레이어 이동
            playerTr.Translate(Vector3.forward * pSpeed * Time.deltaTime);
        }
    }
    
    void OnTriggerExit(Collider coll)
    {
        playerTr = null;
    }
}
