using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EquipmentInfoPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
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
    // EquipmentLevelValueText : ����� ���� (���� ����/�ִ� ����)
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
    // - ��ȭ(Myth) : MythSkillOptionObject // ����
    // EquipmentDescriptionValueText : ��� ����� ���� �ؽ�Ʈ // ����
    // CostGoldValueText : ������ ��� (���� / �ʿ�) ���� �ڽ�Ʈ�� �����ϴٸ� �������� ������(#F3614D)���� �����ش�. �������� �ʴٸ� ���(#FFFFFF)
    // CostMaterialImage : ������ ����� ������
    // CostMaterialValueText : ������ ��� (���� / �ʿ�) ���� �ڽ�Ʈ�� �����ϴٸ� �������� ������(#F3614D)���� �����ش�. �������� �ʴٸ� ���(#FFFFFF)

    // ���ö���¡ �ؽ�Ʈ
    // BackgroundText : ���Ͽ� �ݱ�
    // EquipmentGradeSkillText : ��� ��ų
    // EquipButtonText : ����
    // UnequipButtonText : ����
    // LevelupButtonText : ������
    // MergeButtonText : �ռ�


    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        EquipmentGradeSkillScrollContentObject,
        ButtonGroupObject,
        CostGoldObject,
        CostMaterialObject,
        LevelupCostGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipButton,
        UnquipButton,
        LevelupButton,
        MergeButton,
    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        CostGoldValueText,
        CostMaterialValueText,
        EquipButtonText,
        UnequipButtonText,
        LevelupButtonText,
        MergeButtonText,
        EquipmentGradeSkillText,
        BackgroundText,
        EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        CostMaterialImage,
        EquipmentImage,
        GradeBackgroundImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,

        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        LegendarySkillLockImage,
    }
    #endregion

    public Equipment _equipment;

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());

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
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton); 
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.UnquipButton).gameObject.BindEvent(OnClickUnequipButton);
        GetButton((int)Buttons.UnquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelupButton).gameObject.BindEvent(OnClickLevelupButton);
        GetButton((int)Buttons.LevelupButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();

        // �̹� �����ϰ� �ִ� ����� EquipButton�� ��Ȱ��ȭ
        // �����ϰ� ���� �ʴ� ����� UnequipButton�� ��Ȱ��ȭ
        #endregion


        return true;
    }

    Action _closeAction;
    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        Refresh();
    }

    void Refresh()
    {
        #region ��������
        GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
        GetButton((int)Buttons.UnquipButton).gameObject.SetActive(true);
        if (_equipment.IsEquipped == true)
            GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.UnquipButton).gameObject.SetActive(false);

        // ��� ������ 1�̶�� ���� ��ư ��Ȱ��ȭ #Neo
        if (_equipment.Level == 1)
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(true);

        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        
        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case EquipmentGrade.Common:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "�Ϲ�";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.CommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Common;
                break;

            case EquipmentGrade.Uncommon:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;

            case EquipmentGrade.Rare:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;
                break;

            case EquipmentGrade.Epic:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "����";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;

            case EquipmentGrade.Epic1:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;

            case EquipmentGrade.Epic2:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;

            case EquipmentGrade.Legendary:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "����";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break; 

            case EquipmentGrade.Legendary1:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;

            case EquipmentGrade.Legendary2:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;

            case EquipmentGrade.Legendary3:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 3";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;

            default:
                break;
        }

        // EquipmentNameValueText : ��� ����� �̸�
        GetText((int)Texts.EquipmentNameValueText).text = _equipment.EquipmentData.NameTextID;
        // EquipmentLevelValueText : ����� ���� (���� ����/�ִ� ����)
        GetText((int)Texts.EquipmentLevelValueText).text = $"{_equipment.Level}/{_equipment.EquipmentData.MaxLevel}";
        // EquipmentOptionImage : ��� �ɼ��� ������
        string sprName = _equipment.MaxHpBonus == 0 ? "AttackPoint_Icon.sprite" : "HealthPoint_Icon.sprite";
        GetImage((int)Images.EquipmentOptionImage).sprite = Managers.Resource.Load<Sprite>(sprName);
        // EquipmentOptionValueText : ��� �ɼ� ��ġ
        string bonusVale = _equipment.MaxHpBonus == 0 ? _equipment.AttackBonus.ToString() : _equipment.MaxHpBonus.ToString();
        GetText((int)Texts.EquipmentOptionValueText).text = $"+{bonusVale}";

        // CostGoldValueText : ������ ��� (���� / �ʿ�) ���� �ڽ�Ʈ�� �����ϴٸ� �������� ������(#F3614D)���� �����ش�. �������� �ʴٸ� ���(#FFFFFF)
        if (Managers.Data.EquipLevelDataDic.ContainsKey(_equipment.Level))
        {
            GetText((int)Texts.CostGoldValueText).text = $"{Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeCost}";
            if (Managers.Game.Gold < Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeCost)
                GetText((int)Texts.CostGoldValueText).color = Util.HexToColor("F3614D");
            
            GetText((int)Texts.CostMaterialValueText).text = $"{Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeRequiredItems}";

        }

        // ������ ����� ������ #Neo
        GetImage((int)Images.CostMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);
        #endregion

        #region ���� +1 ���� ��� ����
        string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 ���� Epic2 ->2 ���� Commonó�� ���ڰ� ������ 0 ����
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
        #endregion

        #region ���ų �ɼ� ����
        // ���� ��� ������ ���̺��� �� ��޼� �ɼ�(��ųID)�� ��ų�� ���ٸ� ��޿� �´� �ɼ� ������Ʈ ��Ȱ��ȭ
        GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.RareSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(false);

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.UncommonGradeSkill)) // ��ųŸ�Կ��� ����Ʈ��ų Ÿ�� �����ͷ� ��ü #Neo
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.UncommonGradeSkill];
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.RareGradeSkill)) 
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.RareGradeSkill];
            GetText((int)Texts.RareSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.RareSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.EpicGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.EpicGradeSkill];
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.LegendaryGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.LegendaryGradeSkill];
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(true);
        }
        #endregion

        #region ���ų �ɼ� ���� ����
        EquipmentGrade equipmentGrade = _equipment.EquipmentData.EquipmentGrade;

        // ���� ���� ����
        GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.RareSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");
        GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = Util.HexToColor("9A9A9A");

        GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(true);

        // ��޺� ���� �߰� �� ����
        if (equipmentGrade >= EquipmentGrade.Uncommon)
        {
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = EquipmentUIColors.Uncommon;
            GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= EquipmentGrade.Rare)
        {
            GetText((int)Texts.RareSkillOptionDescriptionValueText).color = EquipmentUIColors.Rare;
            GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= EquipmentGrade.Epic)
        {
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = EquipmentUIColors.Epic;
            GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= EquipmentGrade.Legendary)
        {
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = EquipmentUIColors.Legendary;
            GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(false);
        }
        #endregion

        #region �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ButtonGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostMaterialObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostGoldValueText).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostMaterialValueText).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.LevelupCostGroupObject).GetComponent<RectTransform>());
        #endregion
    }

    void OnClickBackgroundButton() // ��� ��ư (�˾� �ݱ�)
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);

        //Tap To Close �ܺ� ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickEquipmentResetButton() // ��� �ʱ�ȭ �˾� ��ư
    {
        Managers.Sound.PlayButtonClick();
        UI_EquipmentResetPopup resetPopup = (Managers.UI.SceneUI as UI_LobbyScene).EquipmentResetPopupUI;
        resetPopup.SetInfo(_equipment);
        resetPopup.gameObject.SetActive(true);
    }

    void OnClickEquipButton() // ���� ��ư
    {
        Managers.Sound.Play(Define.Sound.Effect, "Equip_Equipment");

        // ��� �����Ѵ�
        Managers.Game.EquipItem(_equipment.EquipmentData.EquipmentType, _equipment);
        Refresh();

        //â�ݱ�
        gameObject.SetActive(false);

        // ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickUnequipButton() // ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        // ��� ���� �����Ѵ�.
        Managers.Game.UnEquipItem(_equipment);
        Refresh();

        //â�ݱ�
        gameObject.SetActive(false);

        // ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickLevelupButton() // ������ ��ư
    {
        Managers.Sound.PlayButtonClick();

        //��񷹺��� �ƽ��������� �۾ƾ���
        if (_equipment.Level >= _equipment.EquipmentData.MaxLevel)
            return;

        //Cost_Gold, Cost_Material �� ��������
     
        int UpgradeCost = Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeCost;
        int UpgradeRequiredItems = Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeRequiredItems;

        // ����� ���� ���� 
        // _equipment.Level

        //���� ���� ��ȭ
        // TEMP : ��ȭ �ӽ÷� ������Ŵ ���߿� �����
        int numMaterial = 0;
        Managers.Game.ItemDictionary.TryGetValue(_equipment.EquipmentData.LevelupMaterialID, out numMaterial);
       
        if (Managers.Game.Gold >= UpgradeCost && numMaterial >= UpgradeRequiredItems)
        {
            _equipment.LevelUp();

            Managers.Game.Gold -= UpgradeCost;

            Managers.Game.RemovMaterialItem(_equipment.EquipmentData.LevelupMaterialID, UpgradeRequiredItems);
            Managers.Sound.Play(Define.Sound.Effect, "Levelup_Equipment");

            Refresh();
        }
        else
        {
            Managers.UI.ShowToast("��ȭ�� �����մϴ�.");
        }

        // ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickMergeButton() // �ռ� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (_equipment.IsEquipped) return;
        UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
        mergePopupUI.SetInfo(_equipment);
        mergePopupUI.gameObject.SetActive(true);
    }
}
