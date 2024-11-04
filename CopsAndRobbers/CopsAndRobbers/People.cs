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
        public List<Goods> Inventory { get; set; }

        public People(string name, int id, int posX, int posY, int dirX, int dirY)
        {
            Name = name;
            Id = id;
            PosX = posX;
            PosY = posY; 
            DirX = dirX; 
            DirY = dirY; 
        }
        public void Move(Location location)
        {
            Random rnd = new Random();
            if (PosX + DirX == 0 || PosX + DirX == location.Width ||
                PosY + DirY == 0 || PosY + DirY == location.Height ||
                (DirX == 0 && DirY == 0))
            {
                DirX = rnd.Next(-1, 2);
                DirY = rnd.Next(-1, 2);
            }
            else
            {
                Console.SetCursorPosition(PosX, PosY);
                Console.Write(" ");
                location.CityGrid[(PosX, PosY)].Remove(this.Id);
                PosY += DirY;
                PosX += DirX;
                
                
            }
        }
    }

    class Citizen : People
    {
        public Citizen(string name, int id, int posX, int posY, int dirX, int dirY) : base(name, id, posX, posY, dirX, dirY)   
        {
            Inventory = new List<Goods>();
            CreateGoods(Inventory);
        }

        private void CreateGoods(List<Goods> goods)
        {
            goods.Add(new Belongings("keys"));
            goods.Add(new Belongings("phone"));
            goods.Add(new Belongings("dosh"));
            goods.Add(new Belongings("watch"));
        }
    }
    class Robber : People
    {
        public Robber(string name,int id, int posX, int posY, int dirX, int dirY) : base(name, id, posX, posY, dirX, dirY)
        {

        }
    }

    class Cop : People
    {
        public Cop(string name, int id, int posX, int posY, int dirX, int dirY) : base(name, id, posX, posY, dirX, dirY)
        {

        }
    }

}
