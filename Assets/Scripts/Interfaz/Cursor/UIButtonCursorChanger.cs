using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cursorIndex; // Index of the cursor variant to show when over this button

    
    // Método llamado cuando el ratón entra sobre el objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CursorChanger.instance != null)
        {
            CursorChanger.instance.SetCursorUI(cursorIndex);
        }
    }

    // Método llamado cuando el ratón sale del objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CursorChanger.instance != null)
        {
            CursorChanger.instance.ResetCursorUI();
        }
    }
}
