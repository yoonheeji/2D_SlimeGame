using Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
using static UnityEngine.ParticleSystem;

public class UI_ShopPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // Package
    // StagePackage (��Ű�� �� ����, ���� ��ư�� ������ ���� ���������� ����ǰ�, ��Ű�� ��ư�� ������ �ش� �������� �ش��ϴ� ��ǰ�� �����ϰԵǾ���.
    //      - StagePackageItemTitleText : ��Ű�� �̸� ( "n �������� ��Ű��" )
    //      - StagePackageContentObject : ��Ű�� ������ ������ UI_StagePackageItem�� �� �θ��ü 
    //      - PackageFirstProductItemCountValueText : ù��° ��ǰ ����
    //      - PackageSecondProductItemCountValueText : �ι�° ��ǰ ����
    //      - PackageThirdProductItemCountValueText : ����° ��ǰ ����
    //      - PackageFourthProductItemCountValueText : �׹�° ��ǰ ����
    //      - StagePackageCostValueText : ��Ű�� ����

    // DalyShop
    // RefreshTimerValueText : ���ϸ� ������ ����Ʈ ���ű��� ���� �ð�
    // ���ŵǴ� ���������� ��� �����ؾ��ϴ��� �� �𸣰���. (���ε� ����)

    // Gacha
    //  PickupGachaGroup
    //      - PickupGuaranteedCountSliderObject : õ�� ���� ��Ȳ�� �����̴��� ǥ��
    //      - BuyLimitValueText : �Ⱦ� �����ð� "hh : mm : ss" ���� ǥ��
    //      - PickupGachaGuaranteedCountValueText : õ�� ���ޱ��� ���� Ƚ��
    //  CommonGacha
    //      - CommonGachaCostValueText : �Ϲ� ��í Ű ǥ�� ( �ʿ� / ���� ) 
    //  AdvancedGacha
    //      - AdvancedGuaranteedCountSliderObject : õ�� ���� ��Ȳ�� �����̴��� ǥ��
    //      - AdvancedGachaGuaranteedCountValueText :õ�� ���ޱ��� ���� Ƚ��
    //      - AdvancedGachaCostValueText : ��� ��í Ű ǥ�� ( �ʿ� / ���� ) 

    // DiaShop
    //  FreeDia
    //      - FreeDiaRequestTimeValueText : ���� ������� ���� �ð� ǥ�� ( hh : mm : ss )
    //      - FreeDiaRedDotObject : ���� ������ ���� �� ������ Ȱ��ȭ ( FreeDiaSoldOutObject�� ��Ȱ��ȭ)
    //  First
    //      - FirstDiaCostValueText : ù��° ���̾� ���� ���� 
    //      - FirstDiaProductBonusObject : ù���� ���� ��Ȱ��ȭ
    //  Second
    //      - SecondDiaCostValueText : �ι�° ���̾� ���� ���� 
    //      - SecondDiaProductBonusObject : ù���� ���� ��Ȱ��ȭ
    //  Third
    //      - ThirdDiaCostValueText : ����° ���̾� ���� ���� 
    //      - ThirdDiaProductBonusObject : ù���� ���� ��Ȱ��ȭ
    //  Fourth
    //      - FourthDiaCostValueText : �׹�° ���̾� ���� ���� 
    //      - FourthDiaProductBonusObject : ù���� ���� ��Ȱ��ȭ
    //  Fifth
    //      - FifthDiaCostValueText : �ټ���° ���̾� ���� ���� 
    //      - FifthDiaProductBonusObject : ù���� ���� ��Ȱ��ȭ

    // KeyShop
    //      - AdKeyRequestTimeValueText : ���� ������� ���� �ð� ǥ�� ( hh : mm : ss )
    //      - AdKeyRedDotObject : ���� ������ ���� �� ������ Ȱ��ȭ ( FreeDiaSoldOutObject�� ��Ȱ��ȭ)

    // GoldShop
    //      - FreeGoldRequestTimeValueText : ���� ������� ���� �ð� ǥ�� ( hh : mm : ss )
    //      - FreeGoldRedDotObject : ���� ������ ���� �� ������ Ȱ��ȭ ( FreeDiaSoldOutObject�� ��Ȱ��ȭ)



    // ���ö���¡
    // PackageTitleText : ��Ű�� ����
    // DailyShopTitleText : ���� ����
    // RefreshTimerText : ���� ���� �ð�
    // GachaTitleText : ��í ����
    // DiaShopTitleText : ���̾� ����
    // GoldShopTitleText : ��� ����
    // DiaShopCommentText : ù ���� �� 2��, 1ȸ ����

    // StagePackageTitleText : é�� ��Ű��
    // BeginnerPackageTitleText : ���� ��Ű��

    // BeginnerPackageBuyLimitText : 1ȸ ����
    // BeginnerPackageDiscountText : ��ġ
    // PickupGachaCommentText : �Ⱦ���í �ڸ�Ʈ
    // PickupGachaTitleText : �Ⱦ� ��í Ÿ��Ʋ (��� ��ǰ���� �̸��� �ٸ� ����)
    // CommonGachaTitleText : �Ϲ� ��í
    // AdvancedGachaTitleText : ��� ��í

    // BuyLimitText : ���� �ð�
    // OpenOneButtonText : 1ȸ ����
    // OpenTenButtonText : 10ȸ ����
    // PickupGachaGuaranteedCountText : �Ⱦ� ���� : 
    // AdvancedGachaGuaranteedCountText : ���� :
    // CommonGachaOpenButtonText : ����
    // AdvancedGachaOpenButtonText : ����

    // KeyShopTitleText : ���� ����

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        ShopScrollContent,

        //StagePackageContentObject,

        //PickupGuaranteedCountSliderObject,
        //AdvancedGuaranteedCountSliderObject,

        //FreeDiaSoldOutObject,
        //FreeDiaRedDotObject,

        FreeGoldSoldOutObject,
        FreeGoldRedDotObject,

        //FirstDiaProductBonusObject,
        //SecondDiaProductBonusObject,
        //ThirdDiaProductBonusObject,
        //FourthDiaProductBonusObject,
        //FifthDiaProductBonusObject,

        //OpenTenCostObject,
        //OpenOneCostObject,
        CommonGachaCostObject,
        AdvancedGachaCostObject,
        //StagePackageCostObject,

        AdKeySoldOutObject,
        AdKeyRedDotObject,

        // ��ø� ����
        //DailyShopTitle,
        //DailyShopGroup,
        //PickupGachaGroup,

    }

    enum Buttons
    {
        //StagePackageButton,
        //StagePackagePrevButton,
        //StagePackageNextButton,
        //BeginnerPackageButton,

        //PickupGachaInfoButton,
        //PickupGachaListButton,
        //OpenOneButton,
        //OpenTenButton,
        CommonGachaOpenButton,
        ADCommonGachaOpenButton,
        ADAdvancedGachaOpenButton,
        CommonGachaListButton,
        AdvancedGachaOpenButton,
        AdvancedGachaOpenTenButton,
        AdvancedGachaListButton,
        //FreeDiaButton,
        //FirstDiaProductButton,
        //SecondDiaProductButton,
        //ThirdDiaProductButton,
        //FourthDiaProductButton,
        //FifthDiaProductButton,

        AdKeyButton,
        SilverKeyProductButton,
        GoldKeyProductButton,

        FreeGoldButton,
        FirstGoldProductButton,
        SecondGoldProductButton,
    }

    enum Texts
    {
        //PackageTitleText,
        //DailyShopTitleText,
        //RefreshTimerText,
        //RefreshTimerValueText,
        GachaTitleText,
        //DiaShopTitleText,
        //DiaShopCommentText,
        KeyShopTitleText,
        GoldShopTitleText,

        //StagePackageTitleText,
        //StagePackageItemTitleText,
        //PackageFirstProductItemCountValueText,
        //PackageSecondProductItemCountValueText,
        //PackageThirdProductItemCountValueText,
        //PackageFourthProductItemCountValueText,
        //StagePackageCostValueText,

        //BeginnerPackageTitleText,
        //BeginnerPackageBuyLimitText,
        //BeginnerPackageDiscountText,

        //PickupGachaTitleText,
        //PickupGachaCommentText,
        //PickupGachaGuaranteedCountText,
        //PickupGachaGuaranteedCountValueText,

        CommonGachaTitleText,
        CommonGachaOpenButtonText,
        CommonGachaCostValueText,

        AdvancedGachaTitleText,
        //AdvancedGachaGuaranteedCountText,
        //AdvancedGachaGuaranteedCountValueText,
        AdvancedGachaOpenButtonText,
        AdvancedGachaCostValueText,
        AdvancedGachaTenCostValueText,
        //BuyLimitText,
        //BuyLimitValueText,
        //OpenOneButtonText,
        //OpenTenButtonText,

        //FreeDiaRequestTimeValueText,
        //FirstDiaCostValueText,
        //SecondDiaCostValueText, 
        //ThirdDiaCostValueText, 
        //FourthDiaCostValueText, 
        //FifthDiaCostValueText,

        FreeGoldRequestTimeValueText,
        AdKeyRequestTimeValueText,

        // ��� ����
        FirstGoldProductTitleText,
        FreeGoldTitleText,
        SecondGoldProductTitleText
    }

    enum Images
    {
        //BeginnerPackageFirstlItemImage,
    }
    #endregion

    ScrollRect _scrollRect;

    UI_GachaListPopup _gachaListPopupUI;
    public UI_GachaListPopup GachaListPopupUI
    {
        get
        {
            if(_gachaListPopupUI == null)
            {
                _gachaListPopupUI = Managers.UI.ShowPopupUI<UI_GachaListPopup>();
            }
            return _gachaListPopupUI;
        }
    }
    

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        Refresh();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Managers.Game.Dia += 300;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BRONZE_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], 10);
            Managers.Game.AddMaterialItem(Define.ID_WEAPON_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_GLOVES_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_RING_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BELT_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_ARMOR_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BOOTS_SCROLL, 10);

            Refresh();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_STAMINA], 10);
            Refresh();
        }
    }
