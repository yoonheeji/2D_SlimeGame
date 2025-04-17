using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_OfflineRewardPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // TotalTimeValueText : ������ ������� ������� �ɸ� �ð� ǥ�� (�ִ� 24�ð�, hh:mm:ss ���� ǥ��)
    // ResultGoldValueText : Ŭ������ é�� �ܰ迡 ���� �������� ��� �� �ð��� ��� ( nnnn/h �� ǥ��
    // ResultExpValueText : Ŭ������ é�� �ܰ迡 ���� �������� ��� �� �ð��� ���� ����ġ ( nnnn/h �� ǥ��
    // RewardItemScrollContentObject : �������� ��Ե� �������� �� �θ� ��ü
    // (���, ����ġ, ������, ĳ���� ��ȭ�� ���� ��������)

    // ���ö���¡
    // OfflineRewardPopupTitleText : �������� ����
    // OfflineRewardCommentText : �� ���� ���������� Ŭ�����ϸ� �� ���� ������ ����ϴ�.
    // TotalTimeText : �� �ð�
    // ButtonCommentText : �ִ� 24�ð� ����
    // FastRewardText : ���� ����
    // ClaimButtonText : ���


    #endregion


    #region Enum
    enum GameObjects
    {
        ContentObject,
        RewardItemScrollContentObject,
        OfflineRewardGoldEffect,
    }

    enum Buttons
    {
        BackgroundButton,
        FastRewardButton,
        ClaimButton,

    }

    enum Texts
    {
        BackgroundText,
        OfflineRewardPopupTitleText,
        OfflineRewardCommentText,
        TotalTimeText,
        TotalTimeValueText,
        ResultGoldValueText,
        //ResultExpValueText,
        FastRewardText,
        ClaimButtonText,
        ButtonCommentText,
    }

    #endregion

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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.FastRewardButton).gameObject.BindEvent(OnClickFastRewardButton);
        GetButton((int)Buttons.FastRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);

        GetObject((int)GameObjects.OfflineRewardGoldEffect).SetActive(false);



#if UNITY_EDITOR

        //TextBindTest();
#endif


        #endregion

        Refresh();
        StartCoroutine(CoTimeCheck());
        return true;
    }

    void Refresh()
    {
        // TotalTimeValueText : ������ ������� ������� �ɸ� �ð� ǥ�� (�ִ� 24�ð�, hh:mm:ss ���� ǥ��)
        StopAllCoroutines();


        if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        {
            // ResultGoldValueText : Ŭ������ é�� �ܰ迡 ���� �������� ��� �� �ð��� ��� ( nnnn/h �� ǥ��
            GetText((int)Texts.ResultGoldValueText).text = $"{offlineReward.Reward_Gold} / �ð�";
            // ResultExpValueText : Ŭ������ é�� �ܰ迡 ���� �������� ��� �� �ð��� ���� ����ġ ( nnnn/h �� ǥ��
        }

        GameObject container = GetObject((int)GameObjects.RewardItemScrollContentObject);
        container.DestroyChilds();
        if (Managers.Time.TimeSinceLastReward.TotalMinutes > 10)
        {
            UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
            int count = (int)Managers.Time.CalculateGoldPerMinute(offlineReward.Reward_Gold);
            item.SetInfo(Define.GOLD_SPRITE_NAME, count);
        }

    }

    IEnumerator CoTimeCheck()
    {
        while (true)
        {

            TimeSpan timeSpan = Managers.Time.TimeSinceLastReward;

            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            if (timeSpan == TimeSpan.FromHours(24))
            {
                formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", 24, 0, 0);
            }

            GetText((int)Texts.TotalTimeValueText).text = formattedTime;

            if (timeSpan.TotalMinutes < 10)
            {
                TimeSpan remainingTime = TimeSpan.FromMinutes(10) - timeSpan;

                // Display remaining time
                //�����ð� ǥ��
                string remaining = string.Format("{0:D2}�� {1:D2}��", remainingTime.Minutes, remainingTime.Seconds);
                GetText((int)Texts.ClaimButtonText).text = remaining;
                GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("989898");
                //��ư ��Ȱ��ȭ,

            }
            else
            {
                GetText((int)Texts.ClaimButtonText).text = "�ޱ�";
                GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Util.HexToColor("50D500");
                GetButton((int)Buttons.ClaimButton).GetOrAddComponent<UI_ButtonAnimation>();
                Refresh();
            }
            yield return new WaitForSeconds(1);

        }
    }
    void OnClickBackgroundButton() // ��� �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);

    }
    void OnClickFastRewardButton() // ���� ������ ��ư
    {
        Managers.Sound.PlayButtonClick();

        // ���� ���� �˾� ȣ��
        if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        {
            UI_FastRewardPopup popup = Managers.UI.ShowPopupUI<UI_FastRewardPopup>();
            popup.SetInfo(offlineReward);
        }

    }
    void OnClickClaimButton() // ���� ��ư (���׹̳� ���)
    {
        Managers.Sound.PlayButtonClick();
        

        if (Managers.Time.TimeSinceLastReward.TotalMinutes < 10)
        {
            return;
        }
        if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        {
            GetObject((int)GameObjects.OfflineRewardGoldEffect).SetActive(true);
            Managers.Time.GiveOfflineReward(offlineReward);
        }
        
        Refresh();
        Managers.UI.ClosePopupUI(this);
    }
}
