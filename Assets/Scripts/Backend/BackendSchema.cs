using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Auction : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

    [MapTo("ownerID")]
#nullable enable
    public PlayerData? OwnerID { get; set; }

    public NFTInfo? NFTID { get; set; }
#nullable disable

    [MapTo("startTime")]
    public DateTimeOffset StartTime { get; set; }

    [MapTo("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [MapTo("startPrice")]
    public int StartPrice { get; set; }

    [MapTo("bidPlayerID")]
#nullable enable
    public PlayerData? BidPlayerID { get; set; }
#nullable disable

    [MapTo("bidPrice")]
    public int BidPrice { get; set; }
}

public partial class NFTContent : IEmbeddedObject
{
    [MapTo("color")]
    [Required]
    public string Color { get; set; }

    [MapTo("posX")]
    public int PosX { get; set; }

    [MapTo("posY")]
    public int PosY { get; set; }

    [MapTo("posZ")]
    public int PosZ { get; set; }
}

public partial class NFTInfo : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

    [MapTo("ownerID")]
#nullable enable
    public PlayerData? OwnerID { get; set; }
#nullable disable

    [MapTo("name")]
    [Required]
    public string Name { get; set; }

    [MapTo("author")]
    [Required]
    public string Author { get; set; }

    [MapTo("createTime")]
    public DateTimeOffset CreateTime { get; set; }

    [MapTo("isMinted")]
    public bool IsMinted { get; set; }

    [MapTo("contents")]
    public IList<NFTContent> Contents { get; }
}

public partial class PlayerData : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

    [MapTo("email")]
    [Required]
    public string Email { get; set; }

    [MapTo("username")]
    [Required]
    public string Username { get; set; }

    [MapTo("password")]
    [Required]
    public string Password { get; set; }

    [MapTo("exp")]
    public int Exp { get; set; }

    [MapTo("position")]
#nullable enable
    public PlayerPosition? Position { get; set; }
#nullable disable

    [MapTo("friends")]
    public IList<PlayerData> Friends { get; }

    public IList<NFTInfo> NFTs { get; }

    [MapTo("taskProgress")]
    public IList<PlayerTask> TaskProgress { get; }

    [MapTo("bidAuction")]
    public IList<PlayerBidAuction> BidAuction { get; }
}

public partial class PlayerBidAuction : IEmbeddedObject
{
    [MapTo("auctionID")]
#nullable enable
    public Auction? AuctionID { get; set; }
#nullable disable

    [MapTo("checkTime")]
    public DateTimeOffset CheckTime { get; set; }
}

public partial class PlayerTask : IEmbeddedObject
{
    [MapTo("taskID")]
#nullable enable
    public Task? TaskID { get; set; }
#nullable disable

    [MapTo("progress")]
    public int Progress { get; set; }

    [MapTo("achieved")]
    public bool Achieved { get; set; }
}

public partial class PlayerPosition : IEmbeddedObject
{
    [MapTo("posX")]
    public double PosX { get; set; }

    [MapTo("posY")]
    public double PosY { get; set; }

    [MapTo("posZ")]
    public double PosZ { get; set; }

    [MapTo("rotX")]
    public double RotX { get; set; }

    [MapTo("rotY")]
    public double RotY { get; set; }

    [MapTo("rotZ")]
    public double RotZ { get; set; }
}

public partial class Task : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

    [MapTo("name")]
    [Required]
    public string Name { get; set; }

    [MapTo("description")]
    [Required]
    public string Description { get; set; }

    [MapTo("maxProgress")]
    public int MaxProgress { get; set; }

    [MapTo("prize")]
    public int Prize { get; set; }
}