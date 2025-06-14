using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Video
{
    [SerializeField] public VideoClip clip;
    [SerializeField] public bool choicesAppear;
    [SerializeField] public bool timerAppear;
    [SerializeField] public GameObject buttonSet;
    [SerializeField] public bool autoContinue;
    [SerializeField] public int autoContinueClipID;
}

public class VideoHandler : MonoBehaviour
{
    [Header("Video List")]
    [SerializeField] public List<Video> videos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendVideoToManager(int id)
    {
        if (videos[id].choicesAppear)
        {
            if (videos[id].timerAppear)
                // If the clip stops at the last frame for the player to make a choice
                // AND
                // a timer exists to start the timer and count down to 0
                VideoManager.GetInstance().LoadVideo(videos[id].clip, true, false, 0, videos[id].buttonSet, true);
            else
                // If the clip stops at the last frame for the player to make a choice without timer
                VideoManager.GetInstance().LoadVideo(videos[id].clip, true, false, 0, videos[id].buttonSet);
        }
        else if (videos[id].autoContinue)
            // If the clip continues to the next automatically after this ends
            VideoManager.GetInstance().LoadVideo(videos[id].clip, false, true, videos[id].autoContinueClipID);
        else
            // If the clip ends and there is no clip afterwards (basically the very last video of the chapter)
            VideoManager.GetInstance().LoadVideo(videos[id].clip);
    }
}
