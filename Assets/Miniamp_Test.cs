using UnityEngine;
using System.Collections;

public class Miniamp_Test : MonoBehaviour {

    RectTransform rectTr;
    public Transform playerTr;
    public Transform Map;
    float inputAxis = 0.0f;
    Vector3 moveDir;

    public float rateX = 1.43f;
    public float rateY = 1f;

	void Start () {
        rectTr = GetComponent<RectTransform>();
        rectTr.position = new Vector3(0f, 0f, 0f);
        Debug.Log("ui : " + Map.localScale);
        Debug.Log("Ga : " + playerTr.localScale);
        Debug.Log(Map.localScale - playerTr.localScale);
	}
	
	void Update () {
        moveDir.x = transform.localScale.x*playerTr.position.x;
        moveDir.y = playerTr.position.y * transform.localScale.y * rateY;
        //Debug.Log(moveDir);
        //rectTr.position = moveDir; 
    }
}
