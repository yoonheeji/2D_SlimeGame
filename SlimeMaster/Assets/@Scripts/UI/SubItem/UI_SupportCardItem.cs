using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Data;

public class UI_SupportCardItem : UI_Base
{

    #region UI ��� ����Ʈ
    // ���� ����
    // CardNameText : ����Ʈ ��ų �̸�
    // SupportSkillImage : ����Ʈ ��ų ������
    // TargetDescriptionText : ����Ʈ ��ų ����
    // SoulValueText : ����Ʈ ��ų �ڽ�Ʈ
    // SoldOutObject : ����Ʈ ī�� ���� �Ϸ�� Ȱ��ȭ 
    // LockToggle : ����� Ȱ��ȭ �Ǿ��ٸ� ����Ʈ ��ų�� ������� ����(����� ���� �Ҷ����� ����)

    // ���ö���¡
    // LockToggleText : ���


    #endregion

    #region Enum
    enum GameObjects
    {
        SoldOutObject,
    }

    enum Texts
    {
        CardNameText,
        SoulValueText,
        LockToggleText,
        SkillDescriptionText,
    }

    enum Images
    {
        SupportSkillImage,
        SupportSkillCardBackgroundImage,
        SupportCardTitleImage,
    }
    enum Toggles
    {
        LockToggle,
    }
    #endregion
    Data.SupportSkillData _supportSkilllData;

    private void OnEnable()
    {
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindToggle(typeof(Toggles));
        #endregion

        GetToggle((int)Toggles.LockToggle).gameObject.BindEvent(OnClickLockToggle);
        gameObject.BindEvent(OnClickBuy);
        GetToggle((int)Toggles.LockToggle).GetOrAddComponent<UI_ButtonAnimation>();

        return true;
    }
  
    public void SetInfo(Data.SupportSkillData supportSkilll)
    {
        transform.localScale = Vector3.one;
        _supportSkilllData = supportSkilll;
        GetObject((int)GameObjects.SoldOutObject).SetActive(false);

        Refresh();
    }

    void Refresh()
    {
        // CardNameText : ����Ʈ ��ų �̸�
        GetText((int)Texts.CardNameText).text = _supportSkilllData.Name;
        // TargetDescriptionText : ����Ʈ ��ų ����
        GetText((int)Texts.SkillDescriptionText).text = _supportSkilllData.Description;
        // SoulValueText : ����Ʈ ��ų �ڽ�Ʈ
        GetText((int)Texts.SoulValueText).text = _supportSkilllData.Price.ToString();
        // SupportSkillImage : ����Ʈ ��ų ������
        GetImage((int)Images.SupportSkillImage).sprite = Managers.Resource.Load<Sprite>(_supportSkilllData.IconLabel);
        // SoldOutObject : ����Ʈ ī�� ���� �Ϸ�� Ȱ��ȭ 
        GetObject((int)GameObjects.SoldOutObject).SetActive(_supportSkilllData.IsPurchased);
        // LockToggle : ����� Ȱ��ȭ �Ǿ��ٸ� ����Ʈ ��ų�� ������� ����(����� ���� �Ҷ����� ����)
        GetToggle((int)Toggles.LockToggle).isOn = _supportSkilllData.IsLocked;

        // ��޿� ���� ��� ���� ����
        switch (_supportSkilllData.SupportSkillGrade)
        {
            case SupportSkillGrade.Common:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.CommonNameColor;
                break;
            case SupportSkillGrade.Uncommon:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.UncommonNameColor;
                break;
            case SupportSkillGrade.Rare:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.RareNameColor;
                break;
            case SupportSkillGrade.Epic:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.EpicNameColor;
                break;
            case SupportSkillGrade.Legend:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SupportCardTitleImage).color = EquipmentUIColors.LegendaryNameColor;
                break;
            default:
                break;
        }
    }

    void OnClickLockToggle()
    {
        Managers.Sound.PlayButtonClick();
        if (_supportSkilllData.IsPurchased)
            return;
        if (GetToggle((int)Toggles.LockToggle).isOn == true)
        {
        _supportSkilllData.IsLocked = true;
            Managers.Game.Player.Skills.LockedSupportSkills.Add(_supportSkilllData);
        }
        else
        {
            _supportSkilllData.IsLocked = false;
            Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);
        }
    }

    void OnClickBuy()
    {
        if (GetObject((int)GameObjects.SoldOutObject).activeInHierarchy == true)
            return;
        if (Managers.Game.Player.SoulCount >= _supportSkilllData.Price)
        {
            Managers.Game.Player.SoulCount -= _supportSkilllData.Price;
            
            if(Managers.Game.Player.Skills.LockedSupportSkills.Contains(_supportSkilllData))
                Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);

            Managers.Game.Player.Skills.AddSupportSkill(_supportSkilllData);
            GetObject((int)GameObjects.SoldOutObject).SetActive(true);
            //���ſϷ�
            GetObject((int)GameObjects.SoldOutObject).SetActive(true);
        }
    }
}
