using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.HdWallet;
using Nethereum.Unity.Rpc;
using UnityEngine;
using UnityEngine.Assertions;

public class SamrtContract : MonoBehaviour
{
    public partial class NonFunTokenDeployment : NonFunTokenDeploymentBase
    {
        public NonFunTokenDeployment() : base(BYTECODE) { }
        public NonFunTokenDeployment(string byteCode) : base(byteCode) { }
    }

    public class NonFunTokenDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801562000010575f80fd5b5060405162002c1338038062002c1383398181016040528101906200003691906200027c565b806040518060400160405280600b81526020017f4e6f6e46756e546f6b656e0000000000000000000000000000000000000000008152506040518060400160405280600681526020017f4e4f4e46554e0000000000000000000000000000000000000000000000000000815250815f9081620000b3919062000510565b508060019081620000c5919062000510565b5050505f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16036200013b575f6040517f1e4fbdf700000000000000000000000000000000000000000000000000000000815260040162000132919062000605565b60405180910390fd5b6200014c816200015460201b60201c565b505062000620565b5f60075f9054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690508160075f6101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b5f80fd5b5f73ffffffffffffffffffffffffffffffffffffffff82169050919050565b5f62000246826200021b565b9050919050565b62000258816200023a565b811462000263575f80fd5b50565b5f8151905062000276816200024d565b92915050565b5f6020828403121562000294576200029362000217565b5b5f620002a38482850162000266565b91505092915050565b5f81519050919050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52604160045260245ffd5b7f4e487b71000000000000000000000000000000000000000000000000000000005f52602260045260245ffd5b5f60028204905060018216806200032857607f821691505b6020821081036200033e576200033d620002e3565b5b50919050565b5f819050815f5260205f209050919050565b5f6020601f8301049050919050565b5f82821b905092915050565b5f60088302620003a27fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8262000365565b620003ae868362000365565b95508019841693508086168417925050509392505050565b5f819050919050565b5f819050919050565b5f620003f8620003f2620003ec84620003c6565b620003cf565b620003c6565b9050919050565b5f819050919050565b6200041383620003d8565b6200042b6200042282620003ff565b84845462000371565b825550505050565b5f90565b6200044162000433565b6200044e81848462000408565b505050565b5b818110156200047557620004695f8262000437565b60018101905062000454565b5050565b601f821115620004c4576200048e8162000344565b620004998462000356565b81016020851015620004a9578190505b620004c1620004b88562000356565b83018262000453565b50505b505050565b5f82821c905092915050565b5f620004e65f1984600802620004c9565b1980831691505092915050565b5f620005008383620004d5565b9150826002028217905092915050565b6200051b82620002ac565b67ffffffffffffffff811115620005375762000536620002b6565b5b62000543825462000310565b6200055082828562000479565b5f60209050601f83116001811462000586575f841562000571578287015190505b6200057d8582620004f3565b865550620005ec565b601f198416620005968662000344565b5f5b82811015620005bf5784890151825560018201915060208501945060208101905062000598565b86831015620005df5784890151620005db601f891682620004d5565b8355505b6001600288020188555050505b505050505050565b620005ff816200023a565b82525050565b5f6020820190506200061a5f830184620005f4565b92915050565b6125e5806200062e5f395ff3fe608060405234801561000f575f80fd5b5060043610610114575f3560e01c8063715018a6116100a0578063b88d4fde1161006f578063b88d4fde146102c8578063c87b56dd146102e4578063d204c45e14610314578063e985e9c514610330578063f2fde38b1461036057610114565b8063715018a6146102665780638da5cb5b1461027057806395d89b411461028e578063a22cb465146102ac57610114565b806323b872dd116100e757806323b872dd146101b257806342842e0e146101ce57806342966c68146101ea5780636352211e1461020657806370a082311461023657610114565b806301ffc9a71461011857806306fdde0314610148578063081812fc14610166578063095ea7b314610196575b5f80fd5b610132600480360381019061012d9190611a9e565b61037c565b60405161013f9190611ae3565b60405180910390f35b61015061038d565b60405161015d9190611b86565b60405180910390f35b610180600480360381019061017b9190611bd9565b61041c565b60405161018d9190611c43565b60405180910390f35b6101b060048036038101906101ab9190611c86565b610437565b005b6101cc60048036038101906101c79190611cc4565b61044d565b005b6101e860048036038101906101e39190611cc4565b61054c565b005b61020460048036038101906101ff9190611bd9565b61056b565b005b610220600480360381019061021b9190611bd9565b610581565b60405161022d9190611c43565b60405180910390f35b610250600480360381019061024b9190611d14565b610592565b60405161025d9190611d4e565b60405180910390f35b61026e610648565b005b61027861065b565b6040516102859190611c43565b60405180910390f35b610296610683565b6040516102a39190611b86565b60405180910390f35b6102c660048036038101906102c19190611d91565b610713565b005b6102e260048036038101906102dd9190611efb565b610729565b005b6102fe60048036038101906102f99190611bd9565b610746565b60405161030b9190611b86565b60405180910390f35b61032e60048036038101906103299190612019565b610758565b005b61034a60048036038101906103459190612073565b610792565b6040516103579190611ae3565b60405180910390f35b61037a60048036038101906103759190611d14565b610820565b005b5f610386826108a4565b9050919050565b60605f805461039b906120de565b80601f01602080910402602001604051908101604052809291908181526020018280546103c7906120de565b80156104125780601f106103e957610100808354040283529160200191610412565b820191905f5260205f20905b8154815290600101906020018083116103f557829003601f168201915b5050505050905090565b5f61042682610904565b506104308261098a565b9050919050565b61044982826104446109c3565b6109ca565b5050565b5f73ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff16036104bd575f6040517f64a0ae920000000000000000000000000000000000000000000000000000000081526004016104b49190611c43565b60405180910390fd5b5f6104d083836104cb6109c3565b6109dc565b90508373ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614610546578382826040517f64283d7b00000000000000000000000000000000000000000000000000000000815260040161053d9392919061210e565b60405180910390fd5b50505050565b61056683838360405180602001604052805f815250610729565b505050565b61057d5f826105786109c3565b6109dc565b5050565b5f61058b82610904565b9050919050565b5f8073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1603610603575f6040517f89c62b640000000000000000000000000000000000000000000000000000000081526004016105fa9190611c43565b60405180910390fd5b60035f8373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f20549050919050565b610650610be7565b6106595f610c6e565b565b5f60075f9054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905090565b606060018054610692906120de565b80601f01602080910402602001604051908101604052809291908181526020018280546106be906120de565b80156107095780601f106106e057610100808354040283529160200191610709565b820191905f5260205f20905b8154815290600101906020018083116106ec57829003601f168201915b5050505050905090565b61072561071e6109c3565b8383610d31565b5050565b61073484848461044d565b61074084848484610e9a565b50505050565b60606107518261104c565b9050919050565b610760610be7565b5f60085f81548092919061077390612170565b9190505590506107838382611157565b61078d8183611174565b505050565b5f60055f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f9054906101000a900460ff16905092915050565b610828610be7565b5f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1603610898575f6040517f1e4fbdf700000000000000000000000000000000000000000000000000000000815260040161088f9190611c43565b60405180910390fd5b6108a181610c6e565b50565b5f634906490660e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff191614806108fd57506108fc826111ce565b5b9050919050565b5f8061090f836112af565b90505f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff160361098157826040517f7e2732890000000000000000000000000000000000000000000000000000000081526004016109789190611d4e565b60405180910390fd5b80915050919050565b5f60045f8381526020019081526020015f205f9054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050919050565b5f33905090565b6109d783838360016112e8565b505050565b5f806109e7846112af565b90505f73ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff1614610a2857610a278184866114a7565b5b5f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614610ab357610a675f855f806112e8565b600160035f8373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f82825403925050819055505b5f73ffffffffffffffffffffffffffffffffffffffff168573ffffffffffffffffffffffffffffffffffffffff1614610b3257600160035f8773ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f82825401925050819055505b8460025f8681526020019081526020015f205f6101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550838573ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef60405160405180910390a4809150509392505050565b610bef6109c3565b73ffffffffffffffffffffffffffffffffffffffff16610c0d61065b565b73ffffffffffffffffffffffffffffffffffffffff1614610c6c57610c306109c3565b6040517f118cdaa7000000000000000000000000000000000000000000000000000000008152600401610c639190611c43565b60405180910390fd5b565b5f60075f9054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690508160075f6101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b5f73ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1603610da157816040517f5b08ba18000000000000000000000000000000000000000000000000000000008152600401610d989190611c43565b60405180910390fd5b8060055f8573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f8473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020015f205f6101000a81548160ff0219169083151502179055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167f17307eab39ab6107e8899845ad3d59bd9653f200f220920489ca2b5937696c3183604051610e8d9190611ae3565b60405180910390a3505050565b5f8373ffffffffffffffffffffffffffffffffffffffff163b1115611046578273ffffffffffffffffffffffffffffffffffffffff1663150b7a02610edd6109c3565b8685856040518563ffffffff1660e01b8152600401610eff9493929190612209565b6020604051808303815f875af1925050508015610f3a57506040513d601f19601f82011682018060405250810190610f379190612267565b60015b610fbb573d805f8114610f68576040519150601f19603f3d011682016040523d82523d5f602084013e610f6d565b606091505b505f815103610fb357836040517f64a0ae92000000000000000000000000000000000000000000000000000000008152600401610faa9190611c43565b60405180910390fd5b805181602001fd5b63150b7a0260e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916817bffffffffffffffffffffffffffffffffffffffffffffffffffffffff19161461104457836040517f64a0ae9200000000000000000000000000000000000000000000000000000000815260040161103b9190611c43565b60405180910390fd5b505b50505050565b606061105782610904565b505f60065f8481526020019081526020015f208054611075906120de565b80601f01602080910402602001604051908101604052809291908181526020018280546110a1906120de565b80156110ec5780601f106110c3576101008083540402835291602001916110ec565b820191905f5260205f20905b8154815290600101906020018083116110cf57829003601f168201915b505050505090505f6110fc61156a565b90505f815103611110578192505050611152565b5f8251111561114457808260405160200161112c9291906122cc565b60405160208183030381529060405292505050611152565b61114d84611580565b925050505b919050565b611170828260405180602001604052805f8152506115e6565b5050565b8060065f8481526020019081526020015f209081611192919061248c565b507ff8e1a15aba9398e019f0b49df1a4fde98ee17ae345cb5f6b5e2c27f5033e8ce7826040516111c29190611d4e565b60405180910390a15050565b5f7f80ac58cd000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916148061129857507f5b5e139f000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916145b806112a857506112a782611601565b5b9050919050565b5f60025f8381526020019081526020015f205f9054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050919050565b808061132057505f73ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1614155b15611452575f61132f84610904565b90505f73ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff161415801561139957508273ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614155b80156113ac57506113aa8184610792565b155b156113ee57826040517fa9fbf51f0000000000000000000000000000000000000000000000000000000081526004016113e59190611c43565b60405180910390fd5b811561145057838573ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b92560405160405180910390a45b505b8360045f8581526020019081526020015f205f6101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555050505050565b6114b283838361166a565b611565575f73ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff160361152657806040517f7e27328900000000000000000000000000000000000000000000000000000000815260040161151d9190611d4e565b60405180910390fd5b81816040517f177e802f00000000000000000000000000000000000000000000000000000000815260040161155c92919061255b565b60405180910390fd5b505050565b606060405180602001604052805f815250905090565b606061158b82610904565b505f61159561156a565b90505f8151116115b35760405180602001604052805f8152506115de565b806115bd8461172a565b6040516020016115ce9291906122cc565b6040516020818303038152906040525b915050919050565b6115f083836117f4565b6115fc5f848484610e9a565b505050565b5f7f01ffc9a7000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916149050919050565b5f8073ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff161415801561172157508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff1614806116e257506116e18484610792565b5b8061172057508273ffffffffffffffffffffffffffffffffffffffff166117088361098a565b73ffffffffffffffffffffffffffffffffffffffff16145b5b90509392505050565b60605f6001611738846118e7565b0190505f8167ffffffffffffffff81111561175657611755611dd7565b5b6040519080825280601f01601f1916602001820160405280156117885781602001600182028036833780820191505090505b5090505f82602001820190505b6001156117e9578080600190039150507f3031323334353637383961626364656600000000000000000000000000000000600a86061a8153600a85816117de576117dd612582565b5b0494505f8503611795575b819350505050919050565b5f73ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1603611864575f6040517f64a0ae9200000000000000000000000000000000000000000000000000000000815260040161185b9190611c43565b60405180910390fd5b5f61187083835f6109dc565b90505f73ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16146118e2575f6040517f73c6ac6e0000000000000000000000000000000000000000000000000000000081526004016118d99190611c43565b60405180910390fd5b505050565b5f805f90507a184f03e93ff9f4daa797ed6e38ed64bf6a1f0100000000000000008310611943577a184f03e93ff9f4daa797ed6e38ed64bf6a1f010000000000000000838161193957611938612582565b5b0492506040810190505b6d04ee2d6d415b85acef81000000008310611980576d04ee2d6d415b85acef8100000000838161197657611975612582565b5b0492506020810190505b662386f26fc1000083106119af57662386f26fc1000083816119a5576119a4612582565b5b0492506010810190505b6305f5e10083106119d8576305f5e10083816119ce576119cd612582565b5b0492506008810190505b61271083106119fd5761271083816119f3576119f2612582565b5b0492506004810190505b60648310611a205760648381611a1657611a15612582565b5b0492506002810190505b600a8310611a2f576001810190505b80915050919050565b5f604051905090565b5f80fd5b5f80fd5b5f7fffffffff0000000000000000000000000000000000000000000000000000000082169050919050565b611a7d81611a49565b8114611a87575f80fd5b50565b5f81359050611a9881611a74565b92915050565b5f60208284031215611ab357611ab2611a41565b5b5f611ac084828501611a8a565b91505092915050565b5f8115159050919050565b611add81611ac9565b82525050565b5f602082019050611af65f830184611ad4565b92915050565b5f81519050919050565b5f82825260208201905092915050565b5f5b83811015611b33578082015181840152602081019050611b18565b5f8484015250505050565b5f601f19601f8301169050919050565b5f611b5882611afc565b611b628185611b06565b9350611b72818560208601611b16565b611b7b81611b3e565b840191505092915050565b5f6020820190508181035f830152611b9e8184611b4e565b905092915050565b5f819050919050565b611bb881611ba6565b8114611bc2575f80fd5b50565b5f81359050611bd381611baf565b92915050565b5f60208284031215611bee57611bed611a41565b5b5f611bfb84828501611bc5565b91505092915050565b5f73ffffffffffffffffffffffffffffffffffffffff82169050919050565b5f611c2d82611c04565b9050919050565b611c3d81611c23565b82525050565b5f602082019050611c565f830184611c34565b92915050565b611c6581611c23565b8114611c6f575f80fd5b50565b5f81359050611c8081611c5c565b92915050565b5f8060408385031215611c9c57611c9b611a41565b5b5f611ca985828601611c72565b9250506020611cba85828601611bc5565b9150509250929050565b5f805f60608486031215611cdb57611cda611a41565b5b5f611ce886828701611c72565b9350506020611cf986828701611c72565b9250506040611d0a86828701611bc5565b9150509250925092565b5f60208284031215611d2957611d28611a41565b5b5f611d3684828501611c72565b91505092915050565b611d4881611ba6565b82525050565b5f602082019050611d615f830184611d3f565b92915050565b611d7081611ac9565b8114611d7a575f80fd5b50565b5f81359050611d8b81611d67565b92915050565b5f8060408385031215611da757611da6611a41565b5b5f611db485828601611c72565b9250506020611dc585828601611d7d565b9150509250929050565b5f80fd5b5f80fd5b7f4e487b71000000000000000000000000000000000000000000000000000000005f52604160045260245ffd5b611e0d82611b3e565b810181811067ffffffffffffffff82111715611e2c57611e2b611dd7565b5b80604052505050565b5f611e3e611a38565b9050611e4a8282611e04565b919050565b5f67ffffffffffffffff821115611e6957611e68611dd7565b5b611e7282611b3e565b9050602081019050919050565b828183375f83830152505050565b5f611e9f611e9a84611e4f565b611e35565b905082815260208101848484011115611ebb57611eba611dd3565b5b611ec6848285611e7f565b509392505050565b5f82601f830112611ee257611ee1611dcf565b5b8135611ef2848260208601611e8d565b91505092915050565b5f805f8060808587031215611f1357611f12611a41565b5b5f611f2087828801611c72565b9450506020611f3187828801611c72565b9350506040611f4287828801611bc5565b925050606085013567ffffffffffffffff811115611f6357611f62611a45565b5b611f6f87828801611ece565b91505092959194509250565b5f67ffffffffffffffff821115611f9557611f94611dd7565b5b611f9e82611b3e565b9050602081019050919050565b5f611fbd611fb884611f7b565b611e35565b905082815260208101848484011115611fd957611fd8611dd3565b5b611fe4848285611e7f565b509392505050565b5f82601f83011261200057611fff611dcf565b5b8135612010848260208601611fab565b91505092915050565b5f806040838503121561202f5761202e611a41565b5b5f61203c85828601611c72565b925050602083013567ffffffffffffffff81111561205d5761205c611a45565b5b61206985828601611fec565b9150509250929050565b5f806040838503121561208957612088611a41565b5b5f61209685828601611c72565b92505060206120a785828601611c72565b9150509250929050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52602260045260245ffd5b5f60028204905060018216806120f557607f821691505b602082108103612108576121076120b1565b5b50919050565b5f6060820190506121215f830186611c34565b61212e6020830185611d3f565b61213b6040830184611c34565b949350505050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52601160045260245ffd5b5f61217a82611ba6565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff82036121ac576121ab612143565b5b600182019050919050565b5f81519050919050565b5f82825260208201905092915050565b5f6121db826121b7565b6121e581856121c1565b93506121f5818560208601611b16565b6121fe81611b3e565b840191505092915050565b5f60808201905061221c5f830187611c34565b6122296020830186611c34565b6122366040830185611d3f565b818103606083015261224881846121d1565b905095945050505050565b5f8151905061226181611a74565b92915050565b5f6020828403121561227c5761227b611a41565b5b5f61228984828501612253565b91505092915050565b5f81905092915050565b5f6122a682611afc565b6122b08185612292565b93506122c0818560208601611b16565b80840191505092915050565b5f6122d7828561229c565b91506122e3828461229c565b91508190509392505050565b5f819050815f5260205f209050919050565b5f6020601f8301049050919050565b5f82821b905092915050565b5f6008830261234b7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff82612310565b6123558683612310565b95508019841693508086168417925050509392505050565b5f819050919050565b5f61239061238b61238684611ba6565b61236d565b611ba6565b9050919050565b5f819050919050565b6123a983612376565b6123bd6123b582612397565b84845461231c565b825550505050565b5f90565b6123d16123c5565b6123dc8184846123a0565b505050565b5b818110156123ff576123f45f826123c9565b6001810190506123e2565b5050565b601f82111561244457612415816122ef565b61241e84612301565b8101602085101561242d578190505b61244161243985612301565b8301826123e1565b50505b505050565b5f82821c905092915050565b5f6124645f1984600802612449565b1980831691505092915050565b5f61247c8383612455565b9150826002028217905092915050565b61249582611afc565b67ffffffffffffffff8111156124ae576124ad611dd7565b5b6124b882546120de565b6124c3828285612403565b5f60209050601f8311600181146124f4575f84156124e2578287015190505b6124ec8582612471565b865550612553565b601f198416612502866122ef565b5f5b8281101561252957848901518255600182019150602085019450602081019050612504565b868310156125465784890151612542601f891682612455565b8355505b6001600288020188555050505b505050505050565b5f60408201905061256e5f830185611c34565b61257b6020830184611d3f565b9392505050565b7f4e487b71000000000000000000000000000000000000000000000000000000005f52601260045260245ffdfea26469706673582212208dba6d27f143132c759c6467e9a8deeff9faaf34e749316486dc1aa7f26076fe64736f6c63430008140033";
        public NonFunTokenDeploymentBase() : base(BYTECODE) { }
        public NonFunTokenDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "initialOwner", 1)]
        public virtual string InitialOwner { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class BurnFunction : BurnFunctionBase { }

    [Function("burn")]
    public class BurnFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class GetApprovedFunction : GetApprovedFunctionBase { }

    [Function("getApproved", "address")]
    public class GetApprovedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerOfFunction : OwnerOfFunctionBase { }

    [Function("ownerOf", "address")]
    public class OwnerOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class SafeMintFunction : SafeMintFunctionBase { }

    [Function("safeMint")]
    public class SafeMintFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("string", "uri", 2)]
        public virtual string Uri { get; set; }
    }

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class SafeTransferFrom1Function : SafeTransferFrom1FunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFrom1FunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("bytes", "data", 4)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

    [Function("setApprovalForAll")]
    public class SetApprovalForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 2)]
        public virtual bool Approved { get; set; }
    }

    public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

    [Function("supportsInterface", "bool")]
    public class SupportsInterfaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes4", "interfaceId", 1)]
        public virtual byte[] InterfaceId { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TokenURIFunction : TokenURIFunctionBase { }

    [Function("tokenURI", "string")]
    public class TokenURIFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true)]
        public virtual string Owner { get; set; }
        [Parameter("address", "approved", 2, true)]
        public virtual string Approved { get; set; }
        [Parameter("uint256", "tokenId", 3, true)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }

    [Event("ApprovalForAll")]
    public class ApprovalForAllEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true)]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2, true)]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 3, false)]
        public virtual bool Approved { get; set; }
    }

    public partial class BatchMetadataUpdateEventDTO : BatchMetadataUpdateEventDTOBase { }

    [Event("BatchMetadataUpdate")]
    public class BatchMetadataUpdateEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_fromTokenId", 1, false)]
        public virtual BigInteger FromTokenId { get; set; }
        [Parameter("uint256", "_toTokenId", 2, false)]
        public virtual BigInteger ToTokenId { get; set; }
    }

    public partial class MetadataUpdateEventDTO : MetadataUpdateEventDTOBase { }

    [Event("MetadataUpdate")]
    public class MetadataUpdateEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_tokenId", 1, false)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true)]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true)]
        public virtual string NewOwner { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3, true)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ERC721IncorrectOwnerError : ERC721IncorrectOwnerErrorBase { }

    [Error("ERC721IncorrectOwner")]
    public class ERC721IncorrectOwnerErrorBase : IErrorDTO
    {
        [Parameter("address", "sender", 1)]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "owner", 3)]
        public virtual string Owner { get; set; }
    }

    public partial class ERC721InsufficientApprovalError : ERC721InsufficientApprovalErrorBase { }

    [Error("ERC721InsufficientApproval")]
    public class ERC721InsufficientApprovalErrorBase : IErrorDTO
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ERC721InvalidApproverError : ERC721InvalidApproverErrorBase { }

    [Error("ERC721InvalidApprover")]
    public class ERC721InvalidApproverErrorBase : IErrorDTO
    {
        [Parameter("address", "approver", 1)]
        public virtual string Approver { get; set; }
    }

    public partial class ERC721InvalidOperatorError : ERC721InvalidOperatorErrorBase { }

    [Error("ERC721InvalidOperator")]
    public class ERC721InvalidOperatorErrorBase : IErrorDTO
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
    }

    public partial class ERC721InvalidOwnerError : ERC721InvalidOwnerErrorBase { }

    [Error("ERC721InvalidOwner")]
    public class ERC721InvalidOwnerErrorBase : IErrorDTO
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class ERC721InvalidReceiverError : ERC721InvalidReceiverErrorBase { }

    [Error("ERC721InvalidReceiver")]
    public class ERC721InvalidReceiverErrorBase : IErrorDTO
    {
        [Parameter("address", "receiver", 1)]
        public virtual string Receiver { get; set; }
    }

    public partial class ERC721InvalidSenderError : ERC721InvalidSenderErrorBase { }

    [Error("ERC721InvalidSender")]
    public class ERC721InvalidSenderErrorBase : IErrorDTO
    {
        [Parameter("address", "sender", 1)]
        public virtual string Sender { get; set; }
    }

    public partial class ERC721NonexistentTokenError : ERC721NonexistentTokenErrorBase { }

    [Error("ERC721NonexistentToken")]
    public class ERC721NonexistentTokenErrorBase : IErrorDTO
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class OwnableInvalidOwnerError : OwnableInvalidOwnerErrorBase { }

    [Error("OwnableInvalidOwner")]
    public class OwnableInvalidOwnerErrorBase : IErrorDTO
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class OwnableUnauthorizedAccountError : OwnableUnauthorizedAccountErrorBase { }

    [Error("OwnableUnauthorizedAccount")]
    public class OwnableUnauthorizedAccountErrorBase : IErrorDTO
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetApprovedOutputDTO : GetApprovedOutputDTOBase { }

    [FunctionOutput]
    public class GetApprovedOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class IsApprovedForAllOutputDTO : IsApprovedForAllOutputDTOBase { }

    [FunctionOutput]
    public class IsApprovedForAllOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerOfOutputDTO : OwnerOfOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOfOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

    [FunctionOutput]
    public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TokenURIOutputDTO : TokenURIOutputDTOBase { }

    [FunctionOutput]
    public class TokenURIOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting TokenDeployAndSendCoroutinesUnityWebRequest example");
        StartCoroutine(DeployTest());
    }

    public IEnumerator DeployTest()
    {
        /* initializing the transaction request sender */
        var url = "http://localhost:8545/";
        var privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
        var account = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";

        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, 31337);
        transactionRequest.UseLegacyAsDefault = true;

        var deployContract = new NonFunTokenDeployment()
        {
            InitialOwner = account,
        };

        /* deploy the contract and True indicates we want to estimate the gas */
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<NonFunTokenDeploymentBase>(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;
        Debug.Log("Deployment transaction hash:" + transactionHash);

        // create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        // checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;
        Debug.Log("Deployment contract address:" + deploymentReceipt.ContractAddress);

        /* Query Name */
        Debug.Log("Querying NFT collection name...");
        var queryNameRequest = new QueryUnityRequest<NameFunction, NameOutputDTO>(url, account);
        yield return queryNameRequest.Query(new NameFunction(), deploymentReceipt.ContractAddress);

        var dtoNameResult = queryNameRequest.Result;
        Debug.Log(dtoNameResult.ReturnValue1);

        /* Query Symbol */
        Debug.Log("Querying NFT collection symbol...");
        var querySymbolRequest = new QueryUnityRequest<SymbolFunction, SymbolOutputDTO>(url, account);
        yield return querySymbolRequest.Query(new SymbolFunction(), deploymentReceipt.ContractAddress);

        var dtoSymbolResult = querySymbolRequest.Result;
        Debug.Log(dtoSymbolResult.ReturnValue1);

        /* Query Balance */
        Debug.Log("Querying the balance count of contractOwner " + account + " ...");
        var queryBalanceRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryBalanceRequest.Query(new BalanceOfFunction() { Owner = account }, deploymentReceipt.ContractAddress);

        var dtoBalanceResult = queryBalanceRequest.Result;
        Debug.Log(account + " has balance " + dtoBalanceResult.ReturnValue1);

        /* safeMint */
        var mintCount = 3;
        for (var i = 1; i <= mintCount; i++)
        {
            var transactionMintRequest = new TransactionSignedUnityRequest(url, privateKey, 31337);
            transactionMintRequest.UseLegacyAsDefault = true;

            var mintAddress = account;
            var mintURI = i.ToString();

            var mintMessage = new SafeMintFunction()
            {
                To = mintAddress,
                Uri = mintURI
            };

            yield return transactionMintRequest.SignAndSendTransaction(mintMessage, deploymentReceipt.ContractAddress);

            var transactionMintHash = transactionMintRequest.Result;
            Debug.Log("Transfer txn hash:" + transactionMintHash);

            transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
            yield return transactionReceiptPolling.PollForReceipt(transactionMintHash, 2);
        }

        /* Query Balance */
        queryBalanceRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryBalanceRequest.Query(new BalanceOfFunction() { Owner = account }, deploymentReceipt.ContractAddress);

        dtoBalanceResult = queryBalanceRequest.Result;
        Debug.Log(account + " has balance " + dtoBalanceResult.ReturnValue1);

        /* Query Owner */
        var tokenId = 0;  // the index of token starts from 0
        Debug.Log("Querying the owner of token " + tokenId + " ...");
        var queryOwnerRequest = new QueryUnityRequest<OwnerOfFunction, OwnerOfOutputDTO>(url, account);
        yield return queryOwnerRequest.Query(new OwnerOfFunction() { TokenId = tokenId }, deploymentReceipt.ContractAddress);

        var dtoOwnerResult = queryOwnerRequest.Result;
        Debug.Log("token " + tokenId + " owner: " + dtoOwnerResult.ReturnValue1);

        /* safeTransferFrom */
        var collector = "0x70997970C51812dc3A010C7d01b50e0d17dc79C8";
        Debug.Log("Transfer " + dtoSymbolResult.ReturnValue1 + "-#" + tokenId + " to collector " + collector);

        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey, 31337);
        transactionTransferRequest.UseLegacyAsDefault = true;

        var safeTransferMessage = new SafeTransferFromFunction()
        {
            From = account,
            To = collector,
            TokenId = tokenId
        };

        yield return transactionTransferRequest.SignAndSendTransaction(safeTransferMessage, deploymentReceipt.ContractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;
        Debug.Log("Transfer txn hash:" + transactionTransferHash);

        transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);

        queryBalanceRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryBalanceRequest.Query(new BalanceOfFunction() { Owner = account }, deploymentReceipt.ContractAddress);

        dtoBalanceResult = queryBalanceRequest.Result;
        Debug.Log(account + " has balance " + dtoBalanceResult.ReturnValue1);

        queryBalanceRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryBalanceRequest.Query(new BalanceOfFunction() { Owner = collector }, deploymentReceipt.ContractAddress);

        dtoBalanceResult = queryBalanceRequest.Result;
        Debug.Log(collector + " has balance " + dtoBalanceResult.ReturnValue1);

        /* Approve */
        tokenId = 1;
        Debug.Log("Approve contractOwner to spend collector " + dtoSymbolResult.ReturnValue1 + "-#" + tokenId + "...");
        var transactionApproveRequest = new TransactionSignedUnityRequest(url, privateKey, 31337);
        transactionApproveRequest.UseLegacyAsDefault = true;

        var approveMessage = new ApproveFunction()
        {
            To = collector,
            TokenId = tokenId
        };

        yield return transactionApproveRequest.SignAndSendTransaction(approveMessage, deploymentReceipt.ContractAddress);

        var transactionApproveHash = transactionApproveRequest.Result;
        Debug.Log("Transfer txn hash:" + transactionApproveHash);

        transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);

        /* Query getApproved */
        Debug.Log("Querying the accounts approved to spend token " + tokenId + " ...");
        var queryGetApprovedRequest = new QueryUnityRequest<GetApprovedFunction, GetApprovedOutputDTO>(url, account);
        yield return queryGetApprovedRequest.Query(new GetApprovedFunction() { TokenId = tokenId }, deploymentReceipt.ContractAddress);

        var dtoGetApprovedResult = queryGetApprovedRequest.Result;
        Debug.Log(dtoGetApprovedResult.ReturnValue1 + " is approved");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}