using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaCompleta_Logica : MonoBehaviour
{
    // Declaración de variables
    public Toggle toggle;
    private AudioManager audioManager; // Controlador de sonido

    void Start()
    {
        // Inicializa el AudioManager al iniciar el script
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Configura el estado inicial del toggle de acuerdo a la configuración de pantalla completa
        toggle.isOn = Screen.fullScreen;

        // Agrega un listener al Toggle para reproducir sonido cuando se cambia el estado
        toggle.onValueChanged.AddListener(OnToggleClick);
    }

    public void ChangeFullScreen(bool value)
    {
        // Cambia el modo de pantalla completa
        Screen.fullScreen = value;
    }

    // Método que se llama cuando se hace clic en el Toggle
    private void OnToggleClick(bool isOn)
    {
        // Reproduce el sonido al hacer clic en el Toggle
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.ClickButton); // Asegúrate de tener un clip asignado en AudioManager
        }
        else
        {
            Debug.LogWarning("AudioManager no encontrado.");
        }
    }
}
