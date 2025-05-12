using UnityEngine;
using UnityEngine.Playables; // Para el PlayableDirector del final

public class SimpleGameManager : MonoBehaviour
{
    public GameObject rocksGroup;           // Arrastra aquí Rocks_Group
    public GameObject tooltipsGroup;        // Arrastra aquí Tooltips_Group
    public GameObject videoPlayerObject;    // Arrastra aquí VideoPlayer_Object
    public PlayableDirector finalTimeline;  // Arrastra aquí el PlayableDirector de este mismo GameObject (o uno dedicado)
    public GameObject particleLudic2;

    private int almacenadosCount = 0;
    private int desechadosCount = 0;
    private const int TARGET_ALMACENABLE = 1;
    private const int TARGET_DESECHO = 3;

    private bool taskCompleted = false;

    void Start()
    {
        // Asegurarse de que todo esté desactivado al inicio si no lo está por defecto
        if (rocksGroup) rocksGroup.SetActive(false);
        if (tooltipsGroup) tooltipsGroup.SetActive(false);
        if (videoPlayerObject) videoPlayerObject.SetActive(false);
    }
    public void ActiveParticleLudic2()
    {
        if(particleLudic2) particleLudic2.SetActive(true);

    }
    // Llamado por RuthDialoguePlayer o un Signal Emitter
    public void ActivateRocksAndTooltips()
    {
        Debug.Log("Activando rocas y tooltips.");
        if (rocksGroup) rocksGroup.SetActive(true);
        if (tooltipsGroup) tooltipsGroup.SetActive(true);
    }

    // Llamado por RuthDialoguePlayer o un Signal Emitter
    public void DeactivateTooltipsAndEnableRockInteraction()
    {
        Debug.Log("Desactivando tooltips. El jugador ahora puede agarrar rocas.");
        if (tooltipsGroup) tooltipsGroup.SetActive(false);
        // La interacción de las rocas se asume que ya está habilitada en sus prefabs (XRGrabInteractable, etc.)
        // Si necesitas activar/desactivar los scripts de agarre, lo harías aquí iterando sobre las rocas.
    }

    // Llamado por SortingBin.cs
    public void RockSorted(string tag)
    {
        if (taskCompleted) return; // Si ya se completó, no hacer nada más

        if (tag == "Almacenable")
        {
            almacenadosCount++;
            Debug.Log($"Almacenados: {almacenadosCount}/{TARGET_ALMACENABLE}");
        }
        else if (tag == "Desechable")
        {
            desechadosCount++;
            Debug.Log($"Desechados: {desechadosCount}/{TARGET_DESECHO}");
        }

        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (almacenadosCount >= TARGET_ALMACENABLE && desechadosCount >= TARGET_DESECHO)
        {
            taskCompleted = true;
            Debug.Log("¡Tarea completada! 1 almacenado y 3 desechados.");
            ProceedToFinalSequence();
        }
    }
    public void ActivateVideoSignal()
    {
        Debug.Log("Activando video.");
        if (videoPlayerObject)
        {
            videoPlayerObject.SetActive(true);

        }

    }
    private void ProceedToFinalSequence()
    {
        

        if (finalTimeline != null)
        {
            finalTimeline.Play(); // Este timeline debería tener el audio de felicitaciones
        }
        else
        {
            // Si no hay timeline final, podrías llamar a un método en RuthDialoguePlayer directamente
            // GameObject.FindObjectOfType<RuthDialoguePlayer>()?.PlayFelicitacionesSignal(); // No es lo ideal, mejor referencia directa
        }
    }
}