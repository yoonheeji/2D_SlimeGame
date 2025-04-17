using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using static Define;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

public class UI_MergePopup : UI_Popup
{
    [SerializeField]
    public ScrollRect _scrollRect;
    Equipment _equipment;

    Equipment _mergeEquipment1;
    Equipment _mergeEquipment2;

    EquipmentSortType _equipmentSortType;

    #region UI ��� ����Ʈ
    // ���� ����
    // EquipInventoryScrollContentObject : ������ ������ �� �θ�ü

    // �κ��丮�� �ִ� ��� ���� �� (or ������ ���·� ���� ��)
    // - TargetEquip : �ռ��� ��� ���� �� Ȱ��ȭ
    // - OptionResultObject : �ռ��� ��� ���� �� Ȱ��ȭ
    // - SelectMergeCommentText : �ռ��� ��� ���� �� �� Ȱ��ȭ (���� �ȳ���)

    // MergeResultGroup
    // EquipResultButton (��� ��� ǥ��)
    //      MergePossibleOutlineImage : �ռ� �����ϸ� ���� ����
    //      - �⺻ : #FFFFFF (���)
    //      - �ռ� ���� : #0AFF00 (�ʷ�)
    //  TargetEquipObject (�ռ��� ���)
    //      TargetEquipGradeBackgroundImage : �ռ� �� ��� ����� �׵θ� (���� ����)
    //      - �Ϲ�(Common) : #AC9B83
    //      - ���(Uncommon)  : #73EC4E
    //      - ���(Rare) : #0F84FF
    //      - ����(Epic) : #B740EA
    //      - ����(Legendary) : #F19B02
    //      - ��ȭ(Myth) : #FC2302
    //      TargetEquipImage : ������ ����� ������
    //      TargetEquipLevelValueText : ������ ����� ����
    //      TargetEquipEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      - ����(Epic) : #B740EA
    //      - ����(Legendary) : #F19B02
    //      - ��ȭ(Myth) : #FC2302
    //      TargetEquipEnforceValueText : ����� ����

    //      EquipResultGradeBackgroundImage : �ռ� �� ��� ����� �׵θ� (���ҽ� ����)
    //      EquipResultImage : ������ ����� ������
    //      EquipResultLevelValueText : �ռ� �� ��� ���� 
    //      EquipResultEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      EquipResultEnforceValueText : ����� ����
    // OptionResult (�ɼ� ��� ����Ʈ)
    //      EquipmentNameText : ��� �̸�
    //      BeforeLevelValueText : �ռ� �� ����
    //      LevelArrowImage : �⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ
    //      AfterLevelValueText : �ռ� �� ���� (�⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ)
    //      ImprovATKObject : ���ݷ� �ɼ��� �ִٸ� Ȱ��ȭ
    //      BeforeATKValueText : �ռ� �� ���ݷ�
    //      ATKArrowImage : �⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ
    //      AfterATKValueText : �ռ� �� ���ݷ� ((�⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ))
    //      ImprovHPObject : ü�� �ɼ��� �ִٸ� Ȱ��ȭ
    //      BeforeHPValueText : �ռ� �� ü��
    //      HPArrowImage : �⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ
    //      AfterHPValueText : �ռ� �� ü�� ((�⺻ ��Ȱ��ȭ, �ռ��� ������ �� Ȱ��ȭ))
    //      ImprovOptionValueText : �ռ� �� �߰� �ɼ�


    // MergeCostGroup (�ռ� ���)
    //   FirstCostEquipNeedObject : �ռ��� ��� ���� �� Ȱ��ȭ
    //      FirstCostEquipGradeBackgroundImage : �ʿ��� ��� ����� �׵θ� (���ҽ� ����)
    //      FirstCostEquipImage : �ʿ��� ����� ���� ������ (���ҽ� ����)
    //      - ���� : @Resources\Sprites\UI\Common\Icon\Ui_Sword_Icon
    //      - �尩 : @Resources\Sprites\UI\Common\Icon\Ui_Glove_Icon
    //      - ���� : @Resources\Sprites\UI\Common\Icon\Ui_Ring_Icon
    //      - ��� : @Resources\Sprites\UI\Common\Icon\Ui_Helmet_Icon
    //      - ���� : @Resources\Sprites\UI\Common\Icon\Ui_Top_Icon
    //      - ���� : @Resources\Sprites\UI\Common\Icon\Ui_Boots_Icon
    //      FirstCostEquipBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      FirstCostEquipEnforceValueText : ����� ����

