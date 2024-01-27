using Realms;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Auction : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

#nullable enable
    public NFTInfo? NFTID { get; set; }
#nullable disable

    [MapTo("endTime")]
    public DateTimeOffset? EndTime { get; set; }

    [MapTo("ownerID")]
#nullable enable
    public PlayerData? OwnerID { get; set; }
#nullable disable

    [MapTo("startPrice")]
    public int StartPrice { get; set; }

    [MapTo("startTime")]
    public DateTimeOffset StartTime { get; set; }
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

    [MapTo("author")]
    [Required]
    public string Author { get; set; }

    [MapTo("contents")]
    public IList<NFTContent> Contents { get; }

    [MapTo("createTime")]
    public DateTimeOffset CreateTime { get; set; }

    [MapTo("isMinted")]
    public bool IsMinted { get; set; }

    [MapTo("name")]
    [Required]
    public string Name { get; set; }

    [MapTo("ownerID")]
#nullable enable
    public PlayerData? OwnerID { get; set; }
#nullable disable
}

public partial class PlayerData : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }

    public IList<NFTInfo> NFTs { get; }

    [MapTo("email")]
    [Required]
    public string Email { get; set; }

    [MapTo("friends")]
    public IList<PlayerData> Friends { get; }

    [MapTo("isOnline")]
    public bool IsOnline { get; set; }

    [MapTo("likedAuction")]
    public IList<Auction> LikedAuction { get; }

    [MapTo("password")]
    [Required]
    public string Password { get; set; }

    [MapTo("position")]
#nullable enable
    public PlayerPosition? Position { get; set; }
#nullable disable

    [MapTo("taskProgress")]
    public IList<double> TaskProgress { get; }

    [MapTo("username")]
    [Required]
    public string Username { get; set; }
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

    [MapTo("description")]
    [Required]
    public string Description { get; set; }

    [MapTo("prize")]
    public int Prize { get; set; }
}