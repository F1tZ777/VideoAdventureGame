using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    static VideoManager instance;
    VideoPlayer player;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        player = this.GetComponent<VideoPlayer>();
    }

    public static VideoManager GetInstance() { return instance; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadVideo(VideoClip clip, bool choicesAppear = false, int continueID = 0)
    {
        player.clip = clip;
        PlayVideo();
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
}
