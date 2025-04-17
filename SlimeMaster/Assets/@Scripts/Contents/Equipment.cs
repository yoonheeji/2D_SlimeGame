using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment 
{
    public string key = "";

    public Data.EquipmentData EquipmentData;
    public int Level { get; set; } = 1;
    public int AttackBonus { get; set; } = 0; 
    public int MaxHpBonus { get; set; } = 0;
    bool _isEquipped = false;
    public bool IsEquipped
    {
        get
        {
            //������� ���������� Ȯ��
            return _isEquipped;
        }
        set
        {
            _isEquipped = value;
        }
    }
    public bool IsOwned { get; set; } = false;
    public bool IsUpgradable { get; set; } = false;
    public bool IsConfirmed { get; set; } = false; // ��� ȹ���� Ȯ���ߴ���
    public bool IsEquipmentSynthesizable { get; set; } = false; // ��� �ռ���������
    public bool IsSelected { get; set; } = false; // �ռ��˾����� ���� �Ǿ��ִ���
    public bool IsUnavailable { get; set; } = false; // �ռ��˾����� ���� �Ұ�������

    public Equipment(string key)
    {
        this.key = key;

        EquipmentData = Managers.Data.EquipDataDic[key];

        SetInfo(Level);
        //AttackBonus = EquipmentData.AtkDmgBonus + Level * EquipmentData.AtkDmgBonusPerUpgrade;
        //MaxHpBonus = EquipmentData.MaxHpBonus + Level * EquipmentData.MaxHpBonusPerUpgrade;
        //this.IsEquipped = IsEquipped;
        IsOwned = true;
    }

    public void SetInfo(string key)
    {

    }
    
    public void SetInfo(int level)
    {
        Level = level;

        AttackBonus = EquipmentData.AtkDmgBonus + (Level - 1) * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + (Level - 1) * EquipmentData.MaxHpBonusPerUpgrade;
    }

    public void LevelUp()
    {
        Level++;
        EquipmentData = Managers.Data.EquipDataDic[key];
        AttackBonus = EquipmentData.AtkDmgBonus + (Level - 1) * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + (Level - 1) * EquipmentData.MaxHpBonusPerUpgrade;
    }

    public void ResetEquipLevel()
    {

        int receiveGold = 0; //�޾ƾ��� ���
        int receiveMaterial = 0; //�޾ƾ��� ���͸���
        while (Level > 1)
        {
            Level--;
            receiveGold += Managers.Data.EquipLevelDataDic[Level].UpgradeCost;//LevelUpCost.UpgradCost;
            receiveMaterial += Managers.Data.EquipLevelDataDic[Level].UpgradeRequiredItems;
        }
        
        Managers.Game.Gold += receiveGold;
        //Managers.Game.Material += receiveMaterial;

        // ���⼭ ����� �ʱ� �� ��������

        AttackBonus = EquipmentData.AtkDmgBonus + Level * EquipmentData.AtkDmgBonusPerUpgrade;
        MaxHpBonus = EquipmentData.MaxHpBonus + Level * EquipmentData.MaxHpBonusPerUpgrade;     
    }

}
