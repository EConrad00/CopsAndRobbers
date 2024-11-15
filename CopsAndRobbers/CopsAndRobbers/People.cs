using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    internal class People
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int DirX { get; set; }
        public int DirY { get; set; }
        public virtual List<Goods> Inventory { get; set; }
        public int MaxX { get; set; }
        public int MinX { get; set; }
        public int MaxY { get; set; }
        public int MinY { get; set; }


        public People(string name, int id, Location location)
        {
            Name = name;
            Id = id;
            SetPosition(location);
            SetDirection(location);
        }
        public void Move(Location location)
        {
            Random rnd = new Random();
            if (PosX + DirX == 0 || PosX + DirX == location.Width ||
                PosY + DirY == 0 || PosY + DirY == location.Height ||
                (DirX == 0 && DirY == 0))
            {
                location.CityGrid[(this.PosX, this.PosY)].Remove(this.Id);
                SetDirection(location);
            }
            else
            {
                Console.SetCursorPosition(PosX, PosY);
                Console.Write(" ");
                location.CityGrid[(this.PosX, this.PosY)].Remove(this.Id);
                PosY += DirY;
                PosX += DirX;
            }
        }

        public virtual void Interaction(People people, Location location)
        {
            location.News.Add($"{this.Name} hälsar på {people.Name}.                                                     ");
        }
    }

    

    class Citizen : People
    {
        private List<Goods> Belongings;
        public override List<Goods> Inventory { get { return this.Belongings; } set => this.Belongings = value; }
        public Citizen(string name, int id, Location location) : base(name, id, location)   
        {
            Belongings = new List<Goods>();
            CreateGoods(Belongings);
        }

        private void CreateGoods(List<Goods> goods)
        {
            goods.Add(new ("keys", this.Id));
            goods.Add(new ("phone", this.Id));
            goods.Add(new ("dosh", this.Id));
            goods.Add(new ("watch", this.Id));
        }
        public override void Interaction(People people, Location location)
        {
            if (people is Robber || people is Cop)
            {
                people.Interaction(this, location);
            }
            else
            {
                base.Interaction(people, location);
            }
        }

        public override void Interaction(List<People> peoples)
        {

        }
    }
    class Robber : People
    {
        private List<Goods> Loot;

        public int PrisonTime { get; set; }
        public override List<Goods> Inventory { get => this.Loot; set => this.Loot = value; }
        public Robber(string name, int id, Location location) : base(name, id, location)
        {
            Loot = new List<Goods>();
        }
        public override void Interaction(People people, Location location)
        {
            if (people is Citizen && people.Inventory.Count() > 0)
            {
                StealFrom(people);
                Render.DisplayStatus((location as City));
                location.News.Add($"Tjuven {this.Name} stal {this.Inventory.Last().ItemName} från medborgaren {people.Name}.                ");
                Thread.Sleep(500);
            }
            else if (people is Cop && this.Inventory.Count() > 0)
            {
                people.Interaction(this, location);
            }
            else if (PrisonTime > 0)
            {

            }
            else
            {
                base.Interaction(people, location);
            }
            if (this == people && PrisonTime > 0)
            {
                PrisonTime -= 1;
                if (PrisonTime == 0)
                {
                    this.ReturnFromPrison(location);
                }   
            }
        }
        private void StealFrom(People people)
        {
            Random rnd = new Random();
            int atIndex = rnd.Next(0, people.Inventory.Count());
            Inventory.Add(people.Inventory[atIndex]);
            people.Inventory.RemoveAt(atIndex);
        }

        private void ReturnFromPrison(Location location)
        {
                Console.SetCursorPosition(this.PosX, this.PosY);
                Console.Write(" ");
                location.CityGrid[(this.PosX, this.PosY)].Remove(this.Id);
                this.SetPosition(location);
                Render.DisplayStatus((location as City));

                if (location.CityGrid.TryGetValue((this.PosX, this.PosY), out List<int> indexList))
                {
                    indexList.Add(this.Id);
                }
                else
                {
                    location.CityGrid.Add((this.PosX, this.PosY), new List<int> { this.Id });
                }
        }
        public override void Interaction(List<People> peoples) 
        {
        
        }
    }

    class Cop : People
    {
        private List<Goods> SeizedGoods;
        public override List<Goods> Inventory { get => this.SeizedGoods; set => this.SeizedGoods = value; }
        public Cop(string name, int id, Location location) : base(name, id, location)
        {
            SeizedGoods = new List<Goods>();
        }
        public override void Interaction(People people, Location location)
        {
            if (people is Robber && people.Inventory.Count() > 0)
            {
                //this.SeizedFrom(people);
                (people as Robber).PrisonTime = people.Inventory.Count() * 10;
                this.SeizedFrom(people);
                this.SendToPrison(location, people);
                //people.Interaction(this, location);
                location.News.Add($"Polisen {this.Name} beslagtog {this.Inventory.Count()} stöldgods från tjuven {people.Name}.                  ");
                Thread.Sleep(500);
            }
            else if (people is Citizen)
            {
                if (SeizedGoods.Find(p => p.OriginalOwnerId == people.Id) != null)
                {
                    List<Goods> tempGoods = new List<Goods>();
                    foreach (Goods goods in SeizedGoods)
                    {
                        if (goods.OriginalOwnerId == people.Id)
                        {
                            tempGoods.Add(goods);
                        }
                    }
                    foreach (Goods goods in tempGoods)
                    {
                        people.Inventory.Add(goods);
                        SeizedGoods.Remove(goods);
                    }
                    location.News.Add($"Polisen {this.Name} lämnade tillbaks {tempGoods.Count()} ägodelar till {people.Name}.                  ");
                    Thread.Sleep(500);
                }
                else
                {
                    base.Interaction(people, location);
                }
            }
            else
            {
                base.Interaction(people, location);
            }
        }
        private void SeizedFrom(People people)
        {
            for (int i = 0; i < people.Inventory.Count(); i++)
            {
                Inventory.Add(people.Inventory[i]);

            }
            people.Inventory.Clear();

        }
    }

}
