using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_BackToHomePopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���ö���¡
    // BackToBattleTitleText : ���� ����
    // BackToBattleContentText : ���� �׸��θ� ������ ��� �ҽ��ϴ�.\n�κ�� ���ư��ðڽ��ϱ�?
    // ConfirmText : ���ư���
    // CancelText : �׸��д�

    #endregion

    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ResumeButton,
        QuitButton,
    }

    enum Texts
    {
        BackToHomeTitleText,
        BackToHameContentText,
        ResumeText,
        QuitText,
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
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        GetButton((int)Buttons.QuitButton).GetOrAddComponent<UI_ButtonAnimation>();

        // �׽�Ʈ��
#if UNITY_EDITOR

        //TextBindTest();
#endif
        #endregion

        Refresh();
        return true;
    }

    public void SetInfo()
    {

        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickResumeButton()
    {
        Managers.UI.ClosePopupUI(this); 
    }
    void OnClickQuitButton()
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = true;
        Managers.Game.Player.StopAllCoroutines();

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
