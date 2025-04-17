using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;
using UnityEngine.UI;

public class UI_MissionItem : UI_Base
{
    #region UI ��� ����Ʈ
    // ���� ����
    // RewardItmeIconImage : ���� �������� ������
    // RewardItmeValueText : ���� �������� ����
    // ProgressSliderObject : �̼��� ���൵ �����̴��� ǥ��

    // MissionNameValueText : �̼��� �̸�
    // MissionProgressValueText : �̼��� ���൵ (���� / ��ǥ)


    // ���ö���¡
    // ProgressText  
    #endregion
    #region Enum
    enum GameObjects
    {
        ProgressSliderObject,
    }

    enum Buttons
    {
        GetButton,
    }

    enum Texts
    {
        RewardItemValueText,
        ProgressText,
        CompleteText,
        MissionNameValueText,
        MissionProgressValueText,
    }

    enum Images
    {
        RewardItmeIconImage,
    }

    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }
    #endregion

    MissionData _missionData;

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

        GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
        GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();

        AchievementContentInit();



        // �׽�Ʈ��
#if UNITY_EDITOR
        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(MissionData missionData)
    {
        transform.localScale = Vector3.one;
        _missionData = missionData;

        Refresh();
    }

    void Refresh()
    {
        // �̼� Ŭ���� ���¿� ���� Ȱ��ȭ
        //      - ProgressText : ������
        //      - GetButton : Ŭ���� ��
        //      - CompleteText : ���� ���� �Ϸ�

        if (_missionData == null)
            return;

        GetText((int)Texts.RewardItemValueText).text = $"{_missionData.RewardValue}";
        GetText((int)Texts.MissionNameValueText).text = $"{_missionData.DescriptionTextID}";
        GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = 0;

        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo missionInfo))
        { 
            if (missionInfo.Progress > 0)
                GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = (float)missionInfo.Progress / _missionData.MissionTargetValue;

            if (missionInfo.Progress >= _missionData.MissionTargetValue)
            {
                SetButtonUI(MissionState.Complete);
                if (missionInfo.IsRewarded == true)
                    SetButtonUI(MissionState.Rewarded);
            }
            else
            {
                SetButtonUI(MissionState.Progress);
            }
            GetText((int)Texts.MissionProgressValueText).text = $"{missionInfo.Progress}/{_missionData.MissionTargetValue}";
        }
        string sprName = Managers.Data.MaterialDic[_missionData.ClearRewardItmeId].SpriteName;
        GetImage((int)Images.RewardItmeIconImage).sprite = Managers.Resource.Load<Sprite>(sprName);
    }

    void SetButtonUI(MissionState state)
    {
        GameObject objComplte = GetButton((int)Buttons.GetButton).gameObject;
        GameObject objProgress = GetText((int)Texts.ProgressText).gameObject;
        GameObject objRewarded = GetText((int)Texts.CompleteText).gameObject;
        
        switch (state)
        {
            case MissionState.Rewarded:
                objRewarded.SetActive(true);
                objComplte.SetActive(false);
                objProgress.SetActive(false);
                break;
            case MissionState.Complete:
                objRewarded.SetActive(false);
                objComplte.SetActive(true);
                objProgress.SetActive(false);
                break;
            case MissionState.Progress:
                objRewarded.SetActive(false);
                objComplte.SetActive(false);
                objProgress.SetActive(true);
                GetText((int)Texts.ProgressText).text = $"������";

                break;
        }
    }
    void AchievementContentInit()
    {
        GetButton((int)Buttons.GetButton).gameObject.SetActive(true); // �ӽ÷� Ȱ��ȭ
        GetText((int)Texts.ProgressText).gameObject.SetActive(false);
        GetText((int)Texts.CompleteText).gameObject.SetActive(false);
    }

    void OnClickGetButton() // ���� �ޱ� ��ư
    {
        Managers.Sound.PlayButtonClick();
        string[] spriteName = new string[1];
        int[] count = new int[1];

        spriteName[0] = Managers.Data.MaterialDic[Define.ID_DIA].SpriteName;
        count[0] = _missionData.RewardValue;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        Managers.Game.Dia += _missionData.RewardValue;
        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo info))
        { 
            info.IsRewarded = true;
        }
        Refresh();

        rewardPopup.SetInfo(spriteName, count);

    }

}
