using System.Collections.Generic;

namespace Bankrupt
{
    class Picky : Player, IPlayerActions
    {
        public List<Property> propertiesOwned = new List<Property>();

        public Picky() : base()
        {
            TypeP = TypeOfPlayer.Exigente;
        }

        public void SetPosition(int diceValue, int boardSize)
        {
            if (diceValue + Position >= boardSize)
            {
                SetCoins(100);
                Position = (diceValue + Position) % boardSize;
            }
            Position += diceValue;
        }

        public int GetPosition()
        {
            return Position;
        }

        public void Action(Property property)
        {
            if (property.Owner != null)
            {
                PayRent(property);
            } 
            else if (Coins >= property.Price && property.Rent > 50)
            {
                property.Sold(this);
                SetCoins(-property.Price);
            }
        }

        public void SetCoins(int value)
        {
            Coins += value;
        }

        public int GetCoins()
        {
            return Coins;
        }

        public void PayRent(Property property)
        {
            if (Coins > property.Rent)
            {
                property.Owner.SetCoins(property.Rent);
                SetCoins(-property.Rent);
            }
            else
            {
                PlayerGameOver();
            }
        }

        public void PlayerGameOver()
        {
            bGameOver = true;
            foreach (Property property in propertiesOwned)
            {
                property.Owner = null;
            }
        }

        public bool GetGameOverStatus()
        {
            return bGameOver;
        }

        public TypeOfPlayer GetTypeOfPlayer()
        {
            return TypeP;
        }
    }
}
