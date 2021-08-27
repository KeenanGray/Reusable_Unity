using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSpline : MonoBehaviour
{
    public LineRenderer PathToFollow;
    public float TotalTime = 1;
    int startingIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveAlongCurve());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToStart()
    {
        transform.position = PathToFollow.GetPosition(0);
        startingIndex = 0;
    }

    public void GoToPoint(int index)
    {
        transform.position = PathToFollow.GetPosition(index);
        startingIndex = index;
    }

    public void GoToEnd()
    {
        transform.position = PathToFollow.GetPosition(PathToFollow.positionCount - 1);
    }

    public void GoToNearest()
    {
        var nearest = PathToFollow.GetPosition(0);
        for (int i = 0; i < PathToFollow.positionCount; i++)
        {
            if (Vector3.Distance(transform.position, PathToFollow.GetPosition(i)) < Vector3.Distance(transform.position, nearest))
            {
                nearest = PathToFollow.GetPosition(i);
                startingIndex = i;
            }
        }
        transform.position = nearest;
    }

    public IEnumerator MoveAlongCurve()
    {
        if (TotalTime <= 0)
            TotalTime = .01f;

        float time = TotalTime;

        GoToNearest();
        // time = time / (PathToFollow.positionCount);

        for (int i = startingIndex; i < PathToFollow.positionCount; i++)
        {
            float elapsedTime = 0;

            var initialPos = transform.position;

            while (elapsedTime < time)
            {
                float t = (elapsedTime / time);
                transform.position = Vector3.Lerp(initialPos, PathToFollow.GetPosition(i), (float)t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        GoToStart();
        StartCoroutine(MoveAlongCurve());
        yield break;
    }

}
