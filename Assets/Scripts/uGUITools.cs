//using System;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using Object = UnityEngine.Object;

//public class uGUITools : MonoBehaviour
//{
//    [MenuItem("uGUI/Anchors to Corners %[")]
//    static void AnchorsToCorners()
//    {
//        foreach (Transform transform in Selection.transforms)
//        {
//            RectTransform t = transform as RectTransform;
//            RectTransform pt = Selection.activeTransform.parent as RectTransform;

//            if (t == null || pt == null) return;

//            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
//                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
//            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
//                                                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

//            t.anchorMin = newAnchorsMin;
//            t.anchorMax = newAnchorsMax;
//            t.offsetMin = t.offsetMax = new Vector2(0, 0);
//        }
//    }

//    [MenuItem("uGUI/Corners to Anchors %]")]
//    static void CornersToAnchors()
//    {
//        foreach (Transform transform in Selection.transforms)
//        {
//            RectTransform t = transform as RectTransform;

//            if (t == null) return;

//            t.offsetMin = t.offsetMax = new Vector2(0, 0);
//        }
//    }

//    [MenuItem("uGUI/Maximize Game Window &P")]
//    static void MaximizeGameWindow()
//    {
//        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
//        EditorWindow.GetWindow(T).maximized = !EditorWindow.GetWindow(T).maximized;
//    }

//    [MenuItem("uGUI/Maximize Scene Window &O")]
//    static void MaximizeSceneWindow()
//    {
//        System.Type T = System.Type.GetType("UnityEditor.SceneView,UnityEditor");
//        EditorWindow.GetWindow(T).maximized = !EditorWindow.GetWindow(T).maximized;
//    }

//    [MenuItem("uGUI/Reset Anchors &;")]
//    static void ResetAnchors()
//    {
//        RectTransform rectTransform = Selection.activeTransform as RectTransform;
//        rectTransform.anchorMin = Vector2.one * 0.5f;
//        rectTransform.anchorMax = Vector2.one * 0.5f;
//    }

//    [MenuItem("uGUI/Reset Size &'")]
//    static void ResetSize()
//    {
//        RectTransform rectTransform = Selection.activeTransform as RectTransform;
//        Vector2 spriteDimensions = Selection.activeGameObject.GetComponent<Image>().sprite.bounds.size;
//        rectTransform.sizeDelta = spriteDimensions * 100;
//    }

//    [MenuItem("uGUI/Adjust width to meet aspect ration &[")]
//    static void WidthToAspectRation()
//    {
//        RectTransform rectTransform = Selection.activeTransform as RectTransform;
//        Vector2 spriteDimensions = Selection.activeGameObject.GetComponent<Image>().sprite.bounds.size;
//        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * spriteDimensions.x / spriteDimensions.y, rectTransform.sizeDelta.y);
//    }

//    [MenuItem("uGUI/Adjust height to meet aspect ration &]")]
//    static void HeightToAspectRation()
//    {
//        RectTransform rectTransform = Selection.activeTransform as RectTransform;
//        Vector2 spriteDimensions = Selection.activeGameObject.GetComponent<Image>().sprite.bounds.size;
//        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x * spriteDimensions.y / spriteDimensions.x);
//    }

//    [MenuItem("uGUI/Mirror Horizontally Around Anchors %;")]
//    static void MirrorHorizontallyAnchors()
//    {
//        MirrorHorizontally(false);
//    }

//    [MenuItem("uGUI/Mirror Horizontally Around Parent Center %:")]
//    static void MirrorHorizontallyParent()
//    {
//        MirrorHorizontally(true);
//    }

//    static void MirrorHorizontally(bool mirrorAnchors)
//    {
//        foreach (Transform transform in Selection.transforms)
//        {
//            RectTransform t = transform as RectTransform;
//            RectTransform pt = Selection.activeTransform.parent as RectTransform;

//            if (t == null || pt == null) return;

//            if (mirrorAnchors)
//            {
//                Vector2 oldAnchorMin = t.anchorMin;
//                t.anchorMin = new Vector2(1 - t.anchorMax.x, t.anchorMin.y);
//                t.anchorMax = new Vector2(1 - oldAnchorMin.x, t.anchorMax.y);
//            }

//            Vector2 oldOffsetMin = t.offsetMin;
//            t.offsetMin = new Vector2(-t.offsetMax.x, t.offsetMin.y);
//            t.offsetMax = new Vector2(-oldOffsetMin.x, t.offsetMax.y);

//            t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
//        }
//    }

//    [MenuItem("uGUI/Mirror Vertically Around Anchors %'")]
//    static void MirrorVerticallyAnchors()
//    {
//        MirrorVertically(false);
//    }

//    [MenuItem("uGUI/Mirror Vertically Around Parent Center %\"")]
//    static void MirrorVerticallyParent()
//    {
//        MirrorVertically(true);
//    }

//    static void MirrorVertically(bool mirrorAnchors)
//    {
//        foreach (Transform transform in Selection.transforms)
//        {
//            RectTransform t = transform as RectTransform;
//            RectTransform pt = Selection.activeTransform.parent as RectTransform;

//            if (t == null || pt == null) return;

//            if (mirrorAnchors)
//            {
//                Vector2 oldAnchorMin = t.anchorMin;
//                t.anchorMin = new Vector2(t.anchorMin.x, 1 - t.anchorMax.y);
//                t.anchorMax = new Vector2(t.anchorMax.x, 1 - oldAnchorMin.y);
//            }

//            Vector2 oldOffsetMin = t.offsetMin;
//            t.offsetMin = new Vector2(t.offsetMin.x, -t.offsetMax.y);
//            t.offsetMax = new Vector2(t.offsetMax.x, -oldOffsetMin.y);

//            t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
//        }
//    }

//    private static EditorWindow _mouseOverWindow;

//    [MenuItem("uGUI/Select Inspector under mouse cursor (use hotkey) #&q")]
//    static void SelectLockableInspector()
//    {
//        if (EditorWindow.mouseOverWindow.GetType().Name == "InspectorWindow")
//        {
//            _mouseOverWindow = EditorWindow.mouseOverWindow;
//            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
//            Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
//            int indexOf = findObjectsOfTypeAll.ToList().IndexOf(_mouseOverWindow);
//            EditorPrefs.SetInt("LockableInspectorIndex", indexOf);
//        }
//    }

//    [MenuItem("uGUI/Toggle Lock &q")]
//    static void ToggleInspectorLock()
//    {
//        if (_mouseOverWindow == null)
//        {
//            if (!EditorPrefs.HasKey("LockableInspectorIndex"))
//                EditorPrefs.SetInt("LockableInspectorIndex", 0);
//            int i = EditorPrefs.GetInt("LockableInspectorIndex");

//            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
//            Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
//            _mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
//        }

//        if (_mouseOverWindow != null && _mouseOverWindow.GetType().Name == "InspectorWindow")
//        {
//            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
//            PropertyInfo propertyInfo = type.GetProperty("isLocked");
//            bool value = (bool)propertyInfo.GetValue(_mouseOverWindow, null);
//            propertyInfo.SetValue(_mouseOverWindow, !value, null);
//            _mouseOverWindow.Repaint();
//        }
//    }

//    [MenuItem("uGUI/Clear Console #&c")]
//    static void ClearConsole()
//    {
//        Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditorInternal.LogEntries");
//        type.GetMethod("Clear").Invoke(null, null);
//    }
//}
