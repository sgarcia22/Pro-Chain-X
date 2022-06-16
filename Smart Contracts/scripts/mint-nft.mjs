const CONTRACT_ADDRESS = "0x00";
const META_DATA_URL = "ipfs://bafyreidx2yqaw4rmnccpk6ycr2rpktlbsv4qdgcmare2jm4q75we3fta4u/metadata.json";

async function mintNFT(contractAddress, metaDataURL) {
   const ExampleNFT = await ethers.getContractFactory("NftShop");
   const [owner] = await ethers.getSigners();
   await ExampleNFT.attach(contractAddress).mintNFT(owner.address, metaDataURL);
   console.log("NFT minted to: ", owner.address);
}

mintNFT(CONTRACT_ADDRESS, META_DATA_URL)
   .then(() => process.exit(0))
   .catch((error) => {
       console.error(error);
       process.exit(1);
   });