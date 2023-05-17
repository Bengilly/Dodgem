using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopItem
{
    public static Sprite cat;
    public static Sprite chicken;
    public static Sprite dog;
    public static Sprite lion;
    public static Sprite penguin;

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
                return 5;
            case Item.Chicken:
                return 10;
            case Item.Dog:
                return 15;
            case Item.Lion:
                return 20;
            case Item.Penguin:
                return 25;
        }
    }

    public static Sprite GetSprite(Item item)
    {
        //try
        //{
        //    cat = Resources.Load<Sprite>(@"/Sprites/caticon");
        //    chicken = Resources.Load<Sprite>(@"/Sprites/caticon");
        //    dog = Resources.Load<Sprite>(@"/Sprites/caticon");
        //    lion = Resources.Load<Sprite>(@"/Sprites/caticon");
        //    penguin = Resources.Load<Sprite>(@"/Sprites/caticon");
        //}
        //catch
        //{
        //    Debug.Log("Error loading file"); 
        //}

        switch (item)
        {
            default:
            case Item.Cat:
                return GameAssets.i.cat;
            //return GameAssets.i.cat;
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
