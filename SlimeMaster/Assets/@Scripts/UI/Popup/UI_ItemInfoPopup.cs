using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemInfoPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
   
    // ���ö���¡ �ؽ�Ʈ
    #endregion

    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {

    }

    enum Texts
    {

    }

    enum Images
    {

    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindToggle(typeof(Images));


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

    }

}
