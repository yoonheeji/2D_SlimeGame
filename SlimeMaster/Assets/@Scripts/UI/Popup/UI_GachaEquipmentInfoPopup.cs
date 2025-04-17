using Data;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System;

public class UI_GachaEquipmentInfoPopup : UI_Popup
{

    #region Enum
    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        EquipmentGradeSkillScrollContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
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
        EquipmentGradeSkillText,
        BackgroundText,
        //EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        EquipmentImage,
        GradeBackgroundImage,
        //EquipmentEnforceBackgroundImage,
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
                GetImage((int)Images.GradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
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

        //if (equipmentGrade >= EquipmentGrade.Legendary)
        //{
        //    GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = EquipmentUIColors.Legendary;
        //    GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(false);
        //}
        #endregion

        #region �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
        #endregion
    }

    void OnClickBackgroundButton() // ��� ��ư (�˾� �ݱ�)
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);

        //Tap To Close �ܺ� ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }
}
