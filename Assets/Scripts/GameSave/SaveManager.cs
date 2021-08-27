using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameSave
{

	public class SaveManager
	{
		static string SavePath;

		public static void SetSavePath(string path)
		{
			SavePath = Application.persistentDataPath + path;
		}
		public static string GetSavePath()
		{
			return SavePath;
		}

		public static void SaveKey(string key, string value)
		{

		}

		public static void ReadSaveFile()
		{
			StreamReader reader = new StreamReader(SavePath);
			Debug.Log(reader.ReadToEnd());
			reader.Close();


		}

		public static void CreateSaveFile()
		{
			StreamWriter writer = new StreamWriter(SavePath, true);
			Debug.Log("Savepath " + SavePath);
			writer.WriteLine("NameOfFile");
			writer.Close();

#if UNITY_EDITOR_OSX
            EditorUtility.RevealInFinder(SavePath);
#endif
#if UNITY_EDITOR_WIN
            EditorUtility.RevealInFinder(SavePath);
#endif
		}
	}
}
