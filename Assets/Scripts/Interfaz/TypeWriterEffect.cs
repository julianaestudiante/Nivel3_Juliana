using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypeWriterEffect : MonoBehaviour
{

    AudioManager audioManager; // controlar Sonido

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); //Se utiliza para que suene el efeco de sonido
    }
   

    TMP_Text _tmpProText;
    string originalText; //  Almacena el texto original del componente TMP antes de que se inicie el efecto

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;

    [SerializeField] GameObject flechita;
    private bool isTyping = false;
    private bool hasFinishedTyping = false;

    [SerializeField] string[] phrases; // array de las frases
    private int currentPhraseIndex = 0; // para rastrear que frase esta siendo escrita

    public float titleDisplayDelay = 1.7f; // retraso antes de activar el titulo del nivel (texto) ej. año 194x
    [SerializeField] TMP_Text title;

    public CambiarEscenas changeScenes;

    void Start()
    {
        _tmpProText = GetComponent<TMP_Text>();
        title.enabled = false;

        if (_tmpProText != null)
        {
            originalText = _tmpProText.text; // Guarda el texto original en "writer"
            _tmpProText.text = ""; // Limpia el texto del componente TMP_Text

            if (flechita != null)
            {
                flechita.SetActive(false);
            }

            changeScenes = FindObjectOfType<CambiarEscenas>();

            StartCoroutine(TypePhrase(phrases[currentPhraseIndex])); // Inicia la corrutina para realizar el efecto de máquina de escribir
        }
    }

    IEnumerator TypePhrase(string phrase)
    {

        // Reproducir sonido SFX cuando se salta la escritura
        audioManager.PlaySFX(audioManager.saltoDeEscritura);

        isTyping = true;
        hasFinishedTyping = false;
        originalText = phrase;
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : ""; // mostrar o no el leadingchar

        yield return new WaitForSeconds(delayBeforeStart); // // Espera el tiempo especificado antes de comenzar a escribir

        foreach (char letter in phrase.ToCharArray())
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length); // Si hay algún texto en pantalla, primero quita el leadingChar temporalmente
            }
            _tmpProText.text += letter; // Añade el siguiente carácter
            _tmpProText.text += leadingChar; // Vuelve a añadir el leadingChar para dar la ilusión de que el cursor sigue parpadeando o moviéndose después de cada carácter
            yield return new WaitForSeconds(timeBtwChars);
        }
        _tmpProText.text = phrase; // Ensure the full phrase is displayed at the end
        isTyping = false; // Typing effect finished
        hasFinishedTyping = true; // Phrase has finished typing

        if (flechita != null)
        {
            flechita.SetActive(true); // Activa el GameObject
        }

    }
    void Update()
    {
        // Comprobar si el jugador intenta avanzar mientras el texto aún está siendo escrito
        if (isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) 
            {
                StopAllCoroutines(); 
                _tmpProText.text = originalText; // el texto actual igual al original 
                isTyping = false; // efecto terminado
                hasFinishedTyping = true;

                // Activar flechita ya que se ha saltado el efecto typewriter
                if (flechita != null)
                {
                    flechita.SetActive(true);
                }
            }
            return; 
        }

        // Si ha terminado de escribir la frase actual y detecta un clic, empezar escribiendo la siguente
        if (hasFinishedTyping && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            ShowNextPhrase(); 
        }
        
    }

    void ShowNextPhrase()
    {
        currentPhraseIndex++; // siguiente frase

        if (currentPhraseIndex < phrases.Length)
        {
            _tmpProText.text = ""; // borramos todo el texto que teniamos
            flechita.SetActive(false); // Activar flechita ya que se ha saltado el efecto typewriter

            StartCoroutine(TypePhrase(phrases[currentPhraseIndex])); // a escribir la siguiente
        }
        else
        {
            Debug.Log("All phrases completed.");
            flechita.SetActive(false);
            _tmpProText.enabled = false;
            StartCoroutine(EnableTitleDisplayDelay());
        }
    }

    IEnumerator EnableTitleDisplayDelay()
    {
        yield return new WaitForSeconds(0.7f);
        Debug.Log("Wait");
        title.enabled = true;

        yield return new WaitForSeconds(titleDisplayDelay);
        Debug.Log("Display title");
        title.enabled = false;

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Changing scenes");

        // cambiar de escena (nivel 0) cuando termine de escribir todas las frases
        if (changeScenes != null)
        {
            changeScenes.CambiarEscenaSinInput(); // metodo del script "CambiarEscenas"
        }
    }
}
