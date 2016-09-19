using UnityEngine;
using System.Collections;

public class Hold_Switch_Show : MonoBehaviour {

    public GameObject elevator;
    bool isOnBox;
    // Use this for initialization
    void Start () {
        elevator.SetActive(false);
        isOnBox = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            elevator.SetActive(true);
        }

        if (col.CompareTag("OBJECT"))
        {
            isOnBox = true;
            elevator.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(isOnBox)
             return;

            elevator.SetActive(false);
        }
        else if(col.CompareTag("OBJECT"))
        {
            elevator.SetActive(false);
            isOnBox = false;
        }
            
    }
}
