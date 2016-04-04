using UnityEngine;
using System.Collections;

public interface Sensorable
{
    void ActiveSensor();
}

public class Sensor : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            transform.parent.GetComponent<Sensorable>().ActiveSensor();
        }
        gameObject.SetActive(false);
    }
}
