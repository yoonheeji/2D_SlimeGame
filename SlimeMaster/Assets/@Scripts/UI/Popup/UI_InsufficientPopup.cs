using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InsufficientPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // InsufficientItemImage : ���ڶ� ��ȭ�� ������
    // InsufficientCostValueText : ���ڶ� ��ȭ�� ��, ������(#F3614D)���� �����ش�

    // ���ö���¡ �ؽ�Ʈ
    // InsufficientPopupTitleText
    // InsufficientText
    // GoDiamondsText
    // GoNowText

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        InsufficientCostObject,
    }
    enum Buttons
    {
        BackgroundButton,
        GoNowButton,
    }

    enum Texts
    {
        BackgroundText,
        InsufficientPopupTitleText,
        InsufficientText,
        InsufficientCostValueText,
        GoDiamondsText,
        GoNowText,
    }

    enum Images
    {
        InsufficientItemImage, // ���ڶ� �ڽ�Ʈ ������
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
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.GoNowButton).gameObject.BindEvent(OnClickGoNowButton);
        GetButton((int)Buttons.GoNowButton).GetOrAddComponent<UI_ButtonAnimation>();



#if UNITY_EDITOR

        //TextBindTest();
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
       
        // �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.InsufficientCostObject).GetComponent<RectTransform>());
    }

    void OnClickBackgroundButton() // ��� ���� �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);

    }
    void OnClickGoNowButton() // ���� �̵� ��ư
    {
        // GoDiaButton : ���� �������� ���̾� ���� ������ �̵�

    }

}
