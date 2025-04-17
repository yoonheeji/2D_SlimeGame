using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SettingPopup : UI_Popup
{
    #region UI ��� ����Ʈ
    // ���� ����
    // UseIDValueText : ���� ���̵�
    // VersionValueText : ���� Ŭ���̾�Ʈ�� ���� ǥ��

    // ��ư
    // SoundEffectOffButton, SoundEffectOnButton : ȿ���� �¿��� ��� (���� ����)
    // BackgroundSoundOffButton, BackgroundSoundOnButton : BGM �¿��� ��� (���� ����)
    // JoystickFixedOffButton, JoystickFixedOnButton : ���̽�ƽ ���� �¿��� ��� (���� ����)

    // ���ö���¡ �ؽ�Ʈ
    // SettingTlileText : ����
    // UserInfoText : ���� ����
    // SoundEffectText : ȿ����
    // BackgroundSoundText : �����
    // JoystickText : ���̽�ƽ
    // LanguageText : ���
    // TermsOfServiceButtonText : ���� �̿���
    // PrivacyPolicyButtonText : �������� ó����ħ
    // FeedbackButtonText : Feedback
    // ErrorFixButtonText : Error Fix

    #endregion


    #region Enum
    enum GameObjects
    {
        ContentObject,
        //LanguageObject
    }
    enum Buttons
    {
        BackgroundButton,
        SoundEffectOffButton,
        SoundEffectOnButton,
        BackgroundSoundOffButton,
        BackgroundSoundOnButton,
        JoystickFixedOffButton,
        JoystickFixedOnButton,
        //LanguageButton,
        //TermsOfServiceButton,
        //PrivacyPolicyButton,
    }

    enum Texts
    {
        SettingTlileText,
        //UserInfoText,
        //UseIDValueText,
        SoundEffectText,
        BackgroundSoundText,
        JoystickText,
        //LanguageText,
        //LanguageValueText,
        //TermsOfServiceButtonText,
        //PrivacyPolicyButtonText,
        VersionValueText
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

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetButton((int)Buttons.SoundEffectOffButton).gameObject.BindEvent(EffectSoundOn);
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.BindEvent(EffectSoundOff);

        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.BindEvent(BackgroundSoundOn);
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.BindEvent(BackgroundSoundOff);

        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.BindEvent(OnCllickJoystickFixed);
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.BindEvent(OnCllickJoystickNonFixed);

        GetText((int)Texts.VersionValueText).text = $"���� : {Application.version}";

        if (Managers.Game.BGMOn == false)
        {
            BackgroundSoundOff();
        } 
        else
        {
            BackgroundSoundOn();
        }

        if (Managers.Game.EffectSoundOn == false)
        {
            EffectSoundOff();
        }
        else
        {
            EffectSoundOn();
        }

        if (Managers.Game.JoystickType == Define.JoystickType.Fixed)
        {
            OnCllickJoystickFixed();
        }
        else
        {
            OnCllickJoystickNonFixed();
        }


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

    void EffectSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = true;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
    }

    void EffectSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = false;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
    }

    void BackgroundSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = true;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(false);
    }

    void BackgroundSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = false;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(true);
    }

    void OnCllickJoystickFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.JoystickType.Fixed;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(false);
    }

    void OnCllickJoystickNonFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.JoystickType.Flexible;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(true);
    }

    void OnClickBackgroundButton() // �ݱ� ��ư
    {
        Managers.UI.ClosePopupUI(this);
    }

}
