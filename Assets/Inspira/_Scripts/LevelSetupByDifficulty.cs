using UnityEngine;
using System.Collections.Generic;

// Estructura para definir la configuración de objetos para UN nivel de dificultad
[System.Serializable]
public class DifficultyObjectSetup
{
    [Tooltip("Nivel de dificultad al que aplica esta configuración (ej. 1, 2, o 3).")]
    public int difficultyLevelValue;

    [Tooltip("Los 3 GameObjects (prefabs) que se instanciarán/usarán para este nivel.")]
    public GameObject[] objectPrefabsOrReferences = new GameObject[3]; // Puede ser prefab o referencia a objeto en escena si se usa preExistingObjects

    [Tooltip("Los 3 Transforms de referencia en la escena donde aparecerán los objetos. Deben corresponder en orden con objectPrefabsOrReferences.")]
    public Transform[] spawnPointReferences = new Transform[3];

    // Constructor para asegurar que los arrays se inicialicen con tamaño 3
    public DifficultyObjectSetup()
    {
        objectPrefabsOrReferences = new GameObject[3];
        spawnPointReferences = new Transform[3];
    }
}

public class LevelSetupByDifficulty : MonoBehaviour
{
    [Header("PlayerPrefs Key")]
    [SerializeField] private string difficultyPlayerPrefsKey = "difficultLevel";

    [Header("Default Difficulty")]
    [Tooltip("Valor de dificultad a usar si la PlayerPref no se encuentra o es inválida.")]
    [SerializeField] private int defaultDifficulty = 1;

    [Header("Difficulty Configurations")]
    [Tooltip("Define la configuración de objetos para cada nivel de dificultad.")]
    public List<DifficultyObjectSetup> difficultySetups = new List<DifficultyObjectSetup>();

    [Header("Object Management")]
    [Tooltip("Si los objetos ya existen en la escena y solo deben moverse/activarse, asígnalos aquí en el orden correcto (0, 1, 2). Si está vacío o un elemento es nulo, se intentará instanciar desde 'objectPrefabsOrReferences' del setup correspondiente.")]
    public List<GameObject> preExistingObjects = new List<GameObject>(3); // Lista para 3 objetos pre-existentes


    private List<GameObject> managedObjectsThisSession = new List<GameObject>(); // Objetos instanciados o gestionados

    void Start()
    {
        // Asegurarse de que preExistingObjects tenga 3 elementos, incluso si son nulos, para evitar errores de índice
        while (preExistingObjects.Count < 3)
        {
            preExistingObjects.Add(null);
        }

        LoadAndApplyDifficultySetup();
    }

    void LoadAndApplyDifficultySetup()
    {
        int currentDifficulty = PlayerPrefs.GetInt(difficultyPlayerPrefsKey, defaultDifficulty);
        Debug.Log($"Nivel de dificultad cargado: {currentDifficulty} (PlayerPrefs Key: '{difficultyPlayerPrefsKey}')", this);

        DifficultyObjectSetup selectedSetup = null;
        foreach (DifficultyObjectSetup setup in difficultySetups)
        {
            if (setup.difficultyLevelValue == currentDifficulty)
            {
                selectedSetup = setup;
                break;
            }
        }

        if (selectedSetup == null)
        {
            Debug.LogError($"No se encontró configuración para el nivel de dificultad {currentDifficulty}. Asegúrate de tener una entrada en 'Difficulty Setups' para este valor.", this);
            return;
        }

        ApplySetup(selectedSetup);
    }

    void ApplySetup(DifficultyObjectSetup setup)
    {
        // Limpiar/desactivar objetos anteriores si fueron instanciados por este script en esta sesión
        foreach (GameObject obj in managedObjectsThisSession)
        {
            if (obj != null)
            {
                // Si el objeto fue instanciado (no es uno de los preExistingObjects originales), lo destruimos.
                // Si era un preExistingObject, solo lo desactivamos si no se va a usar en el nuevo setup.
                // Una forma más simple por ahora: si fue instanciado, destruir. Si era pre-existente, no hacer nada aquí, se reactivará si se usa.
                bool wasInstantiated = true;
                for (int i = 0; i < preExistingObjects.Count; ++i)
                {
                    if (preExistingObjects[i] == obj)
                    {
                        wasInstantiated = false;
                        break;
                    }
                }
                if (wasInstantiated)
                {
                    Destroy(obj);
                }
                else
                {
                    // Podrías desactivarlo aquí si no lo vas a usar, pero es más complejo
                    // obj.SetActive(false);
                }
            }
        }
        managedObjectsThisSession.Clear();

        Debug.Log($"Aplicando configuración para el nivel de dificultad: {setup.difficultyLevelValue}", this);

        for (int i = 0; i < 3; i++) // Asumimos siempre 3 objetos
        {
            if (i >= setup.spawnPointReferences.Length || setup.spawnPointReferences[i] == null)
            {
                Debug.LogWarning($"El Transform de referencia para el spawn point {i} no está asignado o el array es demasiado corto para el nivel {setup.difficultyLevelValue}. Saltando este objeto.", this);
                continue;
            }

            // El prefab/referencia de objeto también debe existir para este índice
            if (i >= setup.objectPrefabsOrReferences.Length)
            {
                Debug.LogWarning($"El array 'objectPrefabsOrReferences' es demasiado corto para el índice {i} en el nivel {setup.difficultyLevelValue}. Saltando este objeto.", this);
                continue;
            }


            GameObject objectToPlace = null;

            // Opción 1: Usar objeto pre-existente si está definido para este índice
            if (preExistingObjects.Count > i && preExistingObjects[i] != null)
            {
                objectToPlace = preExistingObjects[i];
                Debug.Log($"Usando objeto pre-existente '{objectToPlace.name}' para el índice {i}.", this);
            }
            // Opción 2: Instanciar desde prefab si no hay pre-existente para este índice
            else if (setup.objectPrefabsOrReferences[i] != null)
            {
                objectToPlace = Instantiate(setup.objectPrefabsOrReferences[i]);
                Debug.Log($"Instanciando prefab '{setup.objectPrefabsOrReferences[i].name}' para el índice {i}.", this);
            }
            else
            {
                Debug.LogWarning($"Ni objeto pre-existente ni prefab/referencia definido en 'objectPrefabsOrReferences' para el índice {i} en el nivel {setup.difficultyLevelValue}. Saltando.", this);
                continue;
            }

            if (objectToPlace != null)
            {
                // Aplicar Transform usando el Transform de referencia
                Transform spawnReference = setup.spawnPointReferences[i];
                objectToPlace.transform.position = spawnReference.position;
                objectToPlace.transform.rotation = spawnReference.rotation;
                // Si también quieres que coincida la escala del objeto de referencia:
                // objectToPlace.transform.localScale = spawnReference.localScale;

                objectToPlace.SetActive(true); // Asegurarse de que esté activo
                managedObjectsThisSession.Add(objectToPlace); // Añadir a la lista de gestionados
            }
        }
    }
}