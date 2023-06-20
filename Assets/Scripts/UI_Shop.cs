using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

public class UI_Shop : MonoBehaviour, IPointerExitHandler//, IPointerEnterHandler,
{
    private Transform container;
    private Transform shopItemTemplate;
    private IShopPurchaser shopPurchaser;
    private Button buyButton;

    private void Awake()
    {
        container = transform.Find("ShopContainer");
        shopItemTemplate = container.Find("ShopItemTemplate");

        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        //set player as shopPurchaser to buy characters
        shopPurchaser = GameObject.Find("Player").GetComponent<PlayerController>();

        CreateItemButton(UI_ShopItem.Item.Cat, UI_ShopItem.GetSprite(UI_ShopItem.Item.Cat), "Cat", UI_ShopItem.GetPrice(UI_ShopItem.Item.Cat), 0);
        CreateItemButton(UI_ShopItem.Item.Chicken, UI_ShopItem.GetSprite(UI_ShopItem.Item.Chicken), "Chicken", UI_ShopItem.GetPrice(UI_ShopItem.Item.Chicken), 1);
        CreateItemButton(UI_ShopItem.Item.Dog, UI_ShopItem.GetSprite(UI_ShopItem.Item.Dog), "Dog", UI_ShopItem.GetPrice(UI_ShopItem.Item.Dog), 2);
        CreateItemButton(UI_ShopItem.Item.Lion, UI_ShopItem.GetSprite(UI_ShopItem.Item.Lion), "Lion", UI_ShopItem.GetPrice(UI_ShopItem.Item.Lion), 3);
        CreateItemButton(UI_ShopItem.Item.Penguin, UI_ShopItem.GetSprite(UI_ShopItem.Item.Penguin), "Penguin", UI_ShopItem.GetPrice(UI_ShopItem.Item.Penguin), 4);

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

        buyButton = shopItemTransform.Find("BuyButton").GetComponent<Button>();
        buyButton.onClick.AddListener(() => { TryBuyItem(item); });
        //buyButton.OnPointerEnter();
    }

    private void TryBuyItem(UI_ShopItem.Item item)
    {
        if(shopPurchaser.CanBuyItem(UI_ShopItem.GetPrice(item)))
        {
            shopPurchaser.BoughtItem(item);
        }
        else
        {
            UI_TooltipManager._instance.ShowTooltip();
            Debug.Log("Load tooltip");
        }
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    UI_TooltipManager._instance.ShowTooltip();
    //    Debug.Log("Load tooltip");
    //}

    public void OnPointerExit(PointerEventData eventData)
    {
        //only hide tooltip if it is currently enabled
        if(UI_TooltipManager._instance.isActiveAndEnabled)
        {
            ((IPointerExitHandler)buyButton).OnPointerExit(eventData);
            UI_TooltipManager._instance.HideTooltip();
            Debug.Log("Hide tooltip");
        }

    }
} 
