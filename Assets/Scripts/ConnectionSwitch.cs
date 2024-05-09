using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Solana.Unity.SDK;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;


public class ConnectionSwitch : MonoBehaviour
{
    [SerializeField] private Button walletConnect;     // Changed the naming convention to camelCase.
    [SerializeField] private Button walletDisconnect;   // Removed duplicate declaration.
    [SerializeField] private GameObject txtPublicKey;
    [SerializeField] private GameObject txtBalance;     // Declared this variable since it's being used later.

    private void Start()
    {
        walletDisconnect.onClick.AddListener(() => Web3.Instance.Logout());
    }

    private void OnEnable()
    {
        Web3.OnLogin += OnLogin;
        Web3.OnLogout += OnLogout;
    }

    private void OnDisable()
    {
        Web3.OnLogin -= OnLogin;
        Web3.OnLogout -= OnLogout;
    }

    private void OnLogin(Account obj)
    {
        walletConnect.gameObject.SetActive(false);
        walletDisconnect.gameObject.SetActive(true);    // Fixed the typo here.
        txtPublicKey.SetActive(true);
        txtBalance.SetActive(true);
    }

    private void OnLogout()
    {
        walletConnect.gameObject.SetActive(true);
        walletDisconnect.gameObject.SetActive(false);
        txtPublicKey.SetActive(false);
        txtBalance.SetActive(false);
    }
}
