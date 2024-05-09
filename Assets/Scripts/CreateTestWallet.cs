using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solana.Unity.SDK;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;

public class CreateTestWallet : MonoBehaviour
{
    public void CreateWallet()
    {
        Web3.Instance.CreateAccount(
            mnemonic: "41QrRDM2cSQKNetPEkwvaK9vy6uLzVoMceNSNJLnVSopDEjhajvjQzQLfUqTYfx41pqMkTx3eVa2LLTFVnMxarU9", 
            password: "testpsw");

    }
}