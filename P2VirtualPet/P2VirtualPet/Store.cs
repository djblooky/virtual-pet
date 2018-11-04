using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2VirtualPet
{
    class Store
    {
        private int input;
        private Item currentItem;
        private Player player;
        private double priceScale;

        //this is probably inefficient but... it works
        private static Trick jump = new Trick("jump");
        private static Trick fetch = new Trick("fetch");
        private static Trick sleep = new Trick("sleep");
        private static Trick roll = new Trick("roll");
        private static Trick pounce = new Trick("pounce");
        private static Trick[] temp = {jump, fetch, sleep, roll, pounce};

        List<Item> Stock = new List<Item>(); //all possible items that store could have in stock
        Random rand = new Random();

        List<Item> inStock = new List<Item>(); //current items in stock at store


        public Store(Player p, List<Trick> allTricks, double priceScale)
        {
            this.priceScale = priceScale; //keep at top

            //annoying solution i figured out after not being able to just call pet.jump and other tricks while creating items
            int i = 0;
            foreach (Trick trick in allTricks) //goes through and assigns tricks to temp variables
            {
                foreach (Trick t in temp)
                {
                    if (trick.name.ToLower() == t.name)
                    {
                        temp[i].name = trick.name;
                        temp[i].description = trick.description;
                    }
                }
                i++;
            }

            //all possible items declared
            Item tball = new Toy("Tennis Ball", new Trick[] { temp[1], temp[0]}, "excitedly runs after the tennis ball!", 10, priceScale); //pass in array of possible tricks toy can teach
            Item mouse = new Toy("Mouse Toy", new Trick[] { temp[0], temp[4] }, "starts chewing on the Mouse Toy.", 10, priceScale);
            Item hball = new Toy("Hamster Ball", new Trick[] { temp[2], temp[3]}, "begins rolling around the room in the Hamster Ball!", 10, priceScale);

            Item bone = new Treat("Dog Bone", "happily gnaws its teeth on the Dog Bone.", 20, priceScale);
            Item catnip = new Treat("Catnip", "rolls in the Catnip and begins running around crazily!", 20, priceScale);
            Item carrot = new Treat("Baby Carrot", "starts noshing appreciatively on the Baby Carrot.", 20, priceScale);

            Stock.Add(tball);
            Stock.Add(mouse);
            Stock.Add(hball);
            Stock.Add(bone);
            Stock.Add(catnip);
            Stock.Add(carrot);

            Restock();

           // Initialize(tball, mouse, hball, bone, catnip, carrot);
            
            player = p;

        }

     
        
        private void GetInput(int input)
        {
            try
            {
                this.input = Convert.ToInt32(Console.ReadKey(true).KeyChar.ToString());
            }
            catch
            {
                GetInput(input);
            }
        }

        public void OpenShop()
        {
            DisplayShop();

            bool tryAgain = true;

            while(tryAgain == true) //this will continue to allow item selection until something in range is picked
            {
                try
                {
                    GetInput(input);
                    currentItem = inStock[input - 1];
                    tryAgain = false;
                }
                catch(ArgumentOutOfRangeException)
                {
                    if(input == 0)
                    {
                        Console.Clear();
                        player.GameLoop();
                    }
                    else
                    {
                        Console.WriteLine("\n\tThere's no item there! Select something else.");
                        System.Threading.Thread.Sleep(1000);
                        Console.Clear();
                        DisplayShop();
                        tryAgain = true;
                    }

                }
            }
 
            Console.WriteLine($"\n\tWould you like to purchase {currentItem.name} for {currentItem.price} coins?");

            Console.WriteLine("\n\t1.yes");
            Console.WriteLine("\t2.no");

            GetInput(input);

            if (input == 1) //yes buy
            {
                Buy(currentItem);
                Console.Clear();
                OpenShop();
            }
            else if (input == 2) //no dont buy
            {
                Console.Clear();
                OpenShop();
            }

        }


        public void Restock() //generates random items from possible stock and places them in store
        {
            
            for (int i = 0; i < Stock.Count; i++)
            {
                inStock.Add(Stock[rand.Next(0, Stock.Count)]);
            }

          /*  foreach(Item item in Stock)
            {
                inStock.Add(Stock[rand.Next(0, Stock.Capacity)]);
            }*/
        }

        public void DisplayShop()
        {
            Line();
            Console.WriteLine($"\tStore\tCoins: {player.coins}");
            Line();

            Console.WriteLine("\n\t0. Exit the Store");

            int i = 1;
            foreach(Item item in inStock)
            {
                Console.WriteLine($"\t{i}. {item.name} --- {item.price} coins");
                i++;
            }

            if(inStock.Count <= 0)
            {
                Console.WriteLine("\n\t~SOLD OUT~");
            }
        }


        private void Buy(Item item)
        {
            if(item.GetType() ==  typeof(Toy)) //casting items as toys or treats when added to inventory
            {
                player.Inventory.Add(item as Toy);
            }
            else if(item.GetType() == typeof(Treat))
            {
                player.Inventory.Add(item as Treat);
            }
            else
            {
                player.Inventory.Add(item);
            }

            inStock.Remove(item);
            player.coins -= item.price;
        
        }

        private void Line()
        {
            for (int x = 0; x < 40; x++)
                Console.Write("=-=");
        }

    }
}
