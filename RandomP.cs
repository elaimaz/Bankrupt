using System;
using System.Collections.Generic;

namespace Bankrupt
{
    class RandomP : Player, IPlayerActions
    {
        public List<Property> propertiesOwned = new List<Property>();

        public RandomP() : base()
        {
            TypeP = TypeOfPlayer.Aleatorio;
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
            Random rnd = new Random();
            if (property.Owner != null)
            {
                PayRent(property);
            }
            else if (rnd.Next(0, 2) != 0 && property.Price <= Coins)
            {
                property.Sold(this);
                SetCoins(-property.Price);
                propertiesOwned.Add(property);
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
