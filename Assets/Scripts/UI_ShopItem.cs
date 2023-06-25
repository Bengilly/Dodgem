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
                return 1;
            case Item.Chicken:
                return 2;
            case Item.Dog:
                return 3;
            case Item.Lion:
                return 4;
            case Item.Penguin:
                return 5;
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