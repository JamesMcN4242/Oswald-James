public static class BotDecisionMaker
{
    public enum Action
    {
        NONE,
        MOVE,
        ATTACK
    }

    public static Action CalculateAction()
    {
        //TODO: Create curves to evalute the best utility for the bot to take
        return Action.NONE;
    }
}
