using System;

[Serializable]
public struct WaveRecord
{
    public CharacterInfo Character;
    public int Amount;

    public WaveRecord(CharacterInfo character, int amount)
    {
        Character = character;
        Amount = amount;
    }
}
