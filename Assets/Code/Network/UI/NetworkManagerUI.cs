using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverBtn;
    [SerializeField] Button hostBtn;
    [SerializeField] Button clientBtn;
    public TextMeshProUGUI debugConsole;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartHost();
       });
        clientBtn.onClick.AddListener(() =>
       {
           NetworkManager.Singleton.StartClient();
       });

    }
}
