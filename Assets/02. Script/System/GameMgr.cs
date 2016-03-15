using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {

    private static GameMgr instance;
    private static GameObject container;
    public static GameMgr GetInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "ObjectMgr";
            instance = container.AddComponent(typeof(GameMgr)) as GameMgr;
        }
        return instance;
    }



    WorldCtrl worldData;

    // Use this for initialization
    void Start () {
        worldData = new WorldCtrl();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
