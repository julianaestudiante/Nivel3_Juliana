using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class SceneNavigator : MonoBehaviour
{
    [Header ("Views")]
    [Tooltip("Arranstrar aqui todas las vistas generales que se van a cambiar con las flechas")]public GameObject[] view; //Las diferentes vistas guardadas en el inspector
    [Space(20)] public int startingView; //la vista donde empezará la escena
    [Space(20), Min(0)] public int[] roomEndViews;
    [HideInInspector] public int currentView; //la escena actual
    [Space(20)] int maxViews;
    [Header("Arrows")]
    public GameObject[] arrow; //flechas en la interfaz que se usan para moverse

    // Start is called before the first frame update
    void Start()
    {
        currentView = startingView;
        maxViews = view.Length-1;
        for (int i = 0; i < view.Length; i++) 
        {
            if ((currentView==i)) 
            {
                view[i].SetActive(true);
            }
            else
            {
                view[i].SetActive(false);
            }
        }
        //Debug.Log($"Current view:{currentView}");
        //Debug.Log($"Starting view:{startingView}");
        //Debug.Log($"Max view:{maxViews}");
        CheckViewEnd();
    }
    public void CheckViewEnd() 
    {
        if(roomEndViews != null) 
        {
            for (int i = 0; i < roomEndViews.Length; i++)
            {
                if (currentView == 0 || currentView == roomEndViews[i]+1)  // Is la vista actual esta en el borde izquierdo
                {
                    arrow[0].SetActive(false);  // desactiva la flecha izquierda
                    arrow[1].SetActive(true);   // Activa la flecha derecha
                                                //Debug.Log("Left end reached");
                }
                else if (currentView == maxViews || currentView == roomEndViews[i])  // Si la vista actual esta en el borde derecho
                {
                    arrow[0].SetActive(true);    // Activa la flecha izquierda
                    arrow[1].SetActive(false);   // Activa la flecha derecha
                                                 //Debug.Log("Right end reached");
                }
                else  // Si la vista actual no esta a los bordes
                {
                    arrow[0].SetActive(true);    // Activa la flecha izquierda
                    arrow[1].SetActive(true);    // Activa la flecha derecha
                                                 // Debug.Log("Not at the edge views");
                }
            }
        }
    }
    public void ChangeViewRoom(int newView)
    {
        if (currentView != maxViews) //si la vista actual no es el limite a la derecha, activa la escena a la derecha
        {
            view[newView].SetActive(true);
            view[currentView].SetActive(false);
            currentView=newView;
            CheckViewEnd();
            //Debug.Log($"current view is {currentView}");
        }
    }

    public void ChangeViewRight() 
    {
       if(currentView != maxViews) //si la vista actual no es el limite a la derecha, activa la escena a la derecha
        {
            view[currentView+1].SetActive(true);
            view[currentView].SetActive(false);
            currentView++;
            CheckViewEnd();
            //Debug.Log($"current view is {currentView}");
        }
    }
    public void ChangeViewLeft()
    {
        if (currentView != 0)//si la vista actual no es el limite a la izquierda activa la escena a la izquierda
        {
            view[currentView - 1].SetActive(true);
            view[currentView].SetActive(false);
            currentView--;
            CheckViewEnd();
           // Debug.Log($"current view is {currentView}");
        }
    }
    public void EnterCloseUpViewArrows()
    {
        arrow[0].SetActive(false);    // Activa la flecha izquierda
        arrow[1].SetActive(false);
        arrow[2].SetActive(true);
    }
    public void LeaveCloseupViewArrows()
    {
        arrow[2].SetActive(false);
        CheckViewEnd();
    }
}
