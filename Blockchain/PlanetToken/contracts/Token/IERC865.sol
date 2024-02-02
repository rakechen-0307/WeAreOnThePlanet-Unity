// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

/**
 * @title ERC865 Interface
 * @dev see https://github.com/ethereum/EIPs/issues/865
 *
 */

interface IERC865 {
    event TransferPreSigned(
        address indexed from,
        address indexed to,
        address indexed delegate,
        uint256 amount,
        uint256 fee
    );
    event ApprovalPreSigned(
        address indexed from,
        address indexed to,
        address indexed delegate,
        uint256 amount,
        uint256 fee
    );

    function transferPreSigned(
        address _from,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) external;

    function approvePreSigned(
        address _from,
        address _spender,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) external;

    function increaseAllowancePreSigned(
        address _from,
        address _spender,
        uint256 _addedValue,
        uint256 _fee,
        uint256 _nonce
    ) external;

    function decreaseAllowancePreSigned(
        address _from,
        address _spender,
        uint256 _subtractedValue,
        uint256 _fee,
        uint256 _nonce
    ) external;

    function transferFromPreSigned(
        address _spender,
        address _from,
        address _to,
        uint256 _value,
        uint256 _fee,
        uint256 _nonce
    ) external;
}
