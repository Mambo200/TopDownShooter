using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContact
{
    public void ReceiveDamage(float _rawDamage);
    public void Heal(float _healamount);
}
