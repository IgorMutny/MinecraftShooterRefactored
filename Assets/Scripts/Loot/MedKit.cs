using UnityEngine;

public class MedKit : Loot
{
    [SerializeField] private int _healthPoints;

    protected override void Apply(Character character)
    {
        character.Health.GetCure(_healthPoints);
    }
}
