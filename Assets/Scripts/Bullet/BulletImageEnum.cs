using System;
using System.Reflection;

public enum BulletImageEnum
{
    [BulletImage(BulletColliderEnum.Circle)]
    Red,

    [BulletImage(BulletColliderEnum.Circle)]
    Orange,

    [BulletImage(BulletColliderEnum.Circle)]
    Yellow,

    [BulletImage(BulletColliderEnum.Circle)]
    Green,

    [BulletImage(BulletColliderEnum.Circle)]
    LightBlue,

    [BulletImage(BulletColliderEnum.Circle)]
    Blue,

    [BulletImage(BulletColliderEnum.Circle)]
    Purple,

    [BulletImage(BulletColliderEnum.Circle)]
    Black,

    [BulletImage(BulletColliderEnum.Circle)]
    White,

    [BulletImage(BulletColliderEnum.Capsule)]
    Red2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Orange2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Yellow2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Green2,

    [BulletImage(BulletColliderEnum.Capsule)]
    LightBlue2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Blue2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Purple2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Black2,

    [BulletImage(BulletColliderEnum.Capsule)]
    White2,


    [BulletImage(BulletColliderEnum.Circle)]
    Bullet1,


    [BulletImage(BulletColliderEnum.Circle)]
    Bullet2,


    [BulletImage(BulletColliderEnum.Circle)]
    Bullet3,


    [BulletImage(BulletColliderEnum.Circle)]
    Bullet4,


    [BulletImage(BulletColliderEnum.Circle)]
    Bullet5,

    [BulletImage(BulletColliderEnum.Circle)]
    Bullet6,

    [BulletImage(BulletColliderEnum.Capsule)]
    PlayerShot1,

    [BulletImage(BulletColliderEnum.Capsule)]
    PlayerShot2,

    [BulletImage(BulletColliderEnum.Capsule)]
    Spider,

    [BulletImage(BulletColliderEnum.Capsule2)]
    Snake1,

    [BulletImage(BulletColliderEnum.Circle2)]
    Snake2,

    [BulletImage(BulletColliderEnum.Circle2)]
    Bomb
}

public class BulletImageAttribute : Attribute
{
    public BulletColliderEnum Collider { get; set; }
    public BulletImageAttribute(BulletColliderEnum _collider)
    {
        Collider = _collider;
    }
}

public static class EnumToBulletImage
{
    public static BulletColliderEnum GetColluderEnum(this BulletImageEnum Value)
    {
        Type EnumType = Value.GetType();
        FieldInfo fieldInfo = EnumType.GetField(Value.ToString());
        BulletImageAttribute[] Attributes = fieldInfo.GetCustomAttributes(typeof(BulletImageAttribute), false) as BulletImageAttribute[];
        return Attributes[0].Collider;
    }
}
