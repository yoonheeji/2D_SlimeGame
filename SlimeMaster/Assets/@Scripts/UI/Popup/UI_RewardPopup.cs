
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Popup�� �� ���� atctive true false �ϴ���?�����
public class UI_RewardPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // RewardItemScrollContentObject : ������ ������ �� �θ�ü

    // ȣ��Ǵ� ��
    // �̼� �˾� : �̼� �Ϸ� ����
    // �������� ���� �˾� : ���� ���� ��
    // ���� ���� �˾� : ���� ���� ��
    // �������� ���� �˾� : Ŭ���� ���� ���� ��
    // ���� ������ : ��ǰ ����
    // ��� �ʱ�ȭ �˾� : �ʱ�ȭ ��� ���� ��

    // ȣ�� ����
    // ��Ʋ �н� �˾� : �н� ���� 
    // ������ �˾� : ��� ����
    // ���̾� �н� �˾� : ��� ����
    // ù ���� ���� �˾� : ��� ����
    // ���� Ư�� �˾� : ��ǰ ����


    // ���ö���¡ �ؽ�Ʈ
    // RewardPopupTitleText : ����
    // BackgroundText : ���Ͽ� �ݱ�

    #endregion


    #region Enum
    enum GameObjects
    {
        RewardItemScrollContentObject, 
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        RewardPopupTitleText,
        BackgroundText
    }


    #endregion

    public Action OnClosed;
    string[] _spriteName;
    int[] _count;

    public void OnEnable()
    {
        //TODO �˾� ������ ���̴� ��찡 �߻� �ӽ÷� sortingOrder���� 
        //Canvas canvas = GetComponent<Canvas>();
        //canvas.sortingOrder = 300;
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
        #endregion
        Refresh();
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_Reward");

        return true;
    }

    public void SetInfo(string[] spriteName, int[] count, Action callback = null)
    {
        _spriteName = spriteName;
        _count = count;
        OnClosed = callback;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        GetObject((int)GameObjects.RewardItemScrollContentObject).DestroyChilds();
        for (int i = 0; i < _spriteName.Length; i++)
        {
            Debug.Log(_spriteName[i]);
            UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(GetObject((int)GameObjects.RewardItemScrollContentObject).transform);
            item.SetInfo(_spriteName[i], _count[i]);
        }
    }

    void OnClickBackgroundButton() // ȭ�� ��ġ�Ͽ� �ݱ�
    {
        Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);
        OnClosed?.Invoke();
    }

}
