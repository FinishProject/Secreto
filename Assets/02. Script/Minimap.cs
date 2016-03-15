using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

    float minimapSize = 2.0f;
    float offsetX = 10.0f;
    float offsetY = 10.0f;
    float adjustSize = 0.0f;


    public Texture borderTexture;
    public Texture effectTexture;
    public Camera minimapCamera;


    void Update()
    {
        if (minimapCamera == null) return;

        adjustSize = Mathf.RoundToInt(Screen.width / 10);
        minimapCamera.pixelRect = new Rect(offsetX, (Screen.height - (minimapSize * adjustSize)) - offsetY, minimapSize * adjustSize, minimapSize * adjustSize);

    }


    void OnGUI()
    {
        if (borderTexture != null)
        {
            minimapCamera.Render();
            GUI.DrawTexture(new Rect(offsetX, offsetY, minimapSize * adjustSize, minimapSize * adjustSize), effectTexture);
            GUI.DrawTexture(new Rect(offsetX, offsetY, minimapSize * adjustSize, minimapSize * adjustSize), borderTexture);
        }
    }
}
