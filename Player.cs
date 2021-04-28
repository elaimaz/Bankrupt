namespace Bankrupt
{
    public enum TypeOfPlayer
    {
        Impulsivo,
        Exigente,
        Cauteloso,
        Aleatorio,
        None
    }

    public class Player
    {
        public int Coins;
        public int Position;
        public TypeOfPlayer TypeP;
        public bool bGameOver;
        public Player()
        {
            Coins = 300;
            Position = -1;
            bGameOver = false;
        }
    }
}
