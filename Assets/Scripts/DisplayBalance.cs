using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Types;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;


public class DisplayBalance : MonoBehaviour
{
    private TextMeshProUGUI _txtBalance;

    // Start is called before the first frame update
    void Start()
    {
        _txtBalance = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Web3.OnBalanceChange += OnBalanceChange;
    }

    private void OnDisable()
    {
        Web3.OnBalanceChange -= OnBalanceChange;
    }

    private void OnBalanceChange(double amount)
    {
        _txtBalance.text = amount.ToString(CultureInfo.InvariantCulture);
    }
}
