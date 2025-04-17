using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_FirstPaymentRewardPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // ������ ������ ���̺�� ����⿣ �ʹ� �������̶� Define�� DicFirstPayReward�� ������ 
    // FirstRewardBackgroundImage : ������ ��޿����� ���� ����
    // FirstRewardImage : ������ ������
    // FirstRewardCountText : ���� ����

    // SecondRewardBackgroundImage : ������ ��޿����� ���� ����
    // SecondRewardImage : ������ ������
    // SecondRewardCountText : ���� ����

    // ThirdRewardBackgroundImage : ������ ��޿����� ���� ����
    // ThirdRewardImage : ������ ������
    // ThirdRewardCountText : ���� ����

    // FourthRewardBackgroundImage : ������ ��޿����� ���� ����
    // FourthRewardImage : ������ ������
    // FourthRewardCountText : ���� ����

    // ���ö���¡
    // BackgroundText : ��ġ�Ͽ� �ݱ�
    // FirstPaymentRewardPopupTitleText : ù ���� ����
    // FirstPaymentRewardDescriptionText : ���� ���� �� ���� ������ 1ȸ �����մϴ�.
    // GoNowText : �ٷ� ����

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
        GoNowButton,
    }
    enum Texts
    {
        BackgroundText,
        FirstPaymentRewardPopupTitleText,
        FirstPaymentRewardDescriptionText,

        FirstRewardCountText,
        SecondRewardCountText,
        ThirdRewardCountText,
        FourthRewardCountText,

        GoNowText,
    }

    enum Images
    {
        FirstRewardBackgroundImage,
        FirstRewardImage,
        SecondRewardBackgroundImage,
        SecondRewardImage,
        ThirdRewardBackgroundImage,
        ThirdRewardImage,
        FourthRewardBackgroundImage,
        FourthRewardImage,

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
    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    // �������� �̵� ��ư
    void OnClickGoNowButton()
    {
        // ��� �˾��� �ݰ� ���� ������ �̵�
    }
}
