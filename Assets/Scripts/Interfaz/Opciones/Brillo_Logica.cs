using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Brillo_Logica : MonoBehaviour
{
    //Declarando variables
    public Slider slider;
    public float sliderValue;
    public Image darkPanel;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        darkPanel.color = new Color(darkPanel.color.r, darkPanel.color.g, darkPanel.color.b, slider.value);
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        darkPanel.color = new Color(darkPanel.color.r, darkPanel.color.g, darkPanel.color.b, sliderValue);
        PlayerPrefs.SetFloat("brightness", sliderValue);
    }
   
}
