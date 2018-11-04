using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2VirtualPet
{
    class Player
    {
        private int input;
        private int day = 1;
        public double coins = 100, priceScale = 1;
        public Pet pet;
        public DateTime time, start;

        bool running = false;

        Item currentItem;
        Store store;
        public List<Item> Inventory = new List<Item>();
        public List<Trick> Tricks = new List<Trick>();
        Random rand = new Random();

        public Player()
        {
            start = DateTime.Now;
           
            Console.Title = "Virtual Pet Game";
        }

        private void GetInput(int input)
        {
            try
            {
                this.input = Convert.ToInt32(Console.ReadKey(true).KeyChar.ToString()); ////referenced a stack overflow post for this method of conversion
            }
            catch
            {
                GetInput(input);
            }
        }

        public void Menu()
        {
            Console.WriteLine("\n\tWelcome to the Virtual Pet Game!\n\tBy: Haley Phillips");

            Console.WriteLine("\n\t1. Play");
            Console.WriteLine("\t2. Settings");
            Console.WriteLine("\t3. Exit");

            GetInput(input); //quick & easy get input method to turn readkeys into ints for choices

            switch (input)
            {
                case 1: Console.Clear();
                    Game();
                    break;
                case 2: Console.Clear();
                    Settings();
                    break;
                case 3: Environment.Exit(0);
                    break;
            }
        }

        private void HUD()
        {
            Line();
            Console.WriteLine($"\tDay: {day}\tCoins: {coins}");
            Line();
            Console.WriteLine($"\t~{pet.name}~");
            Console.WriteLine("\n\tLevel: {0}", pet.level);
            Console.WriteLine($"\tAge: {pet.age} months");
            Console.WriteLine($"\tColor: {pet.color}");
            Line();
        }

        private void GameMenu()
        {
             Console.WriteLine("\n\tWhat would you like to do today?");

            Console.WriteLine($"\n\t1. Play with {pet.name}.");
            Console.WriteLine("\t2. Visit the store.");
            Console.WriteLine($"\t3. Have {pet.name} do a trick!");
            Console.WriteLine($"\t4. Give {pet.name} a treat.");

            GetInput(input);

            switch (input)
            {
                case 1: 
                    Play();
                    break;
                case 2: Console.Clear();
                    store.OpenShop();
                    break;
                case 3: 
                    TrickMenu();
                    Console.Clear();
                    break;
                case 4: GiveTreat();
                    Console.Clear();
                    break;
            }
        }

        public void GiveTreat()
        {
            if(Inventory.Count <= 0)
            {
                Console.WriteLine("\n\tYour inventory is empty!");
                Wait(1500);
                Console.Clear();
                GameLoop();
            }

            Console.WriteLine("\n\tTreats!\n");
            List<Treat> Treats = new List<Treat>();

            int i = 1;
            foreach(Treat treat in Inventory)
            {
                Console.WriteLine($"\t{i}. {treat.name}");
                i++;
                Treats.Add(treat);
            }

            GetInput(input);

            Console.WriteLine($"\n\t{pet.name} {Treats[input - 1].action}");
            Wait(1500);
            Inventory.Remove(Treats[input - 1]);
        }

        private void Wait(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        private void Play()
        {
            if (Inventory.Count > 0)
            {
                DisplayInventory();
                GetInput(input);
            }
            else
            {
                Console.WriteLine("\n\tYou don't have any items to play with!");
                Wait(1500);
                Console.Clear();
                return; //exits the method
            }

            currentItem = Inventory[input - 1];

            if (!(currentItem is Toy)) //if the selected item is not a toy
            {
                Console.WriteLine($"\n\t{currentItem.name} is not a toy!");
                Wait(1000);
            }
            else if(pet.GetType() == typeof(Cat) && currentItem.name != "Mouse Toy")
                {
                    Console.WriteLine($"\n\t{currentItem.name} is not a Cat toy!");
                    Wait(1500);
                }

                else if (pet.GetType() == typeof(Dog) && currentItem.name != "Tennis Ball")
                {
                    Console.WriteLine($"\n\t{currentItem.name} is not a Dog toy!");
                    Wait(1500);
                }

               else if (pet.GetType() == typeof(Hamster) && currentItem.name != "Hamster Ball")
                {
                    Console.WriteLine($"\n\t{currentItem.name} is not a Hamster toy!");
                    Wait(1500);
                }
            else
            {
                Console.WriteLine($"\n\t{pet.name} {currentItem.action}");
                Wait(1000 * rand.Next(3, 5)); //takes 3-5 seconds to play
                pet.skill++;
            }

                
            


            if (pet.skill >= 3) //level up every 3 skill points
            {
                pet.LevelUp(currentItem as Toy);

                int val = rand.Next(10, 30);
                Console.WriteLine($"\n\tPlayer has earned {val} coins!");
                coins += val;

                pet.skill = 0;
                Console.ReadKey(true);
            }
            
            Console.Clear();
        }


        private void Game()
        {
            
            Console.WriteLine("\n\tWelcome to the pet store!");
            Console.WriteLine("\tWhat are you looking to adopt today?");

            Console.WriteLine("\n\t1. Cat");
            Console.WriteLine("\t2. Dog");
            Console.WriteLine("\t3. Hamster");

            GetInput(input);

            switch (input)
            {
                case 1:
                    Console.WriteLine($"\n\tWow! A {nameof(Cat)}?\n\tWhat will you name it?");
                    pet = new Cat(Console.ReadLine(), this);
                    break;
                case 2:
                    Console.WriteLine($"\n\tWow! A {nameof(Dog)}?\n\tWhat will you name it?");
                    pet = new Dog(Console.ReadLine(), this);
                    break;
                case 3:
                    Console.WriteLine($"\n\tWow! A {nameof(Hamster)}?\n\tWhat will you name it?");
                    pet = new Hamster(Console.ReadLine(), this);
                    break;
            }

            Console.WriteLine($"\n\tWhat color is {pet.name}?");
            pet.color = Console.ReadLine();

            Console.WriteLine($"\n\tWhat breed is {pet.name}?");
            pet.breed = Console.ReadLine();

            Console.WriteLine($"\n\tHow many months old is {pet.name}?");
            pet.age = Convert.ToInt32(Console.ReadLine());

            store = new Store(this, pet.allTricks, priceScale); //passes itself to store

            Console.WriteLine("\n\tGreat choice! Let's get started!");
            Console.WriteLine("\n\t> Press any key to continue.");
            Console.ReadKey(true);
            Console.Clear();

            GameLoop();
        }
        

        public void GameLoop()
        {
            running = true;

            while (running == true)
            {
                time = DateTime.Now;

                if (time.Minute - start.Minute > 2) //new day every 2 minutes
                {
                    NewDay();
                    start = DateTime.Now;
                }

                HUD();
                GameMenu();
            }

            Console.ReadLine();
        }

        private void Settings()
        {
            Console.WriteLine("\n\tSelect a difficulty: ");

            Console.WriteLine("\n\t1. Easy: start with 100 coins, items are least expensive");
            Console.WriteLine("\t2. Medium: start with 75 coins, items are more expensive");
            Console.WriteLine("\t3. Hard: start with 50 coins, items are most expensive");

            GetInput(input);

            switch (input)
            {
                case 1: priceScale = 1; coins = 100;
                    break;
                case 2:  priceScale = 1.5; coins = 75;
                    break;
                case 3: priceScale = 2; coins = 50;
                    break;
            }

            Console.Clear();
            Menu();
        }

        private void NewDay()
        {
            store.Restock();
            day++;
        }

        private void Line()
        {
            for (int x = 0; x < 40; x++)
                Console.Write("=-=");
        }

        public void DisplayInventory()
        {
            Console.WriteLine("\n\t-INVENTORY-");

            int i = 1;
            foreach(Item item in Inventory)
            {
                Console.WriteLine($"\t{i}. {item.name}");
                i++;
            }
        }

        public void TrickMenu()
        {
            //pet.CreateTricks();

            if (Tricks.Count > 0)
            {
                Console.WriteLine("\n\tTRICKS");

                int i = 1;
                foreach (Trick trick in Tricks) //each learned trick is in Tricks list
                {
                    Console.WriteLine($"\t{i}. {trick.name}");
                    i++;
                }

                GetInput(input);

                Console.WriteLine($"\n\t{pet.name} {Tricks[input - 1].description}");
                Wait(1500);
            }
            else
            {
                Console.WriteLine("\n\tNo tricks learned yet!");
                Wait(1500);
            }

            
        }

       

    }
}
