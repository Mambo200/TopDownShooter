// TODO: "BaseValueChanged" überarbeiten. Restliche Methoden befüllen.
// Zum Test ein Upgrade hinzufügen.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrades
{
    public List<Upgrade> Enchantment { get; set; }
    public float MoveSpeedMultiplierTotal { get; private set; }
    public float ShootSpeedMultiplierTotal { get; private set; }
    public float DamageMultiplierTotal { get; private set; }
    public int BulletCollisionTotal { get; private set; }

    public Upgrades()
    {
        Enchantment = new List<Upgrade>();

        ResetAll();
    }

    public void AddUpgradeWithEvent(Upgrade _upgrade)
    {
        _upgrade.levelChanged += LevelChanged;
        _upgrade.baseValueChanged += BaseValueChanged;
        _upgrade.baseValueActiveChanged += BaseValueActiveChanged;
        Enchantment.Add(_upgrade);
        AddToMultiplier(_upgrade);
    }
    public void AddUpgradeWithoutEvent(Upgrade _upgrade)
    {
        Enchantment.Add(_upgrade);
        AddToMultiplier(_upgrade);
    }

    private void BaseValueChanged(Upgrade _upgrade)
    {
        // This is very much impossible. You could remove the old value, but you do not know on which multiplier the upgrade go added.
        // Level does have an impact, too. So to be sure when a base value gets updates, it updates everything just to be sure.
        // Hopefully this does not get called to often lol.

        RecalculateAll();
    }
    private void LevelChanged(Upgrade _upgrade, int _previousLevel)
    {
        // remove old calculation
        MoveSpeedMultiplierTotal /= Upgrade.GetMoveSpeedMultiplierByLevel(_upgrade, _previousLevel, true);
        ShootSpeedMultiplierTotal /= Upgrade.GetShootIntervalByLevel(_upgrade, _previousLevel, true);
        DamageMultiplierTotal /= Upgrade.GetDamageByLevel(_upgrade, _previousLevel, true);
        BulletCollisionTotal -= Upgrade.GettBulletCollideCountByLevel(_upgrade, _previousLevel, true);

        // add new calculation
        AddToMultiplier(_upgrade);
    }
    private void BaseValueActiveChanged(Upgrade _upgrade)
    {
        // The scenario is one value got from not active to active. I remove this value which was not calculated in before and give it now back.
        // Because of that recalculation of everything.
        // But tbh this is very stupid this should never be a thing.
        RecalculateAll();
    }

    public void RecalculateAll()
    {
        ResetAll();
        foreach (var upgrade in Enchantment)
        {
            AddToMultiplier(upgrade);
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

    private void AddToMultiplier(Upgrade _upgrade)
    {
        MoveSpeedMultiplierTotal *= _upgrade.GetCurrentMoveSpeedMultiplier();
        ShootSpeedMultiplierTotal *= _upgrade.GetCurrentShootIntervallMultiplier();
        DamageMultiplierTotal *= _upgrade.GetCurrentDamageMultiplier();
        BulletCollisionTotal += _upgrade.GetCurrentBulletCollideCount();
    }
}
