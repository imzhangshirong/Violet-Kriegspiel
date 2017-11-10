//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------
//by zhangshirong
#if !UNITY_FLASH
#define DYNAMIC_FONT
#endif

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UIRichTextLabels.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UIRichTextLabel), true)]
public class UIRichTextLabelInspector : UIWidgetInspector
{
	public enum FontType
	{
		NGUI,
		Unity,
	}

	UIRichTextLabel mRichTextLabel;
	FontType mFontType;

    string mPrefabSequence = "";
    string mEmotionSequence = "";
    string mSpriteName = "";
    UIAtlas mEmotionAtlas;
    GameObject mPrefabGameObject;
    float mPrefabWidth;
    float mPrefabHeight;


    protected override void OnEnable ()
	{
		base.OnEnable();
		SerializedProperty bit = serializedObject.FindProperty("mFont");
		mFontType = (bit != null && bit.objectReferenceValue != null) ? FontType.NGUI : FontType.Unity;
	}

    void SelectSprite(string spriteName)
    {
        serializedObject.Update();
        mSpriteName = spriteName;
        NGUITools.SetDirty(serializedObject.targetObject);
        NGUISettings.selectedSprite = spriteName;
    }

    void OnSelectAtlas(Object obj)
    {
        serializedObject.Update();
        SerializedProperty sp = serializedObject.FindProperty("mEmotionAtlas");
        sp.objectReferenceValue = obj;
        serializedObject.ApplyModifiedProperties();
        NGUITools.SetDirty(serializedObject.targetObject);
        NGUISettings.atlas = obj as UIAtlas;
        mEmotionAtlas = NGUISettings.atlas;
    }



    void OnNGUIFont (Object obj)
	{
		serializedObject.Update();
		
		SerializedProperty sp = serializedObject.FindProperty("mFont");
		sp.objectReferenceValue = obj;

		sp = serializedObject.FindProperty("mTrueTypeFont");
		sp.objectReferenceValue = null;
		
		serializedObject.ApplyModifiedProperties();
		NGUISettings.ambigiousFont = obj;
	}

	void OnUnityFont (Object obj)
	{
		serializedObject.Update();
		
		SerializedProperty sp = serializedObject.FindProperty("mTrueTypeFont");
		sp.objectReferenceValue = obj;

		sp = serializedObject.FindProperty("mFont");
		sp.objectReferenceValue = null;

		serializedObject.ApplyModifiedProperties();
		NGUISettings.ambigiousFont = obj;
	}

	/// <summary>
	/// Draw the label's properties.
	/// </summary>

