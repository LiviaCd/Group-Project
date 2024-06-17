using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public GameObject cylinder;
    private Renderer cylinderRenderer;
    private Color colorCylinder;
    private VoiceRecognition voiceRecognition;
    void Start()
    {
        cylinderRenderer = cylinder.GetComponent<Renderer>();   
        //gameObject.GetComponent<Renderer>().enabled = true;  
    }

    private void ChangeCylinderColor()
    {
        if (voiceRecognition.IsSpeaking())
        {
            colorCylinder = new Color(214, 69, 65);
            cylinderRenderer.material.SetColor("_Color", colorCylinder);
        }
        else
        {
            colorCylinder = new Color(65, 105, 225);
            cylinderRenderer.material.SetColor("_Color", colorCylinder);
        }
    }
}
