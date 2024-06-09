using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>Up- and downgrades for player</summary>
public class Upgrade : IEquatable<Upgrade>
{
    public const float LEVELMULTIPLIER = .1f;

    public int ID { get; private set; }
    private static int IDCount = -1;

    public event BaseValueChanged baseValueChanged;
    public event LevelChanged levelChanged;
    public event BaseValueActiveChanged baseValueActiveChanged;

    public string Name { get; set; }
    public Rarity Rarity { get; set; }

    public Upgrade()
    {
        Name = "Upgrade";
        ID = IDCount++;
        Level = 1;
        Rarity = Rarity.UNCOMMON;
    }

    /// <summary>
    /// Create Upgrade Class
    /// </summary>
    /// <param name="_name">Name of Upgrade</param>
    /// <param name="_baseMovementSpeedMultiplier">Movementspeed multiplier. If not set or set to 1, it will be deactivated</param>
    /// <param name="_baseShootIntervallMultiplier">Shoot intervall multiplier. If not set or set to 1, it will be deactivated</param>
    /// <param name="_baseDamageMultiplier">Damage multiplier. If not set or set to 1, it will be deactivated</param>
    /// <param name="_baseBulletCollideCount">Bullet collide count. If not set or set to 1, it will be deactivated</param>
    /// <exception cref="ArgumentNullException">Throwed if _name is null/></exception>
    public Upgrade(string _name, int _level = 1, float _baseMovementSpeedMultiplier = 1, float _baseShootIntervallMultiplier = 1, float _baseDamageMultiplier = 1, int _baseBulletCollideCount = 0, Rarity _rarity = Rarity.COMMON, BaseValueChanged _baseValueChanged = null, LevelChanged _levelChanged = null, BaseValueActiveChanged _baseValueActiveChanged = null)
    {
        if (_name == null)
            throw new ArgumentNullException(nameof(_name));
        Name = _name;

        if (_level > 0)
            p_Level = _level;
        else
            Level = 1;

        if (_baseMovementSpeedMultiplier != 1)
        {
            m_BaseMovementSpeedMultiplier = _baseMovementSpeedMultiplier;
            m_BaseMovementSpeedMultiplierActive = true;
        }

        if (_baseShootIntervallMultiplier != 1)
        {
            m_BaseShootIntervallMultiplier = _baseShootIntervallMultiplier;
            m_BaseShootIntervallMultiplierActive = true;
        }

        if (_baseDamageMultiplier != 1)
        {
            m_BaseDamageMultiplier = _baseDamageMultiplier;
            m_BaseDamageMultiplierActive = true;
        }

        if (_baseBulletCollideCount != 0)
        {
            m_BaseBulletCollideCount = _baseBulletCollideCount;
            m_BaseBulletCollideCountActive = true;
        }

        baseValueChanged = _baseValueChanged;
        levelChanged = _levelChanged;
        baseValueActiveChanged = _baseValueActiveChanged;

        Rarity = _rarity;

        ID = IDCount++;
    }

    private int p_Level;
    /// <summary>Level of Upgrade. The higher the better. Level 0 or below is not allowed.</summary>
    public int Level
    {
        get { return p_Level; }
        set
        {
            if (value < 0)
                return;
            var v = p_Level;
            if (p_Level == value)
                return;

            BeforeLevelChanged(value);
            p_Level = value;
            AfterLevelChanged(p_Level);
        }
    }
    #region Previous multipliers
    public float PreviousBaseMovementspeedMultiplier { get; private set; }
    public float PreviousBaseShootIntervallMultiplier { get; private set; }
    public float PreviousBaseDamageMultiplier { get; private set; }
    public int PreviousBaseBulletCollideCount { get; private set; }
    #endregion

    #region Multipliers
    private float m_BaseMovementSpeedMultiplier = 1;
    /// <summary>Speed Multiplier. The higher the better</summary>
    public float BaseMovementSpeedMultiplier
    {
        get => m_BaseMovementSpeedMultiplier;
        set
        {
            PreviousBaseMovementspeedMultiplier = m_BaseMovementSpeedMultiplier;
            m_BaseMovementSpeedMultiplier = value;
            baseValueChanged?.Invoke(this);
        }
    }
    private bool m_BaseMovementSpeedMultiplierActive = false;
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseMovementSpeedMultiplierActive
    {
        get => m_BaseMovementSpeedMultiplierActive;
        set
        {
            m_BaseMovementSpeedMultiplierActive = value;
            baseValueActiveChanged?.Invoke(this);
        }
    }
    // -------------------------------------------------------------------------------------------------------------- //

    private float m_BaseShootIntervallMultiplier = 1;
    /// <summary>Shoot interval multiplier. The lower the better</summary>
    public float BaseShootIntervallMultiplier
    {
        get => m_BaseShootIntervallMultiplier;
        set
        {
            PreviousBaseShootIntervallMultiplier = m_BaseShootIntervallMultiplier;
            m_BaseShootIntervallMultiplier = value;
            baseValueChanged?.Invoke(this);
        }
    }
    private bool m_BaseShootIntervallMultiplierActive = false;
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseShootIntervallMultiplierActive
    {
        get => m_BaseShootIntervallMultiplierActive;
        set
        {
            m_BaseShootIntervallMultiplierActive = value;
            baseValueActiveChanged?.Invoke(this);
        }
    }
    // -------------------------------------------------------------------------------------------------------------- //

    private float m_BaseDamageMultiplier = 1;
    /// <summary>Damage multiplier. The higher the better</summary>
    public float BaseDamageMultiplier
    {
        get => m_BaseDamageMultiplier;
        set
        {
            PreviousBaseDamageMultiplier = m_BaseDamageMultiplier;
            m_BaseDamageMultiplier = value;
            baseValueChanged?.Invoke(this);
        }
    }
    private bool m_BaseDamageMultiplierActive = false;
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseDamageMultiplierActive
    {
        get => m_BaseDamageMultiplierActive;
        set
        {
            m_BaseDamageMultiplierActive = value;
            baseValueActiveChanged?.Invoke(this);
        }
    }
    // -------------------------------------------------------------------------------------------------------------- //

