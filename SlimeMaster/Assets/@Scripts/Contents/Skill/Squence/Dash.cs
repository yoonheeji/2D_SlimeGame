using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Dash : SequenceSkill
{
    Rigidbody2D _rb;
    Coroutine _coroutine;
    private void Awake()
    {
        SkillType = Define.SkillType.Dash;
        AnimagtionName = "Dash";
    }

    public override void DoSkill(Action callback = null)
    {
        UpdateSkillData(DataId);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CoDash(callback));
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator CoDash(Action callback = null)
    {
        _rb = GetComponent<Rigidbody2D>();
        
        float elapsed = 0;
        Vector3 dir;
        Vector2 targetPosition = Managers.Game.Player.CenterPosition; 

        GameObject obj = Managers.Resource.Instantiate("SkillRange", pooling : true);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        SkillRange skillRange = obj.GetOrAddComponent<SkillRange>();

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > SkillData.Duration)
                break;
            dir = ((Vector2)Managers.Game.Player.CenterPosition - _rb.position);
            targetPosition = Managers.Game.Player.CenterPosition + dir.normalized * SkillData.MaxCoverage;

            skillRange.SetInfo(dir, targetPosition, Vector3.Distance(_rb.position, targetPosition) );
            yield return null;
        }

        Managers.Resource.Destroy(obj);

        transform.GetChild(0).GetComponent<Animator>().Play(AnimagtionName);

        while (Vector3.Distance(_rb.position, targetPosition) > 0.3f)
        {
            Vector2 dirVec = targetPosition - _rb.position;

            Vector2 nextVec = dirVec.normalized * SkillData.ProjSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + nextVec);
            
            yield return null;
        }
        yield return new WaitForSeconds(SkillData.AttackInterval);
        callback?.Invoke();
    }

}