	protected override bool ShouldDrawProperties ()
	{
		mRichTextLabel = mWidget as UIRichTextLabel;

		GUILayout.BeginHorizontal();

#if DYNAMIC_FONT
		mFontType = (FontType)EditorGUILayout.EnumPopup(mFontType, "DropDown", GUILayout.Width(74f));
		if (NGUIEditorTools.DrawPrefixButton("Font", GUILayout.Width(64f)))
#else
		mFontType = FontType.NGUI;
		if (NGUIEditorTools.DrawPrefixButton("Font", GUILayout.Width(74f)))
#endif
		{
			if (mFontType == FontType.NGUI)
			{
				ComponentSelector.Show<UIFont>(OnNGUIFont);
			}
			else
			{
				ComponentSelector.Show<Font>(OnUnityFont, new string[] { ".ttf", ".otf" });
			}
		}

		bool isValid = false;
		SerializedProperty fnt = null;
		SerializedProperty ttf = null;

		if (mFontType == FontType.NGUI)
		{
			GUI.changed = false;
			fnt = NGUIEditorTools.DrawProperty("", serializedObject, "mFont", GUILayout.MinWidth(40f));

			if (fnt.objectReferenceValue != null)
			{
				if (GUI.changed) serializedObject.FindProperty("mTrueTypeFont").objectReferenceValue = null;
				NGUISettings.ambigiousFont = fnt.objectReferenceValue;
				isValid = true;
			}
		}
		else
		{
			GUI.changed = false;
			ttf = NGUIEditorTools.DrawProperty("", serializedObject, "mTrueTypeFont", GUILayout.MinWidth(40f));

			if (ttf.objectReferenceValue != null)
			{
				if (GUI.changed) serializedObject.FindProperty("mFont").objectReferenceValue = null;
				NGUISettings.ambigiousFont = ttf.objectReferenceValue;
				isValid = true;
			}
		}

		GUILayout.EndHorizontal();

		if (mFontType == FontType.Unity)
		{
			EditorGUILayout.HelpBox("Dynamic fonts suffer from issues in Unity itself where your characters may disappear, get garbled, or just not show at times. Use this feature at your own risk.\n\n" +
				"When you do run into such issues, please submit a Bug Report to Unity via Help -> Report a Bug (as this is will be a Unity bug, not an NGUI one).", MessageType.Warning);
		}

		NGUIEditorTools.DrawProperty("Material", serializedObject, "mMat");

		EditorGUI.BeginDisabledGroup(!isValid);
		{
			UIFont uiFont = (fnt != null) ? fnt.objectReferenceValue as UIFont : null;
			Font dynFont = (ttf != null) ? ttf.objectReferenceValue as Font : null;

			if (uiFont != null && uiFont.isDynamic)
			{
				dynFont = uiFont.dynamicFont;
				uiFont = null;
			}

			if (dynFont != null)
			{
				GUILayout.BeginHorizontal();
				{
					EditorGUI.BeginDisabledGroup((ttf != null) ? ttf.hasMultipleDifferentValues : fnt.hasMultipleDifferentValues);
					
					SerializedProperty prop = NGUIEditorTools.DrawProperty("Font Size", serializedObject, "mFontSize", GUILayout.Width(142f));
					NGUISettings.fontSize = prop.intValue;
					
					prop = NGUIEditorTools.DrawProperty("", serializedObject, "mFontStyle", GUILayout.MinWidth(40f));
					NGUISettings.fontStyle = (FontStyle)prop.intValue;
					
					NGUIEditorTools.DrawPadding();
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndHorizontal();
			}
			else if (uiFont != null)
			{
				GUILayout.BeginHorizontal();
				SerializedProperty prop = NGUIEditorTools.DrawProperty("Font Size", serializedObject, "mFontSize", GUILayout.Width(142f));

				EditorGUI.BeginDisabledGroup(true);
				if (!serializedObject.isEditingMultipleObjects)
				{
					if (mRichTextLabel.overflowMethod == UIRichTextLabel.Overflow.ShrinkContent)
						GUILayout.Label(" Actual: " + mRichTextLabel.finalFontSize + "/" + mRichTextLabel.defaultFontSize);
					else GUILayout.Label(" Default: " + mRichTextLabel.defaultFontSize);
				}
				EditorGUI.EndDisabledGroup();

				NGUISettings.fontSize = prop.intValue;
				GUILayout.EndHorizontal();
			}

			bool ww = GUI.skin.textField.wordWrap;
			GUI.skin.textField.wordWrap = true;
			SerializedProperty sp = serializedObject.FindProperty("mText");

			if (sp.hasMultipleDifferentValues)
			{
				NGUIEditorTools.DrawProperty("", sp, GUILayout.Height(128f));
			}
			else
			{
				GUIStyle style = new GUIStyle(EditorStyles.textField);
				style.wordWrap = true;

				float height = style.CalcHeight(new GUIContent(sp.stringValue), Screen.width - 100f);
				bool offset = true;

				if (height > 90f)
				{
					offset = false;
					height = style.CalcHeight(new GUIContent(sp.stringValue), Screen.width - 20f);
				}
				else
				{
					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical(GUILayout.Width(76f));
					GUILayout.Space(3f);
					GUILayout.Label("Text");
					GUILayout.EndVertical();
					GUILayout.BeginVertical();
				}
				Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(height));

				GUI.changed = false;
				string text = EditorGUI.TextArea(rect, sp.stringValue, style);
				if (GUI.changed) sp.stringValue = text;

				if (offset)
				{
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}

			GUI.skin.textField.wordWrap = ww;

			NGUIEditorTools.DrawPaddedProperty("Modifier", serializedObject, "mModifier");

			SerializedProperty ov = NGUIEditorTools.DrawPaddedProperty("Overflow", serializedObject, "mOverflow");
			NGUISettings.overflowStyle = (UILabel.Overflow)ov.intValue;
			if (NGUISettings.overflowStyle == UILabel.Overflow.ClampContent)
				NGUIEditorTools.DrawProperty("Use Ellipsis", serializedObject, "mOverflowEllipsis", GUILayout.Width(110f));

			if (NGUISettings.overflowStyle == UILabel.Overflow.ResizeFreely)
			{
				GUILayout.BeginHorizontal();
				SerializedProperty s = NGUIEditorTools.DrawPaddedProperty("Max Width", serializedObject, "mOverflowWidth");
				if (s != null && s.intValue < 1) GUILayout.Label("unlimited");
				GUILayout.EndHorizontal();
			}

			NGUIEditorTools.DrawPaddedProperty("Alignment", serializedObject, "mAlignment");

			if (dynFont != null)
				NGUIEditorTools.DrawPaddedProperty("Keep crisp", serializedObject, "keepCrispWhenShrunk");

			EditorGUI.BeginDisabledGroup(mRichTextLabel.bitmapFont != null && mRichTextLabel.bitmapFont.packedFontShader);
			GUILayout.BeginHorizontal();
			SerializedProperty gr = NGUIEditorTools.DrawProperty("Gradient", serializedObject, "mApplyGradient",
			GUILayout.Width(95f));

			EditorGUI.BeginDisabledGroup(!gr.hasMultipleDifferentValues && !gr.boolValue);
			{
				NGUIEditorTools.SetLabelWidth(30f);
				NGUIEditorTools.DrawProperty("Top", serializedObject, "mGradientTop", GUILayout.MinWidth(40f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				NGUIEditorTools.SetLabelWidth(50f);
				GUILayout.Space(79f);

				NGUIEditorTools.DrawProperty("Bottom", serializedObject, "mGradientBottom", GUILayout.MinWidth(40f));
				NGUIEditorTools.SetLabelWidth(80f);
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Effect", GUILayout.Width(76f));
			sp = NGUIEditorTools.DrawProperty("", serializedObject, "mEffectStyle", GUILayout.MinWidth(16f));

			EditorGUI.BeginDisabledGroup(!sp.hasMultipleDifferentValues && !sp.boolValue);
			{
				NGUIEditorTools.DrawProperty("", serializedObject, "mEffectColor", GUILayout.MinWidth(10f));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(" ", GUILayout.Width(56f));
					NGUIEditorTools.SetLabelWidth(20f);
					NGUIEditorTools.DrawProperty("X", serializedObject, "mEffectDistance.x", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawProperty("Y", serializedObject, "mEffectDistance.y", GUILayout.MinWidth(40f));
					NGUIEditorTools.DrawPadding();
					NGUIEditorTools.SetLabelWidth(80f);
				}
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();

			sp = NGUIEditorTools.DrawProperty("Float spacing", serializedObject, "mUseFloatSpacing", GUILayout.Width(100f));

			if (!sp.boolValue)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("Spacing", GUILayout.Width(56f));
				NGUIEditorTools.SetLabelWidth(20f);
				NGUIEditorTools.DrawProperty("X", serializedObject, "mSpacingX", GUILayout.MinWidth(40f));
				NGUIEditorTools.DrawProperty("Y", serializedObject, "mSpacingY", GUILayout.MinWidth(40f));
				NGUIEditorTools.DrawPadding();
				NGUIEditorTools.SetLabelWidth(80f);
				GUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("Spacing", GUILayout.Width(56f));
				NGUIEditorTools.SetLabelWidth(20f);
				NGUIEditorTools.DrawProperty("X", serializedObject, "mFloatSpacingX", GUILayout.MinWidth(40f));
				NGUIEditorTools.DrawProperty("Y", serializedObject, "mFloatSpacingY", GUILayout.MinWidth(40f));
				NGUIEditorTools.DrawPadding();
				NGUIEditorTools.SetLabelWidth(80f);
				GUILayout.EndHorizontal();
			}
			
			NGUIEditorTools.DrawProperty("Max Lines", serializedObject, "mMaxLineCount", GUILayout.Width(110f));

			GUILayout.BeginHorizontal();
			sp = NGUIEditorTools.DrawProperty("BBCode", serializedObject, "mEncoding", GUILayout.Width(100f));
			EditorGUI.BeginDisabledGroup(!sp.boolValue || mRichTextLabel.bitmapFont == null || !mRichTextLabel.bitmapFont.hasSymbols);
			NGUIEditorTools.SetLabelWidth(60f);
			NGUIEditorTools.DrawPaddedProperty("Symbols", serializedObject, "mSymbols");
			NGUIEditorTools.SetLabelWidth(80f);
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
		}
		EditorGUI.EndDisabledGroup();

        if (NGUIEditorTools.DrawHeader("RichTextPrefabs"))
        {
            NGUIEditorTools.BeginContents();

            List<RichTextPrefabItem> prefabs = mRichTextLabel.Prefabs;
            for (int i = prefabs.Count - 1; i >= 0; i--)
            {
                RichTextPrefabItem prefab = prefabs[i];
                if (prefab.prefabObject == null) prefabs.RemoveAt(i);
            }
            for (int i = 0; i < prefabs.Count; i++)
            {
                RichTextPrefabItem prefab = prefabs[i];
                if (prefab.prefabObject == null) continue;
                GUILayout.BeginHorizontal();
                GUILayout.Label(prefab.sequence, GUILayout.Width(100f));
                GUILayout.Label(prefab.prefabObject.name, GUILayout.Width(100f));
                GUILayout.Label("width:" + prefab.width);
                GUILayout.Label("height:" + prefab.height);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(22f)))
                {
                    NGUIEditorTools.RegisterUndo("Remove prefab", mRichTextLabel);
                    mPrefabSequence = prefab.sequence;
                    //mPrefabGameObject = prefab.prefab;
                    prefabs.Remove(prefab);
                    mRichTextLabel.MarkAsChanged();
                }
                GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();
                GUILayout.Space(4f);
            }

            if (prefabs.Count > 0)
            {
                GUILayout.Space(6f);
            }

            GUILayout.BeginHorizontal();
            mPrefabSequence = EditorGUILayout.TextField(mPrefabSequence, GUILayout.Width(100f));
            mPrefabGameObject = (GameObject)EditorGUILayout.ObjectField(mPrefabGameObject, typeof(GameObject), GUILayout.Width(100f));
            mPrefabWidth = EditorGUILayout.FloatField(mPrefabWidth);
            mPrefabHeight = EditorGUILayout.FloatField(mPrefabHeight);
            isValid = (!string.IsNullOrEmpty(mPrefabSequence)) && (mPrefabGameObject != null);
            GUI.backgroundColor = isValid ? Color.green : Color.grey;

            if (GUILayout.Button("Add", GUILayout.Width(40f)) && isValid)
            {
                NGUIEditorTools.RegisterUndo("Add prefab", mRichTextLabel);
                RichTextPrefabItem tprefab = new RichTextPrefabItem();
                tprefab.sequence = mPrefabSequence;
                tprefab.prefabObject = mPrefabGameObject;
                tprefab.width = mPrefabWidth;
                tprefab.height = mPrefabHeight;
                mRichTextLabel.Prefabs.Add(tprefab);
                mRichTextLabel.MarkAsChanged();
                mPrefabSequence = "";
                mPrefabGameObject = null;
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            if (prefabs.Count == 0)
            {
                EditorGUILayout.HelpBox("In the field above type ':)', choose a Prefab, then hit the Add button.", MessageType.Info);
            }
            else GUILayout.Space(4f);

            NGUIEditorTools.EndContents();
        }

        if (NGUIEditorTools.DrawHeader("RichTextEmotion"))
        {
            NGUIEditorTools.BeginContents();
            GUILayout.BeginHorizontal();
            if (NGUIEditorTools.DrawPrefixButton("Atlas"))
                ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
            SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "mEmotionAtlas", GUILayout.MinWidth(20f));
            GUILayout.EndHorizontal();

            if (mRichTextLabel.EmotionAtlas != null)
            {
                List<RichTextEmotionItem> emotions = mRichTextLabel.Emotions;
                for (int i = 0; i < emotions.Count; i++)
                {
                    RichTextEmotionItem emotion = emotions[i];

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(emotion.sequence, GUILayout.Width(40f));
                    GUILayout.Label(emotion.spriteName);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(22f)))
                    {
                        NGUIEditorTools.RegisterUndo("Remove emotion", mRichTextLabel);
                        mEmotionSequence = emotion.sequence;
                        emotions.Remove(emotion);
                        mRichTextLabel.MarkAsChanged();
                    }
                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4f);
                }

                if (emotions.Count > 0)
                {
                    GUILayout.Space(6f);
                }

                GUILayout.BeginHorizontal();
                mEmotionSequence = EditorGUILayout.TextField(mEmotionSequence, GUILayout.Width(40f));
                NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, mSpriteName, SelectSprite, false);
                isValid = (!string.IsNullOrEmpty(mEmotionSequence)) && (!string.IsNullOrEmpty(mSpriteName));
                GUI.backgroundColor = isValid ? Color.green : Color.grey;

                if (GUILayout.Button("Add", GUILayout.Width(40f)) && isValid)
                {
                    NGUIEditorTools.RegisterUndo("Add Sprite", mRichTextLabel);
                    RichTextEmotionItem temotion = new RichTextEmotionItem();
                    temotion.sequence = mEmotionSequence;
                    temotion.spriteName = mSpriteName;
                    mRichTextLabel.Emotions.Add(temotion);
                    mRichTextLabel.MarkAsChanged();
                    mEmotionSequence = "";
                    mSpriteName = "";
                }
                GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();

                if (emotions.Count == 0)
                {
                    EditorGUILayout.HelpBox("In the field above type ':)', choose a Sprite, then hit the Add button.", MessageType.Info);
                }
                else GUILayout.Space(4f);
            }




            NGUIEditorTools.EndContents();
        }

        isValid = true;


        return isValid;
	}
}
