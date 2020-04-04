using System;
using System.Collections;
using System.Collections.Generic;
using Kameosa.Components;
using UnityEngine;

public interface IDamageable
{
    DamageableComponent DamageableComponent { get; }
}
