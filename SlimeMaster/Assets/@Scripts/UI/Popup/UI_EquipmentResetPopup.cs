using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;
using static Define;

public class UI_EquipmentResetPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // TargetEquipment ( �ʱ�ȭ �� ��� )
    //      TargetEquipmentGradeBackgroundImage : : ��� ����� ��� ǥ�� �� ������ ��޿� ���߾� ����
    //          - �Ϲ�(Common) : #A2A2A2
    //          - ���(Uncommon)  : #57FF0B
    //          - ���(Rare) : #2471E0
    //          - ����(Epic) : #9F37F2
    //          - ����(Legendary) : #F67B09
    //          - ��ȭ(Myth) : #F1331A
    //      TargetEquipmentImage : ��� ��� ������
    //      TargetEquipmentLevelValueText : ��� ��� ����
    //      TargetEquipmentEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //          - ����(Epic) : #B740EA
    //          - ����(Legendary) : #F19B02
    //          - ��ȭ(Myth) : #FC2302 
    //      TargetEnforceValueText : ����� �ܰ�

    // ResetInfoGroup
    // ResetResultGroup ( ���� ��� )
    //      ResultEquipmentGradeBackgroundImage : �ʱ�ȭ�� ����� ��� ǥ�� �� ������ ��޿� ���߾� ����
    //      ResultEquipmentImage : ���� ��� ������
    //      ResultEquipmentEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      ResultEnforceValueText : ����� ����
    // ResultGold ( ���� ��� )
    //      ResultGoldCountValueText : ���� �� �������� ��� �� (��ȭ �ܰ� �����)
    // ResultMaterial ( ���� ��� )
    //      ResultMaterialCountValueText : ���� �� �������� ��� �� (��ȭ �ܰ� �����)

    //#new
    // DowngradeTargetEquipment ( �ٿ�׷��̵� �� ��� )
    //      DowngradeTargetEquipmentGradeBackgroundImage : ��� ����� ��� ǥ�� �� ������ ��޿� ���߾� ����
    //      DowngradeTargetEquipmentImage : : ��� ��� ������
    //      DowngradeTargetEquipmentLevelValueText : ��� ��� ����
    //      DowngradeTargetEquipmentEnforceBackgroundImage : ���� +1 ��޺��� Ȱ��ȭ�ǰ� ��޿� ���� �÷� ����
    //      DowngradeTargetEnforceValueText : ����� �ܰ�

    // ��� ������ ������ �ʱ�ȭ
    // ��� �ٿ� �׷��̵�� ���� �ʱ�ȭ + ��� �ٿ�
    // DowngradeResultGroup 
    // DowngradeEquipment ( �ٿ�׷��̵� ���� ��� )
    //      DowngradeEquipmentGradeBackgroundImage : �ʱ�ȭ�� ����� ��� ǥ�� �� ������ ��޿� ���߾� ����
    //      DowngradeEquipmentImage : ���� ��� ������
    // DowngradEnchantStone  (���� ��ȭ��)
    //      DowngradEnchantStoneBackgroundImage : ��ȭ���� ��� ǥ�� �� ������ ��޿� ���߾� ����
    //      DowngradEnchantStoneImage : ��ȭ�� �̹���
    //      DowngradEnchantStoneCountValueText : ��ȭ�� ����
    // DowngradeGold ( ���� ��� )
    //      DowngradeResultGoldCountValueText : ���� �� �������� ��� �� (��ȭ �ܰ� �����)
    // DowngradeMaterial ( ���� ��� )
    //      DowngradeResultMaterialCountValueText : ���� �� �������� ��� �� (��ȭ �ܰ� �����)

    // ���ö���¡
    // BackgroundText : ��ġ�Ͽ� �ݱ�
    // EquipmentResetPopupTitleText : ��� ����
    // ResetInfoCommentText : ���� �ʱ�ȭ �� ������ ���� ��带 100% �����帳�ϴ�.
    // DowngradeCommentText : ���� �ʱ�ȭ �� ������ ���� ��带 100% �����帳�ϴ�.
    // EquipmentResetButtonText : ���� �ʱ�ȭ
    // EquipmentDowngradeButtonText : �ٿ�׷��̵�
    // ResetTapToggleText : �ʱ�ȭ
    // DowngradeTapToggleText : �ٿ�׷��̵�


    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        ToggleGroupObject,
        TargetEquipment,

        ResetInfoGroupObject,
        DowngradeGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipmentDowngradeButton,
    }

    enum Texts
    {
        BackgroundText,
        EquipmentResetPopupTitleText,
        ResetInfoCommentText,
        DowngradeCommentText,
        EquipmentResetButtonText,
        ResultGoldCountValueText,
        ResultMaterialCountValueText,
        TargetEquipmentLevelValueText,
        TargetEnforceValueText,
        ResultEnforceValueText,

        ResetTapToggleText,
        DowngradeTapToggleText,

        DowngradeTargetEquipmentLevelValueText,
        DowngradeTargetEnforceValueText,
        DowngradEnchantStoneCountValueText,
        DowngradeResultGoldCountValueText,
        DowngradeResultMaterialCountValueText,
        EquipmentDowngradeButtonText,
    }
    enum Images
    {
        TargetEquipmentGradeBackgroundImage,
        TargetEquipmentImage,
        TargetEquipmentEnforceBackgroundImage,
        ResultEquipmentGradeBackgroundImage,
        ResultEquipmentImage,
        ResultEquipmentEnforceBackgroundImage,
        ResultMaterialImage,

        DowngradeTargetEquipmentGradeBackgroundImage,
        DowngradeTargetEquipmentImage,
        DowngradeTargetEquipmentEnforceBackgroundImage,
        DowngradeEquipmentGradeBackgroundImage,
        DowngradeEquipmentImage,
        DowngradEnchantStoneBackgroundImage,
        DowngradEnchantStoneImage,
        DowngradeResultMaterialImage,
    }

    enum Toggles
    {
        ResetTapToggle,
        DowngradeTapToggle,
    }
    #endregion

    bool _isSelectedResetTapTap = false;
    bool _isSelectedDowngradeTapTap = false;
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
        BindToggle(typeof(Toggles));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton);
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipmentDowngradeButton).gameObject.BindEvent(OnClickEquipmentDowngradeButton);
        GetButton((int)Buttons.EquipmentDowngradeButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetToggle((int)Toggles.ResetTapToggle).gameObject.BindEvent(OnClickResetTapToggle);
        GetToggle((int)Toggles.DowngradeTapToggle).gameObject.BindEvent(OnClickDowngradeTapToggle);

        EquipmentResetPopupContentInit();
        OnClickResetTapToggle();

        // �׽�Ʈ��
#if UNITY_EDITOR
        //TextBindTest();
#endif
        #endregion

        //Refresh();
        return true;
    }

    public void SetInfo(Equipment equipment)
    {

        _equipment = equipment;
        Refresh();
        OnClickResetTapToggle();

    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_equipment == null)
        {

            GetObject((int)GameObjects.TargetEquipment).SetActive(false);
        }
        else
        {
            EquipmentResetRefresh();
        }

        // �Ϲ� ��� ��� ó�� (�ٿ�׷��̵� �Ұ�����)
        if (_equipment.EquipmentData.EquipmentGrade == EquipmentGrade.Common)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if (_equipment.EquipmentData.EquipmentGrade == EquipmentGrade.Uncommon)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if(_equipment.EquipmentData.EquipmentGrade == EquipmentGrade.Rare)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if(_equipment.EquipmentData.EquipmentGrade == EquipmentGrade.Epic)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if(_equipment.EquipmentData.EquipmentGrade == EquipmentGrade.Legendary)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else
        {
            EquipmentDowngradeRefresh();
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(true);
        }

        OnClickResetTapToggle();
    }

    // �ʱ�ȭ ��������
    void EquipmentResetRefresh()
    {
        GetImage((int)Images.TargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.TargetEquipmentLevelValueText).text = $"Lv. {_equipment.Level}";

        GetImage((int)Images.ResultEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.ResultGoldCountValueText).text = $"{CalculateResetGold()}";

        GetImage((int)Images.ResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);
        GetText((int)Texts.ResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case EquipmentGrade.Common:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
                break;
            case EquipmentGrade.Uncommon:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case EquipmentGrade.Rare:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case EquipmentGrade.Epic:
            case EquipmentGrade.Epic1:
            case EquipmentGrade.Epic2:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                break;
            case EquipmentGrade.Legendary:
            case EquipmentGrade.Legendary1:
            case EquipmentGrade.Legendary2:
            case EquipmentGrade.Legendary3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                break;
            case EquipmentGrade.Myth:
            case EquipmentGrade.Myth1:
            case EquipmentGrade.Myth2:
            case EquipmentGrade.Myth3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.MythBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = EquipmentUIColors.Myth;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = EquipmentUIColors.MythBg;
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
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.TargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetText((int)Texts.ResultEnforceValueText).text = num.ToString();
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }

    // �ٿ�׷��̵� ��������
    void EquipmentDowngradeRefresh()
    {
        GetImage((int)Images.DowngradeTargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeTargetEquipmentLevelValueText).text = $"Lv. {_equipment.Level}";

        GetImage((int)Images.DowngradeEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeResultGoldCountValueText).text = $"{CalculateResetGold()}";

        GetImage((int)Images.DowngradEnchantStoneImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.EquipDataDic[_equipment.EquipmentData.DowngradeMaterialCode].SpriteName);
        GetText((int)Texts.DowngradEnchantStoneCountValueText).text = $"{_equipment.EquipmentData.DowngradeMaterialCount}";
        
        GetImage((int)Images.DowngradeResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);
        GetText((int)Texts.DowngradeResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case EquipmentGrade.Epic1:
            case EquipmentGrade.Epic2:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
                GetImage((int)Images.DowngradEnchantStoneBackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case EquipmentGrade.Legendary1:
            case EquipmentGrade.Legendary2:
            case EquipmentGrade.Legendary3:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradEnchantStoneBackgroundImage).color = EquipmentUIColors.Legendary;
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
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.DowngradeTargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }

    void EquipmentResetPopupContentInit()
    {
        _isSelectedResetTapTap = false;
        _isSelectedDowngradeTapTap = false;

        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(false);
    }

    int CalculateResetGold()
    {
        int gold = 0;
        for (int i = 1; i < _equipment.Level; i++)
        {
            gold += Managers.Data.EquipLevelDataDic[i].UpgradeCost;
        }
        return gold;
    }

    int CalculateResetMaterialCount()
    {
        int materialCount = 0;
        for (int i = 1; i < _equipment.Level; i++)
        {
            materialCount += Managers.Data.EquipLevelDataDic[i].UpgradeRequiredItems;
        }

        return materialCount;
    }

    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayPopupClose();
        //TODO �˾��� �̷��԰����ϴ°� �´��� �����
        OnClickResetTapToggle();
        gameObject.SetActive(false);
        //Managers.UI.ClosePopupUI(this);

    }

    // ��� ���� ��ư
    void OnClickEquipmentResetButton()
    {
        // 1. gold data -> ���� ������ �ִ� gold���� + gold data
        int gold = CalculateResetGold();

        //2. material data -> ���� ������ �ִ� material���� + material data
        int materialCount = CalculateResetMaterialCount();

        //3. level data -> ���� �����Ϳ� ����
        _equipment.Level = 1;

        string[] spriteName = new string[2];
        int[] count = new int[2];

        spriteName[0] = Define.GOLD_SPRITE_NAME;
        count[0] = gold;

        int materialCode = _equipment.EquipmentData.LevelupMaterialID;
        spriteName[1] = Managers.Data.MaterialDic[materialCode].SpriteName;
        count[1] = materialCount;

        //RewardPopup ���� ResetPopup ����
        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);

        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[materialCode], materialCount);
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);

        // ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
        gameObject.SetActive(false);

        rewardPopup.SetInfo(spriteName, count);

    }

    // ��� �ٿ�׷��̵� ��ư
    void OnClickEquipmentDowngradeButton()
    {
        if (_equipment.EquipmentData.DowngradeEquipmentCode == null)
            return;

        if (Managers.Data.EquipDataDic.TryGetValue(_equipment.EquipmentData.DowngradeEquipmentCode, out Data.EquipmentData downgradedEquip) == false)
            return;

        int gold = 0, materialCount = 0;
        // 1. ���� �ʱ�ȭ�� �� �� ������ �Ѵ�.
        if (_equipment.Level > 1)
        {
            gold = CalculateResetGold();
            materialCount = CalculateResetMaterialCount();
        }
        // ���õ� ����� �Ʒ��ܰ� ��� Add�Ѵ�.
        Managers.Game.AddEquipment(downgradedEquip.DataId);

        // DowngradeMaterialCode�� ������ŭ �κ��丮�� �ִ´�.
        for (int i = 0; i < _equipment.EquipmentData.DowngradeMaterialCount; i++)
        {
            Managers.Game.AddEquipment(_equipment.EquipmentData.DowngradeMaterialCode);
        }
        //Managers.Game.AddMaterialItem(_equipment.EquipmentData.DowngradeMaterialCode, _equipment.EquipmentData.DowngradeMaterialCount);

        // ���õ� ��� �����Ѵ�.
        Managers.Game.OwnedEquipments.Remove(_equipment);

        List<string> spriteNameList = new List<string>();
        List<int> count = new List<int>();
        //���, ���׸���, �Ʒ��ܰ� ���, ��ȭ�� 
        if (gold > 0) 
        {
            spriteNameList.Add(Define.GOLD_SPRITE_NAME);
            count.Add(gold);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);
        }
        if (materialCount > 0)
        {
            int materialCode = _equipment.EquipmentData.LevelupMaterialID;
            spriteNameList.Add(Managers.Data.MaterialDic[materialCode].SpriteName);
            count.Add(materialCount);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[materialCode], materialCount);
        }
        
        spriteNameList.Add(Managers.Data.EquipDataDic[_equipment.EquipmentData.DowngradeMaterialCode].SpriteName);
        count.Add(_equipment.EquipmentData.DowngradeMaterialCount);

        spriteNameList.Add(downgradedEquip.SpriteName);
        count.Add(1);

        //RewardPopup ���� ResetPopup ����
        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        rewardPopup.SetInfo(spriteNameList.ToArray(), count.ToArray());

        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);


        // ��� �ٿ�׷��̵��ϰ� ���°���� ���� �������� ����
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);
        // ��ư ���� ��  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
        gameObject.SetActive(false);

    }

    void OnClickResetTapToggle() // ���� ���
    {
        if (_isSelectedResetTapTap == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;
        EquipmentResetPopupContentInit();
        _isSelectedResetTapTap = true;

        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.ResetTapToggle).isOn = true;
    }

    void OnClickDowngradeTapToggle() // �ٿ�׷��̵� ���
    {
        if (_isSelectedDowngradeTapTap == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;
        EquipmentResetPopupContentInit();
        _isSelectedDowngradeTapTap = true;

        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.DowngradeTapToggle).isOn = true;
    }
}
