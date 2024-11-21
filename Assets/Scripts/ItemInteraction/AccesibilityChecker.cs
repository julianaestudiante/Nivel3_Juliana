using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesibilityChecker : MonoBehaviour
{
    public static AccesibilityChecker Instance;
    public FeedbackTextController feedbackTextController;

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public bool ObjectAccessibilityChecker(Transform objectHit)
    {
        // Obtiene componentes de bloqueo y dependencias del objeto
        Lock itemLocked = objectHit.GetComponent<Lock>();
        Tags itemTags = objectHit.GetComponent<Tags>();
        DependencyHandler itemDependencies = objectHit.GetComponent<DependencyHandler>();
        OrderedDependencies itemDependencyByOrder = objectHit.GetComponent<OrderedDependencies>();
        CombinationLockControl combinationLocked = objectHit.GetComponent<CombinationLockControl>();
        Coleccionable collectable = objectHit.GetComponent<Coleccionable>();

        // Si el objeto está bloqueado, muestra mensaje y devuelve falso
        if (itemLocked != null && itemLocked.isLocked)
        {
            feedbackTextController.PopUpText(itemTags.displayText[0]);
            return false;
        }
        // Si el objeto tiene dependencias no cumplidas, muestra mensaje y devuelve falso
        if (itemDependencies != null && !itemDependencies.dependencyMet)
        {
            feedbackTextController.PopUpText(itemTags.displayText[0]);
            return false;
        }
        if (itemDependencyByOrder != null)
        {

            return itemDependencyByOrder.feedbackTextDesider();
        }
        // Si el objeto está en un candado de combinación, muestra mensaje y devuelve falso
        if (combinationLocked != null && combinationLocked.isLocked)
        {

            return false;
        }
        // Si el objeto es coleccionable, lo recoge y devuelve falso
        if (collectable != null)
        {
            collectable.CollectItem();
            return false;
        }

        // Si no hay restricciones, devuelve verdadero
        return true;
    }
}
