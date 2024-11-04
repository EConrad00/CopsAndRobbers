namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Test");
            //List<People> peoples = new List<People>();
            //peoples.Add (new Citizen("Oscar", 1, 1 , 1 ,1));
            //peoples.Add(new Citizen("Emil", 1, 1, 1, 1));
            //peoples.Add(new Citizen("Johan", 1, 1, 1, 1));
            //peoples.Add(new Citizen("Emil 2", 1, 1, 1, 1));

            //foreach (People people in peoples)
            //{
            //    Console.WriteLine(people.Name + " ");
            //    foreach (Goods goods in people.Goods)
            //    {
            //        Console.Write(goods.ItemName + " ");
            //    }
            //}
            //Console.ReadLine();
            Console.CursorVisible = false;
            City city = new City(12, 12, 12, 20, 80, 0, 0);
            Prison prison = new Prison(20, 10, 0, 22);
            city.DisplayLocation();
            prison.DisplayLocation();
            
            while (true)
            {
                foreach (People people in city.Peoples)
                {

                    people.Move(city);
                    city.UpdateCityGrid(people);
                    city.DisplayPeople(people);
                }
                //bool test = (city.CityGrid.TryGetValue((1, 1), out List<int> index));
                //Console.WriteLine(test);
                //for (int i = 0; i < City.Peoples.Count; i++)
                //{
                //    Helpers.Movement2(i, City);
                //    City.DisplayPeople(i);

                //}

                Thread.Sleep(200);
            }
        }
    }
}
