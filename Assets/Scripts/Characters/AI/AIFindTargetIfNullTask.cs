public class AIFindTargetIfNullTask : AITask
{
    public AIFindTargetIfNullTask(AI ai) : base(ai)
    {
    }

    public override void OnTick()
    {
        Character player = ServiceLocator.Get<CharacterCollection>().Player;

        if (player.IsAlive == true && _ai.Target == null)
        {
            _ai.SetTarget(player);
        }
    }
}
