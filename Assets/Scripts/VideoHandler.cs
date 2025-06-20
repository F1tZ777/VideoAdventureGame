using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Video
{
    [SerializeField] public VideoClip clip;
    [SerializeField] public bool choicesAppear;
    [SerializeField] public GameObject buttonSet; // IF choicesAppear
    [SerializeField] public bool timerAppear; // IF choicesAppear
    [SerializeField] public GameObject timer; // IF choicesAppear AND timerAppear
    [SerializeField] public int timerTime; // IF choicesAppear AND timerAppear
    [SerializeField] public int timerFailClipID; // IF choicesAppear AND timerAppear
    [SerializeField] public bool autoContinue;
    [SerializeField] public int autoContinueClipID; // IF autoContinue
}

public class VideoHandler : MonoBehaviour
{
    [Header("Video List")]
    [SerializeField] public List<Video> videos;

    public void SendVideoToManager(int id)
    {
        //StateMachineManager.GetInstance().EnterLoadState();
        StateMachineManager.GetInstance().SwitchState(StateMachineManager.GetInstance().LoadState);

        Video video = videos[id];

        if (video.choicesAppear)
        {
            if (video.timerAppear)
                // If the clip stops at the last frame for the player to make a choice
                // AND
                // a timer exists to start the timer and count down to 0
                VideoManager.GetInstance().LoadVideo(video.clip, video.buttonSet, video.timer, video.timerFailClipID, video.timerTime);
            else
                // If the clip stops at the last frame for the player to make a choice without timer
                VideoManager.GetInstance().LoadVideo(video.clip, video.buttonSet);
        }
        else if (video.autoContinue)
            // If the clip continues to the next automatically after this ends
            VideoManager.GetInstance().LoadVideo(video.clip, video.autoContinueClipID);
        else
            // If the clip ends and there is no clip afterwards (basically the very last video of the chapter)
            VideoManager.GetInstance().LoadVideo(video.clip);
    }
}
