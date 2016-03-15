using UnityEngine;
using System.Collections;

public class RopeCtrl : MonoBehaviour {
    private Rigidbody[] ropePiece;
    private float inputAxis = 0f; // 입력 받는 키의 값
    // Use this for initialization
    void Start () {
        ropePiece = GetComponentsInChildren<Rigidbody>();
//        Debug.Log(ropePiece[0].name);
    }
	
	// Update is called once per frame
	void Update () {
        inputAxis = Input.GetAxis("Horizontal");
        ropePiece[7].AddForce(Vector3.right * inputAxis * 5f, ForceMode.Force);
    }
}
