using UnityEngine;
using System.Collections;

public class ShootingObject : MonoBehaviour {

    public float delayTime = 10f;
    public Dir shootDir;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateBullet());
    }

    IEnumerator CreateBullet()
    {
        while (true)
        {
            StartCoroutine( 
                ObjectMgr.instance.GetBullet().UseItem().
                GetComponent<BulletObject>().Moveing(gameObject.transform, shootDir, 10f));

            yield return new WaitForSeconds(delayTime);
        }
    }
}
