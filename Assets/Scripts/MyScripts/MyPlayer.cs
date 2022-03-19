using Unity.Netcode;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class MyPlayer : NetworkBehaviour
    {

        private NetworkVariable<int> Position = new NetworkVariable<int>();
        //NetworkVariable<bool> CurrentTurn = new NetworkVariable<bool>();
        MyGame gameHandler;


        private void Start()
        {
            Position.OnValueChanged += ValueChanged;
            //Debug.Log(IsOwner + " owner and local player " + IsLocalPlayer);
        }

        void ValueChanged(int oldValue, int newValue)
        {
            gameHandler.passTurn(this, newValue);
        }

        [ServerRpc]
        void ChangeServerRpc(int value, ServerRpcParams rpcParams = default)
        {
            Position.Value = value;
        }

        void Update()
        {
        }

        public void ChangePosition(int value)
        {
            //
            if(IsHost)
            {
                Position.Value = value;
            }
            else
            {
                ChangeServerRpc(value);
            }
        }

        public void registerHandler(MyGame game)
        {
            gameHandler = game;
        }
    }
}