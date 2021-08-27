//C# Example
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CustomEditorGUI;
using System;

namespace GameSave
{
	public class Save_Editor : EditorWindow
	{
		public GameObject camera;

		string PreferenceName = "";
		string PreferenceValue = "";
		string SaveDir = "";

		[MenuItem("Tools/Save Editor")]
		public static void ShowWindow()
		{
			//Show existing window instance. If one doesn't exist, make one.
			Save_Editor window = ScriptableObject.CreateInstance(typeof(Save_Editor)) as Save_Editor;
			window.ShowUtility();
		}

		void OnGUI()
		{
			// GUILayoutOption[] options = { GUILayout.MaxWidth(0), GUILayout.MinWidth(10.0f) };

			GUILayout.Label("Edit Player Preferences", EditorStyles.boldLabel);

			GUILayout.Label("Save path:", EditorStyles.boldLabel);
			GUILayout.Label(SaveManager.GetSavePath(), EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			var width = 100;
			EditorGUIUtility.labelWidth = width; //sets size of input field
			SaveDir = EditorGUILayout.TextField("Save Dir", SaveDir, GUILayout.MinWidth(width / 2));
			width = 40;
			if (GUILayout.Button("Set", GUILayout.Width(width)))
				SetSaveDirectory(SaveDir);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			width = 40;
			EditorGUIUtility.labelWidth = width; //sets size of input field
			PreferenceName = EditorGUILayout.TextField("Name", PreferenceName, GUILayout.MinWidth(width / 2));
			EditorGUIUtility.labelWidth = width; //sets size of input field
			PreferenceValue = EditorGUILayout.TextField("Value", PreferenceValue, GUILayout.MinWidth(width / 2));
			if (GUILayout.Button("Set", GUILayout.Width(width)))
				SaveKey(PreferenceName, PreferenceValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			LoadSaveData();
			EditorGUILayout.EndHorizontal();
		}

		private void LoadSaveData()
		{
			try
			{
				SaveManager.ReadSaveFile();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				EditorGUILayout.HelpBox("No Save File at this location", MessageType.Error);
				var width = 150;
				if (GUILayout.Button("Create New Save File", GUILayout.Width(width)))
					SaveManager.CreateSaveFile();
			}
		}

		private void SetSaveDirectory(string path)
		{
			SaveManager.SetSavePath(path);
		}

		private void SaveKey(string key, string value)
		{
			SaveManager.SaveKey(key, value);
		}



	}
}