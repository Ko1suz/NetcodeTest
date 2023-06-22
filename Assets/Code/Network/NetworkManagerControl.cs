using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.Networking;
using System.Linq;

public class NetworkManagerControl : MonoBehaviour
{

    NetworkManager networkManager;
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;
    // Start is called before the first frame update
    void Start()
    {
        RegisterNetworkPrefabs();
    }

    private void RegisterNetworkPrefabs()
    {
        var prefabs = _networkPrefabsList.PrefabList.Select(x => x.Prefab);
        foreach (var prefab in prefabs)
        {
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}



