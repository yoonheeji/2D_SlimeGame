using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Data;
using System.Linq;

public class UI_AchievementPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // AchievementScrollObject : ������  AchievementItme�� �� �θ� ��ü

    // ���ö���¡
    // BackgroundText : ��ġ�Ͽ� �ݱ�
    // AchievementTitleText : ����

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        AchievementScrollObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }
    enum Texts
    {
        BackgroundText,
        AchievementTitleText,
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


        // �׽�Ʈ��
#if UNITY_EDITOR

#endif
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

        GetObject((int)GameObjects.AchievementScrollObject).DestroyChilds();

        foreach (AchievementData data in Managers.Achievement.GetProceedingAchievment())
        {
            UI_AchievementItem item = Managers.UI.MakeSubItem<UI_AchievementItem>(GetObject((int)GameObjects.AchievementScrollObject).transform);
            item.SetInfo(data);
        }
    }
    // �� �� ���� �ݱ� ��ư
    void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

}
