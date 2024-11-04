using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    internal class Location
    {
        public int StartPosX { get; set; }
        public int StartPosY { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<string> News { get; set; }
        public List<Robber> Prisoners { get; set; }
        public List<People> Peoples { get; set; }
        
        public Dictionary<(int, int), List<int>> CityGrid { get; set; }

        public Location(int height, int width, int startPosX, int startPosY)
        {
            Height = height;
            Width = width;
            StartPosX = startPosX;
            StartPosY = startPosY;
            CityGrid = new Dictionary<(int, int), List<int>>();
            Peoples = new List<People>();
            //DisplayLocation();
        }

        public void DisplayPeople(People person)
        {
            //for (int i = 0; i < Peoples.Count; i++)
            //{
                Console.SetCursorPosition(person.PosX, person.PosY);
                if (person is Citizen)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("C");
                }
                else if (person is Robber)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("R");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("P");
                }
                Console.ForegroundColor= ConsoleColor.White;
           // }
        }
       

        public void Interaction()
        {

        }

        

        public void DisplayLocation() // Draws city border
        {
            for (int col = StartPosY; col <= (StartPosY + Height); col++)
            {
                Console.SetCursorPosition(StartPosX, col);
                for (int row = StartPosX; row <= (StartPosX + Width); row++)
                {

                    if (col == StartPosY || col == (StartPosY + Height) || row == StartPosX || row == (StartPosX + Width))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }
       
    }
    class City: Location
    {
        public int AmmountOfCitizen { get; }
        public int AmmountOfTheifs { get; }
        public int AmmountOfCops { get; }
        public City(int ammountOfCitizen, int ammountOfTheifs, int ammountOfCops, int height, int width, int startPosX, int startPosY) : base(height, width, startPosX, startPosY)
        {
            CreatePeople(Peoples, ammountOfCitizen, ammountOfTheifs, ammountOfCops);
            InitCityGrid();
        }
        private void InitCityGrid()
        {
            for (int i = 0; i < Peoples.Count(); i++)
            {
                UpdateCityGrid(Peoples[i]);
            }
        }

        public void UpdateCityGrid(People people)
        {
            
            if (CityGrid.TryGetValue((people.PosX, people.PosY), out List<int> indexList))  //Bryta ut till egen metod... Fixat
            {
                for (int i = 0; i < indexList.Count(); i++)
                {
                    people.Interaction(Peoples[indexList[i]]); // Skapa interaction
                }
                indexList.Add(Peoples.IndexOf(people));
                
                //CityGrid.Add(people.PosX, people.PosY), 
               
            }
            else
            {
                CityGrid.Add((people.PosX, people.PosY), new List<int> { Peoples.IndexOf(people) });
            }
        }

        private void CreatePeople(List<People> peoples, int ammountOfCitizen, int ammountOfTheifs, int ammountOfCops)
        {
            Random rnd = new Random();

            for (int i = 0; i < ammountOfCitizen; i++)
            {
                peoples.Add(new Citizen($"Medborgare{i}", peoples.Count(), rnd.Next(1, Width), rnd.Next(1, Height), rnd.Next(-1, 2), rnd.Next(-1, 2)));
            }
            for (int i = 0; i < ammountOfTheifs; i++)
            {
                peoples.Add(new Robber($"Tjuv{i}", peoples.Count(), rnd.Next(1, Width), rnd.Next(1, Height), rnd.Next(-1, 2), rnd.Next(-1, 2)));
            }
            for (int i = 0; i < ammountOfCops; i++)
            {
                peoples.Add(new Cop($"Polis{i}", peoples.Count(), rnd.Next(1, Width), rnd.Next(1, Height), rnd.Next(-1, 2), rnd.Next(-1, 2)));
            }
        }

    }
    class Prison: Location
    {
        public Prison(int height, int width, int startPosX, int startPosY) : base (width, height, startPosX, startPosY)
        {
            
        }
    }
}
