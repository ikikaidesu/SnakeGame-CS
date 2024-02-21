using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic
{
    class Pixel
    {
        public int X;
        public int Y;
        public ConsoleColor Color;
        public int Size;
        public char PixelChar = '█';

        public Pixel(int x, int y, ConsoleColor color, int size)
        {
            X = x;
            Y = y;
            Color = color;
            Size = size;
        }
        
        // этот метод отвечает за рисовку пикселя
        public void Draw()
        {
            Console.ForegroundColor = Color;
            // создаем два цикла для отрисовка размерности элемента
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    // передаваемая изначальная координата * множитель + 1 = следующая координата
                    Console.SetCursorPosition(X * Size + x, Y * Size + y);
                    Console.Write(PixelChar);
                }
            }
        }
        // этот метод отвечает за очистку элемента
        public void Clear()
        {
            Console.ForegroundColor = Color;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Console.SetCursorPosition(X*Size+x, Y*Size+y);
                    Console.Write(" ");
                }
            }
        }

    }
}
