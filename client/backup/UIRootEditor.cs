//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIRoot))]
public class UIRootEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		SerializedProperty sp = NGUIEditorTools.DrawProperty("Scaling Style", serializedObject, "scalingStyle");

		UIRoot.Scaling scaling = (UIRoot.Scaling)sp.intValue;

		if (scaling != UIRoot.Scaling.PixelPerfect && scaling != UIRoot.Scaling.Custom)
		{
			NGUIEditorTools.DrawProperty("Manual Height", serializedObject, "manualHeight");
		}

		if (scaling != UIRoot.Scaling.FixedSize && scaling != UIRoot.Scaling.Custom)
		{
			NGUIEditorTools.DrawProperty("Minimum Height", serializedObject, "minimumHeight");
			NGUIEditorTools.DrawProperty("Maximum Height", serializedObject, "maximumHeight");
		}
		if (scaling == UIRoot.Scaling.Custom)
		{
			NGUIEditorTools.DrawProperty("Base Width", serializedObject, "baseWidth");
			NGUIEditorTools.DrawProperty("Base Height", serializedObject, "baseHeight");
			NGUIEditorTools.DrawProperty("Mode", serializedObject, "mode");
		}
		NGUIEditorTools.DrawProperty("Shrink Portrait UI", serializedObject, "shrinkPortraitUI");
		NGUIEditorTools.DrawProperty("Adjust by DPI", serializedObject, "adjustByDPI");

		serializedObject.ApplyModifiedProperties();
	}
}
