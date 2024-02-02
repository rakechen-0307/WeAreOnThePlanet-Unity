// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Pausable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Burnable.sol";
import "./IERC865NFT.sol";
import "./SafeMath.sol";

contract WeAreonThePlanetNFT is
    ERC721,
    ERC721Enumerable,
    ERC721URIStorage,
    ERC721Pausable,
    Ownable,
    ERC721Burnable,
    IERC865NFT
{
    using SafeMath for uint256;

    /* hashed tx of transfers performed */
    mapping(bytes32 => bool) hashedTxs;

    uint256 private _nextTokenId;

    constructor(
        address initialOwner
    ) ERC721("WeAreonThePlanet-NFT", "WATP-N") Ownable(initialOwner) {}

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function safeMint(address to, string memory uri) public onlyOwner {
        uint256 tokenId = _nextTokenId++;
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, uri);
    }

    // The following functions are overrides required by Solidity.

    function _update(
        address to,
        uint256 tokenId,
        address auth
    )
        internal
        override(ERC721, ERC721Enumerable, ERC721Pausable)
        returns (address)
    {
        return super._update(to, tokenId, auth);
    }

    function _increaseBalance(
        address account,
        uint128 value
    ) internal override(ERC721, ERC721Enumerable) {
        super._increaseBalance(account, value);
    }

    function tokenURI(
        uint256 tokenId
    ) public view override(ERC721, ERC721URIStorage) returns (string memory) {
        return super.tokenURI(tokenId);
    }

    function supportsInterface(
        bytes4 interfaceId
    )
        public
        view
        override(ERC721, ERC721Enumerable, ERC721URIStorage)
        returns (bool)
    {
        return super.supportsInterface(interfaceId);
    }

    // The following are ERC865 functions

    /**
     * @dev Submit a presigned transfer
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _to address The address which you want to transfer to.
     * @param _tokenId uint256 The tokenId of token to be transferred.
     * @param _nonce uint256 Presigned transaction number.
     */
    function transferPreSigned(
        address _from,
        address _to,
        uint256 _tokenId,
        uint256 _nonce
    ) public {
        require(_to != address(0), "Invalid _to address");

        bytes32 hashedParams = getTransferPreSignedHash(
            address(this),
            _to,
            _tokenId,
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
        _safeTransfer(from, _to, _tokenId);

        emit TransferPreSigned(from, _to, msg.sender, _tokenId);
    }

    /**
     * @dev Submit a presigned approval
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _from address The owner address.
     * @param _spender address The address which will spend the funds.
     * @param _tokenId uint256 The tokenId of token to be transferred.
     * @param _nonce uint256 Presigned transaction number.
     */
    function approvePreSigned(
        address _from,
        address _spender,
        uint256 _tokenId,
        uint256 _nonce
    ) public {
        require(_spender != address(0), "Invalid _spender address");

        bytes32 hashedParams = getApprovePreSignedHash(
            address(this),
            _spender,
            _tokenId,
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
        approve(_spender, _tokenId);

        emit ApprovalPreSigned(from, _spender, msg.sender, _tokenId);
    }

    /**
     * @dev Hash (keccak256) of the payload used by transferPreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token.
     * @param _to address The address which you want to transfer to.
     * @param _tokenId uint256 The tokenId of token to be transferred.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getTransferPreSignedHash(
        address _token,
        address _to,
        uint256 _tokenId,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "0d98dcb1": getTransferPreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x0d98dcb1),
                    _token,
                    _to,
                    _tokenId,
                    _nonce
                )
            );
    }

    /**
     * @dev Hash (keccak256) of the payload used by approvePreSigned
     * @notice fee will be given to sender if it's a smart contract make sure it can accept funds
     * @param _token address The address of the token
     * @param _spender address The address which will spend the funds.
     * @param _tokenId uint256 The tokenId of token to be transferred.
     * @param _nonce uint256 Presigned transaction number.
     */
    function getApprovePreSignedHash(
        address _token,
        address _spender,
        uint256 _tokenId,
        uint256 _nonce
    ) public pure returns (bytes32) {
        /* "79250dcf": getApprovePreSignedHash(address,address,uint256,uint256,uint256) */
        return
            keccak256(
                abi.encodePacked(
                    bytes4(0x79250dcf),
                    _token,
                    _spender,
                    _tokenId,
                    _nonce
                )
            );
    }
}