#endif
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        // ��ư ��ũ�� ���� (�巡�� �ȵǴ� ��ư�� �ϳ��� �߰� �ʿ�)
        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);


        // ��Ű�� ����
        //GetButton((int)Buttons.StagePackageButton).gameObject.BindEvent(OnClickStagePackageButton);
        //GetButton((int)Buttons.StagePackageButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.StagePackagePrevButton).gameObject.BindEvent(OnClickStagePackagePrevButton);
        //GetButton((int)Buttons.StagePackagePrevButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.StagePackageNextButton).gameObject.BindEvent(OnClickStagePackageNextButton);
        //GetButton((int)Buttons.StagePackageNextButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.BeginnerPackageButton).gameObject.BindEvent(OnClickBeginnerPackageButton);
        //GetButton((int)Buttons.BeginnerPackageButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ��í ����
        //GetButton((int)Buttons.PickupGachaInfoButton).gameObject.BindEvent(OnClickPickupGachaInfoButton);
        //GetButton((int)Buttons.PickupGachaInfoButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.PickupGachaListButton).gameObject.BindEvent(OnClickGachaListButton);
        //GetButton((int)Buttons.PickupGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.OpenOneButton).gameObject.BindEvent(OnClickOpenOneButton);
        //GetButton((int)Buttons.OpenOneButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.OpenTenButton).gameObject.BindEvent(OnClickOpenTenButton);
        //GetButton((int)Buttons.OpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CommonGachaOpenButton).gameObject.BindEvent(OnClickCommonGachaOpenButton);
        GetButton((int)Buttons.CommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ADCommonGachaOpenButton).gameObject.BindEvent(OnClickADCommonGachaOpenButton);
        GetButton((int)Buttons.ADCommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ADAdvancedGachaOpenButton).gameObject.BindEvent(OnClickADAdvancedGachaOpenButton);
        GetButton((int)Buttons.ADAdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.CommonGachaListButton).gameObject.BindEvent(OnClickCommonGachaListButton);
        GetButton((int)Buttons.CommonGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenButton);
        GetButton((int)Buttons.AdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenTenButton);
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaListButton).gameObject.BindEvent(OnClickAdvancedGachaListButton);
        GetButton((int)Buttons.AdvancedGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ��ø� ����
        
        //GetObject((int)GameObjects.DailyShopTitle).gameObject.SetActive(false);
        //GetObject((int)GameObjects.DailyShopGroup).gameObject.SetActive(false);
        //GetObject((int)GameObjects.PickupGachaGroup).gameObject.SetActive(false);
        //GetObject((int)GameObjects.AdvancedGuaranteedCountSliderObject).gameObject.SetActive(false);


        // ���̾� ����
        //GetObject((int)GameObjects.FreeDiaSoldOutObject).gameObject.SetActive(false);
        //GetObject((int)GameObjects.FreeDiaRedDotObject).gameObject.SetActive(false);
        //GetButton((int)Buttons.FreeDiaButton).gameObject.BindEvent(OnClickFreeDiaButton);
        //GetButton((int)Buttons.FreeDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FirstDiaProductButton).gameObject.BindEvent(OnClickFirstDiaProductButton);
        //GetButton((int)Buttons.FirstDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.SecondDiaProductButton).gameObject.BindEvent(OnClickSecondDiaProductButton);
        //GetButton((int)Buttons.SecondDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.ThirdDiaProductButton).gameObject.BindEvent(OnClickThirdDiaProductButton);
        //GetButton((int)Buttons.ThirdDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FourthDiaProductButton).gameObject.BindEvent(OnClickFourthDiaProductButton);
        //GetButton((int)Buttons.FourthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FifthDiaProductButton).gameObject.BindEvent(OnClickFifthDiaProductButton);
        //GetButton((int)Buttons.FifthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ù���� ���ʽ� ó��
        //GetObject((int)GameObjects.FirstDiaProductBonusObject).gameObject.SetActive(false); // ù���� �̷��� �ִٸ� ��Ȱ��ȭ
        //GetObject((int)GameObjects.SecondDiaProductBonusObject).gameObject.SetActive(false); // ù���� �̷��� �ִٸ� ��Ȱ��ȭ
        //GetObject((int)GameObjects.ThirdDiaProductBonusObject).gameObject.SetActive(false); // ù���� �̷��� �ִٸ� ��Ȱ��ȭ
        //GetObject((int)GameObjects.FourthDiaProductBonusObject).gameObject.SetActive(false); // ù���� �̷��� �ִٸ� ��Ȱ��ȭ
        //GetObject((int)GameObjects.FifthDiaProductBonusObject).gameObject.SetActive(false); // ù���� �̷��� �ִٸ� ��Ȱ��ȭ

        // ���� ����
        GetObject((int)GameObjects.AdKeySoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdKeyRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.AdKeyButton).gameObject.BindEvent(OnClickAdKeyButton);
        GetButton((int)Buttons.AdKeyButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SilverKeyProductButton).gameObject.BindEvent(OnClickSilverKeyProductButton);
        GetButton((int)Buttons.SilverKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.GoldKeyProductButton).gameObject.BindEvent(OnClickGoldKeyProductButton);
        GetButton((int)Buttons.GoldKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ��� ����
        GetObject((int)GameObjects.FreeGoldSoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.FreeGoldRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.FreeGoldButton).gameObject.BindEvent(OnClickFreeGoldButton);
        GetButton((int)Buttons.FreeGoldButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FirstGoldProductButton).gameObject.BindEvent(OnClickFirstGoldProductButton);
        GetButton((int)Buttons.FirstGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondGoldProductButton).gameObject.BindEvent(OnClickSecondGoldProductButton);
        GetButton((int)Buttons.SecondGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        // �׽�Ʈ��
#if UNITY_EDITOR

        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        Refresh();
    }

    void Refresh()
    {

        Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int goldKeyCount);
        Managers.Game.ItemDictionary.TryGetValue(Define.ID_SILVER_KEY, out int silverKeyCount);
        Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int bronzeKeyCount);
        
        GetText((int)Texts.CommonGachaCostValueText).text = $"{silverKeyCount}/1";
        GetText((int)Texts.AdvancedGachaCostValueText).text = $"{goldKeyCount}/1";
        GetText((int)Texts.AdvancedGachaTenCostValueText).text = $"{goldKeyCount}/10";
        GetButton((int)Buttons.ADAdvancedGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsAnvanced > 0);
        GetButton((int)Buttons.ADCommonGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsCommon > 0);
        GetObject((int)GameObjects.FreeGoldSoldOutObject).SetActive(Managers.Game.GoldCountAds == 0); // �ֵ�ƿ� ǥ��
        GetObject((int)GameObjects.AdKeySoldOutObject).SetActive(Managers.Game.BronzeKeyCountAds == 0);

        int goldAmount = 0;
        if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        {
            goldAmount = offlineReward.Reward_Gold;
        }

        GetText((int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText((int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText((int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";

        #region �������� ���� ����
        // �������� ���� ����
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.StagePackageCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenTenCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenOneCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenTenButton).gameObject.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenOneButton).gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CommonGachaCostObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AdvancedGachaCostObject).GetComponent<RectTransform>());
        #endregion
    }


    #region Buttons

    #region ��Ű�� ��ư (����)
    //void OnClickStagePackageButton() // é�� ��Ű��
    //{
    //    // ��ǰ ���� ��� ȣ��
    //}
    //void OnClickStagePackagePrevButton() // ���� é�� ��ư
    //{
    //    // ��Ű���� ������ ���� ��Ű�� �������� ����
    //}
    //void OnClickStagePackageNextButton() // ���� é�� ��ư
    //{
    //    // ��Ű���� ������ ���� ��Ű�� �������� ����
    //}
    //void OnClickBeginnerPackageButton() // ���� ��Ű��
    //{
    //    // ��ǰ ���� ��� ȣ��
    //}
    #endregion

    #region ��í ���� ��ư ����
    //void OnClickGachaListButton() // �Ⱦ� ��í Ȯ��ǥ
    //{
    //    GachaListPopupUI.gameObject.SetActive(true);
    //    GachaListPopupUI.SetInfo(Define.GachaType.PickupGacha);
    //}

    //void OnClickPickupGachaInfoButton() // �Ⱦ� ��í ���� ��ư
    //{
    //    Managers.UI.ShowPopupUI<UI_PickupGachaInfoPopup>();
    //}

    //void OnClickOpenOneButton() // �Ⱦ� ��í 1ȸ ����
    //{
    //    Managers.Game.Dia -= 300;
    //    DoGacha(Define.GachaType.PickupGacha, 1);
    //}

    //void OnClickOpenTenButton() // �Ⱦ� ��í 10ȸ ����
    //{
    //    Managers.Game.Dia -= 2750;
    //    DoGacha(Define.GachaType.PickupGacha, 10);
    //}


    void OnClickCommonGachaOpenButton() // �Ϲ� ��í ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_SILVER_KEY, out int keyCount))
        {
            if (keyCount > 0) 
            { 
                Managers.Game.RemovMaterialItem(Define.ID_SILVER_KEY, 1);
                DoGacha(Define.GachaType.CommonGacha, 1);
                Refresh();
            }
            else
            {
                 Managers.UI.ShowToast("���谡 �����մϴ�.");
            }
        }
    }

    void OnClickADCommonGachaOpenButton() // �Ϲ� ��í ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GachaCountAdsCommon > 0)
        { 
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.GachaCountAdsCommon--;
                DoGacha(Define.GachaType.CommonGacha, 1);
                Refresh();
            });
        }
    }

    void OnClickADAdvancedGachaOpenButton() // ��� ��í ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GachaCountAdsAnvanced > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.GachaCountAdsAnvanced--;
                DoGacha(Define.GachaType.CommonGacha, 1);
                Refresh();
            });
        }
    }
    void OnClickCommonGachaListButton() // �Ϲ� ��í ����Ʈ ��ư
    {
        Managers.Sound.PlayButtonClick();
        // �Ϲ� ��í�� ����Ʈ�� �������
        GachaListPopupUI.SetInfo(Define.GachaType.CommonGacha);

    }
   
    void OnClickAdvancedGachaOpenButton() // ��� ��í ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int keyCount))
        {
            if (keyCount > 0)
            {
                Managers.Game.RemovMaterialItem(Define.ID_GOLD_KEY, 1);
                DoGacha(Define.GachaType.AdvancedGacha, 1);
                Refresh();
            }
            else
            {
                Managers.UI.ShowToast("���谡 �����մϴ�.");
            }
        }

    }
   
    void OnClickAdvancedGachaOpenTenButton() // ��� ��í 10ȸ ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int keyCount))
        {
            if (keyCount >= 10)
            {
                Managers.Game.RemovMaterialItem(Define.ID_GOLD_KEY, 10);
                DoGacha(Define.GachaType.AdvancedGacha, 10);
                Refresh();
            }
            else
            {
                Managers.UI.ShowToast("���谡 �����մϴ�.");
            }
        }
    }

    void OnClickAdvancedGachaListButton() // ��� ��í ����Ʈ ��ư
    {
        Managers.Sound.PlayButtonClick();
        // ��� ��í�� ����Ʈ�� �������
        GachaListPopupUI.SetInfo(Define.GachaType.AdvancedGacha);
    }

    void DoGacha(Define.GachaType gachaType, int count = 1)
    {
        List<Equipment> res = new List<Equipment>();
        res = Managers.Game.DoGacha(gachaType, count).ToList();
        if (Managers.Game.DicMission.TryGetValue(MissionTarget.GachaOpen, out MissionInfo mission))
            mission.Progress++; 
        Managers.UI.ShowPopupUI<UI_GachaResultsPopup>().SetInfo(res);
    }
    #endregion

    #region ���̾� ���� ��ư (����)
    //void OnClickFreeDiaButton() // ���� ���̾� ��ư
    //{
    //    // ������ ���� �� ���� �� ��ġ �� ���� ���̾� ���� (���� �ִϳ� ȿ�� �߰� �ʿ�)
    //    GetObject((int)GameObjects.FreeDiaSoldOutObject).gameObject.SetActive(true); // �ֵ�ƿ� ǥ��

    //}
    //void OnClickFirstDiaProductButton() // ù��° ���̾� ���� ��ư
    //{
    //    // ���� ��� ȣ��
    //}
    //void OnClickSecondDiaProductButton() // �ι�° ���̾� ���� ��ư
    //{
    //    // ���� ��� ȣ��
    //}
    //void OnClickThirdDiaProductButton() // ����° ���̾� ���� ��ư
    //{
    //    // ���� ��� ȣ��
    //}
    //void OnClickFourthDiaProductButton() // �׹�° ���̾� ���� ��ư
    //{
    //    // ���� ��� ȣ��
    //}
    //void OnClickFifthDiaProductButton() // �ټ���° ���̾� ���� ��ư
    //{
    //    // ���� ��� ȣ��
    //}
    #endregion

    #region ���� ���� ��ư
    void OnClickAdKeyButton() // ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.BronzeKeyCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                string[] spriteName = new string[1];
                int[] count = new int[1];

                spriteName[0] = Managers.Data.MaterialDic[Define.ID_BRONZE_KEY].SpriteName;
                count[0] = 1;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.BronzeKeyCountAds--;
                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BRONZE_KEY], 1);
                Refresh();
                rewardPopup.SetInfo(spriteName, count);
            });
        }
    }
    void OnClickSilverKeyProductButton() // �ǹ� ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 150)
        {
            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Managers.Data.MaterialDic[Define.ID_SILVER_KEY].SpriteName;
            count[0] = 1;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 150;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], 1);
            Refresh();
            rewardPopup.SetInfo(spriteName, count);
        }
        else
        {
            Managers.UI.ShowToast("���̾ư� �����մϴ�.");
        }
    }

    void OnClickGoldKeyProductButton() // ��� ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 300)
        {
            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Managers.Data.MaterialDic[Define.ID_GOLD_KEY].SpriteName;
            count[0] = 1;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 300;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 1);
            Refresh();
            rewardPopup.SetInfo(spriteName, count);
        }
        else
        {
            Managers.UI.ShowToast("���̾ư� �����մϴ�.");
        }
    }
    #endregion

    #region ��� ���� ��ư
    void OnClickFreeGoldButton() // ���� ��� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.GoldCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                int goldAmount = 0;
                if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
                {
                    goldAmount = offlineReward.Reward_Gold;
                }

                string[] spriteName = new string[1];
                int[] count = new int[1];

                spriteName[0] =Define.GOLD_SPRITE_NAME;
                count[0] = goldAmount;

                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                
                Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
                Managers.Game.GoldCountAds--;
                Refresh();
                rewardPopup.SetInfo(spriteName, count);

            });
        }
    }
    void OnClickFirstGoldProductButton() // ù��° ��� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 300)
        {
            int goldAmount = 0;
            if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
            {
                goldAmount = offlineReward.Reward_Gold * 3;
            }

            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Define.GOLD_SPRITE_NAME;
            count[0] = goldAmount;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 300;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
            rewardPopup.SetInfo(spriteName, count);
        }
        else
        {
            Managers.UI.ShowToast("���̾ư� �����մϴ�.");
        }

    }
    void OnClickSecondGoldProductButton() // �ι�° ��� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Dia >= 500)
        {
            int goldAmount = 0;
            if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
            {
                goldAmount = offlineReward.Reward_Gold * 5;
            }

            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Define.GOLD_SPRITE_NAME;
            count[0] = goldAmount;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.Dia -= 500;
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], goldAmount);
            rewardPopup.SetInfo(spriteName, count);
        }
        else
        {
            Managers.UI.ShowToast("���̾ư� �����մϴ�.");
        }
    }
    #endregion

    #endregion

    #region ��ư ��ũ�� ����
    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
