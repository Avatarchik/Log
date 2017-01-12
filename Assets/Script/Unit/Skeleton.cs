using UnityEngine;
using System.Collections;

public class Skeleton : Ship
{
    public void WaitingForLaunch(Vector3 _moveSideDir)
    {
        kState = State.None;
        StartCoroutine(MoveSide(_moveSideDir, Random.Range(1.5f,  2.5f)));
    }

    IEnumerator MoveSide(Vector3 _move, float _sec)
    {
        kState = State.Move;
        yield break;
        /*
        float curTime = 0.0f;
        while(true)
        {
            curTime += Time.deltaTime;
            transform.position += _move * Time.deltaTime * 5.0f;

            if (curTime < _sec)
                yield return null;
            else
            {
                mStartingPos = transform.position;
                kState = State.Move;
                yield break;
            }
        }*/
    }
}
