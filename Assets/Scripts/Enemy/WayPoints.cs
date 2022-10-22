using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class WayPoints : MonoBehaviour
    {
        public static Transform[] Points;
        private Transform[] sorted;
        public static Transform start, end;

        public void Awake()
        {
            if (transform.childCount <= 1)
            {
                Debug.Log("no waypoints yet");
                return;
            }
            Points = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                Points[i] = transform.GetChild(i);
            }

            SetPoints(true);
        }

        public void SetPoints(bool sort = false)
        {
            if (sort)
            {
                sorted = new Transform[Points.Length];
                sorted[0] = start;
                sorted[sorted.Length-1] = end;
                for (int i = 1; i < Points.Length ; ++i)
                {
                    sorted[i] = FindClosest(sorted[i - 1]);
                }
                for (int i = 0; i < sorted.Length; ++i)
                {
                    Points[i] = sorted[i];
                }
                return;
            }

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = transform.GetChild(i);
            }
        }

        private Transform FindClosest(Transform closestTo)
        {
            float oldDistance = 9999999f;
            float dist;
            Transform closestTransform = Points[Points.Length - 1];
            for (int i = 0; i < Points.Length; ++i)
            {
                dist = Vector3.Distance(closestTo.transform.position, Points[i].transform.position);
                if (dist < oldDistance && !contains(Points[i]))
                {
                    closestTransform = Points[i];
                    oldDistance = dist;
                }
            }

            return closestTransform;
        }

        private bool contains(Transform point)
        {
            for(int i = 0; i < sorted.Length; ++i)
                if(sorted[i] == point)
                    return true;

            return false;
        }
    }
}