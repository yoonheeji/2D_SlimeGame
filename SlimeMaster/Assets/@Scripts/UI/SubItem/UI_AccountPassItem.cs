using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_AccountPassItem : UI_Base
{
    #region UI ��� ����Ʈ
    // ���� ����
    // AccountLevelValueText : ���� ����

    // FreePassRewardItmeImage : ���� ������ ������ �̹���
    // RarePassRewardItmeImage : ���� ������ ������ �̹���
    // EpicPassRewardItmeImage : ���� ������ ������ �̹���

    // FreePassRewardItmeValueText : ���� ������ ������ ����
    // RarePassRewardItmeValueText : ���� ������ ������ ����
    // EpicPassRewardItmeValueText : ���� ������ ������ ����

    // FreePassRewardLockObject : ���� ���� (��������) �޼� �� ��Ȱ��ȭ
    // RarePassRewardLockObject : ���� ���� (��������) �޼� �� ��Ȱ��ȭ
    // EpicPassRewardLockObject : ���� ���� (��������) �޼� �� ��Ȱ��ȭ

    #endregion


    #region Enum
    enum GameObjects
    {
        FreePassRewardCompleteObject,
        RarePassRewardCompleteObject,
        EpicPassRewardCompleteObject,

        FreePassRewardLockObject,
        RarePassRewardLockObject,
        EpicPassRewardLockObject,
    }

    enum Buttons
    {
        FreePassRewardButton,
        RarePassRewardButton,
        EpicPassRewardButton,
    }

    enum Texts
    {
        AccountLevelValueText,
        FreePassRewardItmeValueText,
        RarePassRewardItmeValueText,
        EpicPassRewardItmeValueText,
    }

    enum Images
    {
        FreePassRewardItmeImage,
        RarePassRewardItmeImage,
        EpicPassRewardItmeImage,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.FreePassRewardButton).gameObject.BindEvent(OnClickFreePassRewardButton);
        GetButton((int)Buttons.FreePassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.RarePassRewardButton).gameObject.BindEvent(OnClickRarePassRewardButton);
        GetButton((int)Buttons.RarePassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EpicPassRewardButton).gameObject.BindEvent(OnClickEpicPassRewardButton);
        GetButton((int)Buttons.EpicPassRewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetObject((int)GameObjects.FreePassRewardLockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.RarePassRewardLockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.EpicPassRewardLockObject).gameObject.SetActive(true);

        GetObject((int)GameObjects.FreePassRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.RarePassRewardCompleteObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.EpicPassRewardCompleteObject).gameObject.SetActive(false);

        // �׽�Ʈ��
#if UNITY_EDITOR

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

    void OnClickFreePassRewardButton()
    {
        // ���� ���� ���� �� Ȱ��ȭ
        // GetObject((int)GameObjects.FreePassRewardLockObject).gameObject.SetActive(true);
    }
    void OnClickRarePassRewardButton()
    {
        // ���� ���� ���� �� Ȱ��ȭ
        // GetObject((int)GameObjects.RarePassRewardLockObject).gameObject.SetActive(true);

    }
    void OnClickEpicPassRewardButton()
    {
        // ���� ���� ���� �� Ȱ��ȭ
        // GetObject((int)GameObjects.EpicPassRewardLockObject).gameObject.SetActive(true);
    }
}
