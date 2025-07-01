//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEditor;

//[System.Serializable]
//public class ButtonSet
//{
//    [LabelText("Buttons Model")]
//    public GameObject buttons;
//    public int onClickClipID;
//    public Vector2 buttonsTransform;
//    public string buttonsText;
//}

//[System.Serializable]
//public class SceneConfigScript : MonoBehaviour
//{
//    public List<ButtonSet> buttons = new List<ButtonSet>();

//    [Button(ButtonSizes.Large)]
//    private void GrabButton()
//    {
//        Button[] foundButtons = GetComponentsInChildren<Button>(true);

//        if (foundButtons.Length == 0)
//        {
//            Debug.LogWarning("No button components found! Please create the button layout in the canvas that " +
//                "is found within the child of this game object. If no canvas is found, drag a canvas from the prefab menu " +
//                "into the child of this game object.");
//            return;
//        }

//        buttons.Clear();

//        foreach (Button button in foundButtons)
//        {
//            if (button == null) { continue; }

//            var btnGo = button.gameObject;

//            // Getting anchored position
//            Vector2 anchoredPos = Vector2.zero;
//            RectTransform rect = btnGo.GetComponent<RectTransform>();
//            if (rect != null)
//                anchoredPos = rect.anchoredPosition;

//            string buttonText = "";
//            var textComp = btnGo.GetComponent<TMP_Text>();
//            if (textComp != null)
//                buttonText = textComp.text;

//            GameObject prefabAsset = null;

//#if UNITY_EDITOR
//            prefabAsset = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(btnGo);
//#endif

//            // Fill data
//            buttons.Add(new ButtonSet
//            {
//                buttons = prefabAsset != null ? prefabAsset : btnGo,
//                buttonsTransform = anchoredPos,
//                buttonsText = buttonText,
//                onClickClipID = 0
//            });
//        }
//    }
//}
