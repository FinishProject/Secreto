using UnityEngine;
using System.Collections;

public class TrampleObject : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player") && col.GetComponent<PlayerCtrl>().IsJumping())
        {
            gameObject.GetComponent<ItemDrop>().DropItem();
            gameObject.SetActive(false);
        }
    }
}
