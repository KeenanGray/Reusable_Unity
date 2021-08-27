//C# Example
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CustomEditorGUI;

public class MoveCameraWindow : EditorWindow
{
    public GameObject camera;

    Vector3 initPos;
    Vector3 initRot;


    float X_Move = 2;
    //    float Y_Move = 2;
    float Z_Move = 2;

    float Turn_Amt;

    Vector3 currentPos;
    Vector3 currentRot;

    [MenuItem("Tools/MovePlayerCamera")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MoveCameraWindow));


    }

    void OnGUI()
    {
        GUILayout.Label("Move Camera", EditorStyles.boldLabel);
        camera = (GameObject)EditorGUILayout.ObjectField("Camera:", camera, typeof(Object), true);

        if (camera != null && initPos == new Vector3(0, 0, 0))
        {
            Debug.Log("initial transform is " + initPos + ", " + initRot);
            initPos = camera.transform.position;
            initRot = camera.transform.eulerAngles;

        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Up"))
            OnUpButtonPressed();

        if (GUILayout.Button("Down"))
            OnDownButtonPressed();

        Z_Move = EditorGUILayout.FloatField("Z Move", Z_Move);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Left"))
            OnLeftButtonPressed();

        if (GUILayout.Button("Right"))
            OnRightButtonPressed();

        X_Move = EditorGUILayout.FloatField("X Move", X_Move);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Turn_Left"))
            OnTurnLeftButtonPressed();

        if (GUILayout.Button("Turn_Right"))
            OnTurnRightButtonPressed();

        Turn_Amt = EditorGUILayout.FloatField("Turn_Amt", Turn_Amt);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Initial  Position"))
            OnSetButtonPressed();
        if (GUILayout.Button("Reset Initial  Position"))
            OnResetButtonPressed();
        EditorGUILayout.EndHorizontal();

        //if (GUILayout.Button("Vert"))
        //    OnUpButtonPressed();
        //Y_Move = EditorGUILayout.IntField("Y Move", Y_Move);

        currentPos = camera.transform.localPosition;
        currentRot = camera.transform.eulerAngles;

    }

    void OnUpButtonPressed()
    {
        camera.transform.Translate(Vector3.forward * Z_Move, Camera.main.transform);
    }

    void OnDownButtonPressed()
    {
        camera.transform.Translate(Vector3.forward * -Z_Move, Camera.main.transform);
    }

    void OnRightButtonPressed()
    {
        camera.transform.Translate(Vector3.right * X_Move, Camera.main.transform);
    }

    void OnLeftButtonPressed()
    {
        camera.transform.Translate(Vector3.right * -X_Move, Camera.main.transform);
    }

    void OnTurnLeftButtonPressed()
    {
        camera.transform.eulerAngles = new Vector3(currentRot.x, currentRot.y - Turn_Amt, currentRot.z);
    }

    void OnTurnRightButtonPressed()
    {
        camera.transform.eulerAngles = new Vector3(currentRot.x, currentRot.y + Turn_Amt, currentRot.z);
    }

    void OnSetButtonPressed()
    {
        initPos = camera.transform.position;
        initRot = camera.transform.eulerAngles;
    }
    void OnResetButtonPressed()
    {
        Debug.Log("pressed reset button");
        camera.transform.position = initPos;
        camera.transform.eulerAngles = initRot;

        initPos = new Vector3(0, 0, 0);
    }
}
