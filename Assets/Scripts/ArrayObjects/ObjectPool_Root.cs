using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[ExecuteInEditMode]
public class ObjectPool_Root : MonoBehaviour
{

    private enum Orientation
    {
        XY,
        YZ,
        XZ
    }

    [HideInInspector]
    GameObject cam;
    [SerializeField]
    Orientation orient = Orientation.YZ;

    [HideInInspector]
    Orientation lastOrientation;

    [SerializeField]
    [HideInInspector]
    Vector3 SizeVector;

    [SerializeField]
    Vector3 Spacing;

    [SerializeField]
    [HideInInspector]
    Vector3[] objectPositions;
    [HideInInspector]
    GameObject[] myObjects;

    Vector3[] ActivePositions;


    bool justStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        justStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myObjects == null)
        {
            myObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                myObjects[i] = transform.GetChild(i).gameObject;
            }
        }
        if (orient != lastOrientation || justStarted)
        {
            justStarted = false;
            lastOrientation = orient;

            ActivePositions = new Vector3[myObjects.Length];
            if (cam == null)
                cam = Camera.main.gameObject;
            if (myObjects == null || objectPositions == null)
                return;

            //orient and set the positions in the list
            OrientGameObjects(ref ActivePositions);
            //move each object into position based on the current orientation of active positions
            for (int i = 0; i < myObjects.Length; i++)
            {
                myObjects[i].transform.localPosition = ActivePositions[i];
                myObjects[i].name = i + " " + IndexToVector(i).ToString();
            }
        }

        var camT = GameObject.Find("CameraTracker").transform;

        Vector3 cameraRelative = camT.InverseTransformPoint(transform.position) * -1;
        cameraRelative += Camera.main.transform.forward;

        //ActivateClosestToOrigin(new Vector3(0, 0, 0));
        ActivateClosestToOrigin(MakeVectorPositive(cameraRelative));

        /*
        for (int i = 0; i < objectPositions.Length - 1; i++)
        {
            Debug.DrawLine(objectPositions[i], objectPositions[i + 1]);
        }
        */

    }

    private Vector3 MakeVectorPositive(Vector3 inVec)
    {
        var newVec = new Vector3(0, 0, 0);
        if (inVec.x > 0)
            newVec.x = inVec.x;
        if (inVec.y > 0)
            newVec.y = inVec.y;
        if (inVec.z > 0)
            newVec.z = inVec.z;
        return newVec;
    }

    private void ActivateClosestToOrigin(Vector3 origin)
    {
        origin = new Vector3(origin.x / Spacing.x, origin.y / Spacing.y, origin.z / Spacing.z);

        int realNum = (int)Mathf.Pow(ActivePositions.Length, .33f);// ActivePositions.Length / 3;

        origin = new Vector3(origin.x - realNum / 2, origin.y - realNum / 2, origin.z);

        if (ActivePositions != null)
        {
            if (ActivePositions.Length > 0)
            {

                //Setup the active positions array so that only the positions nearest the origin are active
                int max_X = (int)((origin.x + realNum));
                int max_Y = (int)((origin.y + realNum));
                int max_Z = (int)((origin.z + realNum));

                int Start_X = (int)Mathf.Max(origin.x, origin.x - realNum);
                int Start_Y = (int)Mathf.Max(origin.y, origin.y - realNum);
                int Start_Z = (int)Mathf.Max(origin.z, origin.z - realNum);

                int end_X = Mathf.Min(max_X, (int)SizeVector.x);
                int end_Y = Mathf.Min(max_Y, (int)SizeVector.y);
                int end_Z = Mathf.Min(max_Z, (int)SizeVector.z);

                int j = 0;
                //Upper Right quadrant
                for (int x = (int)(origin.x); x < end_X; x++)
                {
                    for (int y = (int)(origin.y); y < end_Y; y++)
                    {
                        for (int z = (int)(origin.z); z < end_Z; z++)
                        {
                            if (j < ActivePositions.Length)
                            {
                                try
                                {
                                    ActivePositions[j] = objectPositions[IndexFromXYZ(x, y, z)];
                                    j++;
                                }
                                catch (Exception e)
                                {
                                    if (e is IndexOutOfRangeException)
                                    {
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }

                for (int i = 0; i < ActivePositions.Length; i++)
                {
                    if (i < Math.Pow(realNum, 3))
                    {
                        //myObjects[i].transform.localPosition = new Vector3(0, 0, 0);
                        myObjects[i].transform.localPosition = ActivePositions[i];
                    }
                    else
                    {
                        myObjects[i].transform.localPosition = ActivePositions[0];
                    }
                }
            }
        }
    }

    private int IndexFromXYZ(int x, int y, int z)
    {
        var index = 0f;
        switch (orient)
        {
            case Orientation.XY:
                index = 6;// (SizeVector.y * x) + y;
                break;
            case Orientation.YZ:
                index = (y * SizeVector.z) + (z) + x * ((SizeVector.y * SizeVector.z));
                break;
            case Orientation.XZ:
                index = 0;
                break;
        }
        return (int)index;
    }

    private void GenerateObjectPositions()
    {
        var pos = 0;
        var X_Spacing = Mathf.Abs(transform.GetChild(0).transform.position.x - transform.GetChild(1).transform.position.x);
        var Y_Spacing = Mathf.Abs(transform.GetChild(0).transform.position.y - transform.GetChild(1).transform.position.y);
        var Z_Spacing = Mathf.Abs(transform.GetChild(0).transform.position.z - transform.GetChild(1).transform.position.z);

        for (int row_x = 0; row_x < SizeVector.x; row_x++)
        {
            for (int col_y = 0; col_y < SizeVector.y; col_y++)
            {
                for (int depth_z = 0; depth_z < SizeVector.z; depth_z++)
                {
                    // CreateNewInstance(prefab, row, col, depth);
                    var CurrentTotal = (row_x * (SizeVector.y * SizeVector.z)) + (col_y * SizeVector.z) + (depth_z);

                    objectPositions[pos] = new Vector3(
                        transform.position.x + X_Spacing * row_x,
                       transform.position.y + Y_Spacing * col_y,
                       transform.position.z + Z_Spacing * depth_z);
                    pos++;
                }
            }
        }

    }

    private int ObjectPositionFromVector(Vector3 tmp)
    {
        switch (orient)
        {
            case Orientation.XY:
                return (int)((tmp.x * (SizeVector.y * SizeVector.z)) + (tmp.y * SizeVector.z) + (tmp.z));
            case Orientation.YZ:
                return (int)((tmp.x * (SizeVector.y * SizeVector.z)) + (tmp.y * SizeVector.z) + (tmp.z));
            case Orientation.XZ:
                return (int)((tmp.x * (SizeVector.y * SizeVector.z)) + (tmp.y * SizeVector.z) + (tmp.z));
        }
        return (int)((tmp.x * (SizeVector.y * SizeVector.z)) + (tmp.y * SizeVector.z) + (tmp.z));

    }

    private void OrientGameObjects(ref Vector3[] activePositions)
    {
        for (int i = 0; i < activePositions.Length; i++)
        {
            int offset = 0;

            //Flip to X - Axis
            //Order of cubes is YX

            if (orient == Orientation.XY)
            {
                offset = (i * (int)(SizeVector.z));
                if (i >= SizeVector.x * SizeVector.y)
                {
                    //wrap around when number is too large
                    if (SizeVector.x > 0 && SizeVector.y > 0)
                        offset = (i % (int)(SizeVector.x * SizeVector.y)) * (int)(SizeVector.z) + i / (int)(SizeVector.x * SizeVector.y);
                }
            }

            //Order of cubes is ZX

            if (orient == Orientation.XZ)
            {
                offset = (i % (int)(SizeVector.z)) + (int)(i / SizeVector.z) * (int)(SizeVector.z * SizeVector.y);
                if (offset >= SizeVector.z * SizeVector.x * SizeVector.y)
                {
                    offset = ((i) % (int)(SizeVector.z)) + (int)((i) / SizeVector.z) * (int)(SizeVector.z * SizeVector.y);
                    offset -= (int)(i / (SizeVector.x * SizeVector.z)) * (int)(SizeVector.z * SizeVector.x * SizeVector.y);
                    offset += (int)(SizeVector.z) * (int)(i / (int)(SizeVector.x * SizeVector.z));
                }

            }

            //Order of cubes is ZY
            if (orient == Orientation.YZ)
                offset = i;

            try
            {
                activePositions[i] = objectPositions[offset];
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NullReferenceException)) { }
            }
        }
    }

    //gives a coordinate to a gameobject in postion based on currently chosen orientation
    //And index of position in the list
    private Vector3 IndexToVector(int i)
    {
        float x = -1;
        float y = -1;
        float z = -1;

        switch (orient)
        {

            case Orientation.XY:
                y = i % SizeVector.y;
                x = (i / SizeVector.y) % SizeVector.x;
                z = i / (SizeVector.x * SizeVector.y);  //BAD
                break;
            case Orientation.XZ:
                z = i % SizeVector.z;
                x = (i / SizeVector.z) % SizeVector.x;
                y = i / (SizeVector.x * SizeVector.z);  //BAD
                break;
            case Orientation.YZ:
                z = i % SizeVector.z;
                y = (i / SizeVector.z) % SizeVector.y;
                x = i / (SizeVector.y * SizeVector.z);//BAD
                break;
            default:
                break;
        }
        return new Vector3((int)x, (int)y, (int)z);
    }

    public void SetupObjectPool(Vector3[] positions, int count, Vector3 spacing)
    {
        objectPositions = positions;
        myObjects = new GameObject[count];
        SizeVector = spacing;

        for (int i = 0; i < count; i++)
        {
            myObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    public void SetSpacing(Vector3 vector3)
    {
        Spacing = vector3;
    }
}

