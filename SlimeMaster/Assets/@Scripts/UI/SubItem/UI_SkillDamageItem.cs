using UnityEngine;
using UnityEngine.UI;

public class UI_SkillDamageItem : UI_Base
{

    #region UI ��� ����Ʈ
    // ���� ����
    // TargetImage : ��ų ������
    // SkillNameValueText : ��ų �̸�
    // SkillDamageValueText : ��ų�� ����
    // DamageProbabilityValueText : ��ü �� ������ ����
    // DamageSliderObject ��ü ������ ������ ��Ÿ�� �����̵�

    #endregion

    #region Enum
    enum GameObjects
    {
        DamageSliderObject
    }

    enum Texts
    {
        SkillNameValueText,
        SkillDamageValueText,
        DamageProbabilityValueText,
    }

    enum Images
    {
        SkillImage,
    }
    #endregion
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        #endregion

        Refresh();
        return true;
    }

    public void SetInfo(SkillBase skill)
    { 
        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.IconLabel);
        GetText((int)Texts.SkillNameValueText).text = $"{skill.SkillData.Name}";
        GetText((int)Texts.SkillDamageValueText).text = $"{skill.TotalDamage}";

        float allSkillDamage = Managers.Game.GetTotalDamage();
        float percentage = skill.TotalDamage / Managers.Game.GetTotalDamage();
        
        //�� ��ų �������� 0�϶� 100%�� ǥ��
        if (allSkillDamage == 0)
            percentage = 1;

        GetText((int)Texts.DamageProbabilityValueText).text = (percentage*100).ToString("F2")+"%";       
        GetObject((int)GameObjects.DamageSliderObject).GetComponent<Slider>().value = percentage;

        //transform.localScale = Vector3.one;
        Refresh();
    }

    void Refresh()
    {


    }

}
