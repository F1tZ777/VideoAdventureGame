using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class VideoManager : MonoBehaviour
{
    static VideoManager instance;
    VideoPlayer player;
    VideoHandler handler;
    bool choice = false, autoContinue = false;
    int continueClipID, timerClipID, timerDuration;
    List<ButtonSet> buttonSetActive;
    GameObject timerSetActive, timerCached;
    Transform canvasTransform;
    List<GameObject> buttonsCached = new List<GameObject>();
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        player = this.GetComponent<VideoPlayer>();
    }

    void Start()
    {
        player.loopPointReached += EndReached;
        FetchImportantStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadVideo(VideoClip clip)
    {
        SetClip(clip);
    }

    public void LoadVideo(VideoClip clip, int continueID)
    {
        SetClip(clip);

        autoContinue = true;
        continueClipID = continueID;
    }

    public void LoadVideo(VideoClip clip, List<ButtonSet> buttonSet)
    {
        SetClip(clip);

        choice = true;
        buttonSetActive = buttonSet;
    }

    public void LoadVideo(VideoClip clip, List<ButtonSet> buttonSet, GameObject timer, int timerID, int time)
    {
        SetClip(clip);

        choice = true;
        buttonSetActive = buttonSet;
        timerSetActive = timer;
        timerClipID = timerID;
        timerDuration = time;

        //StateMachineManager.GetInstance().SetSlider(timerSetActive, timerDuration);
    }

    public void SetClip(VideoClip clip)
    {
        ResetData();
        player.clip = clip;
        PlayVideo();
    }

    public void PlayVideo()
    {
        //StateMachineManager.GetInstance().EnterPlayState();
        StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().PlayState);
        player.Play();
    }

    public void StopVideo()
    {
        //StateMachineManager.GetInstance().EnterIdleState();
        StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().IdleState);
        player.Stop();
    }

    public void PauseVideo()
    {
        //StateMachineManager.GetInstance().EnterPauseState();
        StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().PauseState);
        player.Pause();
    }

    void ResetData()
    {
        choice = false;
        timerSetActive = null;
        timerClipID = 0;
        timerDuration = 0;
        buttonSetActive = null;
        autoContinue = false;
        continueClipID = 0;
    }

    void EndReached(VideoPlayer player)
    {
        if (choice)
        {
            buttonsCached.Clear();
            //StateMachineManager.GetInstance().EnterFinishedState();
            StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().FinishedState);
            //buttonSetActive.SetActive(true);
            foreach (var button in buttonSetActive)
            {
                GameObject spawnButton = Instantiate(button.buttons, canvasTransform);
                spawnButton.SetActive(true);

                SetPosInCanvas(spawnButton, button.buttonsTransform);

                // Button On Click initialize here
                UnityEngine.UI.Button buttonComp = spawnButton.GetComponent<UnityEngine.UI.Button>();
                buttonComp.onClick.AddListener(() => PlayOnClick(button.onClickClipID));

                TMP_Text buttonText = spawnButton.GetComponentInChildren<TMP_Text>();
                buttonText.text = button.buttonsText;

                // Add spawnButton into the list of game objects
                buttonsCached.Add(spawnButton);
            }

            if (timerSetActive)
            {
                timerCached = null;
                timerCached = Instantiate(timerSetActive, canvasTransform);
                timerCached.SetActive(true);
                StateMachineManager.GetInstance().SetSlider(timerCached, timerDuration);
                if (timerCached != null)
                    Debug.Log("Timer has been created");
            }

            return;
        }

        if (autoContinue)
        {
            handler.SendVideoToManager(continueClipID);
            return;
        }

        //StateMachineManager.GetInstance().EnterIdleState();
        StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().IdleState);
    }

    public void PlayOnClick(int id)
    {
        DestroyOverlay();
        handler.SendVideoToManager(id);
    }

    public void TimerFailSendClip()
    {
        DestroyOverlay();
        handler.SendVideoToManager(timerClipID);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FetchImportantStuff();
    }

    void SetPosInCanvas(GameObject obj, Vector2 pos)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();

        // Reset scale and rotation to prevent odd layout behavior
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        // Set pivot or anchors if needed (optional)
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        // Set the anchored position (relative to the anchor)
        rect.anchoredPosition = pos;
    }

    void DestroyOverlay()
    {
        if (timerCached)
        {
            Destroy(timerCached);
            Debug.Log("Timer has been destroyed");
        }
        foreach (var button in buttonsCached)
        {
            Destroy(button);
        }
    }

    void FetchHandler()
    {
        handler = FindAnyObjectByType<VideoHandler>();
        if (!handler)
            Debug.LogError("No video handler initialized!");
    }

    void FetchCanvas()
    {
        canvasTransform = FindObjectOfType<Canvas>().gameObject.transform;
        if (!canvasTransform)
            Debug.LogError("Unable to find canvas for some reason");
    }

    void FetchImportantStuff()
    {
        FetchHandler();
        FetchCanvas();
    }

    public static VideoManager GetInstance() => instance;

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        player.loopPointReached -= EndReached;
    }
}
