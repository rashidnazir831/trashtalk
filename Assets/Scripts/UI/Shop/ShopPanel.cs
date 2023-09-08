using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using TrashTalk;

public class ShopPanel : UIPanel
{
   public override void Show()
   {
        gameObject.SetActive(true);
   }

   public override void Hide()
   {
        gameObject.SetActive(false);
   }

    public override void UpdateData(System.Action<object[]> callBack, params object[] parameters)
    {
        print(parameters[0]);
    }

    public void OnBuyCoins(int totalCoins, int price, string productID)
    {
        print("Purchasing " + totalCoins + " Coin");
        print("Cost: " + price);
        print("product: " + productID);

        PurchaseThroughInApp(totalCoins, price, productID);
    }

    void PurchaseThroughInApp(int totalCoins, int price, string productID)
    {
        InappManager.instance.PurchaseItem(productID, (payload, signature) => {

            print("In App Success: " + productID);
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            // keyValuePairs.Add("PageNo", 1);
            WebServiceManager.instance.APIRequest(WebServiceManager.instance.purchaseCoinsFunction, Method.POST, null, keyValuePairs, OnSuccess, OnFail, CACHEABLE.NULL, true, null);

        });
    }

    void OnSuccess(JObject resp, long arg2)
    {

    }

    void OnFail(string msg)
    {
        print(msg);
    }

}
