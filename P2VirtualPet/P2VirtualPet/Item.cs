using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2VirtualPet
{
    class Item
    {
        public double price, priceScale;
        public string name, action;

        public Item(string name)
        {
            this.name = name;
        }

        public Item(string name, string action, double price, double priceScale)
        {
            this.name = name;
            this.action = action;
            this.priceScale = priceScale;
            this.price = price * this.priceScale; //adjusts price of items based on game difficulty
        }

    }

    class Toy : Item
    {
         public Trick[] canLearn; //this array will hold all of this tricks a toy can be used to learn

        public Toy(string name, Trick[] canLearn, string action, double price, double priceScale) : base(name, action, price, priceScale)
        {
            this.canLearn = canLearn;
        }


    }

    class Treat : Item
    {
       
        public Treat(string name, string action, double price, double priceScale) : base(name, action, price, priceScale)
        {
            this.name = name;
        }
    }
}
