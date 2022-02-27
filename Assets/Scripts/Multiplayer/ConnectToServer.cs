using Photon.Pun;
using UnityEngine;

namespace Multiplayer
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        public GameObject lobbyTab;
        public GameObject multiplayerOptionsTab;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            multiplayerOptionsTab.SetActive(false);
            lobbyTab.SetActive(true);
        }
    }
}
