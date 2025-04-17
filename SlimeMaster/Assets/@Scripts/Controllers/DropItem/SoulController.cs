using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SoulController : DropItemController
{
    public int _soudCount = 5;
    Coroutine _coMoveToPlayer;

    public override void OnDisable()
    {
        if (_coMoveToPlayer != null)
        {
            StopCoroutine(_coMoveToPlayer);
            _coMoveToPlayer = null;
        }
    }

    public override bool Init()
    {
        base.Init();
        //�ҿ� ȹ�淮
        _soudCount = Define.STAGE_SOULCOUNT;
        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coMoveToPlayer == null && this.IsValid())
        {
            Sequence seq = DOTween.Sequence();
            Vector3 dir = (transform.position - Managers.Game.SoulDestination).normalized;
            Vector3 target = transform.position + dir * 0.5f;
            seq.Append(transform.DOMove(target, 0.4f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                _coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
            });

        }
    }

    public IEnumerator CoMoveToPlayer()
    {
        float speed = 17f;
        float acceleration = 8.5f;

        while (this.IsValid())
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.SoulDestination);

            // ���� �ð��� ���� �ӵ� ���
            speed += acceleration * Time.deltaTime;

            // ������ �������� ������ �ӵ��� �̵�
            Vector3 direction = (Managers.Game.SoulDestination - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (dist < 0.4f)
            {
                string name = "SoulGet_01";
                Managers.Sound.Play(Define.Sound.Effect, name);
                Managers.Game.Player.SoulCount += _soudCount * Managers.Game.Player.SoulBonusRate;
                Managers.Object.Despawn(this);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
