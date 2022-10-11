using UnityEngine;

namespace Enemy
{
    public class WayPoints : MonoBehaviour
    {
        public static Transform[] Points;

        public void Awake()
        {
            if(Points == null)
                SetPoints();
        }

        public void SetPoints()
        {
            Points = new Transform[transform.childCount];
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = transform.GetChild(i);
            }
        }
    }
}