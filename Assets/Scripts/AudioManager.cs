using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip ClickButton;
    public AudioClip saltoDeEscritura; // Añadir este nuevo clip en AudioManager
    public AudioClip Inicio_Button;
    public AudioClip AjustesButton;
    public AudioClip SalirButton;
    public AudioClip sound5;


    private void Start()
    {
         musicSource.clip = background;
         musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
      
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position); // Reproduce el sonido en la posición actual
        }
        else
        {
            Debug.LogWarning("Clip de sonido no asignado.");
        }
    }

}
