using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PickupGachaInfoPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // �������� �� �˾��� ȣ�� �Ҷ� �Ⱦ�, �Ϲ�, ��� ��í�� ���� ������ �޶��� 

    // EquipmentGradeValueText : ��� ����� ��� ǥ�� �� ������ ��޿� ���߾� ����
    // - �Ϲ�(Common) : #A2A2A2
    // - ���(Uncommon)  : #57FF0B
    // - ���(Rare) : #2471E0
    // - ����(Epic) : #9F37F2
    // - ����(Legendary) : #F67B09
    // - ��ȭ(Myth) : #F1331A
    // EquipmentNameValueText : ��� ����� �̸�
    // EquipmentGradeBackgroundImage : ���� �������� �׵θ� (���� ����)
    // - �Ϲ�(Common) : #AC9B83
    // - ���(Uncommon)  : #73EC4E
    // - ���(Rare) : #0F84FF
    // - ����(Epic) : #B740EA
    // - ����(Legendary) : #F19B02
    // - ��ȭ(Myth) : #FC2302
    // EquipmentLevelValueText : ����� ���� (1/�ִ� ����)
    // EquipmentOptionImage : ��� �ɼ��� ������
    // EquipmentOptionValueText : ��� �ɼ� ��ġ
    // UncommonSkillOptionDescriptionValueText : ��� ��� �ɼ� ����
    // RareSkillOptionDescriptionValueText : ��� ��� �ɼ� ����
    // EpicSkillOptionDescriptionValueText : ���� ��� �ɼ� ����
    // LegendarySkillOptionDescriptionValueText : ���� ��� �ɼ� ����
    // MythSkillOptionDescriptionValueText : ��ȭ ��� �ɼ� ����
    // ���� ��� ������ ���̺��� �� ��޼� �ɼ�(��ųID)�� ��ų�� ���ٸ� ��޿� �´� �ɼ� ������Ʈ ��Ȱ��ȭ
    // - ���(Uncommon)  : UncommonSkillOptionObject
    // - ���(Rare) : RareSkillOptionObject
    // - ����(Epic) : EpicSkillOptionObject
    // - ����(Legendary) : LegendarySkillOptionObject
    // - ��ȭ(Myth) : MythSkillOptionObject
    // EquipmentDescriptionValueText : ��� ����� ���� �ؽ�Ʈ
    // PickupEquipmentButtonGroupObject : �Ⱦ� ��� �����϶� ����Ʈ ����

    // ���ö���¡
    // BackgroundText : ���Ͽ� �ݱ�
    // EquipmentGradeSkillText : ��� ��ų

    #endregion

    #region Enum

    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        MythSkillOptionObject,
        EquipmentInfoPopupTitleObject,
        EquipmentGradeSkillScrollContentObject,
        ButtonGroupObject,
        PickupEquipmentButtonGroupObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Toggles
    {
        PickupWeaponToggle,
        PickupChestToggle, // ���� �̸� üũ�ؼ� �����ؾ���
        PickupHandToggle,
        PickupShoesToggle,
        PickupNecklaceToggle,
        PickupRingToggle,
    }

    enum Texts
    {
        BackgroundText,
        EquipmentGradeSkillText,
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        MythSkillOptionDescriptionValueText,
        EquipmentDescriptionValueText,
    }
    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        PickupWeaponImage,
        PickupChestImage,
        PickupHandImage,
        PickupShoesImage,
        PickupNecklaceImage,
        PickupRingImage,
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
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetToggle((int)Toggles.PickupWeaponToggle).gameObject.BindEvent(OnClickPickupWeaponToggle);
        GetToggle((int)Toggles.PickupWeaponToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupChestToggle).gameObject.BindEvent(OnClickPickupChestToggle);
        GetToggle((int)Toggles.PickupChestToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupHandToggle).gameObject.BindEvent(OnClickPickupHandToggle);
        GetToggle((int)Toggles.PickupHandToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupShoesToggle).gameObject.BindEvent(OnClickPickupShoesToggle);
        GetToggle((int)Toggles.PickupShoesToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupNecklaceToggle).gameObject.BindEvent(OnClickPickupNecklaceToggle);
        GetToggle((int)Toggles.PickupNecklaceToggle).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.PickupRingToggle).gameObject.BindEvent(OnClickPickupRingToggle);
        GetToggle((int)Toggles.PickupRingToggle).GetOrAddComponent<UI_ButtonAnimation>();

        GetObject((int)GameObjects.PickupEquipmentButtonGroupObject).gameObject.SetActive(true);

        // �׽�Ʈ��
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

    }

    void EquipmentInfoInit() // ��� ���� �ʱ�ȭ
    {
        // EquipmentNameValueText
        // EquipmentGradeValueText
        // EquipmentLevelValueText
        // EquipmentOptionValueText
        // UncommonSkillOptionDescriptionValueText
        // RareSkillOptionDescriptionValueText
        // EpicSkillOptionDescriptionValueText
        // LegendarySkillOptionDescriptionValueText
        // MythSkillOptionDescriptionValueText
        // EquipmentDescriptionValueText
    }

    void PickupToggleGroupInit()
    {
        // PickupWeaponImage
        // PickupChestImage
        // PickupHandImage
        // PickupShoesImage
        // PickupNecklaceImage
        // PickupRingImage
    }

    void OnClickPickupWeaponToggle()
    {
        Managers.Sound.PlayButtonClick();

        // �Ⱦ� ���� ���� ����
    }
    void OnClickPickupChestToggle()
    {
        Managers.Sound.PlayButtonClick();

        // �Ⱦ� ���� ���� ����
    }
    void OnClickPickupHandToggle()
    {
        Managers.Sound.PlayButtonClick();
        // �Ⱦ� �尩 ���� ����
    }
    void OnClickPickupShoesToggle()
    {
        Managers.Sound.PlayButtonClick();
        // �Ⱦ� ���� ���� ����
    }
    void OnClickPickupNecklaceToggle()
    {
        Managers.Sound.PlayButtonClick();
        // �Ⱦ� ����� ���� ����
    }
    void OnClickPickupRingToggle()
    {
        Managers.Sound.PlayButtonClick();
        // �Ⱦ� ���� ���� ����
    }

    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #region  Test
    void OnClickStartButton() // ��ư Ŭ�� �׽�Ʈ
    {
        Debug.Log("On click start button");
    }

    void TextBindTest() // �ؽ�Ʈ ���� �׽�Ʈ��
    {
        string TestText = "Test";

        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = TestText;
        }
    }
    #endregion
}
