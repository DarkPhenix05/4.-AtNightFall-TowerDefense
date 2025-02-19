using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static Transform[] _wayPoints;

    public void GetWayPoints()
    {
        _wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            _wayPoints[i] = transform.GetChild(i);
        }
    }
}
