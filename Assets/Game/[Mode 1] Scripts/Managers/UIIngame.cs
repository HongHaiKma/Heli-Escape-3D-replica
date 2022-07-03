using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIIngame : Singleton2<UIIngame>
{
    public Image img_Flash;
    public Image img_Crosshair;
    public Transform tf_MainCanvas;

    [Title("UI")]
    public GameObject go_TapToPlay;
    public GameObject go_Shop;


    public override void OnEnable()
    {
        img_Crosshair.gameObject.SetActive(true);
        go_TapToPlay.SetActive(true);
        go_Shop.SetActive(true);
        // m_Combo = 0;
        // go_Combo.SetActive(false);
    }

    private void Update()
    {
        // if (m_ComboTime > 0)
        // {
        //     m_ComboTime -= Time.deltaTime;
        // }
    }

    public void PlayGame()
    {
        GameManager.Instance.m_GameLoop = GameLoop.Play;
        go_TapToPlay.SetActive(false);
        go_Shop.SetActive(false);
        LevelController.Instance.PlayGame();
        CamController.Instance.PlayGame();
    }

    public void OpenInventory()
    {
        PopupCaller.OpenPopup(UIID.POPUP_INVENTORY);
    }

    public void ResetLevel()
    {
        // m_Combo = 0;
        // go_Combo.SetActive(false);
    }

    public async UniTask Combo()
    {
        // if (m_ComboTime > 0) m_Combo++;
        // else m_Combo = 1;
        //
        // m_ComboTime = 2f;
        //
        // txt_Combo.text = "X" + m_Combo;
        // go_Combo.SetActive(false);
        // await UniTask.Delay(10);
        // m_AnimUI.SetTrigger("Combo");
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         PopupCaller.OpenPopup(UIID.POPUP_WIN);
    //     }
    //     
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         PopupCaller.OpenPopup(UIID.POPUP_LOSE);
    //     }
    // }
}
