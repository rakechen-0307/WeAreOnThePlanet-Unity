// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/token/ERC20/extensions/ERC20Burnable.sol";
import "@openzeppelin/contracts/token/ERC20/extensions/ERC20Pausable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "./IERC865.sol";
import "./SafeMath.sol";

contract WeAreonThePlanetToken is
    ERC20,
    ERC20Burnable,
    ERC20Pausable,
    Ownable,
    IERC865
{
    using SafeMath for uint256;

    /* hashed tx of transfers performed */
    mapping(bytes32 => bool) hashedTxs;

    constructor(
        address initialOwner
    ) ERC20("WeAreonThePlanet-Token", "WATP-T") Ownable(initialOwner) {
        _mint(msg.sender, 100000000000000000000 * 10 ** decimals());
    }

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function mint(address to, uint256 amount) public onlyOwner {
        _mint(to, amount);
    }

    // The following functions are overrides required by Solidity.

    function _update(
        address from,
        address to,
        uint256 value
    ) internal override(ERC20, ERC20Pausable) {
        super._update(from, to, value);
    }

    // The following are ERC865 functions

    /**
     * @dev Submit a presigned transfer
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _to address The address which you want to transfer to.
     * @param _value uint256 The amount of tokens to be transferred.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function transferPreSigned(
        address _from,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public {
        require(_to != address(0), "Invalid _to address");

        bytes32 hashedParams = getTransferPreSignedHash(
            address(this),
            _to,
            _value,
            _fee,
            _nonce
        );
        address from = _from;
        require(from != address(0), "Invalid from address recovered");
        bytes32 hashedTx = keccak256(abi.encodePacked(from, hashedParams));
        require(
            hashedTxs[hashedTx] == false,
            "Transaction hash was already used"
        );
        hashedTxs[hashedTx] = true;
        _transfer(from, _to, _value);
        _transfer(from, msg.sender, _fee);

        emit TransferPreSigned(from, _to, msg.sender, _value, _fee);
    }

    /**
     * @dev Submit a presigned approval
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _spender address The address which will spend the funds.
     * @param _value uint256 The amount of tokens to allow.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function approvePreSigned(
        address _from,
        address _spender,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public {
        require(_spender != address(0), "Invalid _spender address");

        bytes32 hashedParams = getApprovePreSignedHash(
            address(this),
            _spender,
            _value,
            _fee,
            _nonce
        );
        address from = _from;
        require(from != address(0), "Invalid from address recovered");
        bytes32 hashedTx = keccak256(abi.encodePacked(from, hashedParams));
        require(
            hashedTxs[hashedTx] == false,
            "Transaction hash was already used"
        );
        hashedTxs[hashedTx] = true;
        _approve(from, _spender, _value);
        _transfer(from, msg.sender, _fee);

        emit ApprovalPreSigned(from, _spender, msg.sender, _value, _fee);
    }

    /**
     * @dev Increase the amount of tokens that an owner allowed to a spender.
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _spender address The address which will spend the funds.
     * @param _addedValue uint256 The amount of tokens to increase the allowance by.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function increaseAllowancePreSigned(
        address _from,
        address _spender,
        uint256 _addedValue,
        uint256 _fee,
        uint256 _nonce
    ) public {
        require(_spender != address(0), "Invalid _spender address");

        bytes32 hashedParams = getIncreaseAllowancePreSignedHash(
            address(this),
            _spender,
            _addedValue,
            _fee,
            _nonce
        );
        address from = _from;
        require(from != address(0), "Invalid from address recovered");
        bytes32 hashedTx = keccak256(abi.encodePacked(from, hashedParams));
        require(
            hashedTxs[hashedTx] == false,
            "Transaction hash was already used"
        );
        hashedTxs[hashedTx] = true;
        _approve(from, _spender, allowance(from, _spender).add(_addedValue));
        _transfer(from, msg.sender, _fee);

        emit ApprovalPreSigned(
            from,
            _spender,
            msg.sender,
            allowance(from, _spender),
            _fee
        );
    }

    /**
     * @dev Decrease the amount of tokens that an owner allowed to a spender.
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _spender address The address which will spend the funds.
     * @param _subtractedValue uint256 The amount of tokens to decrease the allowance by.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function decreaseAllowancePreSigned(
        address _from,
        address _spender,
        uint256 _subtractedValue,
        uint256 _fee,
        uint256 _nonce
    ) public {
        require(_spender != address(0), "Invalid _spender address");

        bytes32 hashedParams = getDecreaseAllowancePreSignedHash(
            address(this),
            _spender,
            _subtractedValue,
            _fee,
            _nonce
        );
        address from = _from;
        require(from != address(0), "Invalid from address recovered");
        bytes32 hashedTx = keccak256(abi.encodePacked(from, hashedParams));
        require(
            hashedTxs[hashedTx] == false,
            "Transaction hash was already used"
        );
        // if substractedValue is greater than allowance will fail as allowance is uint256
        hashedTxs[hashedTx] = true;
        _approve(
            from,
            _spender,
            allowance(from, _spender).sub(_subtractedValue)
        );
        _transfer(from, msg.sender, _fee);

        emit ApprovalPreSigned(
            from,
            _spender,
            msg.sender,
            allowance(from, _spender),
            _fee
        );
    }

    /**
     * @dev Transfer tokens from one address to another
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _spender Address the address of the spender.
     * @param _from address The address which you want to send tokens from.
     * @param _to address The address which you want to transfer to.
     * @param _value uint256 The amount of tokens to be transferred.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the spender.
     * @param _nonce uint256 Presigned transaction number.
     */
    function transferFromPreSigned(
        address _spender,
        address _from,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public {
        require(_to != address(0), "Invalid _to address");

        bytes32 hashedParams = getTransferFromPreSignedHash(
            address(this),
            _from,
            _to,
            _value,
            _fee,
            _nonce
        );

        address spender = _spender;
        require(spender != address(0), "Invalid spender address recovered");
        bytes32 hashedTx = keccak256(abi.encodePacked(spender, hashedParams));
        require(
            hashedTxs[hashedTx] == false,
            "Transaction hash was already used"
        );
        hashedTxs[hashedTx] = true;
        _transfer(_from, _to, _value);
        _approve(_from, spender, allowance(_from, spender).sub(_value));
        _transfer(spender, msg.sender, _fee);

        emit TransferPreSigned(_from, _to, msg.sender, _value, _fee);
    }

    /**
     * @dev Hash (keccak256) of the payload used by transferPreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token.
     * @param _to address The address which you want to transfer to.
     * @param _value uint256 The amount of tokens to be transferred.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getTransferPreSignedHash(
        address _token,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "0d98dcb1": getTransferPreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x0d98dcb1),
                    _token,
                    _to,
                    _value,
                    _fee,
                    _nonce
                )
            );
    }

    /**
     * @dev Hash (keccak256) of the payload used by approvePreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token
     * @param _spender address The address which will spend the funds.
     * @param _value uint256 The amount of tokens to allow.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getApprovePreSignedHash(
        address _token,
        address _spender,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "79250dcf": getApprovePreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x79250dcf),
                    _token,
                    _spender,
                    _value,
                    _fee,
                    _nonce
                )
            );
    }

    /**
     * @dev Hash (keccak256) of the payload used by increaseAllowancePreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token
     * @param _spender address The address which will spend the funds.
     * @param _addedValue uint256 The amount of tokens to increase the allowance by.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getIncreaseAllowancePreSignedHash(
        address _token,
        address _spender,
        uint256 _addedValue,
        uint256 _fee,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "138e8da1": getIncreaseAllowancePreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x138e8da1),
                    _token,
                    _spender,
                    _addedValue,
                    _fee,
                    _nonce
                )
            );
    }

    /**
     * @dev Hash (keccak256) of the payload used by decreaseAllowancePreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token
     * @param _spender address The address which will spend the funds.
     * @param _subtractedValue uint256 The amount of tokens to decrease the allowance by.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the owner.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getDecreaseAllowancePreSignedHash(
        address _token,
        address _spender,
        uint256 _subtractedValue,
        uint256 _fee,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "5229c56f": getDecreaseAllowancePreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x5229c56f),
                    _token,
                    _spender,
                    _subtractedValue,
                    _fee,
                    _nonce
                )
            );
    }

    /**
     * @dev Hash (keccak256) of the payload used by transferFromPreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token
     * @param _from address The address which you want to send tokens from.
     * @param _to address The address which you want to transfer to.
     * @param _value uint256 The amount of tokens to be transferred.
     * @param _fee uint256 The amount of tokens paid to msg.sender, by the spender.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getTransferFromPreSignedHash(
        address _token,
        address _from,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "a70c41b4": getTransferFromPreSignedHash(address,address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0xa70c41b4),
                    _token,
                    _from,
                    _to,
                    _value,
                    _fee,
                    _nonce
                )
            );
    }
}