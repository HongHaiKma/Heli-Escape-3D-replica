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
        Helper.DebugLog("Open POPUPPPPPPPPPP");
        UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

        GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    }
}

public enum UIID
{
    POPUP_PAUSE = 0,
    POPUP_WIN = 1,
    POPUP_LOSE = 2,
    POPUP_INVENTORY = 3,
}