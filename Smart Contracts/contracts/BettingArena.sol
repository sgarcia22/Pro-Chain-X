// SPDX-License-Identifier: MIT
pragma solidity >=0.7.0 <0.9.0;

import "@openzeppelin/contracts/access/Ownable.sol";

contract BettingArena is Ownable{
    //constants 
    uint housePercentage = 1; 
    uint winnerPercentage = 9; 

    // Could also use structs instead of mappings.
    // (player making the bet) => (how much they are betting)
    mapping(address => uint256) internal userToBets;
    // (player making the bet) => (player the better is betting on)
    mapping(address => address) internal betterToPlayer;
    // betters
    address[] public bettersAddresses;
    uint256 totalValueBet = 0;

    function _resetBets() private {
        totalValueBet = 0;
        for (uint i = 0; i < bettersAddresses.length; i++) {
            userToBets[bettersAddresses[i]] = 0;
            betterToPlayer[bettersAddresses[i]] = address(0);
            delete bettersAddresses;
        }
    }

    // TODO: make sure user hasn't already bet on match, or have function to increase, decrease, or remove bet.
    function placeBet(address bettingOn, uint256 amountToBet) external payable {
        require(userToBets[msg.sender] == 0, "Already placed bet.");
        require(msg.value > 0, "Not enough money for transaction.");
        userToBets[msg.sender] = amountToBet;
        betterToPlayer[msg.sender] = bettingOn;
        bettersAddresses.push(bettingOn);
        // Add to total value bet.
        totalValueBet += amountToBet;
    }

    function payout(address winnerAddress) external {
        uint256 totalPayoutLeft = totalValueBet;

        // Give a cut to the winner.
        _payOutWinnings(payable(winnerAddress), totalValueBet * (winnerPercentage / 100));
        totalPayoutLeft -= (totalValueBet * (winnerPercentage / 100));

        // Take house cut.
        _payOutWinnings(payable(owner()), totalValueBet * (housePercentage / 100));
        totalPayoutLeft -= (totalValueBet * (housePercentage / 100));

        // Pay out winnings to betters.
        for (uint i = 0; i < bettersAddresses.length; i++) {
            address betterAddress = bettersAddresses[i];
            uint256 amountWon = ((userToBets[betterAddress]) / totalPayoutLeft);
            totalPayoutLeft -= amountWon;
            _payOutWinnings(payable(betterAddress), amountWon);
        }

        // Reset all of the bets.
        _resetBets();
    }

    /// @notice pays out winnings to a user 
    /// @param _user the user to whom to pay out 
    /// @param _amount the amount to pay out 
    function _payOutWinnings(address payable _user, uint _amount) private {
        _user.transfer(_amount);
    }
}