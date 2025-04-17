using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    //InventoryScrollContentObject : MaterialInfoButton �������� �ִ� �θ� ��ü
    // �κ��丮 ���� ���� �߰� ����

    // ���ö���¡
    // BackgroundText
    // InventoryPopupTitleText

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        InventoryScrollContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        InventoryPopupTitleText,
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

        // �׽�Ʈ��
#if UNITY_EDITOR
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);


        
        //TextBindTest();
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

    }

    void OnClickBackgroundButton() // ��� �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);

    }
}
