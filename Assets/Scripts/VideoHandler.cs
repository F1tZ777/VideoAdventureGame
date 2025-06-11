using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Video
{
    [SerializeField] public VideoClip clip;
    [SerializeField] public bool choice;
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
        if (videos[id].autoContinue)
            VideoManager.GetInstance().LoadVideo(videos[id].clip, false, videos[id].autoContinueClipID);
        else if (videos[id].choice)
            VideoManager.GetInstance().LoadVideo(videos[id].clip, true);
        else
            VideoManager.GetInstance().LoadVideo(videos[id].clip);
    }
}