    //   FirstCostEquipSelectObject : ù��° ��� ���� �� Ȱ��ȭ 
    //      FirstSelectEquipGradeBackgroundImage : ������ ��� ����� �׵θ� (���ҽ� ����)
    //      FirstSelectEquipImage : ������ ����� ������
    //      FirstSelectEquipLevelValueText :������ ����� ����
    //      FirstSelectEquipEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      FirstSelectEquipEnforceValueText : ����� ����

    //   SecondCostEquipNeedObject : �ռ��� ��� ���� �� Ȱ��ȭ
    //      SecondCostEquipGradeBackgroundImage : �ʿ��� ��� ����� �׵θ� (���ҽ� ����)
    //      SecondCostEquipImage : �ʿ��� ����� ���� ������ (���ҽ� ����)

    //   SecondCostEquipSelectObject : ù��° ��� ���� �� Ȱ��ȭ 
    //      SecondSelectEquipGradeBackgroundImage : ������ ��� ����� �׵θ� (���ҽ� ����)
    //      SecondSelectEquipImage : ������ ����� ������
    //      SecondSelectEquipLevelValueText :������ ����� ����
    //      SecondSelectEquipEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      SecondSelectEquipEnforceValueText : ����� ����

    // MergeButton : �ռ� ��ư, �ռ��� �����ϴٸ� Ȱ��ȭ

    // ���ö���¡ �ؽ�Ʈ
    // ImprovLevelText : �ִ� ����
    // ImprovATKText : ���ݷ�
    // ImprovHPText : ü��
    // EquipmentTlileText : ���
    // SortButtonText : ����
    // MergeAllButtonText : ����ռ�

    #endregion


    enum GameObjects
    {
        ContentObject,
        SelectedEquipObject,
        OptionResultObject,
        ImprovATKObject,
        ImprovHPObject,
        FirstCostEquipNeedObject,
        FirstCostEquipSelectObject,
        SecondCostEquipNeedObject,
        SecondCostEquipSelectObject,
        MergeAllButtonRedDotObject,
        EquipInventoryScrollContentObject,

        MurgeStartEffect,
        MurgeFinishEffect,
    }

    enum Buttons
    {
        EquipResultButton,
        FirstCostButton,
        SecondCostButton,
        SortButton,
        MergeAllButton,
        MergeButton,
        BackButton,
    }

    enum Texts
    {
        SelectedEquipLevelValueText,
        SelectedEquipEnforceValueText,
        EquipmentNameText,
        BeforeGradeValueText,
        AfterGradeValueText,
        BeforeLevelValueText,
        AfterLevelValueText, 
        ImprovLevelText, 
        BeforeATKValueText,
        AfterATKValueText,
        ImprovHPText,
        BeforeHPValueText,
        AfterHPValueText,
        ImprovOptionValueText,
        FirstCostEquipEnforceValueText,
        FirstSelectEquipLevelValueText,
        FirstSelectEquipEnforceValueText,
        SecondSelectEquipLevelValueText,
        SecondSelectEquipEnforceValueText,
        EquipmentTlileText,
        SortButtonText,
        MergeAllButtonText,
        SelectEquipmentCommentText,
        SelectMergeCommentText,
    }

    enum Images
    {
        MergePossibleOutlineImage,
        SelectedEquipGradeBackgroundImage,
        SelectedEquipImage,
        SelectedEquipEnforceBackgroundImage,
        SelectedEquipTypeBackgroundImage,
        SelectedEquipTypeImage,
        LevelArrowImage,
        ATKArrowImage,
        HPArrowImage,
        FirstCostEquipGradeBackgroundImage,
        FirstCostEquipImage,
        FirstCostEquipBackgroundImage,
        FirstSelectEquipGradeBackgroundImage,
        FirstSelectEquipImage,
        FirstSelectEquipEnforceBackgroundImage,
        FirstSelectEquipTypeBackgroundImage,
        FirstSelectEquipTypeImage,
        SecondCostEquipGradeBackgroundImage,
        SecondCostEquipImage,
        SecondSelectEquipGradeBackgroundImage,
        SecondSelectEquipImage,
        SecondSelectEquipEnforceBackgroundImage,
        SecondSelectEquipTypeBackgroundImage,
        SecondSelectEquipTypeImage
    }

