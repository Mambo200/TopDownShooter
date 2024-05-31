// TODO: "BaseValueChanged" überarbeiten. Restliche Methoden befüllen.
// Zum Test ein Upgrade hinzufügen.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrades
{
    public List<Tuple<Upgrade, int>> Enchantment { get; set; }
    public float MoveSpeedMultiplierTotal { get; private set; }
    public float ShootSpeedMultiplierTotal { get; private set; }
    public float DamageMultiplierTotal { get; private set; }
    public int BulletCollisionTotal { get; private set; }

    public Upgrades()
    {
        Enchantment = new List<Tuple<Upgrade, int>>();
    }

    public void AddUpgradeWithEvent(Upgrade _upgrade)
    {
        _upgrade.levelChanged += LevelChanged;
        _upgrade.baseValueChanged += BaseValueChanged;
        _upgrade.baseValueActiveChanged += BaseValueActiveChanged;
        Enchantment.Add(new Tuple<Upgrade, int>(_upgrade, _upgrade.Level));

    }
    public void AddUpgradeWithoutEvent(Upgrade _upgrade)
    {
        Enchantment.Add(new Tuple<Upgrade, int>(_upgrade, _upgrade.Level));
    }

    private void BaseValueChanged(Upgrade _upgrade)
    {
        // remove old values
        MoveSpeedMultiplierTotal /= _upgrade.PreviousBaseMovementspeedMultiplier;
        ShootSpeedMultiplierTotal /= _upgrade.PreviousBaseShootIntervallMultiplier;
        DamageMultiplierTotal /= _upgrade.PreviousBaseDamageMultiplier;
        BulletCollisionTotal -= _upgrade.PreviousBaseBulletCollideCount;

        // add new values
        MoveSpeedMultiplierTotal *= _upgrade.GetCurrentMoveSpeedMultiplier();
        ShootSpeedMultiplierTotal *= _upgrade.GetCurrentShootIntervallMultiplier();
        DamageMultiplierTotal *= _upgrade.GetCurrentDamageMultiplier();
        BulletCollisionTotal += _upgrade.GetCurrentBulletCollideCount();
    }
    private void LevelChanged(Upgrade _upgrade, int _previousLevel)
    {

    }
    private void BaseValueActiveChanged(Upgrade _upgrade)
    {

    }

    public void RecalculateAll()
    {
        ResetAll();
        foreach (var upgrade in Enchantment)
        {
            MoveSpeedMultiplierTotal *= upgrade.Item1.GetCurrentMoveSpeedMultiplier();
            ShootSpeedMultiplierTotal *= upgrade.Item1.GetCurrentShootIntervallMultiplier();
            DamageMultiplierTotal *= upgrade.Item1.GetCurrentDamageMultiplier();
            BulletCollisionTotal += upgrade.Item1.GetCurrentBulletCollideCount();
        }
    }

    /// <summary>
    /// Resets all multiplicators to its defauls value
    /// </summary>
    private void ResetAll()
    {
        MoveSpeedMultiplierTotal = 1;
        ShootSpeedMultiplierTotal = 1;
        DamageMultiplierTotal = 1;
        BulletCollisionTotal = 0;
    }
}
