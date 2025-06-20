using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    static VideoManager instance;
    VideoPlayer player;
    VideoHandler handler;
    bool choice = false, autoContinue = false;
    int continueClipID, timerClipID, timerDuration;
    GameObject buttonSetActive;
    GameObject timerSetActive;
    
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

    public void LoadVideo(VideoClip clip, GameObject buttonSet)
    {
        SetClip(clip);

        choice = true;
        buttonSetActive = buttonSet;
    }

    public void LoadVideo(VideoClip clip, GameObject buttonSet, GameObject timer, int timerID, int time)
    {
        SetClip(clip);

        choice = true;
        buttonSetActive = buttonSet;
        timerSetActive = timer;
        timerClipID = timerID;
        timerDuration = time;

        timerSetActive.SetActive(true);

        StateMachineManager.GetInstance().SetSlider(timerSetActive, timerDuration);
    }

    //public void LoadVideo(VideoClip clip, bool choicesAppear = false, bool autoContinueBool = false, int continueID = 0, GameObject buttonSet = null, bool timerAppear = false)
    //{
    //    // Reset all data
    //    choice = false;
    //    timer = false;
    //    timerStart = false;
    //    buttonSetActive = null;
    //    autoContinue = false;
    //    continueClipID = 0;

    //    player.clip = clip;
    //    PlayVideo();

    //    /* Choices Appear Notes:
    //     * - If choices appear, have the video pause at the very last frame and enable buttonSet game object
    //     * - If timerAppear = true, the timer will count down (slider goes from 1 to 0 using deltaTime)
    //     * - In the buttons' OnClick() method, declare the clipID that will be shown if the specified button
    //     * is click
    //     * - In other words, technically, there is no need for ButtonHandler cuz everything is being handled
    //     * by VideoManager :wilted_rose:
    //     */
    //    if (choicesAppear)
    //    {
    //        choice = true;
    //        buttonSetActive = buttonSet;

    //        if (timerAppear)
    //        {
    //            timer = true;
    //        }
    //    }

    //    /* As for autoContinue:
    //     * - If autoContinue is true, play the video as normal to the end
    //     * - Once the video is finished (figure out how to find that out), use the autoContinueClipID that
    //     * was passed from VideoHandler and pass it back to VideoHandler using the SendVideoToManager() method
    //     * in order to get the rest of the data within that ClipID
    //     */
    //    if (autoContinueBool)
    //    {
    //        autoContinue = true;
    //        continueClipID = continueID;
    //    }
    //}

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
            //StateMachineManager.GetInstance().EnterFinishedState();
            StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().FinishedState);
            buttonSetActive.SetActive(true);

            if (timerSetActive)
            {
                timerSetActive.SetActive(true);
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

    public void TimerFailSendClip()
    {
        handler.SendVideoToManager(timerClipID);
        buttonSetActive.SetActive(false);
        timerSetActive.SetActive(false);
        timerSetActive.GetComponent<Slider>().value = 1;
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
