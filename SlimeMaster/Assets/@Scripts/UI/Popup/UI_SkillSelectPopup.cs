using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SkillSelectPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // BeforeLevelValueText : ���� ����
    // AfterLevelValueText : ���� ����
    // ExpSliderObject : ���Ӿ��� ����ġ�� ����ȭ ( ���� �������� ���� �߰� �ɼ� ����)

    // ���ö���¡
    // CharacterLevelupTitleText : LEVEL UP!
    // SkillSelectTitleText : ��ų ����
    // CardRefreshText : ���ΰ�ħ
    // ADRefreshText : ���ΰ�ħ
    // SkillSelectCommentText : ��� ��ų�� �����Ͻʽÿ�.
    #endregion


    #region Enum
    enum GameObjects
    {
        ContentObject,
        SkillCardSelectListObject,
        ExpSliderObject,
        DisabledObject,
        CharacterLevelObject,
    }
    enum Buttons
    {
        CardRefreshButton,
        ADRefreshButton,
    }

    enum Texts
    {
        SkillSelectCommentText,
        SkillSelectTitleText,
        CardRefreshText,
        CardRefreshCountValueText,
        ADRefreshText,

        CharacterLevelupTitleText,

        CharacterLevelValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
    }

    enum Images
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
    }
    #endregion

    private void OnEnable()
    {
        Init();
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    GameManager _game;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);
        GetButton((int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickADRefreshButton);

        GetObject((int)GameObjects.DisabledObject).gameObject.SetActive(false);
        //GetButton((int)Buttons.CardRefreshButton).gameObject.SetActive(false); 
        //GetButton((int)Buttons.ADRefreshButton).gameObject.SetActive(false);
        #endregion

        _game = Managers.Game;

        Refresh();

        SetRecommendSkills();
        List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_SkillSelect");
        return true;

    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SetRecommendSkills();
        }

    }
#endif

    void Refresh()
    {
        //GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
        GetText((int)Texts.BeforeLevelValueText).text = $"Lv.{_game.Player.Level - 1 }";
        GetText((int)Texts.AfterLevelValueText).text = $"Lv.{_game.Player.Level}";

        if (Managers.Game.Player.SkillRefreshCount > 0)
            GetText((int)Texts.CardRefreshCountValueText).text = $"���� Ƚ�� : {Managers.Game.Player.SkillRefreshCount}";
        else
            GetText((int)Texts.CardRefreshCountValueText).text = $"<color=red>���� Ƚ�� : 0</color>";


        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());

        
    }

    void SetRecommendSkills()
    {
        GameObject container = GetObject((int)GameObjects.SkillCardSelectListObject);
        //�ʱ�ȭ
        container.DestroyChilds();
        List<SkillBase> List = Managers.Game.Player.Skills.RecommendSkills();

        foreach (SkillBase skill in List)
        {
            UI_SkillCardItem item = Managers.UI.MakeSubItem<UI_SkillCardItem>(container.transform);
            item.GetComponent<UI_SkillCardItem>().SetInfo(skill);
        }
    }

    void SetCurrentSkill(int index, SkillBase skill)
    {
        GetImage(index).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.IconLabel);
        GetImage(index).enabled = true;
    }

    void OnClickCardRefreshButton() // ī�� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Player.SkillRefreshCount > 0)
        { 
            SetRecommendSkills();
            Managers.Game.Player.SkillRefreshCount--;   
        }
        Refresh();
    }

    void OnClickADRefreshButton() // ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.SkillRefreshCountAds > 0)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.SkillRefreshCountAds--;
                SetRecommendSkills();
            });
        }
        else
        {
            Managers.UI.ShowToast("���̻� ��� �� �� �����ϴ�.");
        }


    }

}
