import { ethers } from "hardhat";

async function main() {


  const ContractFactory = await ethers.getContractFactory("WeAreonThePlanetNFT");

  // TODO: Set addresses for the contract arguments below

  const signer = await ethers.provider.getSigner();
  const instance = await ContractFactory.deploy("0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266");
  await instance.waitForDeployment();

  console.log(`NFT Contract deployed to ${await instance.getAddress()}`);


  const ContractFactory2 = await ethers.getContractFactory("WeAreonThePlanetToken");

  // TODO: Set addresses for the contract arguments below
  const instance2 = await ContractFactory2.deploy("0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266");
  instance2.waitForDeployment();

  console.log(`TokenContract deployed to ${await instance2.getAddress()}`);
}

// We recommend this pattern to be able to use async/await everywhere
// and properly handle errors.
main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
