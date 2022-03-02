using System;
using Photon.Pun;
using UnityEngine.UI;

namespace Multiplayer
{
    public class CreateAndJoinRoms : MonoBehaviourPunCallbacks
    {
        public InputField createInput;
        public InputField joinInput;

        private void Start()
        {
            //PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Level01");
        }
    }
}
