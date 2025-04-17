using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_FastRewardPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // RewardItemScrollContentObject ������ ������ �� �θ�ü
    // ClaimCostValueText : ���� ������ ���� �� �Ҹ�Ǵ� ���׹̳� ��
    // EemainingCountValueText : �Ϸ� ���� ����Ʈ Ƚ��

    // ���ö���¡ �ؽ�Ʈ
    // FastRewardPopupTitleText : ���� ����
    // FastRewardCommentText : ���� ���� 300�� ��� ����
    // ADFreeText : Free
    #endregion



    #region Enum
    enum GameObjects
    {
        ContentObject,
        ItemContainer, 
    }

    enum Buttons
    {
        BackgroundButton,
        ADFreeButton,
        ClaimButton,
    }

    enum Texts
    {
        BackgroundText,
        FastRewardPopupTitleText,
        FastRewardCommentText,
        ADFreeText,
        ClaimCostValueText,
        EemainingCommentText,
        EemainingCountValueText,
    }

    OfflineRewardData _offlineRewardData;
    bool _isClaim = false;
    #endregion
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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.ADFreeButton).gameObject.BindEvent(OnClickADFreeButton);
        GetButton((int)Buttons.ADFreeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);

        GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Color.white;

#if UNITY_EDITOR

        //TextBindTest();
#endif


        #endregion
        return true;
    }

    public void SetInfo(OfflineRewardData offlineReward)
    {
        _offlineRewardData = offlineReward;
        Refresh();
    }

    void Refresh()
    {
        GameObject container = GetObject((int)GameObjects.ItemContainer);
        container.DestroyChilds();

        if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0)
        {
            GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("50D500");
            _isClaim = true;
            GetButton((int)Buttons.ClaimButton).GetOrAddComponent<UI_ButtonAnimation>();
            //GetButton((int)Buttons.ClaimButton).enabled = false; //��ư Ȱ��ȭ,
        }
        else 
        {
            GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("989898");
            _isClaim = false;
            //GetButton((int)Buttons.ClaimButton).interactable = true; //��ư ��Ȱ��ȭ,
        }

        UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        int count = (_offlineRewardData.Reward_Gold) * 5;
        item.SetInfo(Define.GOLD_SPRITE_NAME, count);

        UI_MaterialItem scroll = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        scroll.SetInfo("Scroll_Random_Icon", _offlineRewardData.FastReward_Scroll);

        UI_MaterialItem box = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        box.SetInfo("Key_Silver_Icon", _offlineRewardData.FastReward_Box);
        //TODO

        GetText((int)Texts.EemainingCountValueText).text = Managers.Game.FastRewardCountStamina.ToString();
        // �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ADFreeButton).gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ClaimButton).gameObject.GetComponent<RectTransform>());
    }

    void OnClickBackgroundButton() // ��� �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickADFreeButton() // ������ ��ư
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.FastRewardCountAds > 0)
        {
            Managers.Game.FastRewardCountAds--;
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Time.GiveFastOfflineReward(_offlineRewardData);
                Managers.UI.ClosePopupUI(this);
            });
        }
        else
        {
            Managers.UI.ShowToast("���̻� ���� �� �����ϴ�."); 
        }
        
    }
    void OnClickClaimButton() // Ȯ�� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0 && _isClaim)
        {
            Managers.Game.Stamina -= 15;
            Managers.Game.FastRewardCountStamina--;
            // ���׹̳��� �Ҹ��ϰ� �˾��� �ݱ� (���� ������ �˾����� ���� �ޱ�)
            Managers.Time.GiveFastOfflineReward(_offlineRewardData);
            Managers.UI.ClosePopupUI(this);
            Refresh();
        }
        else
            return;
       
    }
}
