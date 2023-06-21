using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


public class PlayerNetwork : NetworkBehaviour
{
    public NetworkManagerUI networkManagerUI;
    [SerializeField] private Transform spawnedObjectPrefab;
    private void Awake()
    {
        networkManagerUI = GameObject.FindGameObjectWithTag("NetworkManagerUI").GetComponent<NetworkManagerUI>();
    }
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes _message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _message);
        }
    }



    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previusValue, MyCustomData newValue) =>
        {
            // Debug.Log("Client ID: " + OwnerClientId + "  random number : " + newValue._int + "; boolState: " + newValue._bool + "Message : " + newValue._message);
        };
    }
    void Update()
    {

        if (!IsOwner) return;


        if (Input.GetKeyDown(KeyCode.T))
        {

            Transform spawnedObjectTransfrom = Instantiate(spawnedObjectPrefab);
            spawnedObjectPrefab.GetComponent<NetworkObject>().Spawn(true);
             
            // TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
            // randomNumber.Value = new MyCustomData
            // {
            //     _int = Random.Range(0, 100),
            //     _bool = false,
            //     _message = "TestSercerRpc   +   OwnerClientId=>>" + OwnerClientId
            // };

        }
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float movmentSpeed = 3;
        transform.position += moveDir * movmentSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestSercerRpc   +   OwnerClientId=>>" + OwnerClientId + "; serveerRpcParams SenderClientID =>>" + serverRpcParams.Receive.SenderClientId);
        networkManagerUI.debugConsole.text = serverRpcParams.Receive.SenderClientId.ToString();
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.LogWarning("TestClientRpcs =" + OwnerClientId);
        networkManagerUI.debugConsole.text = clientRpcParams.ToString();
    }
}
