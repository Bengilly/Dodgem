using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopPurchaser
{
    void BoughtItem(UI_ShopItem.Item item);
    bool CanBuyItem(int pointsRequired);
}