using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelectPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // StarOn_0 ~ 4 : ��ų ������ ���� Ȱ��ȭ
    // CharacterImage : ���õ� ĳ������ �̹���
    // CharacterNameValueText : ���õ� ĳ������ �̸�
    // AttackValueText : ĳ������ ���� ���ݷ� 
    // AttackBonusValueText : ���� ���ݷ� (+N)���� ǥ��
    // HealthValueText : ĳ������ ���� ü��
    // HealthBonusValueText : ���� ü�� (+N)���� ǥ��

    // EnhanceCostGoldValueText : ��ȭ �� �ʿ��� ���
    // EnhanceCostMaterialValueText : ��ȭ �� �ʿ��� ���
    // UpgradeCostMaterialValueText : ���׷��̵� �� �ʿ��� ���


    // ���ö���¡
    // CharacterInventoryTlileText : ĳ���� ���
    // EnhanceButtonText : ��ȭ
    // EquipButtonText : ����
    // LevelUpButtonText : ������


    #endregion
    enum GameObjects
    {
        ContentObject,
        CharacterLevelObject,
        AttackPointObject,
        HealthPointObject,
        CharacterEnhancePanelObject,
        CharacterEnhanceContentObject,
        CharacterUpgradeContentObject,
        EnhanceCostObject,
        UpgradeCostObject,
    }

    enum Images
    {
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        AttackImage,
        CharacterImage,
        HealthImage,
    }

    enum Buttons
    {
        EnhanceButton,
        LevelUpButton,
        EquipButton,
        BackButton,
    }

    enum Texts
    {
        CharacterNameValueText,
        AttackValueText,
        AttackBonusValueText,
        HealthValueText,
        HealthBonusValueText,
        CharacterInventoryTlileText,
        EnhanceButtonText,
        EquipButtonText,
        LevelUpButtonText,
        EnhanceCostGoldValueText,
        EnhanceCostMaterialValueText,
        UpgradeCostMaterialValueText,

    }
    enum Toggles
    {
        EnhanceToggle,
        UpgradeToggle,
    }
    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    bool isCharacterEnhancePanelOpen = false; // ��ȭ �г� ���� üũ��

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindToggle(typeof(Toggles));

        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false);
        isCharacterEnhancePanelOpen = false;
        GetImage((int)Images.StarOn_1).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_2).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_3).gameObject.SetActive(false);
        GetImage((int)Images.StarOn_4).gameObject.SetActive(false);

        GetButton((int)Buttons.EnhanceButton).gameObject.BindEvent(OnClickEnhanceButton);
        GetButton((int)Buttons.EnhanceButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelUpButton).gameObject.BindEvent(OnClickLevelUpButton);
        GetButton((int)Buttons.LevelUpButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(false);
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetToggle((int)Toggles.EnhanceToggle).gameObject.BindEvent(OnClickEnhanceToggle);
        GetToggle((int)Toggles.UpgradeToggle).gameObject.BindEvent(OnClickUpgradeToggle);

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
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AttackPointObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.HealthPointObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EnhanceCostObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.UpgradeCostObject).GetComponent<RectTransform>());

    }

    void OnClickEquipButton() // ���� ��ư
    {
        // ���� ���õ� ĳ���͸� �����Ѵ�.
    }
    
    void OnClickEnhanceButton() // ��ȭ ��ư Ŭ��
    {
        Managers.Sound.PlayButtonClick();

        GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(false);
        GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(true);
        
        // �� �������� �ذ��ؾ���
        //GetToggle((int)Toggles.EnhanceToggle).gameObject.GetComponent<Toggle>().onValueChanged.AddListener((call) => { 
        //    if(call)
        //        OnClickEnhanceToggle(); 
        //});
        //GetToggle((int)Toggles.EnhanceToggle).gameObject.GetComponent<Toggle>().isOn = true;

        GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(true);
        isCharacterEnhancePanelOpen = true;
        OnClickEnhanceToggle();
        GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
        
    }

    void OnClickLevelUpButton() // ������ ��ư Ŭ��
    {
        Managers.Sound.PlayButtonClick();
    }

    void OnClickBackButton() // �ڷΰ��� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (isCharacterEnhancePanelOpen)
        {
            GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(true);
            GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false); // ��ȭ �г� �ݱ�
            isCharacterEnhancePanelOpen = false;
            GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
        }
        else
        {
            Managers.UI.ClosePopupUI(this); // �˾� �ݱ�
        }
    }

    void OnClickEnhanceToggle() // ��ȭ ��� Ŭ��
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
        Refresh();
    }

    void OnClickUpgradeToggle() // ���׷��̵� ��� Ŭ��
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(true);
        Refresh();

    }
}