    // ���� ��ư �ؽ�Ʈ
    string sortText_Level = "���� : ����";
    string sortText_Grade = "���� : ���";

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

        GetButton((int)Buttons.EquipResultButton).gameObject.BindEvent(OnClickEquipResultButton);
        GetObject((int)GameObjects.SelectedEquipObject).gameObject.SetActive(false); // �ռ��� ���
        GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); 
        GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false); 
        GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false); // �ɼ� ���

        GetObject((int)GameObjects.MurgeStartEffect).gameObject.SetActive(false); // �ռ� ���� ����Ʈ
        GetObject((int)GameObjects.MurgeFinishEffect).gameObject.SetActive(false); // �ռ� �� ����Ʈ

        GetButton((int)Buttons.FirstCostButton).gameObject.BindEvent(OnClickFirstCostButton);
        GetObject((int)GameObjects.FirstCostEquipNeedObject).gameObject.SetActive(false); // ù��° ���
        GetObject((int)GameObjects.FirstCostEquipSelectObject).gameObject.SetActive(false);

        GetButton((int)Buttons.SecondCostButton).gameObject.BindEvent(OnClickSecondCostButton);
        GetObject((int)GameObjects.SecondCostEquipNeedObject).gameObject.SetActive(false); // �ι�° ���
        GetObject((int)GameObjects.SecondCostEquipSelectObject).gameObject.SetActive(false);

        GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeAllButton).gameObject.BindEvent(OnClickMergeAllButton);
        GetButton((int)Buttons.MergeAllButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ���� ���� ����Ʈ
        _equipmentSortType = EquipmentSortType.Level;
        GetText((int)Texts.SortButtonText).text = sortText_Level;

        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.SetActive(false); // �ռ� ��ư
        GetObject((int)GameObjects.MergeAllButtonRedDotObject).gameObject.SetActive(false); // ��� �ռ� �����


        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton); // �ڷΰ���
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        _mergeEquipment1 = null;
        _mergeEquipment2 = null;
        Refresh();
    }

    public void SetMergeItem(Equipment equipment, bool showUI = true)
    {
        if (equipment.IsEquipped == true)
            return;

        if (equipment.Level > 1)
            return;

        if (_equipment == null)
        {
            _equipment = equipment;
            if (showUI)
            {
                Refresh_SelectedEquipObject();
                SortEquipments();
            }
            return;
        }

        if (_equipment == equipment)
            return;
        
        if (_equipment.EquipmentData.EquipmentType != equipment.EquipmentData.EquipmentType)
            return;

        if (equipment.Equals(_mergeEquipment1))
            return;

        if (equipment.Equals(_mergeEquipment2))
            return;

        if (_mergeEquipment1 == null)
        {
            if (_equipment.EquipmentData.MergeEquipmentType1 == MergeEquipmentType.ItemCode)
            {
                if (equipment.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment1)
                    return;
            }
            else if (_equipment.EquipmentData.MergeEquipmentType1 == MergeEquipmentType.Grade)
            {
                if (equipment.EquipmentData.EquipmentGrade != (EquipmentGrade)Enum.Parse(typeof(EquipmentGrade), _equipment.EquipmentData.MergeEquipment1))
                    return;
            }
            else
                return;

            _mergeEquipment1 = equipment;
            if (showUI)
                Refresh_MergeEquip1();
        }
        else if (_mergeEquipment2 == null)
        {
            if (_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.ItemCode)
            {
                if (equipment.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment2)
                    return;
            }
            else if (_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.Grade)
            {
                if (equipment.EquipmentData.EquipmentGrade != (EquipmentGrade)Enum.Parse(typeof(EquipmentGrade), _equipment.EquipmentData.MergeEquipment2))
                    return;
            }
            else
                return;

            _mergeEquipment2 = equipment;
            if (showUI)
                Refresh_MergeEquip2();
        }
        else
            return;

        //Refresh();
        if (showUI)
            CheckEnableMergeButton();

        SortEquipments();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        Refresh_SelectedEquipObject();
        Refresh_MergeEquip1();
        Refresh_MergeEquip2();
        CheckEnableMergeButton();
        SortEquipments();
    }

    void Refresh_SelectedEquipObject()
    {
        if (_equipment == null)
        {
            // �ռ��� ������ ������� ó��
            GetObject((int)GameObjects.SelectedEquipObject).SetActive(false);
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
            GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(true); // ��� �ڸ�Ʈ
            GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false); // ��� �ڸ�Ʈ
            GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false); // �ɼ� ���
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false); // �ƿ�����
            GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
            return;
        }
        else
        {
            GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
            GetImage((int)Images.SelectedEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_equipment.EquipmentData.EquipmentType}_Icon.sprite");

            switch (_equipment.EquipmentData.EquipmentGrade)
            {
                case EquipmentGrade.Common:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;
                case EquipmentGrade.Uncommon:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;
                case EquipmentGrade.Rare:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;
                case EquipmentGrade.Epic:
                case EquipmentGrade.Epic1:
                case EquipmentGrade.Epic2:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;
                case EquipmentGrade.Legendary:
                case EquipmentGrade.Legendary1:
                case EquipmentGrade.Legendary2:
                case EquipmentGrade.Legendary3:
                    GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;

                default:
                    break;
            }

            string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            // Epic1 -> 1 ���� Epic2 ->2 ���� Commonó�� ���ڰ� ������ 0 ����
            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
                num = int.Parse(match.Value);

            if (num == 0)
            {
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "";
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.SelectedEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.SelectedEquipLevelValueText).text = $"Lv.{_equipment.Level}";

            GetObject((int)GameObjects.SelectedEquipObject).SetActive(true);

            GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false); // �ɼ� ���
            GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); // ��� �ڸ�Ʈ
            GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(true); // ��� �ڸ�Ʈ
        }

        if(_equipment.EquipmentData.MergeEquipmentType1 == MergeEquipmentType.None)
        {
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(false);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
        }
        else if(_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.None)
        {

            //��ȭ�� �Ѱ� �ʿ��ҋ�
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
        }
        else
        {
            //����� �ΰ�
            GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
        }
    }

    void Refresh_MergeEquip1()
    {
        if (_mergeEquipment1 == null)
            GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
        else
        {
            GetImage((int)Images.FirstSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment1.EquipmentData.SpriteName);
            GetImage((int)Images.FirstSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment1.EquipmentData.EquipmentType}_Icon.sprite");
            switch (_mergeEquipment1.EquipmentData.EquipmentGrade) 
            {
                case EquipmentGrade.Common:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;

                case EquipmentGrade.Uncommon:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;

                case EquipmentGrade.Rare:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;

                case EquipmentGrade.Epic:
                case EquipmentGrade.Epic1:
                case EquipmentGrade.Epic2:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;

                case EquipmentGrade.Legendary:
                case EquipmentGrade.Legendary1:
                case EquipmentGrade.Legendary2:
                case EquipmentGrade.Legendary3:
                    GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;

                default:
                    break;
            }

            string gradeName = _mergeEquipment1.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            // Epic1 -> 1 ���� Epic2 ->2 ���� Commonó�� ���ڰ� ������ 0 ����
            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
                num = int.Parse(match.Value);

            if (num == 0)
            {
                GetText((int)Texts.FirstSelectEquipEnforceValueText).text = "";
                GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.FirstSelectEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.FirstSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment1.Level}";
            GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(true);
        }
    }

    void Refresh_MergeEquip2()
    {
        if (_mergeEquipment2 == null)
            GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
        else
        {
            GetImage((int)Images.SecondSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment2.EquipmentData.SpriteName);
            GetImage((int)Images.SecondSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment2.EquipmentData.EquipmentType}_Icon.sprite");

            switch (_mergeEquipment2.EquipmentData.EquipmentGrade)
            {
                case EquipmentGrade.Common:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
                    break;

                case EquipmentGrade.Uncommon:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                    break;

                case EquipmentGrade.Rare:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
                    break;

                case EquipmentGrade.Epic:
                case EquipmentGrade.Epic1:
                case EquipmentGrade.Epic2:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                    GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                    break;

                case EquipmentGrade.Legendary:
                case EquipmentGrade.Legendary1:
                case EquipmentGrade.Legendary2:
                case EquipmentGrade.Legendary3:
                    GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                    GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                    break;

                default:
                    break;
            }

            string gradeName = _mergeEquipment2.EquipmentData.EquipmentGrade.ToString();
            int num = 0;

            // Epic1 -> 1 ���� Epic2 ->2 ���� Commonó�� ���ڰ� ������ 0 ����
            Match match = Regex.Match(gradeName, @"\d+$");
            if (match.Success)
                num = int.Parse(match.Value);

            if (num == 0)
            {
                GetText((int)Texts.SecondSelectEquipEnforceValueText).text = "";
                GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText((int)Texts.SecondSelectEquipEnforceValueText).text = num.ToString();
                GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
            }

            GetText((int)Texts.SecondSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment2.Level}";

            GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(true);
        }
    }

    bool CheckEnableMergeButton()
    {
        #region �ռ� ���� ���� �Ǵ�
        if (_equipment == null)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.MurgeStartEffect).gameObject.SetActive(false); // �ռ� ���� ����Ʈ
            GetObject((int)GameObjects.MurgeFinishEffect).gameObject.SetActive(false); // �ռ� �� ����Ʈ
            return false;
        }

        if(_mergeEquipment2 == null && GetButton((int)Buttons.SecondCostButton).gameObject.activeSelf)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false); // �ƿ�����
            GetObject((int)GameObjects.MurgeStartEffect).gameObject.SetActive(false); // �ռ� ���� ����Ʈ
            GetObject((int)GameObjects.MurgeFinishEffect).gameObject.SetActive(false); // �ռ� �� ����Ʈ
            return false;
        }

        if(_mergeEquipment1 == null)
        {
            GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false); // �ƿ�����
            GetObject((int)GameObjects.MurgeStartEffect).gameObject.SetActive(false); // �ռ� ���� ����Ʈ
            GetObject((int)GameObjects.MurgeFinishEffect).gameObject.SetActive(false); // �ռ� �� ����Ʈ
            return false;
        }
        
        GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(true); // �ɼ� ���
        GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); // ��� �ڸ�Ʈ
        GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false); // ��� �ڸ�Ʈ
        GetObject((int)GameObjects.MurgeStartEffect).gameObject.SetActive(true); // �ռ� ���� ����Ʈ
        GetObject((int)GameObjects.MurgeFinishEffect).gameObject.SetActive(false); // �ռ� �� ����Ʈ

        #endregion

        #region �ɼ�
        GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        string mergedItemId = _equipment.EquipmentData.MergedItemCode;
        GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
        GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(true); // �ɼ� ���
        GetText((int)Texts.EquipmentNameText).text = $"{Managers.Data.EquipDataDic[mergedItemId].NameTextID}"; // �̸�
        GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� �ְ� ����
        GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].MaxLevel}"; // �ռ� �� �ְ� ����

        if (Managers.Data.EquipDataDic[mergedItemId].AtkDmgBonus != 0) // ����� ���ݷ��� 0�� �ƴϸ� ����
        {
            // ���ݷ� �ɼ�
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(true);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(false);

            GetText((int)Texts.BeforeATKValueText).text = $"{Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� ���ݷ�
            GetText((int)Texts.AfterATKValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].MaxLevel}"; // �ռ� �� ���ݷ�
        }
        else
        {
            // ü�� �ɼ�
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(false);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(true);

            GetText((int)Texts.BeforeHPValueText).text = Managers.Data.EquipDataDic[_equipment.EquipmentData.DataId].MaxLevel.ToString(); // �ռ� �� ü��
            GetText((int)Texts.AfterHPValueText).text = Managers.Data.EquipDataDic[mergedItemId].MaxLevel.ToString(); // �ռ� �� ü�� 
        }

        // ��޺� ó��
        switch (Managers.Data.EquipDataDic[mergedItemId].EquipmentGrade)
        {
            case EquipmentGrade.Uncommon:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;

                int uncommonGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].UncommonGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].Description}"; // �߰� �ɼ�

                GetText((int)Texts.BeforeGradeValueText).text = "�Ϲ�"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���"; // �ռ� �� ���
                break;
            case EquipmentGrade.Rare:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;

                int rareGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].RareGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].Description}"; // �߰� �ɼ�

                GetText((int)Texts.BeforeGradeValueText).text = "���"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���"; // �ռ� �� ���
                break;
            case EquipmentGrade.Epic:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                int epicGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].EpicGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].Description}"; // �߰� �ɼ�

                GetText((int)Texts.BeforeGradeValueText).text = "���"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "����"; // �ռ� �� ���
                break;
            case EquipmentGrade.Epic1:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

                GetText((int)Texts.BeforeGradeValueText).text = "����"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���� 1"; // �ռ� �� ���
                break;
            case EquipmentGrade.Epic2:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

                GetText((int)Texts.BeforeGradeValueText).text = "���� 1"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���� 2"; // �ռ� �� ���
                break;
            case EquipmentGrade.Legendary:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                int legendaryGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].LegendaryGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].Description}"; // �߰� �ɼ�

                GetText((int)Texts.BeforeGradeValueText).text = "���� 2"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "����"; // �ռ� �� ���
                break;
            case EquipmentGrade.Legendary1:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

                GetText((int)Texts.BeforeGradeValueText).text = "����"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���� 1"; // �ռ� �� ���
                break;
            case EquipmentGrade.Legendary2:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

                GetText((int)Texts.BeforeGradeValueText).text = "���� 1"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���� 2"; // �ռ� �� ���
                break;
            case EquipmentGrade.Legendary3:
                GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.SelectedEquipEnforceValueText).text = "3";

                GetText((int)Texts.BeforeGradeValueText).text = "���� 2"; // �ռ� �� ���
                GetText((int)Texts.AfterGradeValueText).text = "���� 3"; // �ռ� �� ���
                break;

            default:
                break;
        }
        #endregion

        GetButton((int)Buttons.MergeButton).gameObject.SetActive(true);
        //Refresh();
        return true;
    }

    void SortEquipments()
    {
        Managers.Game.SortEquipment(_equipmentSortType);

        GetObject((int)GameObjects.EquipInventoryScrollContentObject).DestroyChilds();

        // ��� ����Ʈ �ۼ�
        foreach (Equipment inventoryEquipmentItem in Managers.Game.OwnedEquipments)
        {
            bool isSelected = false;
            bool isLock = false;
            // ��� ���� ���� (���õ� or ���� �Ұ� or ���� ����)
            if (_equipment != null)
            {
                if (_equipment == inventoryEquipmentItem || _mergeEquipment1 == inventoryEquipmentItem || _mergeEquipment2 == inventoryEquipmentItem) // ���û��� �˻�
                {
                    isSelected = true;
                    continue; // ���õ� ���� ����Ʈ���� ���� #Neo (�ڵ� ���� �� MurgeStartEffect�� �ʹ� ������ ��µ�)
                }
                else if(_mergeEquipment1 != null) // 2�� ��ᰡ ��� �ְų� �ռ� �Ұ����̸� ���
                {
                    if (_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.None || _mergeEquipment2 != null)
                    {
                        isLock = true;
                    }
                }

                if (_equipment.EquipmentData.EquipmentType != inventoryEquipmentItem.EquipmentData.EquipmentType) // Ÿ�� �˻�
                {
                    isLock = true;
                }
                else
                {
                    if (_equipment.EquipmentData.MergeEquipmentType1 != MergeEquipmentType.None && _mergeEquipment1 == null) // 1�� ��� �Ǵ�
                    {
                        if (_equipment.EquipmentData.MergeEquipmentType1 == MergeEquipmentType.ItemCode) // �ռ� ��ᰡ ���� ������ �϶�
                        {
                            if (inventoryEquipmentItem.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment1)
                                isLock = true;
                        }
                        else if (_equipment.EquipmentData.MergeEquipmentType1 == MergeEquipmentType.Grade) // �ռ� ��ᰡ ���� ��� �϶�
                        {
                            if (inventoryEquipmentItem.EquipmentData.EquipmentGrade != (EquipmentGrade)Enum.Parse(typeof(EquipmentGrade), _equipment.EquipmentData.MergeEquipment1))
                                isLock = true;
                        }
                    }

                    if (_equipment.EquipmentData.MergeEquipmentType2 != MergeEquipmentType.None && _mergeEquipment2 == null) // 2�� ��� �Ǵ�
                    {
                        if (_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.ItemCode) // �ռ� ��ᰡ ���� ������ �϶�
                        {
                            if (inventoryEquipmentItem.EquipmentData.DataId != _equipment.EquipmentData.MergeEquipment2)
                                isLock = true;
                        }
                        else if (_equipment.EquipmentData.MergeEquipmentType2 == MergeEquipmentType.Grade) // �ռ� ��ᰡ ���� ��� �϶�
                        {
                            if (inventoryEquipmentItem.EquipmentData.EquipmentGrade != (EquipmentGrade)Enum.Parse(typeof(EquipmentGrade), _equipment.EquipmentData.MergeEquipment2))
                                isLock = true;
                        }
                        if (inventoryEquipmentItem.Level > 1)
                            isLock = true;
                        if (inventoryEquipmentItem.IsEquipped)
                            isLock = true;
                    }
                }
            }

            UI_MergeEquipItem item = Managers.UI.MakeSubItem<UI_MergeEquipItem>(GetObject((int)GameObjects.EquipInventoryScrollContentObject).transform);
            item.OnClickEquipItem = () =>
            {
                inventoryEquipmentItem.IsConfirmed = true;
            };
            item.SetInfo(inventoryEquipmentItem, Define.UI_ItemParentType.EquipInventoryGroup, _scrollRect, isSelected, isLock);
        }
    }

    void OnClickBackButton() // �ǵ��ư��� ��ư
    {
        Managers.Sound.PlayPopupClose();

        gameObject.SetActive(false);
        //Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    void OnClickEquipResultButton()
    {
        Managers.Sound.PlayButtonClick();
        // ��� ���� �ʱ�ȭ 
        _equipment = null;
        _mergeEquipment1 = null;
        _mergeEquipment2 = null;
        //Refresh_SelectedEquipObject();
        //SortEquipments();
        Refresh();
        // #�����ʿ� : ������ ���̵�ƿ� ������ �ʿ�
    }
    void OnClickFirstCostButton()
    {
        Managers.Sound.PlayButtonClick();
        // FirstCostEquipSelectObject : ��Ȱ��ȭ (�ҷ����鼭 ���� ������ ���� ����ؼ� ������Ʈ�� ��Ȱ��ȭ)
        // #�����ʿ� : ������ ���̵�ƿ� ������ �ʿ�
        _mergeEquipment1 = null;
        Refresh();

    }
    void OnClickSecondCostButton()
    {
        Managers.Sound.PlayButtonClick();

        // SecondCostEquipSelectObject : ��Ȱ��ȭ (�ҷ����鼭 ���� �������� ���� ����ؼ� ������Ʈ�� ��Ȱ��ȭ)
        // #�����ʿ� : ������ ���̵�ƿ� ������ �ʿ�
        _mergeEquipment2 = null;
        Refresh();
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
    void OnClickMergeAllButton()
    {
        Managers.Sound.PlayButtonClick();
        StartCoroutine(CoMergeAll());
    }

    IEnumerator CoMergeAll()
    {
        // �ڵ� �ռ� ��ư
        Managers.Game.SortEquipment(EquipmentSortType.Grade);

        Equipment[] equipments = Managers.Game.OwnedEquipments.ToArray();
        List<Equipment> newEquipments = new List<Equipment>();
        for (int i = 0; i < equipments.Length; i++)
        {
            if (equipments[i].EquipmentData.EquipmentGrade > EquipmentGrade.Epic)
                continue;

            if (equipments[i].IsEquipped)
                continue;

            if (equipments[i] != null)
                SetMergeItem(equipments[i], false);

            if (_equipment == null)
                continue;

            for (int j = i; j < equipments.Length; j++)
            {
                if (equipments[j] != null)
                {
                    SetMergeItem(equipments[j], false);
                    if (CheckEnableMergeButton())
                    {
                        Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2, true);
                        if (newItem != null)
                            newEquipments.Add(newItem);
                    }
                }
            }
            if(i % 5 == 0)
                yield return new WaitForEndOfFrame();
            _equipment = null;
            _mergeEquipment1 = null;
            _mergeEquipment2 = null;
        }
        SortEquipments();
        if (newEquipments.Count > 0)
        {
            //AllMergeResultPopup�ʿ�
            Managers.UI.ShowPopupUI<UI_MergeAllResultPopup>().SetInfo(newEquipments);
        }
        Managers.Game.SaveGame();
    }

    void OnClickMergeButton()
    {
        Managers.Sound.PlayButtonClick();
     
        Equipment beforeEquipment = _equipment;
        // �ռ��� �ϰ� �ռ��� ������� �κ��丮�� �߰��Ѵ�.
        Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2);

        UI_MergeResultPopup resultPopup = (Managers.UI.SceneUI as UI_LobbyScene).MergeResultPopupUI;
        resultPopup.SetInfo(beforeEquipment, newItem, OnClosedMergeResultPopup);
        resultPopup.gameObject.SetActive(true);
        
        SortEquipments();
    }

    // �ռ� ��� �˾��� ������ �׼�
    void OnClosedMergeResultPopup()
    {
        // ��� ���� �ʱ�ȭ 
        OnClickEquipResultButton();
    }
}
