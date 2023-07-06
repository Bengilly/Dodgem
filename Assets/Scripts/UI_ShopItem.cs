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

    //get the price of shop items
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

    //get the sprite image of each item
    public static Sprite GetSprite(Item item)
    {
        switch (item)
        {
            default:
            case Item.Cat:
                return GameAssets.Instance.cat;
            case Item.Chicken:
                return GameAssets.Instance.chicken;
            case Item.Dog:
                return GameAssets.Instance.dog;
            case Item.Lion:
                return GameAssets.Instance.lion;
            case Item.Penguin:
                return GameAssets.Instance.penguin;
        }
    }
}