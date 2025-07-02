using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class CSVHandler
{
    public static string GetPrefabName(GameObject obj)
    {
#if UNITY_EDITOR
        if (obj != null)
        {
            // Check if the object is a prefab asset
            GameObject prefab = PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab
                ? obj : null;

            return prefab != null ? "Prefabs/" + prefab.name : "null"; // Return the prefab name if it is a prefab, name it null if not
        }
#endif
        return "null"; // Just give back null
    }

    public static string GetVideoClipName (VideoClip clip)
    {
        if (clip != null)
            return $"VideoClips/{clip.name}";
        return "null";
    }

    public static void ExportToCSV(List<Video> videos, string filePath)
    {
        List<string> lines = new List<string>();

        // Create header
        lines.Add("VideoClipID,VideoClipName,ChoicesAppear,ButtonSetObject,ButtonSetName,ButtonPosX,ButtonPosY,OnClickClipID,TimerAppear,TimerObject,TimerDuration,TimerFailClipID,AutoContinue,AutoContinueClipID");

        int index = 0;

        foreach (var video in videos)
        {
            string timerObject = GetPrefabName(video.timer);
            string clipName = GetVideoClipName(video.clip);

            if (video.buttonSet != null && video.buttonSet.Count > 0)
            {
                foreach (var buttonSet in video.buttonSet)
                {
                    string buttonSetObject = GetPrefabName(buttonSet.buttons);

                    string line = $"{index}," +
                                  $"{clipName}," +
                                  $"{video.choicesAppear}," +
                                  $"{buttonSetObject}," +
                                  $"{buttonSet.buttonsText}," +
                                  $"{buttonSet.buttonsTransform.x}," +
                                  $"{buttonSet.buttonsTransform.y}," +
                                  $"{buttonSet.onClickClipID}," +
                                  $"{video.timerAppear}," +
                                  $"{timerObject}," +
                                  $"{video.timerDuration}," +
                                  $"{video.timerFailClipID}," +
                                  $"{video.autoContinue}," +
                                  $"{video.autoContinueClipID}";

                    lines.Add(line);
                }
            }
            else
            {
                string line = $"{index}," +
                              $"{clipName}," +
                              $"{video.choicesAppear}," +
                              $"null," + // GameObject will be listed as null
                              $"," + // Text is empty
                              $"0," + // RectTransform X is 0
                              $"0," + // RectTransform Y is 0
                              $"0," + // onClickClipID is 0
                              $"{video.timerAppear}," +
                              $"{timerObject}," +
                              $"{video.timerDuration}," +
                              $"{video.timerFailClipID}," +
                              $"{video.autoContinue}," +
                              $"{video.autoContinueClipID}";
                lines.Add(line);
            }

            index++;
        }

        File.WriteAllLines(filePath, lines);
    }

    //public static List<Video> ImportFromCSV(string filePath)
    //{
    //    List<Video> videos = new List<Video>();
    //    var lines = File.ReadAllLines(filePath);

    //    // Skip header row
    //    for (int i = 1; i < lines.Length; i++)
    //    {
    //        var line = lines[i].Split(',');

    //        Video video = new Video();
    //        ButtonSet buttonSet = new ButtonSet();

    //        // Extract Video data (assuming one button set per line for simplicity)
    //        video.clip = Resources.Load<VideoClip>(line[0]);
    //        video.choicesAppear = bool.Parse(line[1]);
    //        buttonSet.buttons = Resources.Load<GameObject>(line[2]);
    //        buttonSet.buttonsText = line[3];
    //        buttonSet.buttonsTransform = new Vector2(float.Parse(line[4]), float.Parse(line[5]));
    //        buttonSet.onClickClipID = int.Parse(line[6]);
    //        video.timerAppear = bool.Parse(line[7]);
    //        video.timer = Resources.Load<GameObject>(line[8]);
    //        video.timerDuration = int.Parse(line[9]);
    //        video.timerFailClipID = int.Parse(line[10]);
    //        video.autoContinue = bool.Parse(line[11]);
    //        video.autoContinueClipID = int.Parse(line[12]);

    //        // Assign button set to video
    //        video.buttonSet = new List<ButtonSet> { buttonSet };

    //        videos.Add(video);
    //    }

    //    return videos;
    //}

    public static List<Video> ImportCSV(string filePath) 
    {
        Dictionary<int, Video> videoDict = new Dictionary<int, Video>();
        var lines = File.ReadAllLines(filePath);

        // Skip header row so start at 1
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Split(',');

            int clipID = int.Parse(line[0]);

            // Get existing video instance or create new video instance
            if (!videoDict.TryGetValue(clipID, out Video video))
            {
                video = new Video 
                {
                    clip = line[1] != "null" ? Resources.Load<VideoClip>(line[1]) : null,
                    choicesAppear = bool.Parse(line[2]),
                    timerAppear = bool.Parse(line[8]),
                    timer = line[9] != "null" ? Resources.Load<GameObject>(line[9]) : null,
                    timerDuration = int.Parse(line[10]),
                    timerFailClipID = int.Parse(line[11]),
                    autoContinue = bool.Parse(line[12]),
                    autoContinueClipID = int.Parse(line[13]),
                    buttonSet = new List<ButtonSet>()
                };

                videoDict[clipID] = video;
            }

            var buttonSet = new ButtonSet
            {
                buttons = line[3] != "null" ? Resources.Load<GameObject>(line[3]) : null,
                buttonsText = line[4],
                buttonsTransform = new Vector2(float.Parse(line[5]), float.Parse(line[6])),
                onClickClipID = int.Parse(line[7])
            };

            if (bool.Parse(line[2]))
                video.buttonSet.Add(buttonSet);
        }

        return new List<Video>(videoDict.Values);
    }
}
