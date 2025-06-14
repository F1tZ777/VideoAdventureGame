using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    static VideoManager instance;
    VideoPlayer player;
    VideoHandler handler;
    bool choice = false, autoContinue = false, timer = false;
    [HideInInspector] public bool timerStart = false;
    int continueClipID;
    GameObject buttonSetActive;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        player = this.GetComponent<VideoPlayer>();
    }
    void Start()
    {
        player.loopPointReached += EndReached;
        FetchHandler();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadVideo(VideoClip clip, bool choicesAppear = false, bool autoContinueBool = false, int continueID = 0, GameObject buttonSet = null, bool timerAppear = false)
    {
        // Reset all data
        choice = false;
        timer = false;
        timerStart = false;
        buttonSetActive = null;
        autoContinue = false;
        continueClipID = 0;

        player.clip = clip;
        PlayVideo();

        /* Choices Appear Notes:
         * - If choices appear, have the video pause at the very last frame and enable buttonSet game object
         * - If timerAppear = true, the timer will count down (slider goes from 1 to 0 using deltaTime)
         * - In the buttons' OnClick() method, declare the clipID that will be shown if the specified button
         * is click
         * - In other words, technically, there is no need for ButtonHandler cuz everything is being handled
         * by VideoManager :wilted_rose:
         */
        if (choicesAppear)
        {
            choice = true;
            buttonSetActive = buttonSet;

            if (timerAppear)
            {
                timer = true;
            }
        }

        /* As for autoContinue:
         * - If autoContinue is true, play the video as normal to the end
         * - Once the video is finished (figure out how to find that out), use the autoContinueClipID that
         * was passed from VideoHandler and pass it back to VideoHandler using the SendVideoToManager() method
         * in order to get the rest of the data within that ClipID
         */
        if (autoContinueBool)
        {
            autoContinue = true;
            continueClipID = continueID;
        }
    }

    public void PlayVideo()
    {
        player.Play();
    }

    public void StopVideo()
    {
        player.Stop();
    }

    public void PauseVideo()
    {
        player.Pause();
    }

    void EndReached(VideoPlayer player)
    {
        if (choice)
        {
            buttonSetActive.SetActive(true);

            if (timer)
            {
                timerStart = true;
            }
        }

        if (autoContinue)
            handler.SendVideoToManager(continueClipID);
    }

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

    public static VideoManager GetInstance() => instance;
}
