using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_BattlePopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // StageNameText : ������ ������ �������� ǥ��
    // SurvivalWaveValueText : �ش� ������������ �����ߴ� �ƽ� ���̺� �� (�������� Ŭ���� �� ó�� ��� �ʿ�)
    // StageImage : ������ ������ ���������� �̹���
    // �� ��ư�� �����(RedDotObject) : �������� �˸����� ������ Ȱ��ȭ (��Ȳ ��� �ʿ�)
    // GameStartCostValueText : ���� ��ŸƮ �� �ʿ��� �����̳� ǥ���ϰ� ���ǿ� ���� �ؽ�Ʈ ���� ���� 
    // - �÷��� ���� : #FFFFFF
    // - �÷��� �Ұ��� : #FF1E00
    // PaymentRewardButton : ù���� ������ ���މ�ٸ� ��Ȱ��ȭ

    // ���ö���¡
    // SurvivalWaveText : ���� ���̺�
    // PaymentRewardTextText : ���� ����
    // AccountPassText : ���� �н�
    // DiaPassButtonText : ���̾� �н�
    // MissionButtonText : �̼�
    // SettingButtonText : ����
    // AttendanceCheckButtonText : �⼮
    // GameStartButtonText : START
    // OfflineRewardText : ����
    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        SettingButtonRedDotObject,
        //BattlepassButtonRedDotObject,
        //AccountPassButtonRedDotObject,
        MissionButtonRedDotObject,
        AchievementButtonRedDotObject,
        AttendanceCheckButtonRedDotObject,
        //BagIconImageRedDotObject,
        //EventCenterButtonRedDotObject,
        OfflineRewardButtonRedDotObject,
        GameStartCostGroupObject, // ��������
        SurvivalTimeObject, // ��������
        StageRewardProgressFillArea,
        StageRewardProgressSliderObject,
        FirstClearRewardUnlockObject,
        SecondClearRewardUnlockObject,
        ThirdClearRewardUnlockObject,
        FirstClearRedDotObject,
        SecondClearRedDotObject,
        ThirdClearRedDotObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Buttons
    {
        SettingButton,
        //PaymentRewardButton,
        //AccountPassButton,
        MissionButton,
        AchievementButton,
        AttendanceCheckButton,
        StageSelectButton,
        OfflineRewardButton,
        GameStartButton,

        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
    }

    enum Texts
    {
        StageNameText,
        SurvivalWaveText,
        SurvivalWaveValueText,
        GameStartButtonText,
        GameStartCostValueText,
        OfflineRewardText,

        //PaymentRewardTextText,
        //AccountPassText,
        SettingButtonText,
        MissionButtonText,
        AchievementButtonText,
        AttendanceCheckButtonText,

        FirstClearRewardText,
        SecondClearRewardText,
        ThirdClearRewardText,
    }

    enum Images
    {
        StageImage,
        //StageRewardIconImage, // é�� ���� ����

        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }

    enum RewardBoxState
    {
        Lock,
        Unlock,
        Complete,
        RedDot
    }
    #endregion

    Data.StageData _currentStageData;

    class RewardBox
    {
        public GameObject ItemImage;
        public GameObject UnLockObject;
        public GameObject CompleteObject;
        public GameObject RedDotObject;
        public RewardBoxState state;
    }

    List<RewardBox> _boxes = new List<RewardBox>();

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoCheckContinue());
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        Debug.Log("UI_BattlePopup");
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        // ��ư ����� (�ʱ���� ��Ȱ��ȭ)
        GetObject((int)GameObjects.SettingButtonRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.AccountPassButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.MissionButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AchievementButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.OfflineRewardButtonRedDotObject).SetActive(false);

        // ��ư ��� 
        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.GameStartButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.OfflineRewardButton).gameObject.BindEvent(OnClickOfflineRewardButton);
        GetButton((int)Buttons.OfflineRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.PaymentRewardButton).gameObject.BindEvent(OnClickPaymentRewardButton);
        //GetButton((int)Buttons.PaymentRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.AccountPassButton).gameObject.BindEvent(OnClickAccountPassButton);
        //GetButton((int)Buttons.AccountPassButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MissionButton).gameObject.BindEvent(OnClickMissionButton);
        GetButton((int)Buttons.MissionButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AchievementButton).gameObject.BindEvent(OnClickAchievementButton);
        GetButton((int)Buttons.AchievementButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AttendanceCheckButton).gameObject.BindEvent(OnClickAttendanceCheckButton);
        GetButton((int)Buttons.AttendanceCheckButton).GetOrAddComponent<UI_ButtonAnimation>();

        // ���� ���̺�
        GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
        GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(false);

        // �������� ����
        GetButton((int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton((int)Buttons.FirstClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton((int)Buttons.SecondClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton((int)Buttons.ThirdClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        #endregion

        InitBoxes();
        Refresh();
        return true;
    }

    void Refresh()
    {
        if (Managers.Game.CurrentStageData == null)
        {
            Managers.Game.CurrentStageData = Managers.Data.StageDic[1];
        }

        //Managers.Game.CurrentStageData = Managers.Data.StageDic[Managers.Game.GetMaxStageClearIndex() + 1];

        // StageNameText : ������ ������ �������� ǥ��
        GetText((int)Texts.StageNameText).text = Managers.Game.CurrentStageData.StageName;

        // SurvivalWaveValueText : �ش� ������������ �����ߴ� �ƽ� ���̺� �� (�������� Ŭ���� �� ó�� ��� �ʿ�)
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out StageClearInfo info))
        {
            if (info.MaxWaveIndex == 0)
            {
                //GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = 0;
                GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
            }
            else
                GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
        }
        else
            GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";

        // StageImage : ������ ������ ���������� �̹���
        GetImage((int)Images.StageImage).sprite = Managers.Resource.Load<Sprite>(Managers.Game.CurrentStageData.StageImage);

        // �� ��ư�� �����(RedDotObject) : �������� �˸����� ������ Ȱ��ȭ (��Ȳ ��� �ʿ�)
        // GameStartCostValueText : ���� ��ŸƮ �� �ʿ��� �����̳� ǥ���ϰ� ���ǿ� ���� �ؽ�Ʈ ���� ���� 
        // - �÷��� ���� : #FFFFFF
        // - �÷��� �Ұ��� : #FF1E00
        // PaymentRewardButton : ù���� ������ ���މ�ٸ� ��Ȱ��ȭ

        //if() // ���� ������ ���׹̳ʰ� 5�̸� �϶� 
        //    GetText((int)Texts.GameStartCostText).color = #FFFFFF;
        //else // ���� ������ ���׹̳ʰ� 5�̻� �϶� 
        //    GetText((int)Texts.GameStartCostText).color = #F1331A;

        // �������� ���� ( Ŭ���� ���ǿ� ���� ���� ��ȭ �ʿ�)
        if (info != null)
        {
            _currentStageData = Managers.Game.CurrentStageData;
            int itemCode = _currentStageData.FirstWaveClearRewardItemId;

            //�ڽ�
            InitBoxes();
            SetRewardBoxes(info);
            GetText((int)Texts.FirstClearRewardText).text = $"{_currentStageData.FirstWaveCountValue}";
            GetText((int)Texts.SecondClearRewardText).text = $"{_currentStageData.SecondWaveCountValue}";
            GetText((int)Texts.ThirdClearRewardText).text = $"{_currentStageData.ThirdWaveCountValue}";

            #region ���� ���̺� 
            //�����̴�
            int wave = info.MaxWaveIndex;

            if (info.isClear == true)
            {
                GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("60FF08");
                GetText((int)Texts.SurvivalWaveValueText).text = "�������� Ŭ����";
                GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
            }
            else
            {
                // ó�� ����
                if (info.MaxWaveIndex == 0)
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                }
                // ������
                else
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Util.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
                }

                //// ���ο� ��������
                //if (Managers.Game.DicStageClearInfo.TryGetValue(_currentStageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
                //    return;
                //if (PrevInfo.isClear == true)
                //{
                //    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                //    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                //    GetText((int)Texts.SurvivalWaveValueText).text = "��� ����";
                //    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                //}
            }
            #endregion

        }


        // �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameStartCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SurvivalTimeObject).GetComponent<RectTransform>());
    }

    IEnumerator CoCheckContinue()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerPrefs.GetInt("ISFIRST") == 1)
        {
            Managers.UI.ShowPopupUI<UI_BeginnerSupportRewardPopup>();
            PlayerPrefs.SetInt("ISFIRST", 0);
        }

        if (Managers.Game.ContinueInfo.isContinue == true)
        {
            Managers.UI.ShowPopupUI<UI_BackToBattlePopup>();
        }
        else
        {
            Managers.Game.ClearContinueData();

        }
    }

    void InitBoxes()
    {
        #region init
        RewardBox box = new RewardBox
        {
            ItemImage = GetImage((int)Images.FirstClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.FirstClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.FirstClearRedDotObject).gameObject,
        };
        _boxes.Add(box);

        RewardBox box2 = new RewardBox
        {
            ItemImage = GetImage((int)Images.SecondClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.SecondClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.SecondClearRedDotObject).gameObject,
        };
        _boxes.Add(box2);

        RewardBox box3 = new RewardBox
        {
            ItemImage = GetImage((int)Images.ThirdClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.ThirdClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.ThirdClearRedDotObject).gameObject,
        };
        _boxes.Add(box3);

        #endregion
        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].UnLockObject.SetActive(true);
            _boxes[i].CompleteObject.SetActive(false);
            _boxes[i].RedDotObject.SetActive(false);
        }

    }

    void SetRewardBoxes(StageClearInfo info)
    {
        int wave = info.MaxWaveIndex + 1;

        if (wave < 3)
        {
            InitBoxes();
        }
        else if (wave < 6)
        {
            //ù��° ���� ����
            if (info.isOpenFirstBox == true)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);
        }
        else if (wave < 10)
        {
            // 1 2 ���� ����
            if (info.isOpenFirstBox == true)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);

            if (info.isOpenSecondBox == true)
                SetBoxState(1, RewardBoxState.Complete);
            else
                SetBoxState(1, RewardBoxState.RedDot);
        }
        else
        {
            //��� ���� ����
            if (info.isOpenFirstBox == true)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);

            if (info.isOpenSecondBox == true)
                SetBoxState(1, RewardBoxState.Complete);
            else
                SetBoxState(1, RewardBoxState.RedDot);

            if (info.isOpenThirdBox == true)
                SetBoxState(2, RewardBoxState.Complete);
            else
                SetBoxState(2, RewardBoxState.RedDot);
        }


    }

    void SetBoxState(int index, RewardBoxState state)
    {
        _boxes[index].UnLockObject.SetActive(false);
        _boxes[index].RedDotObject.SetActive(false);
        _boxes[index].CompleteObject.SetActive(false);
        _boxes[index].state = state;

        switch (state)
        {
            case RewardBoxState.Unlock:
                _boxes[index].UnLockObject.SetActive(false);
                break;
            case RewardBoxState.Complete:
                _boxes[index].CompleteObject.SetActive(true);
                break;
            case RewardBoxState.RedDot:
                _boxes[index].RedDotObject.SetActive(true);
                break;
            case RewardBoxState.Lock:
                _boxes[index].UnLockObject.SetActive(true);
                break;

        }
    }

    #region ButtonClick
    void OnClickGameStartButton() // ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = false;
        if (Managers.Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
            return;
        }

        Managers.Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers.Game.DicMission.TryGetValue(MissionTarget.StageEnter, out MissionInfo mission))
            mission.Progress++;
        Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
        //Managers.Game.CurrentStageData = Managers.Data.StageDic[_currentStageInfo];
    }

    void OnClickStageSelectButton() // �������� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();

        UI_StageSelectPopup stageSelectPopupUI = Managers.UI.ShowPopupUI<UI_StageSelectPopup>();

        stageSelectPopupUI.OnPopupClosed = () =>
        {
            Refresh();
        };
        //�������� ���� �����ؼ� ó�� �� �Ŀ� �ֽ� �������� �ҷ����� ó�� �ʿ�.
        //����� �ӽ÷� 1�������� �ҷ����� �س���
        stageSelectPopupUI.SetInfo(Managers.Game.CurrentStageData);
    }

    void OnClickOfflineRewardButton() // �������� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_OfflineRewardPopup>();
    }

    void OnClickSettingButton() // ���� ��ư ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    //void OnClickPaymentRewardButton() // ù���� ���� ��ư
    //{
    //    Managers.UI.ShowPopupUI<UI_FirstPaymentRewardPopup>();
    //}

    //void OnClickAccountPassButton() // ���� �н� ��ư
    //{
    //    Managers.UI.ShowPopupUI<UI_AccountPassPopup>();
    //}

    void OnClickMissionButton() // �̼� ��ư
    {
        Managers.Sound.PlayButtonClick();
        //Managers.Ads.RequestAndLoadRewardedAd();
        Managers.UI.ShowPopupUI<UI_MissionPopup>();
    }

    void OnClickAchievementButton() // ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_AchievementPopup>();
    }

    void OnClickAttendanceCheckButton() // �⼮üũ ��ư
    {
        Managers.Sound.PlayButtonClick();
        UI_CheckOutPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutPopup>();
        popup.SetInfo(Managers.Time.AttendanceDay);
    }

    void OnClickFirstClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[0].state != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenFirstBox = true;
            SetBoxState(0, RewardBoxState.Complete);

            string[] spriteName = new string[1];
            int[] count = new int[1];

            int itemId = _currentStageData.FirstWaveClearRewardItemId;

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteName[0] = materialData.SpriteName;
                count[0] = _currentStageData.FirstWaveClearRewardItemValue;
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);

                Managers.Game.ExchangeMaterial(materialData, count[0]);
                rewardPopup.SetInfo(spriteName, count);

            }

        }
    }

    void OnClickSecondClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[1].state != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenSecondBox = true;
            SetBoxState(1, RewardBoxState.Complete);

            string[] spriteName = new string[1];
            int[] count = new int[1];

            int itemId = _currentStageData.SecondWaveClearRewardItemId;

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteName[0] = materialData.SpriteName;
                count[0] = _currentStageData.SecondWaveClearRewardItemValue;
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.ExchangeMaterial(materialData, count[0]);
                rewardPopup.SetInfo(spriteName, count);
            }
        }
    }

    void OnClickThirdClearRewardButton()
    {
        Managers.Sound.PlayButtonClick();

        if (_boxes[2].state != RewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenThirdBox = true;
            SetBoxState(2, RewardBoxState.Complete);

            string[] spriteName = new string[1];
            int[] count = new int[1];

            int itemId = _currentStageData.ThirdWaveClearRewardItemId;
            Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData);

            //���̺� ����
            spriteName[0] = materialData.SpriteName;
            count[0] = _currentStageData.ThirdWaveClearRewardItemValue;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.ExchangeMaterial(materialData, count[0]);
            rewardPopup.SetInfo(spriteName, count);
        }
    }

    #endregion

}
