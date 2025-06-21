using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using Sirenix.OdinInspector;

[System.Serializable]
public class ButtonSet
{
    [LabelText("Buttons Model")]
    public GameObject buttons;
    public int onClickClipID;
    public Vector2 buttonsTransform;
    public string buttonsText;
}

[System.Serializable]
[InlineProperty]
[FoldoutGroup("Video Entry", expanded: true)]
public class Video
{
    [FoldoutGroup("Video Entry")]
    [PreviewField(70), HideLabel]
    public VideoClip clip;

    [FoldoutGroup("Video Entry")]
    [ToggleLeft]
    public bool choicesAppear;

    [FoldoutGroup("Video Entry")]
    [ShowIf("@choicesAppear")]
    public List<ButtonSet> buttonSet;

    [FoldoutGroup("Video Entry")]
    [ShowIf(nameof(choicesAppear))]
    [ToggleLeft]
    public bool timerAppear;

    [FoldoutGroup("Video Entry")]
    [ShowIf("@choicesAppear && timerAppear")]
    public GameObject timer;

    [FoldoutGroup("Video Entry")]
    [ShowIf("@choicesAppear && timerAppear")]
    public int timerDuration;

    [FoldoutGroup("Video Entry")]
    [ShowIf("@choicesAppear && timerAppear")]
    public int timerFailClipID;

    [FoldoutGroup("Video Entry")]
    [ToggleLeft]
    public bool autoContinue;

    [FoldoutGroup("Video Entry")]
    [ShowIf(nameof(autoContinue))]
    public int autoContinueClipID;
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
            {
                // If the clip stops at the last frame for the player to make a choice
                // AND
                // a timer exists to start the timer and count down to 0
                VideoManager.GetInstance().LoadVideo(video.clip, video.buttonSet, video.timer, video.timerFailClipID, video.timerDuration);
            }
            else
            {
                // If the clip stops at the last frame for the player to make a choice without timer
                VideoManager.GetInstance().LoadVideo(video.clip, video.buttonSet);
            }
        }
        else if (video.autoContinue)
            // If the clip continues to the next automatically after this ends
            VideoManager.GetInstance().LoadVideo(video.clip, video.autoContinueClipID);
        else
            // If the clip ends and there is no clip afterwards (basically the very last video of the chapter)
            VideoManager.GetInstance().LoadVideo(video.clip);
    }
}
