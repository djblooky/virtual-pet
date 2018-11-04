using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2VirtualPet
{
    class Pet
    {
        public Player player;
        private int input;
        public string name, color, breed;
        public int age;
        public int level = 1;
        public int skill = 0;
        private DateTime bday; //bday is randomized date, occurs monthly

        public List<Trick> allTricks = new List<Trick>();
        Random rand = new Random();
        
       
        public Pet(string name, Player player)
        {
            this.name = name;
            this.player = player;
            CreateTricks();
        }

        private void GetInput(int input)
        {
            this.input = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()); ////referenced a stack overflow post for this method of conversion
        }

       
        
        public virtual void CreateTricks() //creates and adds all generic tricks to tricks list
        {
            Trick jump = new Trick("Jump", "leaps into the air!");
            Trick sleep = new Trick("Sleep", "gets tired and falls asleep!");

            allTricks.Add(jump);
            allTricks.Add(sleep);
        }

        public void LevelUp(Toy toy)
        {
            level++;
            Console.WriteLine($"\n\t{name} has leveled up to level {level}!");
            
            if (level % 2 == 0) //every 2 levels learn a trick
            {
                LearnTrick(toy);
            }

            System.Threading.Thread.Sleep(2000);
        }

        public virtual void LearnTrick(Toy toy)
        {
            
            Trick currentTrick = toy.canLearn[rand.Next(0, toy.canLearn.Length)]; //randomly selects a trick that the toy can be used to learn

            Console.WriteLine($"\n\t{name} has learned a new trick: {currentTrick.name}");
            player.Tricks.Add(currentTrick);
        }

    }

    class Dog : Pet
    {
        public List<Trick> dogTricks = new List<Trick>();

        Random rand = new Random();

        public Dog(string name, Player player) : base(name, player)
        {
            this.name = name;
            this.player = player;
        }

        public override void CreateTricks() //creates and adds all dog tricks to list of dog tricks and all tricks
        {
            base.CreateTricks(); //calls in and adds generic tricks

            Trick fetch = new Trick("Fetch", "run after the ball and bring it back!");

            //create and add all dog tricks
            dogTricks.Add(fetch); 

            foreach (Trick trick in dogTricks) //adds all the dog tricks into list of total tricks
            {
                allTricks.Add(trick);
            }
        }

    }

    class Cat : Pet
    {

        public List<Trick> catTricks = new List<Trick>();
        Random rand = new Random();

        public Cat(string name, Player player) : base(name, player)
        {
            this.name = name;
            this.player = player;
        }

        public override void CreateTricks() //creates and adds all cat tricks to list of cat tricks and all tricks
        {
            base.CreateTricks(); //calls in and adds generic tricks

            Trick pounce = new Trick("Pounce", "coils up and then... springs forward!");

            catTricks.Add(pounce); //create all cat tricks

            foreach (Trick trick in catTricks)
            {
                allTricks.Add(trick);
            }
        }
    
    }

    class Hamster : Pet
    {
        public List<Trick> hamTricks = new List<Trick>();

        Random rand = new Random();

        public Hamster(string name, Player player) : base(name, player)
        {
            this.name = name;
            this.player = player;
        }

        public override void CreateTricks() //creates and adds all hamster tricks to list of hamster tricks and all tricks
        {
            base.CreateTricks(); //calls in and adds generic tricks

            //create all hamster tricks
            Trick roll = new Trick("Roll", "rolls around very skillfully.");
            
            
            foreach (Trick trick in hamTricks)
            {
                allTricks.Add(trick);
            }
        }

    }

    class Trick
    {
        public string name;
        public string description;

        public Trick(string name)
        {
            this.name = name;
        }

        public Trick(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
