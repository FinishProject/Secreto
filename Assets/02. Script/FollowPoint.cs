using UnityEngine;
using System.Collections;

public class FollowPoint : MonoBehaviour {

    public Transform pointTr;

    private Vector3 relativePos;
    private Quaternion lookRot;
    public Animator anim;

    public float state = 0f;
    private float speed = 0f;

    WahleCtrl olaCtrl;

    void Start()
    {
        olaCtrl = GetComponent<WahleCtrl>();
    }

    void Update()
    {
        anim.SetBool("Move", true);

        relativePos = pointTr.position - transform.position;
        lookRot = Quaternion.LookRotation(relativePos);

        if (state == 0)
        {
            speed = olaCtrl.IncrementSpeed(speed, 5f, 1f);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRot, 2f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, pointTr.position - (pointTr.right),
                    speed * Time.deltaTime);
        }

        else if (state == 1)
        {
            transform.RotateAround(transform.position, Vector3.forward, 100f * Time.deltaTime);
            transform.Translate(new Vector3(0f, 2f * Time.deltaTime, 2f * Time.deltaTime));
        }
    }
}
