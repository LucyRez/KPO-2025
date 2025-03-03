using System;
using System.Threading;

namespace TaskVisitor
{

     interface IPetVisitor {
        void Visit(Cat cat);
        void Visit(Dog dog);
        void Visit(Turtle turtle);
    }



    class WetVisitor : IPetVisitor
    {
        
        public void Visit(Cat cat)
        {
            Console.WriteLine("Checking up on cat...");
            Thread.Sleep(2000);
            Console.WriteLine("Checking the fur...");
            Thread.Sleep(2000);
            Console.WriteLine("Checking the whiskers...");
            Thread.Sleep(2000);
            Console.WriteLine(cat.HealthState == 0 ? "The cat is doing fine" : "The cat is sick :(");
        }

        public void Visit(Dog dog)
        {
           Console.WriteLine("Checking up on dog...");
            Thread.Sleep(2000);
            Console.WriteLine("The tail is wagging...");
            Thread.Sleep(2000);
            Console.WriteLine("Checking the nose...");
            Thread.Sleep(2000);
            Console.WriteLine(dog.HealthState == 0 ? "The dog is doing fine" : "The dog is sick :(");
        }

        public void Visit(Turtle turtle)
        {
            Console.WriteLine("Checking up on turtle...");
            Thread.Sleep(2000);
            Console.WriteLine("Checking the shell...");
            Thread.Sleep(2000);
            Console.WriteLine(turtle.HealthState == 0 ? "The turtle is doing fine" : "The turtle is sick :(");
        }
    }


    interface IPet { 
        void Accept(IPetVisitor petVisitor);

    }

    class Cat : IPet
    {
        private int healthState;

    
        public int HealthState => healthState;
        private static Random random = new Random();

        public Cat()
        {
            healthState = random.Next(2);
        }

        public void Accept(IPetVisitor petVisitor)
        {
            petVisitor.Visit(this);
        }
    }

     class Dog : IPet
    {
        private int healthState;
        public int HealthState => healthState;
        private static Random random = new Random();

        public Dog()
        {
            healthState = random.Next(2);
        }

        public void Accept(IPetVisitor petVisitor)
        {
            petVisitor.Visit(this);
        }

    }

    class Turtle : IPet
    {
        private int healthState;

        public int HealthState => healthState;
        private static Random random = new Random();

        public Turtle()
        {
            healthState = random.Next(2);
        }

        public void Accept(IPetVisitor petVisitor)
        {
            petVisitor.Visit(this);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            IPet[] pets = { new Cat(), new Dog(), new Turtle() };
            IPetVisitor petVisitor = new WetVisitor();

            try
            {
                foreach (var pet in pets)
                {
                    pet.Accept(petVisitor);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("The program was interrupted unexpectedly");
            }
        }
    }
}
