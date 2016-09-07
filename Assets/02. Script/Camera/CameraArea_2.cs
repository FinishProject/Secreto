using UnityEngine;
using System.Collections;

public class CameraArea_2 : MonoBehaviour {

    public float val;
    public bool moving;
    public Transform moving_bace;

    private Vector3 orign;
    
    void Start()
    {
        orign = transform.position;
        if (moving)
            StartCoroutine(UpdatePos());
    }

    IEnumerator UpdatePos()
    {

        transform.position = orign + moving_bace.transform.position;
        val = moving_bace.position.y + 1;

        yield return null;
    }

}
