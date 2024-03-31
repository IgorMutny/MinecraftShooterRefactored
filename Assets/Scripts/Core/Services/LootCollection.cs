public class LootCollection : IService
{
    private Character _player;

    public void SetPlayer(Character player)
    {
        _player = player;
    }
}
