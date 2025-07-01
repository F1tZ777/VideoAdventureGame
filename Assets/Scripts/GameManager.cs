using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Characters
{
    [SerializeField] public string name;
    [SerializeField] public int relationshipPoints;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //public Dictionary<string, int> charStats;
    public List<Characters> characters;
    string nameToChange;
    VideoHandler handler;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FetchHandler();
        BeginPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Start the scene with the first video clip immediately
    // Can call this function in ScenesManager in the future
    public void BeginPlay()
    {
        handler.SendVideoToManager(0);
    }

    void UpdateStats(string name, bool happy)
    {
        foreach (Characters character in characters)
        {
            if (character.name == name)
            {
                if (happy)
                    character.relationshipPoints++;
                else
                {
                    if (character.relationshipPoints == 0)
                        return;
                    else
                        character.relationshipPoints--;
                }

                return;
            }
        }

        Debug.LogError("No item in list with that name!");
    }

    public void SetNameToUpdate(string name) => nameToChange = name; // ALWAYS SET NAME FIRST BEFORE HAPPINESS

    public void SetHappiness(bool happy) => UpdateStats(nameToChange, happy);

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FetchHandler();
    }

    void FetchHandler()
    {
        handler = FindAnyObjectByType<VideoHandler>();
        if (!handler)
            Debug.LogError("No video handler initialized!");
    }

    public static GameManager GetInstance() => Instance;

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
