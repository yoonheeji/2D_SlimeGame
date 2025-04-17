using Data;
using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
using Sequence = DG.Tweening.Sequence;

public class UI_EquipmentPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // CharacterImage : ĳ���� �̹��� (�ִϸ��̼��̶�� ���� �ʿ�)
    // AttackValueText : ĳ������ ���� ���ݷ� ǥ�� (���� ��ȭ�� ���� �ʿ�)
    // HealthValueText : ĳ������ ���� ü�� ǥ�� (���� ��ȭ�� ���� �ʿ�)
    // MergeButtonRedDotObject : �ռ��� �����ϴٸ� ����� ���
    // EquipInventoryObject : �����ϰ� �ִ� ��� �� �θ�ü
    // (�ִ� 150��, �κ��丮 ���� ���� ���� �� ���� ���� ���ϰ� �κ��丮 ��� �˾� ȣ��)
    // ItemInventoryObject : �����ϰ� �ִ� �������� �� �θ�ü

    // ���ö���¡ �ؽ�Ʈ
    // EquipInventoryTlileText : ���
    // ItemInventoryTlileText : ������

    #endregion
    #region Enum
    enum GameObjects
    {
        ContentObject,
        WeaponEquipObject, //���� ���� �� �� �θ�ü
        GlovesEquipObject, // �尩 ���� �� �� �θ�ü
        RingEquipObject, // ���� ���� �� �� �θ�ü
        BeltEquipObject, // ��� ���� �� �� �θ�ü
        ArmorEquipObject, // ���� ���� �� �� �θ�ü
        BootsEquipObject, // ���� ���� �� �� �θ�ü
        CharacterRedDotObject,
        MergeButtonRedDotObject,
        EquipInventoryObject,
        ItemInventoryObject,
        EquipInventoryGroupObject,
        ItemInventoryGroupObject,
    }

    enum Buttons
    {
        CharacterButton,
        SortButton,
        MergeButton,
    }

    enum Images
    {
        CharacterImage,
    }

    enum Texts
    {
        AttackValueText,
        HealthValueText,
        SortButtonText,
        MergeButtonText,
        EquipInventoryTlileText,
        ItemInventoryTlileText,
    }
    #endregion
    #region LifeCycle
    private void OnEnable()
    {
        //Managers.Game.EquipInfoChanged += Refresh;
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    private void OnDisable()
    {
        //Managers.Game.EquipInfoChanged -= Refresh;
    }
    #endregion
    [SerializeField]
    public ScrollRect ScrollRect;
    EquipmentSortType _equipmentSortType;

    // ���� ��ư �ؽ�Ʈ
    string sortText_Level = "���� : ����";
    string sortText_Grade = "���� : ���";

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
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetObject((int)GameObjects.CharacterRedDotObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.MergeButtonRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
        GetButton((int)Buttons.CharacterButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CharacterButton).gameObject.SetActive(false); // ��ö� ����

        GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ���� ���� ����Ʈ
        _equipmentSortType = EquipmentSortType.Level;
        GetText((int)Texts.SortButtonText).text = sortText_Level;

        //GetObject((int)GameObjects.RandomGenButton).BindEvent(() =>
        //{
        //    Managers.Sound.PlayButtonClick();
        //    Managers.Game.GenerateRandomEquipment();
        //    Refresh();
        //});

#if UNITY_EDITOR
        // �׽�Ʈ��
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
        if (_init == false)
            return;

        #region �ʱ�ȭ
        GameObject WeaponContainer = GetObject((int)GameObjects.WeaponEquipObject);
        GameObject GlovesContainer = GetObject((int)GameObjects.GlovesEquipObject);
        GameObject RingContainer = GetObject((int)GameObjects.RingEquipObject);
        GameObject BeltContainer = GetObject((int)GameObjects.BeltEquipObject);
        GameObject ArmorContainer = GetObject((int)GameObjects.ArmorEquipObject);
        GameObject BootsContainer = GetObject((int)GameObjects.BootsEquipObject);

        WeaponContainer.DestroyChilds();
        GlovesContainer.DestroyChilds();
        RingContainer.DestroyChilds();
        BeltContainer.DestroyChilds();
        ArmorContainer.DestroyChilds();
        BootsContainer.DestroyChilds();

        #endregion
        #region ���
        //1. ��� ����Ʈ�� �ҷ��ͼ� ����κ��丮�� �߰�
        foreach (Equipment item in Managers.Game.OwnedEquipments)
        {
            //����������� 
            if (item.IsEquipped)
            {
                switch (item.EquipmentData.EquipmentType)
                {
                    case Define.EquipmentType.Weapon:
                        UI_EquipItem weapon = Managers.UI.MakeSubItem<UI_EquipItem>(WeaponContainer.transform);
                        weapon.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Boots:
                        UI_EquipItem boots = Managers.UI.MakeSubItem<UI_EquipItem>(BootsContainer.transform);
                        boots.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Ring:
                        UI_EquipItem ring = Managers.UI.MakeSubItem<UI_EquipItem>(RingContainer.transform);
                        ring.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Belt:
                        UI_EquipItem belt = Managers.UI.MakeSubItem<UI_EquipItem>(BeltContainer.transform);
                        belt.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Armor:
                        UI_EquipItem armor = Managers.UI.MakeSubItem<UI_EquipItem>(ArmorContainer.transform);
                        armor.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EquipmentType.Gloves:
                        UI_EquipItem gloves = Managers.UI.MakeSubItem<UI_EquipItem>(GlovesContainer.transform);
                        gloves.SetInfo(item, Define.UI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                }
            }
        }
        SortEquipments();
        #endregion

        #region ĳ����

        // ���ݷ�,HP ����
        var (hp, attack) = Managers.Game.GetCurrentChracterStat();
        GetText((int)Texts.AttackValueText).text = (Managers.Game.CurrentCharacter.Atk + attack).ToString();
        GetText((int)Texts.HealthValueText).text = (Managers.Game.CurrentCharacter.MaxHp + hp).ToString();

        #endregion

        SetItem();

        // �������� ���� ����ItemInventoryObject 
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryGroupObject).GetComponent<RectTransform>());

        
    }


    void OnClickCharacterButton() // ĳ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_CharacterSelectPopup>();
    }


    void OnClickSortButton() // ���� ��ư
    {
        Managers.Sound.PlayButtonClick();

        // ������ ����, ������� ���� ���������� ���Ĺ�� ����
        if (_equipmentSortType == EquipmentSortType.Level)
        {
            _equipmentSortType = EquipmentSortType.Grade;
            GetText((int)Texts.SortButtonText).text = sortText_Grade;
        }

        else if (_equipmentSortType == EquipmentSortType.Grade)
        {
            _equipmentSortType = EquipmentSortType.Level;
            GetText((int)Texts.SortButtonText).text = sortText_Level;
        }

        SortEquipments();
    }

    void OnClickMergeButton() // �ռ� ��ư
    {
        Managers.Sound.PlayButtonClick();
        UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
        mergePopupUI.SetInfo(null);
        mergePopupUI.gameObject.SetActive(true);
    }

    void SortEquipments()
    {
        Managers.Game.SortEquipment(_equipmentSortType);

        GetObject((int)GameObjects.EquipInventoryObject).DestroyChilds();

        foreach (Equipment item in Managers.Game.OwnedEquipments)
        {
            if (item.IsEquipped)
                continue;

            UI_EquipItem popup = Managers.Resource.Instantiate("UI_EquipItem", GetObject((int)GameObjects.EquipInventoryObject).transform, true).GetOrAddComponent<UI_EquipItem>();

            popup.transform.SetParent(GetObject((int)GameObjects.EquipInventoryObject).transform);
            popup.SetInfo(item, Define.UI_ItemParentType.EquipInventoryGroup, ScrollRect);
        }
    }

    public void SetItem()
    {
        GameObject Container = GetObject((int)GameObjects.ItemInventoryObject);
        Container.DestroyChilds();

         foreach (int id in Managers.Game.ItemDictionary.Keys)
        {
            if (Managers.Data.MaterialDic.TryGetValue(id, out MaterialData material) == true)
            {
                UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(Container.transform);
                int count = Managers.Game.ItemDictionary[id];
                
                item.SetInfo(material, transform, count, ScrollRect);
            }
        }
    }

}
