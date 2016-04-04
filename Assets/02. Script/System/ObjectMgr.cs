using UnityEngine;
using System.Collections;

public class ObjectMgr : MonoBehaviour {

    private ItemPool bullet = null;
    public GameObject BulletPrefab;
    public int Number;

    public static ObjectMgr instance;

    void Awake () {
        instance = this;
        ObjectPoolInit();
    }

    public void ObjectPoolInit()
    {
        bullet = gameObject.AddComponent<ItemPool>();
        bullet.CreateItemPool(BulletPrefab, Number);

    }

    public ItemPool GetBullet()
    {
        return bullet;
    }

}
