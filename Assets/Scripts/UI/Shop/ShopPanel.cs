using UnityEngine;

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
}
