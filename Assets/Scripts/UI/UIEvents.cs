public static class UIEvents
{
    public static event System.Action<string> ShowPanelRequested;
    public static event System.Action<string> HidePanelRequested;
    public static event System.Action<string, System.Action<object[]>, object[]> UpdateDataRequest;


    public static void ShowPanel(string panelName)
    {
        ShowPanelRequested?.Invoke(panelName);
    }

    public static void HidePanel(string panelName)
    {
        HidePanelRequested?.Invoke(panelName);
    }

    public static void UpdateData(string panelName, System.Action<object[]> callBack,params object [] parameters)
    {
        UpdateDataRequest?.Invoke(panelName, callBack, parameters);
    }
}
