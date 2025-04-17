using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;
using UnityEngine.UI.Extensions;
using System.Linq;
using UnityEngine.UI;
using System;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class UI_StageSelectPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // StageScrollContentObject : UI_ChapterInfoItem�� �� �θ� ��ü
    // AppearingMonsterContentObject : UI_MonsterInfo�� �� �θ� ��ü
    // StageImage : ���������� �̹��� (���̺� �߰� �ʿ�)
    // StageNameValueText : ���������� �̸� (���̺� �߰� �ʿ�)
    // StageRewardProgressSliderObject : �������� Ŭ���� �� �����̴� ���(é���� �ִ� �������� ��, 1�� ���)

    // é�� Ŭ���� ���� ������ ���� (�⺻���� -> ���� ���� ���� ���� -> ���� ���� �Ϸ� ����)
    // FirstClearRewardText : ù��° ���� ���� 
    //      ù���� ����
    //      - FirstClearRewardItemBackgroundImage : ���� �������� �׵θ� (���� ����)
    //      - �Ϲ�(Common) : #AC9B83
    //      - ���(Uncommon)  : #73EC4E
    //      - ���(Rare) : #0F84FF
    //      - ����(Epic) : #B740EA
    //      - ����(Legendary) : #F19B02
    //      - ��ȭ(Myth) : #FC2302
    //      - FirstClearRewardItemImage : ���� �������� ������
    //      - FirstClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - FirstClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - FirstClearRedDotObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ) 


    //      �ι��� ����
    // SecondClearRewardText : �ι�° ���� ���� 
    //      - SecondClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - SecondClearRewardItemImage : ���� �������� ������
    //      - SecondClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - SecondClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - SecondClearRedDotObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)

    //      ������ ����
    // ThirdClearRewardText : ����° ���� ���� 
    //      - ThirdClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - ThirdClearRewardItemImage : ���� �������� ������
    //      - ThirdClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - ThirdClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - ThirdClearRedDotObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)


    // ���ö���¡
    // StageSelectTitleText : ��������
    // AppearingMonsterText : ���� ����
    // StageSelectButtonText : ����

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        AppearingMonsterContentObject,
        StageSelectScrollView,

    }

    enum Buttons
    {
        StageSelectButton,
        BackButton,
    }

    enum Texts
    {
        StageSelectTitleText,
        AppearingMonsterText,
        StageSelectButtonText,

    }
    
    enum Images
    {
        LArrowImage,
        RArrowImage
    }

    #endregion

    StageData _stageData;
    HorizontalScrollSnap _scrollsnap;

    public Action OnPopupClosed;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }
   
    private void OnDisable()
    {
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

        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

        _scrollsnap = Util.FindChild<HorizontalScrollSnap>(gameObject, recursive : true);
        _scrollsnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        _scrollsnap.StartingScreen = Managers.Game.CurrentStageData.StageIndex -1;
        // �׽�Ʈ��
#if UNITY_EDITOR

        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(StageData stageData)
    {
        _stageData = stageData;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_stageData == null)
            return;

        #region �ʱ�ȭ

        //AppearingMonsterContainer.DestroyChilds();
        #region �������� ����Ʈ
        GameObject StageContainer = GetObject((int)GameObjects.StageScrollContentObject);
        StageContainer.DestroyChilds();

        _scrollsnap.ChildObjects = new GameObject[Managers.Data.StageDic.Count];

        foreach (StageData stageData in Managers.Data.StageDic.Values)
        {
            UI_StageInfoItem item = Managers.UI.MakeSubItem<UI_StageInfoItem>(StageContainer.transform);
            item.SetInfo(stageData);
            _scrollsnap.ChildObjects[stageData.StageIndex - 1] = item.gameObject;
        }

        #endregion
        StageInfoRefresh();
        #endregion

    }

    void StageInfoRefresh()
    {
        #region �������� ����
        UIRefresh();
        #endregion

        #region ��ũ�ѵ� �������� �������

        List<int> monsterList = _stageData.AppearingMonsters.ToList();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();
        for (int i = 0; i < monsterList.Count; i++)
        {
            UI_MonsterInfoItem monsterInfoItemUI = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(container.transform);
     
            monsterInfoItemUI.SetInfo(monsterList[i], _stageData.StageLevel, this.transform);
        }
        #endregion
    }

    void UIRefresh()
    {
        // �⺻ ����
        GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
        GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);

        #region �������� ȭ��ǥ
        if (_stageData.StageIndex == 1)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(false);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (_stageData.StageIndex >= 2 && _stageData.StageIndex < 50)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
        }
        else if (_stageData.StageIndex == 50)
        {
            GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
            GetImage((int)Images.RArrowImage).gameObject.SetActive(false);
        }
        #endregion

        #region �������� ���� ��ư
        if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex, out StageClearInfo info) == false)
            return;
        //���� ó�� �����ϰ� ��������â�� ���� �� ���
        if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
        {
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        }
        // �������� ������
        if (info.StageIndex <=_stageData.StageIndex)
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        // ���ο� ��������
        if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
            return;
        if (PrevInfo.isClear == true)
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
        else
            GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        #endregion
    }

    void OnClickStageSelectButton()
    {
        //if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex, out StageClearInfo info) == false)
        //    return;
        ////���� ó�� �����ϰ� ��������â�� ���� �� ���
        //if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
        //{
        //    Managers.Game.CurrentStageData = _stageData;
        //    OnPopupClosed?.Invoke();
        //    Managers.UI.ClosePopupUI(this);
        //}

        //if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
        //    return;
        //if (PrevInfo.isClear == true)
        //{
        //    Managers.Game.CurrentStageData = _stageData;
        //    OnPopupClosed?.Invoke();
        //    Managers.UI.ClosePopupUI(this);
        //}

        Managers.Game.CurrentStageData = _stageData;
        OnPopupClosed?.Invoke();
        Managers.UI.ClosePopupUI(this);

    }
    
    void OnClickBackButton() // �ǵ��� ����
    {
        OnPopupClosed?.Invoke();
        Managers.UI.ClosePopupUI(this);
    }



    void OnChangeStage(int index)
    {
        //���� �������� ����
        _stageData = Managers.Data.StageDic[index + 1];

        int[] monsterData = _stageData.AppearingMonsters.ToArray();

        GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
        container.DestroyChilds();

        for (int i = 0; i < monsterData.Length; i++)
        {
            UI_MonsterInfoItem item = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(GetObject((int)GameObjects.AppearingMonsterContentObject).transform);
            item.SetInfo(monsterData[i], _stageData.StageLevel, this.transform);
            //������ Ÿ�� ����� ó���ؾ��ҵ�
        }

        UIRefresh();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AppearingMonsterContentObject).GetComponent<RectTransform>());
    }



}
