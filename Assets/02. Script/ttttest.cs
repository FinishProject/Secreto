using UnityEngine;
using System.Collections;
public interface OnPlayer
{
    void DoSomething();
}
public class ttttest : MonoBehaviour, OnPlayer
{
    public void DoSomething()
    {
        Debug.Log("Player Hit");
    }
}
