using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static Define;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> _skillList = new List<SkillBase>();
    public List<SkillBase> SkillList { get { return _skillList; } } 
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
    public List<SupportSkillData> LockedSupportSkills { get; } = new List<SupportSkillData>();
    public List<SupportSkillData> SupportSkills = new List<SupportSkillData>();

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }
    [SerializeField]
    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<SkillType, int>();

    public event Action UpdateSkillUi;
    public ObjectType _ownerType;

    public void Awake()
    {
        _ownerType = GetComponent<CreatureController>().ObjectType;

    }

    public void Init()
    {
        //SkillList.Clear();
        //SupportSkills.Clear();
        //ActivatedSkills.Clear();
        //SavedSkill.Clear();
    }
    public void SetInfo(ObjectType type)
    {
        _ownerType = type;
    }

    public void LoadSkill(Define.SkillType skillType, int level)
    {
        //��罺ų�� 0���� ������. ���� �� ��ŭ �������� ����
        AddSkill(skillType);
        for(int i = 0; i < level; i++)
            LevelUpSkill(skillType);

    }

    public void AddSkill(Define.SkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();

        if (skillType == SkillType.FrozenHeart || skillType == SkillType.SavageSmash || skillType == SkillType.EletronicField)
        {
            GameObject go = Managers.Resource.Instantiate(skillType.ToString(), gameObject.transform);
            if (go != null)
            {
                SkillBase skill = go.GetOrAddComponent<SkillBase>();
                SkillList.Add(skill);
                if(SavedBattleSkill.ContainsKey(skillType))
                    SavedBattleSkill[skillType] = skill.Level;
                else
                    SavedBattleSkill.Add(skillType, skill.Level);
            }
        }
        else
        {
            // AddComponent�� �ϸ��
            SequenceSkill skill = gameObject.AddComponent(Type.GetType(className)) as SequenceSkill;
            if (skill != null)
            {
                skill.ActivateSkill();
                skill.Owner = GetComponent<CreatureController>();
                skill.DataId = skillId;
                SkillList.Add(skill);
                SequenceSkills.Add(skill);
            }
            else
            {
                RepeatSkill skillbase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
                SkillList.Add(skillbase);
                if (SavedBattleSkill.ContainsKey(skillType))
                    SavedBattleSkill[skillType] = skillbase.Level;
                else
                    SavedBattleSkill.Add(skillType, skillbase.Level);
            }
        }
    }

    public void AddActiavtedSkills(SkillBase skill)
    {
        ActivatedSkills.Add(skill);
    }

    int _sequenceIndex = 0;

    public void StartNextSequenceSkill()
    {
        if (_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    void OnFinishedSequenceSkill()
    {
        _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    bool _stopped = false;

    public void StopSkills()
    {
        _stopped = true;

        foreach (var skill in ActivatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }

    public void AddSupportSkill(SupportSkillData skill, bool isLoadSkill = false)
    {
        #region ����Ʈ��ų �ߺ�����
        skill.IsPurchased = true;

        //1. ��ų ��� ���� �ٷ� ������ �͵�
        if (skill.SupportSkillName == SupportSkillName.Healing)
        {
            Managers.Game.Player.Healing(skill.HealRate);
            return;
        }

        SupportSkills.Add(skill);
        UpdateSkillUi?.Invoke();


        // �̹� ����� ���� ������ ������ ��ų����Ÿ�� ������Ʈ �����ʰ� Add ��Ų�� UI���� �߰��Ѵ�.
        if (isLoadSkill == true)
            return;

        if (skill.SupportSkillType == SupportSkillType.General)
        {
            GeneralSupportSkillBonus(skill);
        }
        else if (skill.SupportSkillType == SupportSkillType.Special)
        {
            //��Ʋ��ų�� ������ ��ġ�� ��ų�ΰ�� UpdateskilLData();
            foreach (SkillBase playerSkill in SkillList)
            {
                if (skill.SupportSkillName.ToString() == playerSkill.SkillType.ToString())
                {
                    playerSkill.UpdateSkillData();
                }
            }
        }

        #endregion
    }

    public void LevelUpSkill(Define.SkillType skillType)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillType == skillType)
            {
                SkillList[i].OnLevelUp();
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = SkillList[i].Level;
                }
                UpdateSkillUi?.Invoke();
            }
        }
    }

    public void OnSkillBookChanged()
    {
        UpdateSkillUi?.Invoke();
    }

    public void Clear()
    {
        SavedBattleSkill.Clear();
        SupportSkills.Clear();
    }

    #region ����Ʈ��ų ���ʽ� �߰� 
    public void GeneralSupportSkillBonus(SupportSkillData skill)
    {
        List<SupportSkillData> generalList = SupportSkills.Where(skill => skill.SupportSkillType == SupportSkillType.General).ToList();

        PlayerController player = Managers.Game.Player;
        player.CriRate += skill.CriRate;
        player.MaxHpBonusRate += skill.HpRate;
        player.ExpBonusRate += skill.ExpBonusRate;
        player.AttackRate += skill.AtkRate;
        player.DefRate += skill.DefRate;
        player.DamageReduction += skill.DamageReduction;
        player.SoulBonusRate += skill.SoulBonusRate;
        player.HealBonusRate += skill.HealBonusRate;
        player.MoveSpeedRate += skill.MoveSpeedRate;
        player.HpRegen += skill.HpRegen;
        player.CriDamage += skill.CriDmg;
        player.CollectDistBonus += skill.MagneticRange;

        player.UpdatePlayerStat();
    }

    public void OnPlayerLevelUpBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == SupportSkillType.LevelUp).ToList();

        float moveRate = 0;
        float atkRate = 0;
        float criRate = 0;
        float cridmg = 0;
        float reduceDamage = 0;

        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == SupportSkillName.Resurrection)
                continue;
            moveRate += passive.MoveSpeedRate;
            atkRate += passive.AtkRate;
            criRate += passive.CriRate;
            cridmg += passive.CriDmg;
            reduceDamage += passive.DamageReduction;
        }

        PlayerController player = Managers.Game.Player;
        player.MoveSpeedRate += moveRate;
        player.AttackRate += atkRate;
        player.CriRate += criRate;
        player.CriDamage += cridmg;
        player.DamageReduction += reduceDamage;

        player.UpdatePlayerStat();
    }

    public void OnMonsterKillBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == SupportSkillType.MonsterKill).ToList();

        float dmgReduction = 0;
        float atkRate = 0;
        float healAmount = 0;
        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == SupportSkillName.Resurrection)
                continue;
            dmgReduction += passive.DamageReduction;
            atkRate += passive.AtkRate;
            healAmount += passive.HealRate;
        }

        PlayerController player = Managers.Game.Player;
        player.DamageReduction += dmgReduction;
        player.AttackRate += atkRate;

        player.UpdatePlayerStat();
        Managers.Game.Player.Healing(healAmount);

    }

    public void OnEliteDeadBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == SupportSkillType.EliteKill).ToList();

        float soulCount = 0;
        float expBonus = 0;

        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == SupportSkillName.Resurrection)
                continue;
            soulCount += passive.SoulAmount;
            expBonus += passive.ExpBonusRate;
        }

        PlayerController player = Managers.Game.Player;
        player.SoulCount += soulCount;
        player.ExpBonusRate += expBonus;
    }
    #endregion

    #region ��ų ��í

    public SkillBase RecommandDropSkill()
    {
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.IsLearnedSkill);

        List<SkillBase> recommendSkills = activeSkills.FindAll(s => s.Level < 5);
        recommendSkills.Shuffle();
        //Util.Shuffle(recommandSkills);
        return recommendSkills[0];
    }

    public List<SkillBase> RecommendSkills()
    {
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.IsLearnedSkill);

        //1. �̹� 6���� ��ų�� ������� ��� ��ų�� 5�� �̸��� ��ų�� ��õ
        if (activeSkills.Count == MAX_SKILL_COUNT)
        {
            List<SkillBase> recommendSkills = activeSkills.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            //Util.Shuffle(recommandSkills);
            return recommendSkills.Take(3).ToList();
        }
        else
        {
            // ������ 5 �̸��� ��ų 
            List<SkillBase> recommendSkills = skillList.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            //Util.Shuffle(recommandSkills);
            return recommendSkills.Take(3).ToList();
        }
    }

    public List<SupportSkillData> RecommendSupportkills()
    {
        GameManager game = Managers.Game;
        game.SoulShopList.Clear();

        //1. Lock �� ��ų�� �ִ��� Ȯ���ϰ� �߰��ϱ�
        foreach (SupportSkillData skill in LockedSupportSkills)
        {
            skill.IsLocked = true;
            game.SoulShopList.Add(skill);
        }

        int recommandCount = 4 - game.SoulShopList.Count;

        for (int i = 0; i < recommandCount; i++)
        {

            //1. ��� ��í
            SupportSkillGrade grade = GetRandomGrade();
            // 2. �ش� ��� ��ų ��� ��������
            List<SupportSkillData> skills = GetSupportSkills(grade);

            if (skills.Count > 0)
                game.SoulShopList.Add(skills[UnityEngine.Random.Range(0, skills.Count)]);
            else
                AddRecommendSkills(grade);
        }

        return game.SoulShopList;
    }

    private List<SupportSkillData> GetSupportSkills(SupportSkillGrade grade)
    {
        return Managers.Data.SupportSkillDic.Values
                .Where(skill => skill.SupportSkillGrade == grade && skill.CheckRecommendationCondition())
                .ToList();
    }

    //���, �ش����� ī�带 �� �̾����� ���� ����� ī�带 �̴���.
    void AddRecommendSkills(SupportSkillGrade grade)
    {
        if ((int)grade > Enum.GetValues(typeof(SupportSkillGrade)).Length)
            return;
        List<SupportSkillData> commonSkills = new List<SupportSkillData>();
        SupportSkillGrade nextGrade = grade + 1;

        // 2. �ش� ��� ��ų ��� ��������
        commonSkills = GetSupportSkills(nextGrade);

        if (commonSkills.Count > 0)
            Managers.Game.SoulShopList.Add(commonSkills[UnityEngine.Random.Range(0, commonSkills.Count)]);
        else
            AddRecommendSkills(nextGrade);
    }

    public static SupportSkillGrade GetRandomGrade()
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Common])
        {
            return SupportSkillGrade.Common;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Uncommon])
        {
            return SupportSkillGrade.Uncommon;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Uncommon] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Epic])
        {
            return SupportSkillGrade.Epic;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Uncommon] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Epic] + SUPPORTSKILL_GRADE_PROB[(int)SupportSkillGrade.Rare])
        {
            return SupportSkillGrade.Rare;
        }
        else
        {
            return SupportSkillGrade.Legend;
        }
    }

    public bool CanLearnSkill(SupportSkillData skill)
    {
        if (skill.IsPurchased)
            return false;

        //if (SupportSkills.Count == 6)
        //{
        //    // �̹� ��� ��ų�� ���´�.
        // ��ȹ���� �ʿ�
        //}

        return true;
    }

    #endregion
}
