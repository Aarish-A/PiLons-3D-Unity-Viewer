using Assets.Scripts.Application.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomPropertyDrawer(typeof(PictureBlock))]
public class PictureBlockPropDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.EndProperty();
    }
}

[CustomEditor(typeof(Layout))]
public class LayoutEditor : Editor {

	Layout _target;

	Vector2 scrollPos;

	SerializedProperty TargetProp;
	SerializedProperty LayoutTypeProp;
	SerializedProperty TitleProp;
	SerializedProperty ContentProp;
	SerializedProperty PanelProp;
    SerializedProperty OutreachContentProp;
    SerializedProperty HeaderToggleProp;
	SerializedProperty HeaderTextProp;

	void OnEnable() {

		TargetProp = serializedObject.FindProperty("TargetProp");
		LayoutTypeProp = serializedObject.FindProperty("LayoutType");
		TitleProp = serializedObject.FindProperty("Title");
		ContentProp = serializedObject.FindProperty("Content");
		PanelProp = serializedObject.FindProperty("Panels");
        OutreachContentProp = serializedObject.FindProperty("OutreachContent");
		HeaderToggleProp = serializedObject.FindProperty("HeaderToggle");
		HeaderTextProp = serializedObject.FindProperty("HeaderText");

		_target = (Layout)target;
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(LayoutTypeProp);

		switch (_target.LayoutType) {

			case LayoutType.Viewer3D: {

				EditorGUILayout.PropertyField(TitleProp, new GUIContent("Section Title"));
				ShowArray(PanelProp);
				break;
			}

            case LayoutType.CommunityOutreach: {

                EditorGUILayout.PropertyField(TitleProp, new GUIContent("Section Title"));
                ShowArray(OutreachContentProp);
                break;
            }
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void ShowTextBox(SerializedProperty textProp, int height = 350) {

		EditorGUILayout.LabelField(textProp.name);
		//scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MinHeight(height), GUILayout.ExpandHeight(true));
		textProp.stringValue = EditorGUILayout.TextArea(textProp.stringValue, GUILayout.ExpandHeight(true), GUILayout.MinHeight(50), GUILayout.ExpandWidth(true));
		//EditorGUILayout.EndScrollView();
	}

	private void AddButtons(SerializedProperty array, int i) {

		int oldSize = array.arraySize;
		if (GUILayout.Button(new GUIContent("\u21b4", "Move Down"))) array.MoveArrayElement(i, i + 1);
		if (GUILayout.Button(new GUIContent("-", "Delete")))
			if (array.arraySize == oldSize) array.DeleteArrayElementAtIndex(i);
	}

	private void ShowArray(SerializedProperty array) {

		EditorGUILayout.PropertyField(array);

		if (array.isExpanded) {

			for (int i = 0; i < array.arraySize; i++) {

				if (array.arrayElementType == "PictureBlock") ShowPictureBlock(array.GetArrayElementAtIndex(i));
				else if (array.arrayElementType == "PanelBlock") ShowPanel(array.GetArrayElementAtIndex(i));
				else if (array.arrayElementType == "OutreachBlock") ShowOutreachBlock(array.GetArrayElementAtIndex(i));
                else EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i), GUIContent.none);

				EditorGUILayout.BeginHorizontal();
				AddButtons(array, i);
				EditorGUILayout.EndHorizontal();
			}
			if (GUILayout.Button(new GUIContent("+", "Add"))) array.arraySize += 1;
		}
	}

	private void ShowPictureBlock(SerializedProperty prop) {

		EditorGUILayout.BeginVertical();
		EditorGUILayout.PropertyField(prop.FindPropertyRelative("Image"));
		EditorGUILayout.PropertyField(prop.FindPropertyRelative("Video"));
		ShowTextBox(prop.FindPropertyRelative("Text"), 250);
		EditorGUILayout.EndVertical();

	}

    private void ShowOutreachBlock (SerializedProperty prop) {

        EditorGUILayout.BeginVertical();
        ShowArray(prop.FindPropertyRelative("Slides"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("Header"));
        ShowTextBox(prop.FindPropertyRelative("Description"), 250);
        EditorGUILayout.EndVertical();
    }
	
	private void ShowPanel(SerializedProperty prop) {

		EditorGUILayout.BeginVertical();
		EditorGUILayout.PropertyField(prop.FindPropertyRelative("Panel"));
		EditorGUILayout.PropertyField(prop.FindPropertyRelative("Video"));
		EditorGUILayout.EndVertical();
	}
}