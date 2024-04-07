using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Suicide Bombing")]
public class SuicideBombingInfo : WeaponInfo
{
    [field: SerializeField] public GameObject Explosion { get; private set; }
    [field: SerializeField] public float DelayBeforeExplosion { get; private set; }
}
