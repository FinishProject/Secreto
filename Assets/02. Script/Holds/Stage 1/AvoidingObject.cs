using UnityEngine;
using System.Collections;

public class AvoidingObject : MonoBehaviour {
    public  float range;
    private float playerX;
    private Transform tr;
    private bool bPlayerisLeft = true;

	// Use this for initialization
	void Start () {
        tr = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        playerX = GameObject.FindWithTag("Player").transform.position.x;
        Vector3 temp = tr.transform.position;
        if (bPlayerisLeft && playerX + range > tr.position.x - tr.localScale.x && playerX < tr.position.x - tr.localScale.x)
        {
            temp.y += 0.5f;
            if(temp.y >= 5)
            {
                bPlayerisLeft = false;
            }
        }
        else if(!bPlayerisLeft && playerX - range < tr.position.x + tr.localScale.x && playerX > tr.position.x + tr.localScale.x)
        {
            temp.y -= 0.5f;
            if (temp.y <= tr.localScale.x)
            {
                bPlayerisLeft = true;
            }
        }
        tr.transform.position = Vector3.Lerp(tr.transform.position, temp, 1);
    }
}
