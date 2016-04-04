using UnityEngine;
using System.Collections;

public class OpenByCollectionItem : MonoBehaviour , Sensorable
{
    public int needItemId;

    public void ActiveSensor()
    {
        if(Inventory.instance.Find(needItemId) != null)
        StartCoroutine(OpenTheDoor());
    }

    IEnumerator OpenTheDoor()
    {
        Transform thisTr = gameObject.transform;
        Vector3 TargetPos = thisTr.position;
        TargetPos.y += thisTr.localScale.y;
        while (true)
        {
            thisTr.position = Vector3.Lerp(thisTr.position, TargetPos, Time.deltaTime);

            if (Vector3.Distance(thisTr.position, TargetPos) < 0.5f)
                break;

            yield return null;
        }
    }
    
}
