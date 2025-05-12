using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // Asegúrate de tener este using

/// <summary>
/// Un XR Socket Interactor que solo acepta interactables con una Tag específica.
/// Hereda toda la funcionalidad de XRSocketInteractor y añade una comprobación de Tag.
/// </summary>
/// 


public class XRSocketTagInteractor : XRSocketInteractor // Hereda de XRSocketInteractor
{
    [Header("Tag Filtering")]
    [Tooltip("La Tag que debe tener un interactable para ser aceptado en este socket. Déjalo vacío para permitir cualquier tag.")]
    public string targetTag; // Variable pública para establecer la Tag en el Inspector

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == targetTag;
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.tag == targetTag;
    }
}
