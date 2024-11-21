using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Calidad_Logica : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int dropdownValue;
    private AudioManager audioManager; // Control de sonido
    // Start is called before the first frame update
    void Start()
    {
        // Inicializa el AudioManager al iniciar el script
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        dropdownValue = PlayerPrefs.GetInt("Quality", 3);
        QualitySettings.SetQualityLevel(dropdownValue);
        dropdown.value = dropdownValue;

        // Agrega un listener al Dropdown para reproducir sonido cuando se selecciona una opción
        dropdown.onValueChanged.AddListener(OnDropdownClick);
    }
    public void ChangeQuality(int value)
    {
        dropdownValue = value;
        QualitySettings.SetQualityLevel(dropdownValue);
        PlayerPrefs.SetInt("Quality", value);
    }
    // Método que se llama cuando se hace clic en el Dropdown
    private void OnDropdownClick(int value)
    {
        // Reproduce el sonido al hacer clic en el Dropdown
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
