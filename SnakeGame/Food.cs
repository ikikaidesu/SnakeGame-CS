using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Food
    {
        public int WindowWidth;
        public int WindowHeight;
        public ConsoleColor Color;
        public int Size;
        public char PixelChar = '█';
        public Random rnd = new Random();
        public int x;
        public int y;
        public Graphic.Pixel food;

        // создаем конструктор где берем цвет и размер так как координаты будут браться рандомно
        public Food(int windowwidth, int windowheight, ConsoleColor color, int size, Snake.Snake snake)
        {
            WindowWidth = windowwidth;
            WindowHeight = windowheight;
            Color = color;
            Size = size;
            // создаем рандомные координаты в пределах карты
            do
            {
                x = rnd.Next(Size + 1, WindowWidth - 3);
                y = rnd.Next(Size + 1, WindowHeight - 2);
            }
            while ((snake.Body.Any(ch => ch.X == x && ch.Y == y)) || (snake.Head.X == x && snake.Head.Y == y));
            // создаем экземпляр еды
            Graphic.Pixel food = new Graphic.Pixel(x,y,Color,Size);
            // сразу же отрисовывываем его
            food.Draw();
        }
    }
}
