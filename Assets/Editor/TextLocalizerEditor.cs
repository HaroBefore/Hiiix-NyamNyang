using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextLocalizer))]
public class TextLocalizerEditor : OdinEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();
        TextLocalizer localizer = serializedObject.targetObject as TextLocalizer;
        if (localizer == null) return; 
        localizer.ReloadStrText();
    }
}
