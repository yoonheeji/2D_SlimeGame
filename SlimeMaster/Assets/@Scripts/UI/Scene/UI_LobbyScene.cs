using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    #region UI ��� ����Ʈ
    // ���ö���¡ �ؽ�Ʈ
    // ShopToggleText : ����
    // EquipmentToggleText : ���
    // BattleToggleText : ����
    // ChallengeToggleText : ����
    // EvolveToggleText : ��
    #endregion

    #region Enum
    enum GameObjects
    {
        ShopToggleRedDotObject, // �˸� ��Ȳ �� ��� �� �����
        EquipmentToggleRedDotObject,
        BattleToggleRedDotObject,
        //ChallengeToggleRedDotObject,
        //EvolveToggleRedDotObject,

        MenuToggleGroup,
        CheckShopImageObject,
        CheckEquipmentImageObject,
        CheckBattleImageObject,
        //CheckChallengeImageObject,
        //CheckEvolveImageObject,
    }

    enum Buttons
    {
        //UserIconButton, // ���� ���� ���� �˾� ���� ȣ��
    }

    enum Texts
    {

        ShopToggleText,
        EquipmentToggleText,
        BattleToggleText,
        //ChallengeToggleText,
        //EvolveToggleText,
        //UserNameText,
        //UserLevelText,
        StaminaValueText,
        DiaValueText,
        GoldValueText

    }

    enum Toggles
    {
        ShopToggle,
        EquipmentToggle,
        BattleToggle,
        //ChallengeToggle,
        //EvolveToggle,
    }

    enum Images
    {
        Backgroundimage,
    }

    #endregion

    UI_BattlePopup _battlePopupUI;
    bool _isSelectedBattle = false;
    UI_EvolvePopup _evolvePopupUI;
    UI_EquipmentPopup _equipmentPopupUI;
    public UI_EquipmentPopup EquipmentPopupUI { get { return _equipmentPopupUI; } }
    bool _isSelectedEquipment = false;
    UI_ShopPopup _shopPopupUI;
    bool _isSelectedShop = false;
    UI_ChallengePopup _challengePopupUI;
    UI_MergePopup _mergePopupUI;
    public UI_MergePopup MergePopupUI { get { return _mergePopupUI; } }
    UI_EquipmentInfoPopup _equipmentInfoPopupUI;
    public UI_EquipmentInfoPopup EquipmentInfoPopupUI { get { return _equipmentInfoPopupUI; } }
    UI_EquipmentResetPopup _equipmentResetPopupUI;
    public UI_EquipmentResetPopup EquipmentResetPopupUI { get { return _equipmentResetPopupUI; } }
    UI_RewardPopup _rewardPopupUI;
    public UI_RewardPopup RewardPopupUI { get { return _rewardPopupUI; } }
    UI_MergeResultPopup _mergeResultPopupUI;
    public UI_MergeResultPopup MergeResultPopupUI { get { return _mergeResultPopupUI; } }

    public void OnDestroy()
    {
        if(Managers.Game != null)
            Managers.Game.OnResourcesChagned -= Refresh;
    }

    public override bool Init()
    {
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Toggles));
        BindImage(typeof(Images)); 

        // ��� Ŭ�� �� �ൿ
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);

        _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        _equipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
        _battlePopupUI = Managers.UI.ShowPopupUI<UI_BattlePopup>();
        _challengePopupUI = Managers.UI.ShowPopupUI<UI_ChallengePopup>();
        _evolvePopupUI = Managers.UI.ShowPopupUI<UI_EvolvePopup>();
        _equipmentInfoPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        _mergePopupUI = Managers.UI.ShowPopupUI<UI_MergePopup>();
        _equipmentResetPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentResetPopup>();
        _rewardPopupUI = Managers.UI.ShowPopupUI<UI_RewardPopup>();
        _mergeResultPopupUI = Managers.UI.ShowPopupUI<UI_MergeResultPopup>();

        //��ۿ� ���� ContentObject.SetActive()�� ���� ������Ʈ
        TogglesInit();
        GetToggle((int)Toggles.BattleToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickBattleToggle();

        #endregion

        Managers.Game.OnResourcesChagned += Refresh;
        Refresh();

        return true;
    }

    void Refresh()
    {
        GetText((int)Texts.StaminaValueText).text = $"{Managers.Game.Stamina}/{Define.MAX_STAMINA}";
        GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
        GetText((int)Texts.GoldValueText).text = Managers.Game.Gold.ToString();

        // ��� ���� �� �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    #region  Toggle SetActive

    // ��� �ʱ�ȭ
    void TogglesInit()
    {
        #region �˾� �ʱ�ȭ

        _shopPopupUI.gameObject.SetActive(false);
        _equipmentPopupUI.gameObject.SetActive(false);
        _battlePopupUI.gameObject.SetActive(false);
        _challengePopupUI.gameObject.SetActive(false);
        _evolvePopupUI.gameObject.SetActive(false);
        _equipmentInfoPopupUI.gameObject.SetActive(false);
        _mergePopupUI.gameObject.SetActive(false);
        _equipmentResetPopupUI.gameObject.SetActive(false);
        _rewardPopupUI.gameObject.SetActive(false);
        _mergeResultPopupUI.gameObject.SetActive(false);

        #endregion

        #region ��� ��ư �ʱ�ȭ
        // �� Ŭ�� ���� Ʈ���� �ʱ�ȭ
        _isSelectedEquipment = false;
        _isSelectedShop = false;
        _isSelectedBattle = false;
        //GetToggle((int)Toggles.ChallengeToggle).enabled = false; // ���� �� ��Ȱ��ȭ #Neo
        //GetToggle((int)Toggles.EvolveToggle).enabled = false; // ��ȭ �� ��Ȱ��ȭ #Neo

        // ��ư ����� �ʱ�ȭ
        GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.ChallengeToggleRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.EvolveToggleRedDotObject).SetActive(false);

        // ���� ��� ������ �ʱ�ȭ
        GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);
        //GetObject((int)GameObjects.CheckChallengeImageObject).SetActive(false);
        //GetObject((int)GameObjects.CheckEvolveImageObject).SetActive(false);

        GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        //GetObject((int)GameObjects.CheckChallengeImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        //GetObject((int)GameObjects.CheckEvolveImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);


        // �޴� �ؽ�Ʈ �ʱ�ȭ
        GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);
        //GetText((int)Texts.ChallengeToggleText).gameObject.SetActive(false);
        //GetText((int)Texts.EvolveToggleText).gameObject.SetActive(false);

        // ��� ũ�� �ʱ�ȭ
        GetToggle((int)Toggles.ShopToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.EquipmentToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.BattleToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        //GetToggle((int)Toggles.ChallengeToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        //GetToggle((int)Toggles.EvolveToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);

        #endregion

    }

    void ShowUI(GameObject contentPopup, Toggle toggle, TMP_Text text, GameObject obj2, float duration = 0.1f)
    {
        TogglesInit();
        
        contentPopup.SetActive(true);
        toggle.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 150);
        text.gameObject.SetActive(true);
        obj2.SetActive(true);
        obj2.GetComponent<RectTransform>().DOSizeDelta(new Vector2(200, 180), duration).SetEase(Ease.InOutQuad);

        Refresh();
    }

    void OnClickShopToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Util.HexToColor("525DAD"); // ��� ���� ����
        if (_isSelectedShop == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;
        ShowUI(_shopPopupUI.gameObject, GetToggle((int)Toggles.ShopToggle), GetText((int)Texts.ShopToggleText), GetObject((int)GameObjects.CheckShopImageObject));
        _isSelectedShop = true;
    }

    void OnClickEquipmentToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Util.HexToColor("5C254B"); // ��� ���� ����
        if (_isSelectedEquipment == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;

        ShowUI(_equipmentPopupUI.gameObject, GetToggle((int)Toggles.EquipmentToggle), GetText((int)Texts.EquipmentToggleText), GetObject((int)GameObjects.CheckEquipmentImageObject));
        _isSelectedEquipment = true;

        _equipmentPopupUI.SetInfo();
    }

    void OnClickBattleToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Util.HexToColor("1F5FA0"); // ��� ���� ����
        if (_isSelectedBattle == true) // Ȱ��ȭ �� ��� Ŭ�� ����
            return;
        ShowUI(_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        _isSelectedBattle = true;
    }

    #endregion

}
