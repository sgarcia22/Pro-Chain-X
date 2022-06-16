const { expect } = require("chai");
const META_DATA_URL = "ipfs://bafyreidx2yqaw4rmnccpk6ycr2rpktlbsv4qdgcmare2jm4q75we3fta4u/metadata.json";

describe("Pro Chain X NFT Contract", function () {
  it("Mint NFT to message sender", async function () {
    const [owner] = await ethers.getSigners();

    const contractFactory = await ethers.getContractFactory("NftShop");
    const contract = await contractFactory.deploy();
    await contract.setURI("ipfs://bafyreidx2yqaw4rmnccpk6ycr2rpktlbsv4qdgcmare2jm4q75we3fta4u/metadata.json");
    // expect(await contract.mintNFT()).to.equal(45);
    await contract.mint();
    const balance = await contract.balanceOf(owner.address, 0);

    // // console.log(await ethers.provider.getBalance(owner.address));
    expect(Number(balance.toString())).to.equal(1);
  });
});