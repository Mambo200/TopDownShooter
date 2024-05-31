using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>Up- and downgrades for player</summary>
public class Upgrade
{
    public const float LEVELMULTIPLIER = 1.2f;

    private int p_Level;
    /// <summary>Level of Upgrade. The higher the better</summary>
    public int Level
    {
        get { return p_Level; }
        set
        {
            var v = p_Level;
            if (p_Level == value)
                return;

            BeforeLevelChanged(value);
            p_Level = value;
            AfterLevelChanged(p_Level);
        }
    }

    #region Multipliers
    private float m_BaseMovementSpeedMultiplier = 1;
    /// <summary>Speed Multiplier. The higher the better</summary>
    public float BaseMovementSpeedMultiplier 
    { 
        get => m_BaseMovementSpeedMultiplier ;
        private set => m_BaseMovementSpeedMultiplier = value;
    }
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseMovementSpeedMultiplierActive { get; set; }


    public float m_BaseShootIntervallMultiplier = 1;
    /// <summary>Shoot interval multiplier. The lower the better</summary>
    public float BaseShootIntervallMultiplier
    { 
        get => m_BaseShootIntervallMultiplier;
        private set => m_BaseShootIntervallMultiplier = value;
    }
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseShootIntervallMultiplierActive { get; set; }


    public float m_BaseDamageMultiplier = 1;
    /// <summary>Damage multiplier. The higher the better</summary>
    public float BaseDamageMultiplier
    {
        get => m_BaseDamageMultiplier;
        private set => m_BaseDamageMultiplier = value;
    }
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseDamageMultiplierActive { get; set; }


    public int m_BaseBulletCollideCount = 0;
    /// <summary>Says how much enemies a bullet can collide with. The higher the better</summary>
    public int BaseBulletCollideCount
    {
        get => m_BaseBulletCollideCount;
        private set => m_BaseBulletCollideCount = value;
    }
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseBulletCollideCountActive { get; set; }
    #endregion

    #region Multiplier calculation
    /// <summary>
    /// Calculate multiplier for Movementspeed
    /// </summary>
    /// <returns>Movementspeed multiplier</returns>
    public float GetCurrentMoveSpeedMultiplier()
    {
        if (!BaseMovementSpeedMultiplierActive)
            return 1;
        else
            return m_BaseDamageMultiplier + (Level * LEVELMULTIPLIER); 
    }

    /// <summary>
    /// Calculate multiplier for shoot interval
    /// </summary>
    /// <returns>Shoot interval multiplier</returns>
    public float GetCurrentShootIntervallMultiplier()
    {
        if (!BaseShootIntervallMultiplierActive)
            return 1;
        else
            return BaseShootIntervallMultiplier + (Level * LEVELMULTIPLIER);
    }

    /// <summary>
    /// Calculate multiplier for damage
    /// </summary>
    /// <returns>Damage multiplier</returns>
    public float GetCurrentDamageMultiplier()
    {
        if (!BaseDamageMultiplierActive)
            return 1;
        else
            return BaseDamageMultiplier + (Level * LEVELMULTIPLIER);
    }

    /// <summary>
    /// Calculate amount for bullet collide count
    /// </summary>
    /// <returns>bullet collide count amount</returns>
    public int GetCurrentBulletCollideCount()
    {
        if (!BaseBulletCollideCountActive)
            return 0;
        else
            return (int)(BaseBulletCollideCount + (Level * LEVELMULTIPLIER));
    }
    #endregion

    /// <summary>Called before <see cref="Level"/> changes</summary>
    protected virtual void BeforeLevelChanged(int _newLevel)
    {

    }
    /// <summary>Called after <see cref="Level"/> changed</summary>
    protected virtual void AfterLevelChanged(int _previousLevel)
    {

    }
}
