using Realms;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Auction : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int NFTID { get; set; }
    public int OwnerID { get; set; }
    public int StartPrice { get; set; }
    public DateTimeOffset StartTime { get; set; }
}

public partial class NFTContent : IEmbeddedObject
{
    [Required]
    public string Color { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int PosZ { get; set; }
}

public partial class NFTInfo : IEmbeddedObject
{
    [Required]
    public string Author { get; set; }
    public IList<NFTContent> Contents { get; }
    public DateTimeOffset CreateTime { get; set; }
    public bool IsMinted { get; set; }
    public int NFTID { get; set; }
    [Required]
    public string Name { get; set; }
    public int OwnerID { get; set; }
}

public partial class PlayerData : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public int Id { get; set; }
    [Required]
    public string Email { get; set; }
    public IList<PlayerData> Friends { get; }
    public IList<Auction> LikedAuction { get; }
    public IList<NFTInfo> NFTs { get; }
    [Required]
    public string Password { get; set; }
    public PlayerPosition? Position { get; set; }
    [Required]
    public string UserName { get; set; }
}

public partial class PlayerPosition : IEmbeddedObject
{
    public int PlayerID { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public double PosZ { get; set; }
    public double RotX { get; set; }
    public double RotY { get; set; }
    public double RotZ { get; set; }
}