    private int m_BaseBulletCollideCount = 0;
    /// <summary>Says how much enemies a bullet can collide with. The higher the better</summary>
    public int BaseBulletCollideCount
    {
        get => m_BaseBulletCollideCount;
        set
        {
            PreviousBaseBulletCollideCount = m_BaseBulletCollideCount;
            m_BaseBulletCollideCount = value;
            baseValueChanged?.Invoke(this);
        }
    }
    private bool m_BaseBulletCollideCountActive = false;
    /// <summary>Multiplier is active in this Upgrade</summary>
    public bool BaseBulletCollideCountActive
    {
        get => m_BaseBulletCollideCountActive;
        set
        {
            m_BaseBulletCollideCountActive = value;
            baseValueActiveChanged?.Invoke(this);
        }
    }
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
            return GetMoveSpeedMultiplierByLevel(this, Level, false);
    }
    /// <summary>
    /// Calculate multiplier for movementspeed by level
    /// </summary>
    /// <param name="_upgrade">Upgrade</param>
    /// <param name="_level">Level the upgrade has</param>
    /// <returns>Movementspeed multiplier at level</returns>
    public static float GetMoveSpeedMultiplierByLevel(Upgrade _upgrade, int _level, bool _checkActiveness)
    {
        float tr = _upgrade.BaseDamageMultiplier + (_level * LEVELMULTIPLIER);
        if (!_checkActiveness)
            return tr;

        if (!_upgrade.BaseMovementSpeedMultiplierActive) return 1;
        else
            return tr;
    }
    public static float GetCustomMoveSpeedMultiplier(float _multiplier, int _level)
    {
        return _multiplier + (_level * LEVELMULTIPLIER);
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
            return GetShootIntervalByLevel(this, Level, false);
    }
    /// <summary>
    /// Calculate multiplier for shoot interval by level
    /// </summary>
    /// <param name="_upgrade">Upgrade</param>
    /// <param name="_level">Level the upgrade has</param>
    /// <returns>Shoot interval multiplier at level</returns>
    public static float GetShootIntervalByLevel(Upgrade _upgrade, int _level, bool _checkActiveness)
    {
        float tr = _upgrade.BaseShootIntervallMultiplier - (_level * LEVELMULTIPLIER);
        if (!_checkActiveness)
            return tr;

        if (!_upgrade.BaseShootIntervallMultiplierActive) return 1;
        else
            return tr;
    }
    public static float GetCustomShootInterval(float _multiplier, int _level)
    {
        return _multiplier + (_level * LEVELMULTIPLIER);
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
            return GetDamageByLevel(this, Level, false);
    }
    /// <summary>
    /// Calculate multiplier for damage by level
    /// </summary>
    /// <param name="_upgrade">Upgrade</param>
    /// <param name="_level">Level the upgrade has</param>
    /// <returns>Damage multiplier at level</returns>
    public static float GetDamageByLevel(Upgrade _upgrade, int _level, bool _checkActiveness)
    {
        float tr = _upgrade.BaseDamageMultiplier + (_level * LEVELMULTIPLIER);
        if (!_checkActiveness)
            return tr;

        if (!_upgrade.BaseDamageMultiplierActive) return 1;
        else
            return tr;
    }
    public static float GetCustomDamageMultiplier(float _multiplier, int _level)
    {
        return _multiplier + (_level * LEVELMULTIPLIER);
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
            return GettBulletCollideCountByLevel(this, Level, false);
    }
    /// <summary>
    /// Calculate multiplier for bullet collide count by level
    /// </summary>
    /// <param name="_upgrade">Upgrade</param>
    /// <param name="_level">Level the upgrade has</param>
    /// <returns>Bullet collide cound at level</returns>
    public static int GettBulletCollideCountByLevel(Upgrade _upgrade, int _level, bool _checkActiveness)
    {
        int tr = (int)(_upgrade.BaseBulletCollideCount + (_level * LEVELMULTIPLIER));
        if (!_checkActiveness)
            return tr;

        if (!_upgrade.BaseDamageMultiplierActive) return 0;
        else
            return tr;
    }
    public static int GetCustomBulletCollideCount(int _count, int _level)
    {
        return (int)(_count + (_level * LEVELMULTIPLIER));
    }
    #endregion

    /// <summary>Called before <see cref="Level"/> changes</summary>
    protected virtual void BeforeLevelChanged(int _newLevel)
    {

    }
    /// <summary>Called after <see cref="Level"/> changed</summary>
    protected virtual void AfterLevelChanged(int _previousLevel)
    {
        levelChanged?.Invoke(this, _previousLevel);
    }

    #region Equals and override
    public override bool Equals(object obj)
    {
        return Equals(obj as Upgrade);
    }

    public bool Equals(Upgrade other)
    {
        return other is not null &&
               ID == other.ID;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID);
    }
    #endregion

    #region Operator
    public static bool operator ==(Upgrade left, Upgrade right)
    {
        return EqualityComparer<Upgrade>.Default.Equals(left, right);
    }

    public static bool operator !=(Upgrade left, Upgrade right)
    {
        return !(left == right);
    }
    #endregion
}
public enum Rarity
{
    NONE,
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY,
    RELIC 
}

public delegate void LevelChanged(Upgrade _upgrade, int _previousLevel);
public delegate void BaseValueChanged(Upgrade _upgrade);
public delegate void BaseValueActiveChanged(Upgrade _upgrade);