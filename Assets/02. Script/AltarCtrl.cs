using UnityEngine;
using System.Collections;

public class AltarCtrl : MonoBehaviour {

    public Renderer upRender;
    public Renderer downRender;
    public Renderer BoxRender;
    Color basicColor, BoxColor;

    public GameObject altarEffect;

    private bool isOnBox = false;

    void Start()
    {
        basicColor = new Color(0f, 0.8117652f, 1.5f);
        BoxColor = new Color(0f, 0.4103448f, 0.7f);
        downRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
        upRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
        BoxRender.sharedMaterial.SetColor("_EmissionColor", BoxColor);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("OBJECT"))
        {
            downRender.sharedMaterial.SetColor("_EmissionColor", Color.red);
            upRender.sharedMaterial.SetColor("_EmissionColor", Color.red);
            

            if (col.CompareTag("OBJECT"))
            {
                altarEffect.SetActive(true);
                BoxRender.sharedMaterial.SetColor("_EmissionColor", Color.red);
                isOnBox = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (!isOnBox)
            {
                downRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
                upRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
            }
            
        }
        else if (col.CompareTag("OBJECT"))
        {
            altarEffect.SetActive(false);
            BoxRender.sharedMaterial.SetColor("_EmissionColor", BoxColor);
            downRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
            upRender.sharedMaterial.SetColor("_EmissionColor", basicColor);
            isOnBox = false;
        }
    }
}
