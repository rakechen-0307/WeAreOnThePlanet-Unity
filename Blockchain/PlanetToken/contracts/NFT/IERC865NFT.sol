// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

/**
 * @title ERC865 Interface
 * @dev see https://github.com/ethereum/EIPs/issues/865
 *
 */

interface IERC865NFT {
    event TransferPreSigned(
        address indexed from,
        address indexed to,
        address indexed delegate,
        uint256 tokenId
    );
    event ApprovalPreSigned(
        address indexed from,
        address indexed to,
        address indexed delegate,
        uint256 tokenId
    );

    function transferPreSigned(
        address _from,
        address _to,
        uint256 _tokenId,
        uint256 _nonce
    ) external;

    function approvePreSigned(
        address _from,
        address _spender,
        uint256 _tokenId,
        uint256 _nonce
    ) external;
}
