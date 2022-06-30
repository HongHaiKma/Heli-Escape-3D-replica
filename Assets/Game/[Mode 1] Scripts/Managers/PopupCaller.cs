public class PopupCaller
{
    public static void OpenWinPopup(bool _isClose = false, bool _isSetup = false)
    {
        PopupWin popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN) as PopupWin;

        GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    }

    public static void OpenLosePopup(bool _isClose = false, bool _isSetup = false)
    {
        PopupLose popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN) as PopupLose;

        GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    }
    
    public static void OpenPopup(UIID _uiid, bool _isClose = false, bool _isSetup = false)
    {
        UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

        GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    }
    
    public static UICanvas GetPopup(UIID _uiid)
    {
        UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

        return popup;
    }
    
    public static PopupInventory GetPopupInventory()
    {
        PopupInventory popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_INVENTORY) as PopupInventory;

        return popup;
    }
}

public enum UIID
{
    POPUP_PAUSE = 0,
    POPUP_WIN = 1,
    POPUP_LOSE = 2,
    POPUP_INVENTORY = 3,
}