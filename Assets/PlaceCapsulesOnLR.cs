using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaceCapsulesOnLR : MonoBehaviour
{

    public GameObject CapsuleContainer;
    LineRenderer lr;
    Transform current;
    public float SCALE;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        var capsules = CapsuleContainer.transform.childCount;
        //delete capsules if we have extras
        if (capsules > lr.positionCount)
        {
            for (int i = capsules - lr.positionCount; i > 0; i--)
            {
                DestroyImmediate(CapsuleContainer.transform.GetChild(0).gameObject);
            }
        }
        //add capsules if we need more
        if (lr.positionCount > capsules)
        {
            for (int i = lr.positionCount - capsules; i > 0; i--)
            {
                Instantiate(CapsuleContainer.transform.GetChild(0),CapsuleContainer.transform);
            }
        }

        for (int i = 0; i < lr.positionCount; i++)
        {
            current = CapsuleContainer.transform.GetChild(i).transform;

            current.position = lr.GetPosition(i);

            if (i < lr.positionCount - 1)
            {

                current.LookAt(lr.GetPosition(i + 1));
                current.RotateAround(current.position, current.transform.right, 90);
                current.RotateAround(current.position, current.transform.up, 180);
            }
            else
            {

                current.LookAt(lr.GetPosition(i - 1));
                current.RotateAround(current.position, current.transform.right, 90);
                current.RotateAround(current.position, current.transform.up, 180);


            }
            var scale = new Vector3(1, SCALE, 1);// new Vector3(1, Mathf.Abs(current.position.y - lr.GetPosition(i - 1).y), 1);
            current.localScale = scale;
        }
    }
}
