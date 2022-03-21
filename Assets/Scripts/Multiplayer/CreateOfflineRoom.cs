using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Multiplayer
{
    public class CreateOfflineRoom : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.CreateRoom(null);
            PhotonNetwork.JoinRoom("OfflineRoom");
        }   
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRoom("OfflineRoom");
            print("Connected");
        }
 
        public override void OnJoinedLobby()
        {
            print("On Joined Lobby");
        }
 
        public override void OnJoinedRoom()
        {
            Debug.Log("Player has joined");
        }
 
        public override void OnDisconnected(DisconnectCause cause)
        {
            print("DisconnectFrom Photon");
        }
    }
}