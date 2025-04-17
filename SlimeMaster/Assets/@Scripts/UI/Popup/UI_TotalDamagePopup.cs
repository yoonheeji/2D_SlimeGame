using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UI_TotalDamagePopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // TotalDamageContentObject : �� �� �θ�ü

    // ���ö���¡
    // BackgroundText : ���Ͽ� �ݱ�
    // TotalDamagePopupTitleText : �� ������
    #endregion


    enum GameObjects
    {
        ContentObject,
        TotalDamageContentObject,
    }

    enum Buttons
    {
        BackgroundButton
    }

    enum Texts
    {
        BackgroundText,
        TotalDamagePopupTitleText,
    }

    enum Images
    {

    }

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


        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickClosePopup);

#if UNITY_EDITOR
        // �׽�Ʈ��
        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        GetObject((int)GameObjects.TotalDamageContentObject).DestroyChilds();
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        foreach (SkillBase skill in skillList.FindAll(skill => skill.IsLearnedSkill))
        {
            UI_SkillDamageItem item = Managers.UI.MakeSubItem<UI_SkillDamageItem>(GetObject((int)GameObjects.TotalDamageContentObject).transform);
            item.SetInfo(skill);
            item.transform.localScale = Vector3.one;
        }


        Refresh();
    }

    void Refresh()
    {

    }
    public void OnClickClosePopup()
    {
        Managers.UI.ClosePopupUI(this);
    }

}
