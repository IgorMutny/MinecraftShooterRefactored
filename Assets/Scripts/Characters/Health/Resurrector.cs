public class Resurrector
{
    private Health _health;
    private GameDataService _gameDataService;
    private TotemInfo _immortalityInfo;

    public bool CanBeResurrected { get; private set; }

    public Resurrector(Health health, GameDataService gameDataService)
    {
        _health = health;
        _gameDataService = gameDataService;

        SetResurrection();
    }

    public Resurrector(Health health)
    {
        _health = health;
        CanBeResurrected = false;
    }

    private void SetResurrection()
    {
        ItemInfoCollection itemInfoCollection =
            ServiceLocator.Get<SettingsService>().Get<ItemInfoCollection>();

        foreach (ItemInfo item in itemInfoCollection.Totems)
        {
            if (_gameDataService.HasItem(item.Id) == true)
            {
                TotemInfo totem = (TotemInfo)item;
                if (totem.TotemType == TotemType.Immortality)
                {
                    _immortalityInfo = totem;
                    CanBeResurrected = true;
                    return;
                }
            }
        }
    }

    public void Resurrect()
    {
        CanBeResurrected = false;
        _health.GetCure(_health.MaxHealth);

        GameDataService gameDataService = ServiceLocator.Get<GameDataService>();
        gameDataService.RemoveItem(_immortalityInfo.Id);

        _health.OnResurrected();
    }

}
