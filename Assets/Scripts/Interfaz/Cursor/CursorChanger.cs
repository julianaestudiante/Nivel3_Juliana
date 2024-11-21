using UnityEngine;
using UnityEngine.EventSystems; // Requerido para detectar eventos de UI

public class CursorChanger : MonoBehaviour
{
    [HideInInspector] public static CursorChanger instance;
    public Texture2D[] variantesCursor;      // Texturas de cursor personalizadas
    public Texture2D cursorPorDefecto;       // Textura del cursor por defecto
    public Vector2 cursorHotspot = Vector2.zero;
    public Texture2D currentCursor;

    private bool isDefaultCursorActive;       // Verifica si el cursor por defecto está activo
    private bool isOnUIButton = false;         // Indica si el cursor está sobre un botón de UI
    private float resetTimer = 0f;             // Temporizador para resetear el cursor
    private float cursorResetDelay = 0.1f;     // Retraso antes de restablecer el cursor

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
         //   Debug.Log("CursorChanger instance created.");
        }
        else
        {
            Destroy(this); // Destruye si ya existe otra instancia
            Debug.LogWarning("Another instance of CursorChanger already exists. Destroying duplicate.");
        }
    }

    void Start()
    {
        SetCursorToDefault(); // Establece el cursor por defecto al inicio
       // Debug.Log("Cursor set to default at start.");
    }

    public void ChangeCursor(int cursorIndex)
    {
        if (cursorIndex < variantesCursor.Length)
        {
            cursorHotspot = new Vector2(variantesCursor[cursorIndex].width / 2, variantesCursor[cursorIndex].height / 2);
            Cursor.SetCursor(variantesCursor[cursorIndex], cursorHotspot, CursorMode.Auto);
            currentCursor = variantesCursor[cursorIndex];
            isDefaultCursorActive = false;
            resetTimer = cursorResetDelay;

            //Debug.Log($"Cursor changed to variant index {cursorIndex}. Timer set to {cursorResetDelay} seconds.");
        }
        else
        {
           // Debug.LogWarning($"Cursor index {cursorIndex} is out of range. No cursor change applied.");
        }
    }

    public void SetCursorToDefault()
    {
        if (!isDefaultCursorActive)
        {
            if (!isOnUIButton || currentCursor == variantesCursor[0] || Input.GetMouseButtonUp(0))
            {
                cursorHotspot = new Vector2(8, 8);
                Cursor.SetCursor(cursorPorDefecto, cursorHotspot, CursorMode.Auto);
                currentCursor = cursorPorDefecto;
                isDefaultCursorActive = true;
                // Debug.Log("Cursor reset to default.");
            }
        }
    }

    public void SetCursorUI(int cursorIndex)
    {
        ChangeCursor(cursorIndex); // Cambia el cursor al índice especificado para UI
        isOnUIButton = true;
        //Debug.Log($"Cursor changed for UI element at index {cursorIndex}. Cursor set to UI mode.");
    }

    public void ResetCursorUI()
    {
        SetCursorToDefault(); // Restablece el cursor al por defecto
        isOnUIButton = false;
        //Debug.Log("Cursor reset from UI to default.");
    }

    public RaycastHit2D CheckLayersInOrder(Vector2 origin)
    {
        string[] layerOrder = { "Prioritario", "Default" };
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero, Mathf.Infinity);

        if (hits.Length > 0)
        {
            int topPriorityLayer = LayerMask.NameToLayer(layerOrder[0]);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == topPriorityLayer)
                {
                   // Debug.Log($"Top-priority layer object detected: {hit.collider.gameObject.name}");
                    return hit;
                }
            }

           // Debug.Log("No top-priority layer objects detected; returning first hit object.");
            return hits[0];
        }

        //Debug.Log("No objects detected by raycast.");
        return new RaycastHit2D();
    }
}
