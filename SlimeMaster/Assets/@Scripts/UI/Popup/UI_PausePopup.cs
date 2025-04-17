using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_PausePopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // BattleSkillSlotGroupObject�� ���� �����ϰ� �ִ� ���� ��ų ǥ��
    // SupportSkillSlotGroupObject�� ���� �����ϰ� �ִ� ����Ʈ ��ų ǥ��

    // ���ö���¡
    // PauseTitleText : �Ͻ�����
    #endregion


    #region Enum

    enum GameObjects
    {
        ContentObject,
        //BattleSkillSlotGroupObject, // ��Ʋ ��ų ����
        //SupportSkillSlotGroupObject, // ����Ʈ ��ų ����
    }
    enum Buttons
    {
        ResumeButton,
        HomeButton,
        StatisticsButton,
        SoundButton,
        SettingButton,
    }


   enum Texts
    {
        PauseTitleText,
        //OwnBattleSkillInfoText,
        //OwnSupportSkillInfoText,
        ResumeButtonText
    }

  
    #endregion

    SkillBase skill;
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
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton((int)Buttons.HomeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>(); 
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>(); 
        GetButton((int)Buttons.SoundButton).gameObject.BindEvent(OnClickSoundButton);
        GetButton((int)Buttons.SoundButton).GetOrAddComponent<UI_ButtonAnimation>(); 
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();

#if UNITY_EDITOR
        // �׽�Ʈ��

#endif
        #endregion
        //SetBattleSkill();
        return true;
    }


    //public void SetBattleSkill()
    //{

    //    GameObject container = GetObject((int)GameObjects.BattleSkillSlotGroupObject);
    //    GameObject container2 = GetObject((int)GameObjects.SupportSkillSlotGroupObject);
    //    //�ʱ�ȭ
    //    foreach (Transform child in container.transform)
    //        Managers.Resource.Destroy(child.gameObject);

    //    foreach (Transform child in container2.transform)
    //        Managers.Resource.Destroy(child.gameObject);

    //    //������ų
    //    foreach (SkillBase skill in Managers.Game.Player.Skills.ActivatedSkills)
    //    {
    //        UI_SkillSlotItem item = Managers.UI.MakeSubItem<UI_SkillSlotItem>(container.transform);
    //        item.GetComponent<UI_SkillSlotItem>().SetUI(skill.SkillData.IconLabel, skill.Level);
    //    }
        
    //    ////����Ʈ��ų
    //    //foreach (SupportSkillData skill in Managers.Game.Player.Skills.SupportSkills)
    //    //{
    //    //    UI_SkillSlotItem item = Managers.UI.MakeSubItem<UI_SkillSlotItem>(container2.transform);
    //    //    item.GetComponent<UI_SkillSlotItem>().SetUI(skill.IconLabel);
    //    //}
    //}

    void OnClickResumeButton() // �ǵ��ư��� ��ư
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickHomeButton() // �κ� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_BackToHomePopup>();
    }

    void OnClickSettingButton() // ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }
    void OnClickSoundButton() // ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
    }
    void OnClickStatisticsButton() // ��� ��ư
    {
        Managers.Sound.PlayButtonClick();
        // ��� �˾� ȣ��(���� �ȸ���)
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }

}
