using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : SequenceSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.BasicAttack;
        AnimagtionName = "Attack";
    }

    public override void OnChangedSkillData()
    {
    }

    public override void DoSkill(Action callback = null)
    {
        CreatureController owner = GetComponent<CreatureController>();
        if (owner.CreatureState != Define.CreatureState.Skill)
            return;

        UpdateSkillData(DataId);

        _coroutine = null;
        _coroutine = StartCoroutine(CoSkill(callback));
    }

    Coroutine _coroutine;
    IEnumerator CoSkill(Action callback = null)
    {
        // ��ų���� ���̵���� ����� 
        GameObject obj = Managers.Resource.Instantiate("SkillRange", pooling: true);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        SkillRange sr = obj.GetComponent<SkillRange>();
        float radius = SkillData.ProjRange;
        float wait = sr.SetCircle(radius * 2);
        transform.GetChild(0).GetComponent<Animator>().Play(AnimagtionName);
        yield return new WaitForSeconds(wait);
        
        Managers.Resource.Destroy(obj);
        
        // �÷��̾�� ���� �Ÿ��� radius ���ϸ� ����� �ֱ�
        // 1. Ÿ�� �ݶ��̴� ������
        float targetRadus = Managers.Game.Player.ColliderRadius;
        // 2. ��ų���� ������ radius

        // �� �������� �Ÿ��� �������� �� ���� ������
        if (Vector3.Distance(transform.position, Managers.Game.Player.CenterPosition) < radius + targetRadus)
            Managers.Game.Player.OnDamaged(Owner, this, 0);

        // Hit Effect
        GameObject HitEffectObj = Managers.Resource.Instantiate("BossSmashHitEffect", pooling: true);
        HitEffectObj.transform.SetParent(transform);
        HitEffectObj.transform.localPosition = Vector3.zero;
        HitEffectObj.transform.localScale = Vector3.one * radius * 0.3f;
        yield return new WaitForSeconds(0.7f);
        Managers.Resource.Destroy(HitEffectObj);

        yield return new WaitForSeconds(SkillData.AttackInterval);

        callback?.Invoke();
    }
}
