using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_AccountPassPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // AccountPassScrollContentObject :  UI_AccountPassItem�� �� �θ� ��ü

    // ���ö���¡
    // BackgroundText : ��ġ�Ͽ� �ݱ�
    // AchievementTitleText : ����
    // AccountPassDescriptionText : ���� ���� ������ �ø� ������ ������ ���� �� �ֽ��ϴ�.
    // FreePassTitleText : Free
    // RarePassButtonText : ���� �н�
    // RarePassPriceText : ���� �н� ����
    // EpicPassButtonText : ���� �н�
    // EpicPassPriceText : ���� �н� ����
    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        AccountPassScrollContentObject,
    }
    enum Buttons
    {
        BackButton,

        RarePassButton,
        EpicPassButton,
    }
    enum Texts
    {
        BackgroundText,
        AchievementTitleText,
        AccountPassDescriptionText,

        FreePassTitleText,
        RarePassButtonText,
        RarePassPriceText,
        EpicPassButtonText,
        EpicPassPriceText,
    }

    #endregion


    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetButton((int)Buttons.RarePassButton).gameObject.BindEvent(OnClickRarePassButton);
        GetButton((int)Buttons.RarePassButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EpicPassButton).gameObject.BindEvent(OnClickEpicPassButton);
        GetButton((int)Buttons.EpicPassButton).GetOrAddComponent<UI_ButtonAnimation>();



        // �׽�Ʈ��
#if UNITY_EDITOR

#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo()
    {

        Refresh();
    }

    void Refresh()
    {


    }

    void OnClickRarePassButton()
    {
        // ���� �н� ����, ������� ȣ��
    }
    void OnClickEpicPassButton()
    {
        // ���� �н� ����, ������� ȣ��
    }

    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

}
