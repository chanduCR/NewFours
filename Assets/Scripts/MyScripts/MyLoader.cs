using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyProject
{
    public class MyLoader : MonoBehaviour
    {
        string ip;
        int port;
        bool host;
        //server settings//
        string serverID = "myServer";
        const int maxPlayers = 2;

        public void LoadNextScene(string ip, int port, bool host)
        {
            this.ip = ip;
            this.port = port;
            this.host = host;
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.LoadScene(1);
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ip;
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = port;
            if (host)
            {
                //NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
                NetworkManager.Singleton.ConnectionApprovalCallback += ClientCheck;
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                NetworkManager.Singleton.NetworkConfig.ConnectionData = GetBytes(serverID);
                NetworkManager.Singleton.StartClient();
            }
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        private void ClientCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            //bool createPlayerObject = true;
            //Debug.Log(NetworkManager.Singleton.ConnectedClientsIds.Count);
            bool approve= NetworkManager.Singleton.ConnectedClientsIds.Count < maxPlayers;
            bool createPlayerObject = approve;
            //callback(createPlayerObject, prefabHash, approve, positionToSpawnAt, rotationToSpawnWith);
            callback(createPlayerObject, null, approve, null, null);
        }

        private void ClientConnected(ulong clientId)
        {
            //Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
        }

        private byte[] GetBytes(string text)
        {
            return System.Text.Encoding.ASCII.GetBytes(text);
        }

        public int TotalPlayers()
        {
            return maxPlayers;
        }
    }
}