using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(VideoHandler))]
public class VideoHandlerEditor : Editor
{
    VideoHandler m_VideoHandler;
    SerializedObject GetTarget;
    SerializedProperty VideoList;
    int ListSize;

    ReorderableList reorderableVideoList;

    void OnEnable()
    {
        m_VideoHandler = (VideoHandler)target;
        GetTarget = new SerializedObject(m_VideoHandler);
        VideoList = GetTarget.FindProperty("videos");

        // Setup reorderable list
        reorderableVideoList = new ReorderableList(GetTarget, VideoList, true, true, true, true);

        reorderableVideoList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Video Properties");

            // Prepare rect for the int field on the right side (about 50px wide)
            Rect countRect = new Rect(rect.xMax - 55, rect.y + 2, 50, EditorGUIUtility.singleLineHeight);

            // Get current array size
            ListSize = VideoList.arraySize;

            // Draw int field aligned right
            int newSize = EditorGUI.IntField(countRect, ListSize);

            // Clamp size to 0 or positive only
            if (newSize < 0) newSize = 0;

            // If user changed the size, update list
            if (newSize != ListSize)
            {
                while (newSize > VideoList.arraySize)
                    VideoList.InsertArrayElementAtIndex(VideoList.arraySize);

                while (newSize < VideoList.arraySize)
                    VideoList.DeleteArrayElementAtIndex(VideoList.arraySize - 1);
            }
        };

        reorderableVideoList.elementHeightCallback = (int index) =>
        {
            var element = VideoList.GetArrayElementAtIndex(index);
            float lineHeight = EditorGUIUtility.singleLineHeight + 2;
            float height = lineHeight; // foldout height

            if (element.isExpanded)
            {
                SerializedProperty choicesAppearProp = element.FindPropertyRelative("choicesAppear");
                SerializedProperty timerAppearProp = element.FindPropertyRelative("timerAppear");
                SerializedProperty autoContinueProp = element.FindPropertyRelative("autoContinue");

                // Always shown: clip, choicesAppear, autoContinue
                height += 3 * lineHeight;

                if (choicesAppearProp.boolValue)
                {
                    height += lineHeight * 2; // buttonSet, timerAppear

                    if (timerAppearProp.boolValue)
                        height += lineHeight * 3; // timer, timerTime, timerFailClipID
                }

                if (autoContinueProp.boolValue)
                    height += lineHeight; // autoContinueClipID

                height += 10;
            }

            return height;
        };

        reorderableVideoList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = VideoList.GetArrayElementAtIndex(index);

            // Detect hover to highlight
            if (rect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(rect, new Color(0.3f, 0.5f, 1f, 0.2f));
            }

            // Drag handle takes ~12px on the left, so offset foldout start by about 15px
            Rect foldoutRect = new Rect(rect.x + 15, rect.y, rect.width - 15, EditorGUIUtility.singleLineHeight);

            // Foldout
            element.isExpanded = EditorGUI.Foldout(
                foldoutRect,
                element.isExpanded,
                $"Video {index}",
                true);

            if (element.isExpanded )
            {
                EditorGUI.indentLevel++;

                float lineHeight = EditorGUIUtility.singleLineHeight + 2;
                Rect fieldRect = new Rect(rect.x, rect.y + lineHeight, rect.width, EditorGUIUtility.singleLineHeight );

                //[SerializeField] public VideoClip clip;
                //[SerializeField] public bool choicesAppear;
                //[SerializeField] public GameObject buttonSet; // IF choicesAppear
                //[SerializeField] public bool timerAppear; // IF choicesAppear
                //[SerializeField] public GameObject timer; // IF choicesAppear AND timerAppear
                //[SerializeField] public int timerTime; // IF choicesAppear AND timerAppear
                //[SerializeField] public int timerFailClipID; // IF choicesAppear AND timerAppear
                //[SerializeField] public bool autoContinue;
                //[SerializeField] public int autoContinueClipID;

                SerializedProperty clipProp = element.FindPropertyRelative("clip");
                SerializedProperty choicesAppearProp = element.FindPropertyRelative("choicesAppear");
                SerializedProperty buttonSetProp = element.FindPropertyRelative("buttonSet");
                SerializedProperty timerAppearProp = element.FindPropertyRelative("timerAppear");
                SerializedProperty timerProp = element.FindPropertyRelative("timer");
                SerializedProperty timerTimeProp = element.FindPropertyRelative("timerTime");
                SerializedProperty timerFailClipIDProp = element.FindPropertyRelative("timerFailClipID");
                SerializedProperty autoContinueProp = element.FindPropertyRelative("autoContinue");
                SerializedProperty autoContinueClipIDProp = element.FindPropertyRelative("autoContinueClipID");

                EditorGUI.PropertyField(fieldRect, clipProp, new GUIContent("Video Clip"));

                fieldRect.y += lineHeight;
                EditorGUI.PropertyField(fieldRect, choicesAppearProp);

                if (choicesAppearProp.boolValue)
                {
                    fieldRect.y += lineHeight;
                    EditorGUI.PropertyField(fieldRect, buttonSetProp);

                    fieldRect.y += lineHeight;
                    EditorGUI.PropertyField(fieldRect, timerAppearProp);

                    if (timerAppearProp.boolValue)
                    {
                        fieldRect.y += lineHeight;
                        EditorGUI.PropertyField(fieldRect, timerProp);

                        fieldRect.y += lineHeight;
                        EditorGUI.PropertyField(fieldRect, timerTimeProp);

                        fieldRect.y += lineHeight;
                        EditorGUI.PropertyField(fieldRect, timerFailClipIDProp);
                    }
                }

                fieldRect.y += lineHeight;
                EditorGUI.PropertyField(fieldRect, autoContinueProp);

                if (autoContinueProp.boolValue)
                {
                    fieldRect.y += lineHeight;
                    EditorGUI.PropertyField(fieldRect, autoContinueClipIDProp);
                }

                EditorGUI.indentLevel--;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        // Update list
        GetTarget.Update();

        reorderableVideoList.DoLayoutList();

        GetTarget.ApplyModifiedProperties();
    }
}
