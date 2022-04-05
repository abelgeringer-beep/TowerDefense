using Photon.Pun;

namespace Multiplayer
{
    public class CreateOfflineRoom : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("OfflineRoom");
        }
    }
}