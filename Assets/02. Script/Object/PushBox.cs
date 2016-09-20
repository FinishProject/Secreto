using UnityEngine;
using System.Collections;

public class PushBox : MonoBehaviour {

    public float speed = 1f;

    private Vector3 moveDir = Vector3.zero;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && Input.GetKey(KeyCode.LeftShift) && PlayerCtrl.inputAxis != 0f)
        {
            
            rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            PlayerCtrl.instance.SetPushAnim(true);
            moveDir = col.transform.forward;
            transform.position += moveDir * speed * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerCtrl.instance.SetPushAnim(false);
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ 
                | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

   void OnTriggerExit(Collider col)
    {
        PlayerCtrl.instance.SetPushAnim(false);
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
