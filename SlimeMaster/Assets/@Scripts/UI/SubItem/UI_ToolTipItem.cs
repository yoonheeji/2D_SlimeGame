using UnityEngine;
using static Define;


public class UI_ToolTipItem : UI_Base
{
    #region Enum

    enum Images
    {
        TargetImage,
        BackgroundImage,
    }

    enum Buttons
    {
        CloseButton
    }

    enum Texts
    {
        TargetNameText,
        TargetDescriptionText
    }
    #endregion

    RectTransform m_Canvas;
    Camera m_UiCamera;

    private void OnEnable()
    {
        GetText((int)Texts.TargetNameText).gameObject.SetActive(false); // �⺻ ��Ȱ��ȭ ����
        GetText((int)Texts.TargetNameText).gameObject.SetActive(false); // �⺻ ��Ȱ��ȭ ����
    }

    private void Awake()
    {
        Init();

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

        #endregion

        Refresh();
        return true;
    }

    // ����Ʈ ��ų ����
    public void SetInfo(Data.SupportSkillData skillData, RectTransform targetPos, RectTransform parentsCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(skillData.IconLabel);
        GetText((int)Texts.TargetNameText).gameObject.SetActive(true);
        GetText((int)Texts.TargetNameText).text = skillData.Name;
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
        GetText((int)Texts.TargetDescriptionText).text = skillData.Description;
        // ��޿� ���� ��� ���� ����
        switch (skillData.SupportSkillGrade)
        {
            case SupportSkillGrade.Common:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
                break;
            case SupportSkillGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case SupportSkillGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case SupportSkillGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case SupportSkillGrade.Legend:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }

        ToolTipPosSet(targetPos, parentsCanvas); // ��ġ ����

        Refresh();
    }

    // ��� ����
    public void SetInfo(Data.MaterialData materialData, RectTransform targetPos, RectTransform parentsCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(materialData.SpriteName);
        GetText((int)Texts.TargetNameText).gameObject.SetActive(true); 
        GetText((int)Texts.TargetNameText).text = materialData.NameTextID;
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true); 
        GetText((int)Texts.TargetDescriptionText).text = materialData.DescriptionTextID;
        // ��޿� ���� ��� ���� ����
        switch (materialData.MaterialGrade)
        {
            case MaterialGrade.Common:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
                break;
            case MaterialGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
                break;
            case MaterialGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
                break;
            case MaterialGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
                break;
            case MaterialGrade.Legendary:
                GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
                break;
            default:
                break;
        }

        ToolTipPosSet(targetPos, parentsCanvas); // ��ġ ����
        Refresh();
    }

    // ���� ����
    public void SetInfo(Data.CreatureData creatureData, RectTransform targetPos, RectTransform parentsCanvas)
    {
        GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(creatureData.IconLabel);
        GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
        GetText((int)Texts.TargetDescriptionText).text = creatureData.DescriptionTextID;
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;

        ToolTipPosSet(targetPos, parentsCanvas); // ��ġ ����
        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickCloseButton()
    {
        Managers.Sound.PlayButtonClick();
        //�ڽ� ����
        Managers.Resource.Destroy(gameObject);
    }

    // ���� ��ġ ����
    void ToolTipPosSet(RectTransform targetPos, RectTransform parentsCanvas)
    {
        // �⺻ ����
        gameObject.transform.position = targetPos.transform.position;

        // ���� ���� ����
        float sizeY = targetPos.sizeDelta.y / 2;
        transform.localPosition += new Vector3(0f, sizeY);

        // ���� ���� ����
        if (targetPos.transform.localPosition.x > 0) // ������
        {
            float canvasMaxX = parentsCanvas.sizeDelta.x / 2;
            float targetPosMaxX = transform.localPosition.x + transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMaxX < targetPosMaxX)
            {
                float deltaX = targetPosMaxX - canvasMaxX;
                transform.localPosition = -new Vector3(deltaX+20, 0f) + transform.localPosition;
            }

        }
        else // ����
        {
            float canvasMinX = -parentsCanvas.sizeDelta.x / 2;
            float targetPosMinX = transform.localPosition.x - transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMinX > targetPosMinX)
            {
                float deltaX = canvasMinX - targetPosMinX;
                transform.localPosition = new Vector3(deltaX+20, 0f) + transform.localPosition;
            }

        }
        
    }
}
