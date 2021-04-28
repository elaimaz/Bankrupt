namespace Bankrupt
{
    class Property
    {
        public int Price;
        public int Rent;
        public IPlayerActions Owner;

        public Property(int _price, int _rent)
        {
            Price = _price;
            Rent = _rent;
            Owner = null;
        }

        public void Sold(IPlayerActions newOwner)
        {
            Owner = newOwner;
        }
    }
}
