using Data;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class UI_GameScene : UI_Scene
{
    #region UI ��� ����Ʈ
    // ���� ����
    // GoldValueText : ���� ��� ���� (��带 ������ ���� ��������)
    // KillValueText : ���� �� �� ���� (ų �� �� ���� ��������) 
    // WaveValueText : ���� �������� ���̺� ��
    // TimeLimitValueText : ���� ���̺� ���� ���� ���� �ð� (�ʸ��� ����)
    // SupportSkillCardListObject : ��ȥ������ ����Ʈ ī�尡 ���� �θ�ü
    // ResetCostValueText : ��ȥ ������ ����Ʈ ī�� ����Ʈ ���� ��� (-n���� ǥ��, ���¸��� 150�� �þ, �⺻ 150 )
    // TotalDamageContentObject: UI_SkillDamageItem�� �θ�ü

    //  BattleSkillCountValueText : ��Ʋ ��ų�� ���� ǥ��
    //  SupportSkillCountValueText : ����Ʈ ��ų�� ���� ǥ��
    // SupportSkillListScrollContentObject : ��� ����Ʈ ��ų���� �� �θ�ü

    // ���̺� �˶�
    // MonsterAlarmObject : ���� ���̺갡 ���� 3���� 3�ʰ� ��� (�ִ� �߰� ����)
    // BossAlarmObject : ������ ���� 3���� 3�ʰ� ��� (�ִ� �߰� ����)

    // ����Ʈ ����
    // EliteInfoObject : ����Ʈ ���� �� Ȱ��ȭ
    // EliteHpSliderObject : ������ ����Ʈ�� ���� ü��% ����
    // EliteNameValueText : ������ ����Ʈ�� �̸�

    // ���� ����
    // BossInfoObject : ���� ���� �� Ȱ��ȭ
    // BossHpSliderObject : ������ ������ ���� ü��% ����
    // BossNameValueText : ������ ������ �̸�


    // ���ö���¡
    // CardListResetText : �ʱ�ȭ
    // MonsterCommentText : ���Ͱ� �����ɴϴ�
    // BossCommentText : ���� ����
    // OwnBattleSkillInfoText : ���� ��ų
    // OwnSupportSkillInfoText : ����Ʈ ��ų

    #endregion

    #region Enum
    enum GameObjects
    {
        ExpSliderObject, // �����̴��� ���� �ʿ�
        WaveObject,
        SoulImage,
        OnDamaged,
        SoulShopObject,
        SupportSkillCardListObject,
        OwnBattleSkillInfoObject,
        //TotalDamageContentObject,
        SupportSkillListScrollObject,
        SupportSkillListScrollContentObject,
        WhiteFlash,
        MonsterAlarmObject,
        BossAlarmObject,

        EliteInfoObject,
        EliteHpSliderObject,
        BossInfoObject,
        BossHpSliderObject,

        BattleSkillSlotGroupObject,
    }
    enum Buttons
    {
        PauseButton,
        DevelopButton,
        SoulShopButton,
        CardListResetButton,
        SoulShopCloseButton,
        TotalDamageButton,
        SupportSkillListButton,
        SoulShopLeadButton,
        SoulShopBackgroundButton,
    }
    enum Texts
    {
        WaveValueText,
        TimeLimitValueText,
        SoulValueText,
        KillValueText,
        CharacterLevelValueText,
        CardListResetText,
        ResetCostValueText,
        //BattleSkillCountValueText,
        SupportSkillCountValueText,

        EliteNameValueText,
        BossNameValueText,

        MonsterCommentText,
        BossCommentText,
    }
    enum Images
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
        SupportSkilI_Icon_0,
        SupportSkilI_Icon_1,
        SupportSkilI_Icon_2,
        SupportSkilI_Icon_3,
        SupportSkilI_Icon_4,
        SupportSkilI_Icon_5,
    }
    enum AlramType
    {
        wave,
        boss
    }
    #endregion

    GameManager _game;
    Coroutine _coWaveAlarm;

    bool _isSupportSkillListButton = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.PauseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopButton).gameObject.BindEvent(OnClickSoulShopButton);
        GetButton((int)Buttons.CardListResetButton).gameObject.BindEvent(OnClickCardListResetButton);
        GetButton((int)Buttons.CardListResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopLeadButton).gameObject.BindEvent(OnClickSoulShopButton);
        GetButton((int)Buttons.SoulShopLeadButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopCloseButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
        GetButton((int)Buttons.SoulShopCloseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.TotalDamageButton).gameObject.BindEvent(OnClickTotalDamageButton);
        GetButton((int)Buttons.TotalDamageButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SupportSkillListButton).gameObject.BindEvent(OnClickSupportSkillListButton);
        GetButton((int)Buttons.SupportSkillListButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // ��ȥ ���� ��ư �ʱ� ����
        GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true); // ��ȥ ���� ��ư �ʱ� ����
        GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
        GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // ��ȥ ���� ���
        GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // ��Ʋ ��ų ����Ʈ �ʱ� ����
        GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // ����Ʈ ��ų ����Ʈ �ʱ� ����
        _isSupportSkillListButton = false;

        GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false); // ���� ���̺� �˶� �ʱ� ����
        GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(false); // ���� �˶� �ʱ� ����

        GetObject((int)GameObjects.EliteInfoObject).gameObject.SetActive(false); // ����Ʈ ����(ü�¹�) �ʱ� ����
        GetObject((int)GameObjects.BossInfoObject).gameObject.SetActive(false); // ���� ����(ü�¹�) �ʱ� ����

        #endregion

        _game = Managers.Game;

        Managers.Game.Player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        //Managers.Game.Player.OnMonsterDataUpdated = OnOnMonsterDataUpdated;
        Managers.Game.Player.OnPlayerLevelUp = OnPlayerLevelUp;
        Managers.Game.Player.OnPlayerDamaged = OnDamaged;

        GetComponent<Canvas>().worldCamera = Camera.main; 

        return true;
    }

    private void Awake()
    {
        Init();
        Managers.Game.Player.Skills.UpdateSkillUi += OnLevelUpSkill;
        Managers.Game.Player.OnPlayerMove += OnPlayerMove;
        Refresh();
    }

    private void OnDestroy()
    {
        if (Managers.Game.Player != null)
        {
            Managers.Game.Player.Skills.UpdateSkillUi -= OnLevelUpSkill;
            Managers.Game.Player.OnPlayerMove -= OnPlayerMove;
        }
    }



    public void SetInfo() // ������ �޾ƿö�
    {
        Refresh();
    }

    void Refresh() // ������ ����
    {
        // ���� ��ų ���� ����
        SetBattleSkill();
        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
        GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
        GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
        // ���̺� �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.WaveObject).GetComponent<RectTransform>());
    }

    void OnLevelUpSkill()
    {
        ClearSkillSlot();

        //��Ʋ��ų������
        List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();
        for (int i = 0; i < activeSkills.Count; i++)
            AddSkillSlot(i, activeSkills[i].SkillData.IconLabel);

        //GetText((int)Texts.BattleSkillCountValueText).text = activeSkills.Count.ToString();

        //����Ʈ��ų 
        int index = 6;// ����Ʈ��ų enum�� 6������ ������
        int count = Mathf.Min(6, Managers.Game.Player.Skills.SupportSkills.Count);
        for (int i = 0; i < count; i++)
            AddSkillSlot(i + index, Managers.Game.Player.Skills.SupportSkills[i].IconLabel);

        GetText((int)Texts.SupportSkillCountValueText).text = Managers.Game.Player.Skills.SupportSkills.Count.ToString();

    }

    public void SetBattleSkill()
    {

        GameObject container = GetObject((int)GameObjects.BattleSkillSlotGroupObject);
        //�ʱ�ȭ
        foreach (Transform child in container.transform)
            Managers.Resource.Destroy(child.gameObject);

        //������ų
        foreach (SkillBase skill in Managers.Game.Player.Skills.ActivatedSkills)
        {
            UI_SkillSlotItem item = Managers.UI.MakeSubItem<UI_SkillSlotItem>(container.transform);
            item.GetComponent<UI_SkillSlotItem>().SetUI(skill.SkillData.IconLabel, skill.Level);
        }

    } // ��Ʋ ��ų ����Ʈ ���� ����

    void OnClickPauseButton() // �Ͻ� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_PausePopup>();
    }

    void OnClickSoulShopButton() // ��ȥ ���� ��ư
    {
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_SoulShop");
        //���� Ȱ��ȭ
        SetActiveSoulShop(true);

        if (Managers.Game.ContinueInfo.SoulShopList.Count == 0)
            ResetSupportCard();
        else
            LoadSupportCard();

        Refresh();
    }

    void OnClickCardListResetButton() // ��ȥ���� ����Ʈ ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        // ����Ʈ ī�� ����Ʈ ���� ����
        int cardListResetCost = (int)Define.SOUL_SHOP_COST_PROB[6];
        if (Managers.Game.Player.SoulCount >= cardListResetCost)
        {
            Managers.Game.Player.SoulCount -= cardListResetCost;
            ResetSupportCard();
        }   
    }

    void OnClickCardListResetTestButton() // �׽�Ʈ�� ��ȥ���� ����Ʈ ���� ��ư 
    {
        Managers.Sound.PlayButtonClick();

        // ����Ʈ ī�� ����Ʈ ���� ����
        ResetSupportCard();
    }

    void OnClickSoulShopCloseButton() // ��ȥ ���� �ݱ� ��ư
    {
        Managers.Sound.PlayButtonClick();

        //���� ��Ȱ��ȭ
        SetActiveSoulShop(false);
    }

    public void OnPlayerMove()
    {
        Vector2 uiPos = GetObject((int)GameObjects.SoulImage).transform.position;
        Managers.Game.SoulDestination = uiPos;
    }


    void OnClickTotalDamageButton() // ��Ż ������ ��ư
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }

    void ResetSupportSkillList()
    {
        GameObject container = GetObject((int)GameObjects.SupportSkillListScrollContentObject);
        container.DestroyChilds();

        List<Data.SupportSkillData> temp = Managers.Game.Player.Skills.SupportSkills.OrderByDescending(x => x.DataId).ToList();
        foreach (Data.SupportSkillData skill in temp)
        {
            UI_SupportSkillItem item = Managers.UI.MakeSubItem<UI_SupportSkillItem>(container.transform);
            ScrollRect scrollRect = GetObject((int)GameObjects.SupportSkillListScrollObject).GetComponent<ScrollRect>();
            item.SeteInfo(skill, this.transform, scrollRect);
            Managers.Game.SoulShopList.Add(skill);
        }
    }

    void ResetSupportCard()
    {
        #region ����Ʈ��ų ī�� ���� �����ֱ�
        GameObject supportSkillContainer = GetObject((int)GameObjects.SupportSkillCardListObject);
        supportSkillContainer.DestroyChilds();
        Managers.Game.SoulShopList.Clear();

        foreach (SupportSkillData supportSkill in Managers.Game.Player.Skills.RecommendSupportkills())
        {
            supportSkill.IsPurchased = false;
            UI_SupportCardItem skillData = Managers.UI.MakeSubItem<UI_SupportCardItem>(supportSkillContainer.transform);
            skillData.SetInfo(supportSkill);
        }

        #endregion
    }

    void LoadSupportCard()
    {
        #region ����Ʈ��ų ī�� ���� �����ֱ�
        GameObject supportSkillContainer = GetObject((int)GameObjects.SupportSkillCardListObject);
        supportSkillContainer.DestroyChilds();

        foreach (SupportSkillData supportSkill in Managers.Game.SoulShopList)
        {
            UI_SupportCardItem skillData = Managers.UI.MakeSubItem<UI_SupportCardItem>(supportSkillContainer.transform);
            skillData.SetInfo(supportSkill);
        }

        #endregion
    }

    void AddSkillSlot(int index, string iconLabel)
    {
        GetImage(index).sprite = Managers.Resource.Load<Sprite>(iconLabel);
        GetImage(index).enabled = true;
    }

    void ClearSkillSlot()
    {
        int count = Enum.GetValues(typeof(Images)).Length;
        for (int i = 0; i < count; i++)
        {
            GetImage(i).enabled = false;
        }
    }
    void SetActiveSoulShop(bool isActive)
    {
        //float height = 0;
        if (isActive)
        { 
            SoulShopInit(); // ��ȥ ���� �ʱ� ����
            GetComponent<Canvas>().sortingOrder = Define.UI_GAMESCENE_SORT_OPEN;
        }
        else
        {
            GetComponent<Canvas>().sortingOrder = Define.UI_GAMESCENE_SORT_CLOSED;

            // ��ȥ ���� �ʱ� ����
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); 

            PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
            GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // ��ȥ ���� ��ư ��Ȱ��ȭ
            GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // ��ȥ ���� ���
            GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // ��Ʋ ��ų ����Ʈ ��Ȱ��ȭ
            GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // ����Ʈ ��ų ����Ʈ ��Ȱ��ȭ
            _isSupportSkillListButton = false;
            Managers.UI.IsActiveSoulShop = false;
        }
    }

    void SoulShopInit() // ��ȥ ���� �ʱ� ����
    {
        // ��ȥ ���� �÷���
        GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
        GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
        GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
        GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 700); 

        PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
        GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(true); // ��ȥ ���� ��ư Ȱ��ȭ
        GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(true); // ��ȥ ���� ���
        GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(true); // ��Ʋ ��ų ����Ʈ Ȱ��ȭ
        PopupOpenAnimation(GetObject((int)GameObjects.OwnBattleSkillInfoObject));
        GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // ����Ʈ ��ų ����Ʈ ��Ȱ��ȭ
        _isSupportSkillListButton = false;
        Managers.UI.IsActiveSoulShop = true;
    }

    void OnClickSupportSkillListButton() // ����Ʈ ��ų ����Ʈ ��ư
    {
        Managers.Sound.PlayButtonClick();
        if (_isSupportSkillListButton) // �̹� �����ٸ� �ݱ�
            SoulShopInit(); // ��ȥ ���� �ʱ� ����
        else // ������ �ʾҴٸ� ����Ʈ ��ų ����Ʈ ����
        {
            // ����Ʈ ��ų ����Ʈ ����
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1120); 

            PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
            GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // ��Ʋ ��ų ����Ʈ ��Ȱ��ȭ
            GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(true); // ����Ʈ ��ų ����Ʈ Ȱ��ȭ
            _isSupportSkillListButton = true;

            ResetSupportSkillList();
        }
    }

    #region Actions
    void OnPlayerDataUpdated()
    {
        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
        GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
        GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
    }

    void OnPlayerLevelUp()
    {
        if (Managers.Game.IsGameEnd) return;

        List<SkillBase> list = Managers.Game.Player.Skills.RecommendSkills();

        if (list.Count > 0)
        {
            Managers.UI.ShowPopupUI<UI_SkillSelectPopup>();
        }

        //GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = (float)_game.Player.Exp / _game.Player.TotalExp;
        GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
    }

    public void MonsterInfoUpdate(MonsterController monster)
    {
        if (monster.ObjectType == Define.ObjectType.EliteMonster)
        {
            if (monster.CreatureState != Define.CreatureState.Dead)
            {
                GetObject((int)GameObjects.EliteInfoObject).SetActive(true);
                GetObject((int)GameObjects.EliteHpSliderObject).GetComponent<Slider>().value = monster.Hp / monster.MaxHp;
                GetText((int)Texts.EliteNameValueText).text = monster.CreatureData.DescriptionTextID;
            }
            else
            {
                GetObject((int)GameObjects.EliteInfoObject).SetActive(false);
            }
        }
        else if (monster.ObjectType == Define.ObjectType.Boss)
        {
            if (monster.CreatureState != Define.CreatureState.Dead)
            {
                GetObject((int)GameObjects.BossInfoObject).SetActive(true);
                GetObject((int)GameObjects.BossHpSliderObject).GetComponent<Slider>().value = monster.Hp / monster.MaxHp;
                GetText((int)Texts.BossNameValueText).text = monster.CreatureData.DescriptionTextID;
            }
            else
            {
                GetObject((int)GameObjects.BossInfoObject).SetActive(false);
            }
        }
    }

    public void OnWaveStart(int currentStageIndex)
    {
        GetText((int)Texts.WaveValueText).text = currentStageIndex.ToString();
    }

    public void OnSecondChange(int time)
    {
        //���̺� ���� 3���� ���̺� �˶�
        if (time == 3 && Managers.Game.CurrentWaveIndex < 9)
        {
            StartCoroutine(SwitchAlarm(AlramType.wave));
        }

        //���� ���� 3���� ���� �˶�
        if (_game.CurrentWaveData.BossId.Count > 0)
        {
            int bossGenTime = Define.BOSS_GEN_TIME;
            if (time == bossGenTime)
                StartCoroutine(SwitchAlarm(AlramType.boss));
        }
        GetText((int)Texts.TimeLimitValueText).text = time.ToString();
        if (time == 0)
            GetText((int)Texts.TimeLimitValueText).text = "";
    }

    public void OnWaveEnd()
    {
        // ���� ���̺� �˶� 
        GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false);
    }

    public void OnDamaged()
    {
        StartCoroutine(CoBloodScreen());
    }
    public void DoWhiteFlash()
    {
        StartCoroutine(CoWhiteScreen());
    }

    #endregion

    IEnumerator CoBloodScreen()
    {
        Color targetColor = new Color(1, 0, 0, 0.3f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(targetColor, 0.3f))
            .Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(Color.clear, 0.3f)).OnComplete(() => { });
    }

    IEnumerator CoWhiteScreen()
    {
        Color targetColor = new Color(1, 1, 1, 1f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(1, 0.1f))
            .Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(0, 0.2f)).OnComplete(() => { });
    }

    IEnumerator SwitchAlarm(AlramType type)
    {
        switch (type)
        {
            case AlramType.wave:
                Managers.Sound.Play(Define.Sound.Effect, "Warning_Wave");
                GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(true);
                yield return new WaitForSeconds(3f);
                GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false);
                break;
            case AlramType.boss:
                Managers.Sound.Play(Define.Sound.Effect, "Warning_Boss");
                GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(true);
                yield return new WaitForSeconds(3f);
                GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(false);
                break;
        }
    }


}
