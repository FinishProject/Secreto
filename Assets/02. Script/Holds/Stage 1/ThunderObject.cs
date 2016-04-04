using UnityEngine;
using System.Collections;

public class ThunderObject : MonoBehaviour {

    private bool isOnFloor = false;
    public float speed = 10.0f;

    
	void Start () {
        StartCoroutine(Thander());
	}

    void OnColliderEnter(Collider col)
    {
        if (col.tag.Equals("Ground"))
        {
            isOnFloor = true;
        }
    }

    IEnumerator Thander()
    {
        Transform thisTr = gameObject.transform;
        Vector3 moveVector = thisTr.position;
        while (!isOnFloor)
        {
            moveVector.y = thisTr.position.y + (-1 * speed * Time.deltaTime);
            thisTr.position = moveVector;
            yield return null;
        }
    }
}
