using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [MapTo("account")]
    [Required]
    public string Account { get; set; }

    [MapTo("exp")]
    public int Exp { get; set; }

#nullable enable
    [MapTo("position")]
    public PlayerPosition? Position { get; set; }
#nullable disable

    [MapTo("friends")]
    public IList<PlayerData> Friends { get; }

    [MapTo("pendingFriends")]
    public IList<PendingFreiendInfo> PendingFriends { get; }

    public IList<NFTInfo> NFTs { get; }

    [MapTo("bidAuction")]
    public IList<PlayerBidAuction> BidAuction { get; }

    [MapTo("taskProgress")]
    public IList<PlayerTask> TaskProgress { get; }
}

public partial class PlayerBidAuction : IEmbeddedObject
{
#nullable enable
    [MapTo("auction")]
    public Auction? Auction { get; set; }
#nullable disable

    [MapTo("checkTime")]
    public DateTimeOffset CheckTime { get; set; }
}

public partial class PlayerPosition : IEmbeddedObject
{
    [MapTo("planetID")]
    public int PlanetID { get; set; }

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

public partial class PlayerTask : IEmbeddedObject
{
#nullable enable
    [MapTo("task")]
    public Task? Task { get; set; }
#nullable disable

    [MapTo("progress")]
    public double Progress { get; set; }

    [MapTo("achieved")]
    public bool Achieved { get; set; }
}

public partial class PendingFreiendInfo : IEmbeddedObject
{
#nullable enable
    [MapTo("player")]
    public PlayerData? Player { get; set; }
#nullable disable

    [MapTo("isSender")]
    public bool IsSender { get; set; }
}

public partial class Auction : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

#nullable enable
    [MapTo("owner")]
    public PlayerData? Owner { get; set; }
#nullable disable

#nullable enable
    public NFTInfo? NFT { get; set; }
#nullable disable

    [MapTo("startTime")]
    public DateTimeOffset StartTime { get; set; }

    [MapTo("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [MapTo("startPrice")]
    public int StartPrice { get; set; }

#nullable enable
    [MapTo("bidPlayer")]
    public PlayerData? BidPlayer { get; set; }
#nullable disable

    [MapTo("bidPrice")]
    public int BidPrice { get; set; }
}

public partial class NFTInfo : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

#nullable enable
    [MapTo("owner")]
    public PlayerData? Owner { get; set; }
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

    [MapTo("isShown")]
    public bool IsShown { get; set; }

    [MapTo("isPending")]
    public bool IsPending { get; set; }

    [MapTo("contents")]
    public IList<NFTContent> Contents { get; }
}

public partial class NFTContent : IEmbeddedObject
{
#nullable enable
    [MapTo("color")]
    public RGBColor? Color { get; set; }
#nullable disable

    [MapTo("posX")]
    public double PosX { get; set; }

    [MapTo("posY")]
    public double PosY { get; set; }

    [MapTo("posZ")]
    public double PosZ { get; set; }
}

public partial class RGBColor : IEmbeddedObject
{
    [MapTo("r")]
    public double R { get; set; }

    [MapTo("g")]
    public double G { get; set; }

    [MapTo("b")]
    public double B { get; set; }
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
    public double MaxProgress { get; set; }

    [MapTo("prize")]
    public int Prize { get; set; }

    [MapTo("exp")]
    public int Exp { get; set; }
}
