namespace Bankrupt
{
    interface IPlayerActions
    {
        public void SetPosition(int diceValue, int boardSize);
        public int GetPosition();
        public void Action(Property property);
        public void SetCoins(int value);
        public int GetCoins();
        public void PayRent(Property property);
        public void PlayerGameOver();
        public bool GetGameOverStatus();
        public TypeOfPlayer GetTypeOfPlayer();
    }
}
