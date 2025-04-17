using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;

public class UI_MissionPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����

    // DailyMissionProgressSliderObject : �̼� �Ϸ� �� �����̴� ��� (�ִ� 6, 1�� ���)
    // DailyMissionRemainingTimeText : ���ϸ� �̼� �ʱ�ȭ���� ���� �ð�

    // ���� �̼� ���� ������ ���� (�⺻���� -> ���� ���� ���� ���� -> ���� ���� �Ϸ� ����)
    //      ù���� ���� (2�� �Ϸ�)
    //      - DailyMissionFirstClearRewardItemBackgroundImage : ���� �������� �׵θ� (���� ����)
    //      - �Ϲ�(Common) : #AC9B83
    //      - ���(Uncommon)  : #73EC4E
    //      - ���(Rare) : #0F84FF
    //      - ����(Epic) : #B740EA
    //      - ����(Legendary) : #F19B02
    //      - ��ȭ(Myth) : #FC2302
    //      - DailyMissionFirstClearRewardItemImage : ���� �������� ������
    //      - DailyMissionFirstClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - DailyMissionFirstClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - DailyMissionFirstClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ) 
    // DailyMissionScrollObject : ���ϸ� �̼ǿ� MissionItme�� �� �θ� ��ü


    //      �ι��� ���� (4�� �Ϸ�)
    //      - DailyMissionSecondClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - DailyMissionSecondClearRewardItemImage : ���� �������� ������
    //      - DailyMissionSecondClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - DailyMissionSecondClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - DailyMissionSecondClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)

    //      ������ ���� (6�� �Ϸ�)
    //      - DailyMissionThirdClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - DailyMissionThirdClearRewardItemImage : ���� �������� ������
    //      - DailyMissionThirdClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - DailyMissionThirdClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - DailyMissionThirdClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)


    // �ְ� �̼� ���� ������ ���� (�⺻���� -> ���� ���� ���� ���� -> ���� ���� �Ϸ� ����)
    //      ù���� ���� (2�� �Ϸ�)
    //      - WeeklyMissionFirstClearRewardItemBackgroundImage : ���� �������� �׵θ� (���� ����)
    //      - WeeklyMissionFirstClearRewardItemImage : ���� �������� ������
    //      - WeeklyMissionFirstClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - WeeklyMissionFirstClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - WeeklyMissionFirstClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ) 

    //      �ι��� ���� (4�� �Ϸ�)
    //      - WeeklyMissionSecondClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - WeeklyMissionSecondClearRewardItemImage : ���� �������� ������
    //      - WeeklyMissionSecondClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - WeeklyMissionSecondClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - WeeklyMissionSecondClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)

    //      ������ ���� (6�� �Ϸ�)
    //      - WeeklyMissionThirdClearRewardItemBackgroundImage : : ���� �������� �׵θ� (���� ����)
    //      - WeeklyMissionThirdClearRewardItemImage : ���� �������� ������
    //      - WeeklyMissionThirdClearRewardItemCountValueText : ���� �������� ���� (����)
    //      - WeeklyMissionThirdClearRewardUnlockObject : ���� ���� ������ �� ��Ȱ��ȭ (�⺻ Ȱ��ȭ) 
    //      - WeeklyMissionThirdClearOutlineObject : ���� ���� ������ �� Ȱ��ȭ (�⺻ ��Ȱ��ȭ)
    // WeeklyMissionScrollObject : ���ϸ� �̼ǿ� MissionItme�� �� �θ� ��ü


    // ���ö���¡
    // BackgroundText : ��ġ�Ͽ� �ݱ�
    // DailyMissionTitleText : ���� �̼�
    // WeeklyMissionTitleText : �ְ� �̼�
    // DailyMissionCompleteText : �Ϸ�
    // DailyMissionRemainingTimeValueText : ���� �ð�
    // WeeklyMissionCompleteText : �Ϸ�
    // WeeklyMissionRemainingTimeValueText : ���� �ð�

    #endregion

    #region Enum
    
    enum GameObjects
    {
        ContentObject,
        DailyMissionContentObject,
        DailyMissionScrollObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        DailyMissionTitleText,
        DailyMissionCommentText,

    }

    enum Images
    {
        GradientImage,

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
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        #endregion

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        GetObject((int)GameObjects.DailyMissionScrollObject).DestroyChilds();
        foreach(KeyValuePair<int, MissionData> data in Managers.Data.MissionDataDic)
        {
            if(data.Value.MissionType == Define.MissionType.Daily)
            {
                UI_MissionItem dailyMission = Managers.UI.MakeSubItem<UI_MissionItem>(GetObject((int)GameObjects.DailyMissionScrollObject).transform);
                dailyMission.SetInfo(data.Value);
            }
        }
    }

    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton() 
    {
        Managers.UI.ClosePopupUI(this);
    }
}
