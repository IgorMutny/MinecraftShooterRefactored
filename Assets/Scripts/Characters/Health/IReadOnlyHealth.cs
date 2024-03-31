using System;

public interface IReadOnlyHealth
{
    public int MaxHealth { get; }
    public float Defence { get; }
    public bool CanBeResurrected { get; }

    public event Action<Character> Died;
    public event Action<Character> Attacked;
    public event Action<int> Cured;
    public event Action<int> Damaged;
    public event Action Resurrected;
}
