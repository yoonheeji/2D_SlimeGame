using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;
using static Define;
using UnityEngine.UI;
using System;

public class UI_MergeResultPopup : UI_Popup
{
    Equipment _beforeEquipment;
    Equipment _afterEquipment;

    #region UI ��� ����Ʈ
    // ���� ����
    // EquipmentGradeBackgroundImage : �ռ� �� ��� ����� �׵θ� (���� ����)
    //      - �Ϲ�(Common) : #AC9B83
    //      - ���(Uncommon)  : #73EC4E
    //      - ���(Rare) : #0F84FF
    //      - ����(Epic) : #B740EA
    //      - ����(Legendary) : #F19B02
    //      - ��ȭ(Myth) : #FC2302
    // EquipmentEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �̹��� ���� ����
    //      - ����(Epic) : #9F37F2
    //      - ����(Legendary) : #F67B09
    //      - ��ȭ(Myth) : #F1331A
    // EquipmentNameValueText : ����� �̸�
    // EnforceValueText : ���� +1 ���� ��� ����
    // EquipmentImage : ����� ������
    // EquipmentLevelValueText : ����� ���� ����

    // OptionResult (�ɼ� ���)
    //      BeforeLevelValueText : �ռ� �� ��� �ִ� ����
    //      AfterLevelValueText : �ռ� �� ��� �ִ� ����
    //      ImprovATKObject
    //      BeforeATKValueText : �ռ� �� ��� ���ݷ�
    //      AfterATKValueText : �ռ� �� ��� ���ݷ�


    // OptionResult (�ɼ� ���)
    //      BeforeLevelValueText : �ռ� �� ��� �ִ� ����
    //      AfterLevelValueText : �ռ� �� ��� �ִ� ����
    //      ImprovATKObject : ����� ���ݷ� �ɼ��� ������ Ȱ��ȭ
    //      BeforeATKValueText : �ռ� �� ��� ���ݷ�
    //      AfterATKValueText : �ռ� �� ��� ���ݷ�
    //      ImprovHPObject : ����� ü�� �ɼ��� ������ Ȱ��ȭ
    //      BeforeHPValueText : �ռ� �� ��� ü��
    //      AfterHPValueText : �ռ� �� ��� ü��
    //      ImprovOptionValueText : �ռ� �� �߰� �ɼ�

    // ���ö���¡
    // BackgroundText : ���Ͽ� �ݱ�
    // MergeResultCommentText : �ռ� ���
    // ImprovLevelText : �ִ� ����
    // ImprovATKText : ���ݷ�
    // ImprovHPText : ü��
    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        ImprovATKObject,
        ImprovHPObject,

    }
    enum Buttons
    {
        BackgroundButton,
        //ImprovHPObject,
    }

    enum Texts
    {
        BackgroundText,
        MergeResultCommentText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EnforceValueText,
        ImprovLevelText,
        BeforeLevelValueText,
        AfterLevelValueText,
        ImprovATKText,
        BeforeATKValueText,
        AfterATKValueText,
        ImprovHPText,
        BeforeHPValueText,
        AfterHPValueText,
        ImprovOptionValueText,
        EquipmentGradeValueText,
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

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        Managers.Sound.Play(Define.Sound.Effect, "Result_CommonMerge");

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        // �׽�Ʈ��
#if UNITY_EDITOR
        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    Action _closeAction;
    public void SetInfo(Equipment beforeEquipment, Equipment afterEquipment, Action callback = null)
    {
        _beforeEquipment = beforeEquipment;
        _afterEquipment = afterEquipment;
        _closeAction = callback;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
        if (_beforeEquipment == null)
            return;
        if (_afterEquipment == null)
            return;

        #region �⺻ ����
        // EquipmentImage : ����� ������
        Sprite spr = Managers.Resource.Load<Sprite>(_afterEquipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_afterEquipment.EquipmentData.EquipmentType}_Icon.sprite");

        GetImage((int)Images.EquipmentImage).sprite = spr;
        // ��� �̸�
        GetText((int)Texts.EquipmentNameValueText).text = $"{_afterEquipment.EquipmentData.NameTextID}";
        // ��� ����
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{_beforeEquipment.Level}";
        #endregion

        #region ��� ���� + �߰� �ɼ�
        switch (_afterEquipment.EquipmentData.EquipmentGrade)
        {
            case EquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetText((int)Texts.EquipmentGradeValueText).text = "���";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
                int uncommonGradeSkillId = _afterEquipment.EquipmentData.UncommonGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].Description}"; // �߰� �ɼ�
                break;

            case EquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetText((int)Texts.EquipmentGradeValueText).text = "���";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;

                int rareGradeSkillId = _afterEquipment.EquipmentData.RareGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].Description}"; // �߰� �ɼ�
                break;

            case EquipmentGrade.Epic:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetText((int)Texts.EquipmentGradeValueText).text = "����";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                int epicGradeSkillId = _afterEquipment.EquipmentData.EpicGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].Description}"; // �߰� �ɼ�
                break;

            case EquipmentGrade.Epic1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "���� 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                break;

            case EquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.EpicNameColor;
                break;

            case EquipmentGrade.Legendary:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetText((int)Texts.EquipmentGradeValueText).text = "����";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                int legendaryGradeSkillId = _afterEquipment.EquipmentData.LegendaryGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].Description}"; // �߰� �ɼ�
                break;

            case EquipmentGrade.Legendary1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 1";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;

            case EquipmentGrade.Legendary2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 2";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;

            case EquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "���� 3";
                GetText((int)Texts.EquipmentGradeValueText).color = EquipmentUIColors.LegendaryNameColor;
                break;

            default:
                break;
        }
        #endregion

        #region ���� +1 ���� ��� ����
        string gradeName = _afterEquipment.EquipmentData.EquipmentGrade.ToString();
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

        #region �ɼ�
        // �ִ� ����
        GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� ���ݷ�
        GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� ���ݷ�

        // �ɷ�ġ ���
        if (_beforeEquipment.EquipmentData.AtkDmgBonus != 0) // ���ݷ��� 0�� �ƴϸ� ����
        {
            // ���ݷ� �ɼ�
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(true);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(false);

            GetText((int)Texts.BeforeATKValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� ���ݷ�
            GetText((int)Texts.AfterATKValueText).text = $"{Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel}"; // �ռ� �� ���ݷ�
        }
        else
        {
            // ü�� �ɼ�
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(false);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(true);

            GetText((int)Texts.BeforeHPValueText).text = Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel.ToString(); // �ռ� �� ü��
            GetText((int)Texts.AfterHPValueText).text = Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel.ToString(); // �ռ� �� ü�� 
        }
        #endregion
    }

    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        _closeAction?.Invoke();
        gameObject.SetActive(false);
    }
}
