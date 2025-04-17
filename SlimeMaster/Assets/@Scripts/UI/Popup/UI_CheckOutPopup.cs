using Data;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_CheckOutPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // 30���� �⼮���� �������� 10�ϸ��� ���带 �ʱ�ȭ �ϰ� 15��, 20��, 30�϶� �߰� ������ �����Ѵ�.
    // CheckOutBoardObject : 10���� UI_CheckOutItem�� �� �θ�ü (�⼮ ����)
    // CheckOutProgressSliderObject : 30�� ���� ���� �⼮���� �����̴��� ǥ��


    // FirstClearRewardBackgroundImage : ����������� ��޿� ���� ���󺯰� (15��)
    // FirstClearRewardItemImage : ���� �������� �̹��� (15��) 
    // FirstClearRewardCountText : ���� �������� ���� (15��)

    // SecondClearRewardBackgroundImage : ����������� ��޿� ���� ���󺯰� (20��)
    // SecondClearRewardItemImage : ���� �������� �̹��� (20��)
    // SecondClearRewardCountText : ���� �������� ���� (20��)

    // ThirdClearRewardBackgroundImage : ����������� ��޿� ���� ���󺯰� (30��)
    // ThirdClearRewardItemImage : ���� �������� �̹��� (30��)
    // ThirdClearRewardCountText : ���� �������� ���� (30��)

    // FirstClearRedDotObject : �⼮�ϼ� ���� ���� �� Ȱ��ȭ (15��)
    // SecondClearRedDotObject : �⼮�ϼ� ���� ���� �� Ȱ��ȭ (20��)
    // ThirdClearRedDotObject : �⼮�ϼ� ���� ���� �� Ȱ��ȭ (30��)

    // FirstClearRewardCompleteObject : ������ �����ϰ� Ȱ��ȭ (15��)
    // SecondClearRewardCompleteObject : ������ �����ϰ� Ȱ��ȭ (20��)
    // ThirdClearRewardCompleteObject : ������ �����ϰ� Ȱ��ȭ (30��)


    // ���ö���¡
    // CheckOutPopupTitleText : �⼮ üũ
    // CheckOutDescriptionText : �⼮�ϼ� 30���� ������ ���尡 �ʱ�ȭ�˴ϴ�.
    // BackgroundText : ��ġ�Ͽ� �ݱ�

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        CheckOutProgressSliderObject,
        CheckOutBoardObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        CheckOutPopupTitleText,

        FirstClearRewardCountText,
        SecondClearRewardCountText,
        ThirdClearRewardCountText,
        DaysCountText,
        CheckOutDescriptionText,
    }
    enum Images
    {
        FirstClearRewardBackgroundImage,
        SecondClearRewardBackgroundImage,
        ThirdClearRewardBackgroundImage,

        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }
    #endregion

    public int _userCheckOutDay; 
    int _monthlyCount; 
    int _dailyCount;
    Transform _makeSubItemParents;

    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        //Refresh();
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
        GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(false);
        #endregion

        Refresh();
        return true;
    }


    public void SetInfo(int checkOutDay)
    {
        _userCheckOutDay = checkOutDay;
        Debug.Log(_userCheckOutDay);
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_userCheckOutDay == 0)
            return;


        _monthlyCount = _userCheckOutDay % 30;
        _dailyCount = _monthlyCount % 10;
        // �⼮�� ī��Ʈ ���
        // �������� 0�̸� 10�Ϸ� ����
        if (_dailyCount == 0)
        {
            _dailyCount = 10;
        }

        // 10�� ������ �ʱ�ȭ
        GetObject((int)GameObjects.CheckOutBoardObject).DestroyChilds();
        _makeSubItemParents = GetObject((int)GameObjects.CheckOutBoardObject).transform;
        // dailyCount ���� ���� SetInfo�� true���� �Ѱ���
        for (int count = 1; count <= 10; count++)
        {
            UI_CheckOutItem item = Managers.UI.MakeSubItem<UI_CheckOutItem>(_makeSubItemParents);
            item.transform.SetAsLastSibling();
            if (_dailyCount >= count)
                item.SetInfo(count, true);
            else
                item.SetInfo(count, false);
        }

        // ���� ���� �ʱ�ȭ
        if (_monthlyCount >= 10 && _monthlyCount < 20) // 10��
        {
            GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
        }
        else if (_monthlyCount >= 20 && _monthlyCount < 30) // 20��
        {
            GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true);
        }
        else if (_monthlyCount >= 30) // 30��
        {
            GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(true);
        }

        GetText((int)Texts.DaysCountText).text = $"{_monthlyCount}��";
        GetObject((int)GameObjects.CheckOutProgressSliderObject).GetComponent<Slider>().value = _monthlyCount;
    }

    void OnClickBackgroundButton()
    {
        Managers.Sound.PlayButtonClick();
        _userCheckOutDay = 0;
        Managers.UI.ClosePopupUI(this);
    }

    #region  Test
    public void OnClickButtonTest() // ��ư Ŭ�� �׽�Ʈ
    {
        Debug.Log("On click start button");
        //_userCheckOutDay += 1;
        //Refresh();
    }

    void TextBindTest() // �ؽ�Ʈ ���� �׽�Ʈ��
    {
        string TestText = "Test";

        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = TestText;
        }
    }
    void OnClickCheckOutTestButton() // �⼮�� �ø��� �׽�Ʈ ��ư
    {
        _monthlyCount += 1;
        _dailyCount += 1;
        GetText((int)Texts.DaysCountText).text = $"{_monthlyCount}";

        Init();
    }

    #endregion


}
