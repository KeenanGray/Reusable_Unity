using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomEditorGUI
{
	public static class E_CustomEditorUtilities
	{
		public static void AuthenticateField(bool boolean, string msg)
		{
			if (boolean)
				return;

			GUIStyle s = new GUIStyle(EditorStyles.label);
			s.normal.textColor = Color.red;

			EditorGUILayout.LabelField(msg, s);
		}

	}
}