using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCamara_Menu : MonoBehaviour
{
    public Transform optionsPosition;  // hacer referencia a la posicion de la camara que va a cambiar cuando el usuario hace clic a los ajustes
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private float originalZoom;
    public float targetZoom = 4f;      // valor del zoom - orthograpic size: por defecto es 5
    public float speed = 4f;           // velocidad del movimiento de la camara
    public float zoomSpeed = 2f;       // velocidad del zoom

    private bool moveToTarget = false; // si la camara esta moviendo hacia la posicion querida (sirve para parar el movimiento de la camara cuando ha llegado a la posicion objetivo)
    private bool zooming = false;      

    void Start()
    {
        // posicion inicial de la camara
        originalPosition = transform.position;         
        originalZoom = Camera.main.orthographicSize;   
    }

    void Update()
    {
        if (moveToTarget)
        {
            // mover la camara a la posicion objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // comprobar si ha llegado o no
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f) // si la distancia entre la posición actual de la cámara y la posición objetivo es menor que 0.1 unidades:
            {
                transform.position = targetPosition; 
                moveToTarget = false;
            }
        }

        if (zooming)
        {
            // hacer zoom
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);

            // comprobar 
            if (Mathf.Abs(Camera.main.orthographicSize - targetZoom) < 0.001f)
            {
                Camera.main.orthographicSize = targetZoom; 
                zooming = false;
            }
        }
    }

    public void MoveCameraToOptions()
    {
        targetPosition = new Vector3(optionsPosition.position.x, optionsPosition.position.y, transform.position.z);
        targetZoom = 4f; 
        moveToTarget = true; 
        zooming = true;      
    }

    public void MoveCameraToMainMenu()
    {
        targetPosition = originalPosition;   
        targetZoom = originalZoom;           
        moveToTarget = true;                 
        zooming = true;                      
    }
}
