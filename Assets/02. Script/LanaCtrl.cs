using UnityEngine;
using System.Collections;

public class LanaCtrl : MonoBehaviour {

    private float diatance = 0f;

    private Animator anim;
    private Transform playerTr;
    public Transform target;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(CheckDistance());
	}

    IEnumerator CheckDistance()
    {
        while (true)
        {
            diatance = (playerTr.position - transform.position).sqrMagnitude;

            if (diatance <= 50f)
            {
                StartCoroutine(MoveUpdate());
                break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator MoveUpdate()
    {
        while (true)
        {
            if (diatance <= 50f)
                AppearNpc();
            else
                anim.Stop();

            yield return null;
        }
    }

    void AppearNpc()
    {
        anim.SetBool("Appear", true);

        Vector3 center = (target.position + this.transform.position) * 0.5f;
        center -= new Vector3(0, 1, 1);
        Vector3 fromRelCenter = this.transform.position - center;
        Vector3 toRelCenter = target.position - center;
        transform.position = Vector3.Slerp(fromRelCenter, toRelCenter, 3f * Time.deltaTime);
        transform.position += center;

        transform.LookAt(playerTr);
    }
}
