using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumen_Logica : MonoBehaviour
{
    //Declarando variables
    public Slider slider;
    public float sliderValue;
    public Image imageMute;
    public Image imageVolume;


    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumeAudio", 0.5f);
        AudioListener.volume = slider.value;
        RevisarSiMute();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("volumeAudio", sliderValue);
        AudioListener.volume = sliderValue;
        RevisarSiMute();
    }

    public void RevisarSiMute()
    {
        imageMute.enabled = sliderValue == 0f;
        imageVolume.enabled = sliderValue != 0f;
    }
}
