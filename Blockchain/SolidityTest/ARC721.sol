// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

interface IARC165 {
    function supportsInterface(bytes4 interfaceId) external view returns (bool);
}

interface IARC721 {
    // Event
    event Transfer(
        address indexed from,
        address indexed to,
        uint256 indexed tokenId
    );
    event Approval(
        address indexed owner,
        address indexed approved,
        uint256 indexed tokenId
    );
    event ApprovalForAll(
        address indexed owner,
        address indexed operator,
        bool approved
    );

    // Query
    function balanceOf(address owner) external view returns (uint256 balance); // count all NFTs assigned to an owner

    function ownerOf(uint256 tokenId) external view returns (address owner); // find the owner of an NFT

    // Transfer
    function transferFrom(address from, address to, uint256 tokenId) external; // transfer ownership of an NFT -- THE CALLER IS RESPONSIBLE

    function safeTransferFrom(
        address from,
        address to,
        uint256 tokenId
    ) external; // transfers the ownership of an NFT from one address to another address

    function safeTransferFrom(
        address from,
        address to,
        uint256 tokenId,
        bytes calldata data
    ) external;

    // Approve
    function approve(address to, uint256 tokenId) external; // change or reaffirm the approved address for an NFT

    function setApprovalForAll(address operator, bool _approved) external; // enable or disable approval for a third party ("operator") to manage

    function getApproved(
        uint256 tokenId
    ) external view returns (address operator); // get the approved address for a single NFT

    function isApprovedForAll(
        address owner,
        address operator
    ) external view returns (bool); // query if an address is an authorized operator for another address

    // Mint
    function mint(address to, uint256 tokenId) external;

    function safeMint(address to, uint256 tokenId) external;

    function safeMint(address to, uint256 tokenId, bytes memory data) external;

    // Burn
    function burn(uint256 tokenId) external;
}

interface IARC721Receiver {
    function onARC721Received(
        address operator,
        address from,
        uint tokenId,
        bytes calldata data
    ) external returns (bytes4);
}

interface IARC721Metadata {
    function name() external view returns (string memory);

    function symbol() external view returns (string memory);

    function tokenURI(uint256 tokenId) external view returns (string memory);
}

