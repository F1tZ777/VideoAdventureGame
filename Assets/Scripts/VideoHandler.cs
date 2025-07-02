using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEditor;

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
    [HideInInspector]
    public VideoHandler parentHandler;

    [FoldoutGroup("Video Entry")]
    [PreviewField(70), HideLabel]
    public VideoClip clip;

    [FoldoutGroup("Video Entry")]
    [ToggleLeft]
    public bool choicesAppear;

    [FoldoutGroup("Video Entry")]
    [ShowIf("@choicesAppear")]
    public List<ButtonSet> buttonSet = new();
    //public SceneConfigScript buttonSet;

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

#if UNITY_EDITOR
    [FoldoutGroup("Video Entry")]
    [Button("Populate Buttons", ButtonSizes.Large)]
    [ShowIf(nameof(choicesAppear))]
    private void PopulateButtons()
    {

        if (parentHandler == null)
        {
            Debug.LogError("Parent VideoHandler not set. Populate will not work.");
            return;
        }

        Button[] foundButtons = parentHandler.GetComponentsInChildren<Button>(true);
        if (foundButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found under VideoHandler.");
            return;
        }

        buttonSet.Clear();

        foreach (Button button in foundButtons)
        {
            GameObject btnGo = button.gameObject;

            Vector2 anchoredPos = Vector2.zero;
            RectTransform rect = btnGo.GetComponent<RectTransform>();
            if (rect != null)
                anchoredPos = rect.anchoredPosition;

            string buttonText = "";
            TMP_Text textComp = btnGo.GetComponentInChildren<TMP_Text>();
            if (textComp != null)
                buttonText = textComp.text;

            GameObject prefabAsset = null;

#if UNITY_EDITOR
            prefabAsset = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(btnGo);
#endif

            buttonSet.Add(new ButtonSet
            {
                buttons = prefabAsset != null ? prefabAsset : btnGo,
                buttonsTransform = anchoredPos,
                buttonsText = buttonText,
                onClickClipID = 0
            });
        }

        Debug.Log($"Populated {buttonSet.Count} buttons for video: {clip?.name}");
    }
#endif
}

public class VideoHandler : MonoBehaviour
{
    [Header("Video List")]
    [SerializeField] public List<Video> videos;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (videos == null)
            return;
        foreach (var video in videos)
        {
            if (video != null)
                video.parentHandler = this;
        }
    }

    [Button("Import From CSV", ButtonSizes.Large)]
    [ButtonGroup("ImportExport")]
    private void ImportCSV()
    {
        string path = "Assets/Resources/videoData.csv";
        videos = CSVHandler.ImportCSV(path);
    }

    [Button("Export To CSV", ButtonSizes.Large)]
    [ButtonGroup("ImportExport")]
    private void ExportCSV()
    {
        string path = "Assets/Resources/videoData.csv";
        CSVHandler.ExportToCSV(videos, path);
    }
#endif

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
