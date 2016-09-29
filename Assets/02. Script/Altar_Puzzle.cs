using UnityEngine;
using System.Collections;

public class Altar_Puzzle : MonoBehaviour {

    public GameObject[] stepHolds;

    void OnCollisionStay(Collision col)
    {
        if (col.collider.CompareTag("OBJECT"))
        {
            for(int i=0; i<stepHolds.Length; i++)
            {
                stepHolds[i].SetActive(true);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("OBJECT"))
        {
            for (int i = 0; i < stepHolds.Length; i++)
            {
                stepHolds[i].SetActive(false);
            }
        }
    }
}
