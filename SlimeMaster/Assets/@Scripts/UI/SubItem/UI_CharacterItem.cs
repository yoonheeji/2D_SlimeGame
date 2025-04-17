using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_CharacterItem : UI_Base
{
    #region UI ��� ����Ʈ
    // ���� ����
    // SelectObject : ������ ĳ���͸� �������� ���� ��Ȳ�� �˷��ִ� �ƿ�����
    // CharacterImage : ĳ���� �̹���
    // CharacterLevelValueText : ĳ���� ����
    // EquipedObject : ���� ĳ�����϶� Ȱ��ȭ
    // LockObject : �����ϰ� �ִٸ� ��Ȱ��ȭ
    // EquipmentRedDotObject : ĳ���Ͱ� ��ȸ(������ or ���׷��̵�)�� �����ϴٸ� Ȱ��ȭ (�ִϸ��̼� �߰� �ʿ�) 

    // ���ö���¡
    // EquipedText
    #endregion

    #region Enums
    enum GameObjects
    {
        SelectObject,
        EquipedObject,
        LockObject,
        EquipmentRedDotObject,
    }

    enum Texts
    {
        CharacterLevelValueText,
        EquipedText,
    }

    enum Images
    {
        CharacterImage,
    }

    enum Buttons
    {
        UI_CharacterItem,
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

        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.LockObject).gameObject.SetActive(true);
        GetObject((int)GameObjects.EquipedObject).gameObject.SetActive(false); 
        GetObject((int)GameObjects.EquipmentRedDotObject).gameObject.SetActive(false); 

        GetButton((int)Buttons.UI_CharacterItem).gameObject.BindEvent(OnClickCharacterItem);

#if UNITY_EDITOR
        // �׽�Ʈ��
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

    void OnClickCharacterItem() // ĳ���� ���� ��ư
    {
        Managers.Sound.PlayButtonClick();
        GetObject((int)GameObjects.SelectObject).gameObject.SetActive(true);
    }
}
