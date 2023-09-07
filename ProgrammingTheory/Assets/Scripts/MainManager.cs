using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public GameObject[] AnimalPrefabs { get { return animalsPrefabs; } private set { animalsPrefabs = value; } }
    [SerializeField]
    private GameObject[] animalsPrefabs;

    public GameObject Animal { get { return animal; } private set { animal = value; } }
    [SerializeField]
    private GameObject animal;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectAnimal(int selection)
    {
        Animal = AnimalPrefabs[selection];
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