contract ARC721 is IARC721, IARC721Metadata, IARC165 {
    mapping(address => uint256) _balances;
    mapping(uint256 => address) _owners;
    mapping(uint256 => address) _tokenApprovals;
    mapping(address => mapping(address => bool)) _operatorApprovals;
    string _name;
    string _symbol;
    mapping(uint256 => string) _tokenURIs;

    constructor(string memory name_, string memory symbol_) {
        _name = name_;
        _symbol = symbol_;
    }

    function balanceOf(address owner) public view returns (uint256) {
        require(owner != address(0), "ERROR: address 0 can't be owner");
        return _balances[owner];
    }

    function ownerOf(uint256 tokenId) public view returns (address) {
        address owner = _owners[tokenId];
        require(owner != address(0), "ERROR: tokenId is not a valid ID");
        return owner;
    }

    function approve(address to, uint256 tokenId) public {
        address owner = _owners[tokenId];
        require(owner != to, "ERROR: owner = to");
        // caller is msg.sender
        require(
            owner == msg.sender || isApprovedForAll(owner, msg.sender),
            "ERROR: caller is not token owner or be approved for all"
        );
        _tokenApprovals[tokenId] = to;
        emit Approval(owner, to, tokenId); // trigger event
    }

    function setApprovalForAll(address operator, bool _approved) public {
        require(msg.sender != operator, "ERROR: owner = operator");
        _operatorApprovals[msg.sender][operator] = _approved;
        emit ApprovalForAll(msg.sender, operator, _approved);
    }

    function getApproved(uint256 tokenId) public view returns (address) {
        address owner = _owners[tokenId];
        require(owner != address(0), "ERROR: token is not minted yet or burnt");
        return _tokenApprovals[tokenId];
    }

    function isApprovedForAll(
        address owner,
        address operator
    ) public view returns (bool) {
        return _operatorApprovals[owner][operator];
    }

    function transferFrom(address from, address to, uint256 tokenId) public {
        _transfer(from, to, tokenId);
    }

    function _transfer(address from, address to, uint256 tokenId) internal {
        address owner = _owners[tokenId];
        require(owner == from, "ERROR: owner is not the from address");
        require(
            msg.sender == owner ||
                isApprovedForAll(owner, msg.sender) ||
                getApproved(tokenId) == msg.sender,
            "ERROR: caller doesn't have permission to transfer"
        );
        delete _tokenApprovals[tokenId];
        _balances[to] += 1;
        _balances[from] -= 1;
        _owners[tokenId] = to;
        emit Transfer(from, to, tokenId);
    }

    function safeTransferFrom(
        address from,
        address to,
        uint256 tokenId,
        bytes calldata data
    ) public {
        _safeTransfer(from, to, tokenId, data);
    }

    function _safeTransfer(
        address from,
        address to,
        uint256 tokenId,
        bytes memory data
    ) internal {
        _transfer(from, to, tokenId);
        require(
            _checkOnARC721Received(from, to, tokenId, data),
            "ERROR: ARC721Receiver is not implemented"
        );
    }

    function safeTransferFrom(
        address from,
        address to,
        uint256 tokenId
    ) public {
        _safeTransfer(from, to, tokenId, "");
    }

    function mint(address to, uint256 tokenId) public {
        require(to != address(0), "ERROR: mint to address 0");
        address owner = _owners[tokenId];
        require(owner == address(0), "ERROR: tokenId has already existed");
        _balances[to] += 1;
        _owners[tokenId] = to;
        emit Transfer(address(0), to, tokenId);
    }

    function safeMint(address to, uint256 tokenId, bytes memory data) public {
        mint(to, tokenId);
        require(
            _checkOnARC721Received(address(0), to, tokenId, data),
            "ERROR: ARC721Receiver is not implemented"
        );
    }

    function safeMint(address to, uint256 tokenId) public {
        safeMint(to, tokenId, "");
    }

    function burn(uint256 tokenId) public {
        address owner = _owners[tokenId];
        require(msg.sender == owner, "ERROR: only owner can burn");
        _balances[owner] -= 1;
        delete owner;
        delete _tokenApprovals[tokenId];
        emit Transfer(owner, address(0), tokenId);
    }

    function name() public view returns (string memory) {
        return _name;
    }

    function symbol() public view returns (string memory) {
        return _symbol;
    }

    function tokenURI(uint256 tokenId) public view returns (string memory) {
        address owner = _owners[tokenId];
        require(owner != address(0), "ERROR: tokenId is not valid");
        return _tokenURIs[tokenId];
    }

    function setTokenURI(uint256 tokenId, string memory URI) public {
        address owner = _owners[tokenId];
        require(owner != address(0), "ERROR: tokenId is not valid");
        _tokenURIs[tokenId] = URI;
    }

    function supportsInterface(bytes4 interfaceId) public pure returns (bool) {
        return
            interfaceId == type(IARC721).interfaceId ||
            interfaceId == type(IARC721Metadata).interfaceId ||
            interfaceId == type(IARC165).interfaceId;
    }

    function _checkOnARC721Received(
        address from,
        address to,
        uint256 tokenId,
        bytes memory data
    ) private returns (bool) {
        if (to.code.length > 0 /* to is a contract*/) {
            try
                IARC721Receiver(to).onARC721Received(
                    msg.sender,
                    from,
                    tokenId,
                    data
                )
            returns (bytes4 retval) {
                return retval == IARC721Receiver.onARC721Received.selector;
            } catch (bytes memory reason) {
                if (reason.length == 0) {
                    revert(
                        "ARC721: transfer to non ARC721Receiver implementer"
                    );
                } else {
                    /// @solidity memory-safe-assembly
                    assembly {
                        revert(add(32, reason), mload(reason))
                    }
                }
            }
        } else {
            return true;
        }
    }
}
