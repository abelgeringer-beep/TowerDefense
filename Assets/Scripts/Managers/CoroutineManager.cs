using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager Instance;

        void Start()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void DestroyGameObject(GameObject go, float timer = 0)
        {
            if(go.GetPhotonView().IsMine)
                StartCoroutine(Destroy(go, timer));
        }

        private IEnumerator Destroy(GameObject go, float timer = 0)
        {
            yield return new WaitForSeconds(timer);
            PhotonNetwork.Destroy(go);
        }
    }
}