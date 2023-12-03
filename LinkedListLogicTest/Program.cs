using System;

namespace LinkedListLogicTest
{
    class Cat
    {
        public string Color { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }

        public Cat(string color, bool isMale, string name = "Cat")
        {
            Color = color;
            Name = name;
            IsMale = isMale;
        }
        public Cat(string color, string name = "Cat")
        {
            Color = color;
            Name = name;
            Random gen = new Random();
            IsMale = Convert.ToBoolean(gen.Next(0, 2));
        }
    }
    class LinkedBList
    {
        private class Box
        {
            private Box? LastBox;
            private Box? NextBox;
            private Cat CatinBx;


            public void clear_box()
            {
                CatinBx = null;
                LastBox = null;
                NextBox = null;
            }

            public Box(Cat catinbx, Box lastBox, Box nextBox)
            {
                CatinBx = catinbx;
                LastBox = lastBox;
                NextBox = nextBox;
            }
            public Box(Cat catinbx, Box lastBox)
            {
                CatinBx = catinbx;
                LastBox = lastBox;
                NextBox = null;
            }
            public Box(Cat catinbx)
            {
                CatinBx = catinbx;
                LastBox = null;
                NextBox = null;
            }


            public ref  Box get_last()
            {
                if (LastBox == null) { throw new NullReferenceException("Object you are trying to get does not exist"); }
                else { return ref LastBox; }
            }
            public void set_last(Box box) { LastBox = box; }
            public ref Box get_next()
            {
                if (NextBox == null) { throw new NullReferenceException("Object you are trying to get does not exist"); }
                else { return ref NextBox; }
            }

            public ref Cat get_cat() { return ref CatinBx; }
            public void set_next(Box box) { NextBox = box; }

        }
        private Box? StartBox;
        private Box? EndBox;
        private int lenght;


        public LinkedBList(Cat startCat, Cat endCat)
        {
            StartBox = new Box(startCat);
            EndBox = new Box(endCat);
            StartBox.set_next(EndBox);
            EndBox.set_last(StartBox);
            lenght = 2;
        }
        public LinkedBList(Cat cat)
        {
            Box catinbox = new Box(cat);
            StartBox = catinbox;
            EndBox = catinbox;
            StartBox.set_next(EndBox);
            EndBox.set_last(StartBox);
            lenght = 2;
        }


        private  Box get_box(int boxid)
        {
            if (boxid < 0 || boxid >= lenght) { throw new ArgumentOutOfRangeException(null, "Element id is out of the list range: [0; " + Convert.ToString(lenght - 1) + "]"); }
            int mid = (int)Math.Floor((double)(lenght / 2));
            if (boxid == 0)
            {
                return StartBox;
            }
            if (boxid == lenght - 1)
            {
                return EndBox;
            }
            if (boxid <= mid)
            {
                Box box = StartBox;
                for (int i = 0; i < boxid; i++)
                {
                    box = box.get_next();
                }
                return box;
            }
            else
            {
                Box box = EndBox;
                for (int i = lenght - 1; i > lenght - boxid; i--)
                {
                    box = box.get_last();
                }
                return box;
            }
        }
        public Cat get_cat(int id)
        {
            return get_box(id).get_cat();
        }
        public int get_lenght() { return lenght; }


        public void add_cat(Cat cat, int catid)
        {
            if (catid < 0 || catid > lenght) { throw new ArgumentOutOfRangeException(null, "Element id is out of the list range: [0; " + Convert.ToString(lenght - 1) + "]"); }
            Box box = new Box(cat);
            if (lenght == 0)
            {
                StartBox = box;
                EndBox = box;
                StartBox.set_next(EndBox);
                EndBox.set_last(StartBox);
                lenght = 1;
                return;
            }
            if (catid == 0)
            {
                StartBox.set_last(box);
                box.set_next(StartBox);
                StartBox = box;
            }
            else if (catid == lenght)
            {
                EndBox.set_next(box);
                box.set_last(EndBox);
                EndBox = box;
            }
            else
            {
                Box prewbox = get_box(catid - 1);
                Box placebox = get_box(catid);
                box.set_last(prewbox);
                prewbox.set_next(box);
                box.set_next(placebox);
                placebox.set_last(box);
            }
            lenght++;
        }


        public Cat[] get_array()
        {
            if (lenght == 0) { return []; }
            Cat[] cat_arr = new Cat[lenght];
            ref Box box =ref StartBox;
            for (int i = 0; i < lenght ; ++i)
            {
                if (i == 0) 
                {
                    cat_arr[i] = box.get_cat();
                    continue;
                }
                if (box.get_cat() == null) { }
                box = box.get_next();
                cat_arr[i] = box.get_cat();
            }
            return cat_arr;
        }


        private void remove_box(int boxid)
        {
            if (boxid < 0 || boxid > lenght) { throw new ArgumentOutOfRangeException(null, "Element id is out of the list range: [0; " + Convert.ToString(lenght - 1) + "]"); }
            Box box = get_box(boxid);
            if (lenght == 1)
            {
                box.clear_box();
                lenght = 0;
                return;
            }
            if (boxid == 0)
            {
                StartBox.get_next().set_last(null);
                StartBox = StartBox.get_next();
            }
            else if (boxid == lenght - 1)
            {
                EndBox.get_last().set_next(null);
                EndBox = EndBox.get_last();
            }
            else
            {
                Box prewbox = box.get_next();
                Box nextbox = box.get_last();
                nextbox.set_last(prewbox);
                prewbox.set_next(nextbox);
            }
            box.clear_box();
            lenght--;

        }
        public void remove_cat(int catid)
        {
            remove_box(catid);
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Cat blackcat = new Cat("black");
            Cat whitecat = new Cat("white");
            Cat yellowcat = new Cat("yellow");
            LinkedBList myCats = new LinkedBList(blackcat, whitecat);
            myCats.add_cat(yellowcat, 0);
            Console.WriteLine(myCats.get_cat(0).Color);
            Console.WriteLine(myCats.get_cat(1).Color);
            Console.WriteLine(myCats.get_cat(2).Color);
            myCats.remove_cat(2);
            Console.WriteLine(myCats.get_cat(0).Color);
            Console.WriteLine(myCats.get_cat(1).Color);
            myCats.add_cat(whitecat, 0);
            myCats.remove_cat(1);
            Console.WriteLine(myCats.get_cat(0).Color);
            Console.WriteLine(myCats.get_cat(1).Color);
            myCats.remove_cat(0);
            myCats.remove_cat(0);
            myCats.add_cat(yellowcat, 0);
            myCats.add_cat(whitecat, 1);
            myCats.add_cat(blackcat, 1);
            Console.WriteLine(myCats.get_lenght());
            Console.WriteLine(myCats.get_cat(0).Color);
            Cat[] cat_arr = myCats.get_array();
            foreach(Cat cat in cat_arr) Console.Write(cat.Color + ", ");
            Console.ReadLine();
        }
    }
}
