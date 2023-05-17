using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("ShopContainer");
        shopItemTemplate = container.Find("ShopItemTemplate");
        //shopItemTemplate.Find("ItemImageTest").GetComponent<Image>().sprite = Resources.Load<Sprite>(@"/Sprites/caticon");
        shopItemTemplate.gameObject.SetActive(false);

    }

    private void Start()
    {

        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Cat), "Cat", UI_ShopItem.GetPrice(UI_ShopItem.Item.Cat), 0);
        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Chicken), "Chicken", UI_ShopItem.GetPrice(UI_ShopItem.Item.Chicken), 1);
        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Dog), "Dog", UI_ShopItem.GetPrice(UI_ShopItem.Item.Dog), 2);
        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Lion), "Lion", UI_ShopItem.GetPrice(UI_ShopItem.Item.Lion), 3);
        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Penguin), "Penguin", UI_ShopItem.GetPrice(UI_ShopItem.Item.Penguin), 4);

    }

    private void CreateItemButton(UI_ShopItem.Item item, Sprite itemSprite, string itemName, int itemPrice, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = -120f;
        shopItemRectTransform.anchoredPosition = new Vector2(-shopItemHeight * positionIndex, 0);

        shopItemTransform.Find("ItemText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("BuyButton").Find("BuyAmount").GetComponent<TextMeshProUGUI>().SetText(itemPrice.ToString());
        shopItemTransform.Find("ItemImage").GetComponent<Image>().sprite = itemSprite;
        Debug.Log(itemSprite);

        shopItemTransform.Find("BuyButton").Find("BuyAmount").GetComponent<Button>().onClick.AddListener(TryBuyItem(item));

    }

    private UnityAction TryBuyItem(UI_ShopItem.Item item)
    {
        throw new NotImplementedException();
    }
} 
