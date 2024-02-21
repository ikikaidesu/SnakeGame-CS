using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Snake
    {
        public int X;
        public int Y;
        public ConsoleColor HeadColor;
        public ConsoleColor BodyColor;
        public int Size;
        public char PixelChar = '█';
        public Queue<Graphic.Pixel> Body = new Queue<Graphic.Pixel>();
        public Graphic.Pixel Head;

        public Snake(int x, int y, ConsoleColor headcolor, ConsoleColor bodycolor, int size)
        {
            X = x;
            Y = y;
            HeadColor = headcolor;
            BodyColor = bodycolor;
            Size = size;
            // создаем голову змее
            Head = new Graphic.Pixel(x, y, HeadColor, Size);
            // создаем тело и добавляем его в очередь так как с ней легко работать для движения змейки
            // ты просто добавляешь в очередь новое значение а старое удаляешь и так оно идет за головой
            for (int i = 3; i > 0; i--)
            {
                Body.Enqueue(new Graphic.Pixel(Head.X-i, Head.Y, BodyColor, Size));
            }
            Draw();
        }
        
        // отрисовка змейки
        public void Draw()
        {
            // голова является экземляром Pixel поэтому просто вызываем метод Draw
            Head.Draw();
            // обходим очередь с телом и также вызываем
            foreach (var i in Body)
            {
                i.Draw();
            }
        }

        // очистка змейки
        // нужно при рестарте игры и движения
        public void Clear()
        {
            Head.Clear();
            foreach (var i in Body)
            {
                i.Clear();
            }
        }
        public void Move(SnakeGame.Movement Movement, bool food = false)
        {
            // очищаем змейку
            Clear();
            // добавляем в начало вместо головы кусочек тела
            Body.Enqueue(new Graphic.Pixel(Head.X, Head.Y, BodyColor, Size));
            // убираем с конца кусочек
            if (!food) Body.Dequeue();
            // отрисовываем голову с новым направлением
            switch (Movement)
            {
                case SnakeGame.Movement.Up: { Head = new Graphic.Pixel(Head.X, Head.Y - 1, HeadColor, Size); break; }
                case SnakeGame.Movement.Down: { Head = new Graphic.Pixel(Head.X, Head.Y + 1, HeadColor, Size); break; }
                case SnakeGame.Movement.Left: { Head = new Graphic.Pixel(Head.X - 1, Head.Y, HeadColor, Size); break; }
                case SnakeGame.Movement.Right: { Head = new Graphic.Pixel(Head.X + 1, Head.Y, HeadColor, Size); break; }
            }
            // вызываем метод отрисовку
            Draw();
        }
    }

}
