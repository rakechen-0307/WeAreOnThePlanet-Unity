using System.Numerics;
using TMPro;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class WalletChecker : MonoBehaviour
{
    [SerializeField] private TMP_Text money;
    private void Start()
    {
        checkWallet();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            checkWallet();
        }
    }

    private async void checkWallet()
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);


        var data = await contract.Call(method, new object[]
        {
                PlayerPrefs.GetString("Account")
        });


        BigInteger balanceOf = BigInteger.Parse(data[0].ToString())/1000000000000000000;
        string gottenMoney = balanceOf.ToString();
        money.text = gottenMoney;
    }
}
