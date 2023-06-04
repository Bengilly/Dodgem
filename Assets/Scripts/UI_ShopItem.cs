using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopItem
{
    public enum Item
    {
        Cat,
        Chicken,
        Dog,
        Lion,
        Penguin,
    }

    public static int GetPrice(Item item)
    {
        switch (item)
        {
            default:
            case Item.Cat:
                return 0;
            case Item.Chicken:
                return 0;
            case Item.Dog:
                return 0;
            case Item.Lion:
                return 0;
            case Item.Penguin:
                return 0;
        }
    }

    public static Sprite GetSprite(Item item)
    {
        switch (item)
        {
            default:
            case Item.Cat:
                return GameAssets.i.cat;
            case Item.Chicken:
                return GameAssets.i.chicken;
            case Item.Dog:
                return GameAssets.i.dog;
            case Item.Lion:
                return GameAssets.i.lion;
            case Item.Penguin:
                return GameAssets.i.penguin;
        }
    }
}
