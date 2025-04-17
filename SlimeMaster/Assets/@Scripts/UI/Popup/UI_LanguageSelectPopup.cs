using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_LanguageSelectPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // ���� ������ �� ���� �Ǿ����

    // ���ö���¡
    // BackgroundText : ���Ͽ� �ݱ�
    // ConfirmButtonText : Ȯ��

    // KoreanText : ���� ���� ���� ǥ��
    // EnglishText 
    // JapaneseText 
    // SimplifiedText
    // TraditionalText
    // FranceText

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ConfirmButton,
        BackgroundButton,
    }
    enum Toggles
    {
        KoreanToggle,
        EnglishToggle,
        JapaneseToggle,
        SimplifiedToggle,
        TraditionalToggle,
        FranceToggle,
    }
    enum Texts
    {
        BackgroundText,
        ConfirmButtonText,

        KoreanText,
        EnglishText,
        JapaneseText,
        SimplifiedText,
        TraditionalText,
        FranceText,
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
        BindToggle(typeof(Toggles));

        GetToggle((int)Toggles.KoreanToggle).gameObject.BindEvent(OnClickKoreanToggle);
        GetToggle((int)Toggles.KoreanToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.EnglishToggle).gameObject.BindEvent(OnClickEnglishToggle);
        GetToggle((int)Toggles.EnglishToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.JapaneseToggle).gameObject.BindEvent(OnClickJapaneseToggle);
        GetToggle((int)Toggles.JapaneseToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.SimplifiedToggle).gameObject.BindEvent(OnClickSimplifiedToggle);
        GetToggle((int)Toggles.SimplifiedToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.TraditionalToggle).gameObject.BindEvent(OnClickTraditionalToggle);
        GetToggle((int)Toggles.TraditionalToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.FranceToggle).gameObject.BindEvent(OnClickFranceToggle);
        GetToggle((int)Toggles.FranceToggle).GetOrAddComponent<UI_ButtonAnimation>();


        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);


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

    #region Toggles
    void OnClickKoreanToggle() // �ѱ���
    {
        // �ѱ��� ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    void OnClickEnglishToggle() // ����
    {
        // ���� ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    void OnClickJapaneseToggle() // �Ϻ���
    {
        // �Ϻ��� ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    void OnClickSimplifiedToggle() // �߱��� ��ü
    {
        // �߱��� ��ü ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    void OnClickTraditionalToggle() // �߱��� ��ü
    {
        // �߱��� ��ü ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    void OnClickFranceToggle() // ��������
    {
        // �������� ���� (���� Ȯ�ι�ư ������ ��� ����)
    }
    #endregion



    void OnClickConfirmButton() // Ȯ�� ��ư
    {
        // ������ �� ���� �ϰ� �˾� �ݱ�
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickBackgroundButton() // ��ġ�Ͽ� �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);
    }
}
