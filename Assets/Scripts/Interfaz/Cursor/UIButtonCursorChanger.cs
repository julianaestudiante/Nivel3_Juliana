using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonCursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cursorIndex; // Index of the cursor variant to show when over this button

    
    // M�todo llamado cuando el rat�n entra sobre el objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CursorChanger.instance != null)
        {
            CursorChanger.instance.SetCursorUI(cursorIndex);
        }
    }

    // M�todo llamado cuando el rat�n sale del objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CursorChanger.instance != null)
        {
            CursorChanger.instance.ResetCursorUI();
        }
    }
}
