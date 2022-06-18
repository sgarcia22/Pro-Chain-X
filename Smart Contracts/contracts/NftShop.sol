// SPDX-License-Identifier: MIT
pragma solidity >=0.7.0 <0.9.0;

//Importing ERC 1155 Token contract from OpenZeppelin
import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "hardhat/console.sol";

contract NftShop is ERC1155, Ownable  {
   uint256 public price = 0.01 ether;

   constructor() ERC1155("ipfs://bafyreidx2yqaw4rmnccpk6ycr2rpktlbsv4qdgcmare2jm4q75we3fta4u/metadata.json")  {
   }

   function setURI(string memory newuri) public onlyOwner {
      _setURI(newuri);
   }

   function setPrice(uint256 _price) public onlyOwner {
      price = _price;
    }

   function mint() external payable {
        // Check if address already minted NFT
        require(balanceOf(msg.sender, 0) == uint256(0), "ERC1155: token already minted");
        // Check if message contains required amount
        require(msg.value >= price, "Not enough ether for transaction");
        // Mint the NFT
       _mint(msg.sender, 0, 1, "");
   }
}