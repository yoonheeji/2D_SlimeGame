using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChallengePopup : UI_Popup
{
    enum Texts
    {
        UnlockInfoText,

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        //�ٱ��
        //�ؽ�Ʈ ����
        //  getweapondamage

        Refresh();
        return true;
    }

    public void SetInfo()
    {
    }

    void Refresh()
    { 
    }




}
