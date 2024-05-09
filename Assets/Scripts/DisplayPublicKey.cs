using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Types;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;

public class DisplayPublicKey : MonoBehaviour
{

    private TextMeshProUGUI _txtPublicKey;

    // Start is called before the first frame update
    void Start()
    {
        _txtPublicKey = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Web3.OnLogin += OnLogin;
    }

    private void OnDisable()
    {
        Web3.OnLogin -= OnLogin;
    }

    private void OnLogin(Account account)
    {
        _txtPublicKey.text = account.PublicKey;
    }
}
