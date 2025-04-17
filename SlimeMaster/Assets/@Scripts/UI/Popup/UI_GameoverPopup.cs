using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameoverPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // GameoverStageValueText : �ش� �������� ��
    // GameoverLastWaveValueText : �ױ��� ������ ���̺� ��
    // GameoverGoldValueText : �ױ��� ���� ���� ���
    // GameoverKillValueText : �ױ��� ���� ų ��

    // ���ö���¡ �ؽ�Ʈ
    // GameoverPopupTitleText : ���� ����
    // LastWaveText : ������ ���̺�
    // ConfirmButtonText : OK

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
        GameoverKillObject,
    }
    enum Texts
    {
        GameoverPopupTitleText,
        GameoverStageValueText,
        LastWaveText,
        GameoverLastWaveValueText,
        GameoverKillValueText,
        ConfirmButtonText,
    }

    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,

    }
    #endregion
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
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        Managers.Sound.Play(Define.Sound.Effect, "PopupOpen_Gameover");
#if UNITY_EDITOR

        //TextBindTest();
#endif

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        // GameoverStageValueText : �ش� �������� ��
        GetText((int)Texts.GameoverStageValueText).text = $"{Managers.Game.CurrentStageData.StageIndex} STAGE";
        // GameoverLastWaveValueText : �ױ��� ������ ���̺� ��
        GetText((int)Texts.GameoverLastWaveValueText).text = $"{Managers.Game.CurrentWaveIndex + 1}";
        // GameoverKillValueText : �ױ��� ���� ų ��
        GetText((int)Texts.GameoverKillValueText).text = $"{Managers.Game.Player.KillCount}";

        Refresh();
    }

    void Refresh()
    {
        // �������� ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameoverKillObject).GetComponent<RectTransform>());
    }

    void OnClickStatisticsButton() // ��� ��ư
    {
        Managers.Sound.PlayButtonClick();
        // ��� �˾� ȣ��p
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();

    }
    void OnClickConfirmButton() // Ȯ�� ��ư
    {
        Managers.Sound.PlayButtonClick();

        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            // ��� ����
            if (Managers.Game.CurrentWaveIndex > info.MaxWaveIndex)
            {
                info.MaxWaveIndex = Managers.Game.CurrentWaveIndex;
                Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
            }
        }

        Managers.Game.ClearContinueData();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }
}
