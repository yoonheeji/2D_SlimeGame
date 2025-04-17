using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_MergeEquipItem : UI_Base
{
    #region Enum
    enum GameObjects
    {
        EquipmentRedDotObject,
        NewTextObject,
        EquippedObject,
        SelectObject,
        LockObject,
        SpecialImage,
    }

    enum Texts
    {
        EquipmentLevelValueText,
        EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
    }
    #endregion

    public Equipment Equipment;
    public Action OnClickEquipItem;
    ScrollRect _scrollRect;
    bool _isDrag = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        gameObject.BindEvent(OnClickEquipItemButton);


        return true;
    }

    public void SetInfo(Equipment item, UI_ItemParentType parentType, ScrollRect scrollRect, bool isSelected, bool isLock)
    {
        Equipment = item;
        transform.localScale = Vector3.one;
        _scrollRect = scrollRect;

        #region ���� ����
        // EquipmentGradeBackgroundImage : �ռ� �� ��� ����� �׵θ� (���� ����)
        // EquipmentEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �̹��� ���� ����
        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case EquipmentGrade.Common:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Common;
                break;

            case EquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;

            case EquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;
                break;

            case EquipmentGrade.Epic:
            case EquipmentGrade.Epic1:
            case EquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;

            case EquipmentGrade.Legendary:
            case EquipmentGrade.Legendary1:
            case EquipmentGrade.Legendary2:
            case EquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;

            default:
                break;
        }
        #endregion

        #region ���� +1 ���� ��� ����
        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
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

        // EquipmentImage : ����� ������
        Sprite spr = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipmentImage).sprite = spr;
        // ��� Ÿ�� ������
        Sprite tpy = Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage((int)Images.EquipmentTypeImage).sprite = tpy;
        // EquipmentLevelValueText : ����� ���� ����
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{Equipment.Level}";
        // EquipmentRedDotObject : ��� ��ȭ�� �����Ҷ� ��� 
        GetObject((int)GameObjects.EquipmentRedDotObject).SetActive(Equipment.IsUpgradable);
        // NewTextObject : ��� ó�� ���������� ���
        GetObject((int)GameObjects.NewTextObject).SetActive(!Equipment.IsConfirmed);
        // EquippedObject : �ռ� �˾����� ������� ǥ�ÿ�
        GetObject((int)GameObjects.EquippedObject).SetActive(Equipment.IsEquipped);
        // SelectObject : �ռ� �˾����� ��� ���� ǥ�ÿ�
        GetObject((int)GameObjects.SelectObject).SetActive(Equipment.IsSelected);
        // Special�������϶� 
        bool isSpecial = Equipment.EquipmentData.GachaRarity == GachaRarity.Special ? true : false;
        //�ɸ��� �������� ���Կ� ������ "������" ������Ʈ ��
        GetObject((int)GameObjects.SpecialImage).SetActive(isSpecial);
        if (parentType == UI_ItemParentType.CharacterEquipmentGroup)
        {
            GetObject((int)GameObjects.EquippedObject).SetActive(false);
        }

        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(isSelected);
        GetObject((int)GameObjects.LockObject).gameObject.SetActive(isLock);
    }

    void Refresh()
    {


    }

    void OnClickEquipItemButton() // ��� ���� ��
    {
        Managers.Sound.PlayButtonClick();
        if (_isDrag) return;
        if (!Equipment.IsConfirmed)
            OnClickEquipItem?.Invoke();

        (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI.SetMergeItem(Equipment);
        // �ش� ����� ��� ���� �˾��� ȣ��
        //UI_EquipmentInfoPopup infoPopup = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        //if (infoPopup != null)
        //{
        //    infoPopup.SetInfo(Equipment);
        //}
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
}
