using System;
using System.Threading;
using System.Diagnostics;

namespace Main
{
    public class Program
    {
        // размер окна
        // ширина
        public const int WindowWidth = 30;
        // высота
        public const int WindowHeight = 20;
        // множитель размера
        public const int WindowMultiplier = 3;
        // цвет для барьера
        public const ConsoleColor Color = ConsoleColor.DarkGray;
        // цвет головы
        public const ConsoleColor SnakeColor = ConsoleColor.Magenta;
        // цвет тела
        public const ConsoleColor BodyColor = ConsoleColor.DarkMagenta;
        // цвет еды
        public const ConsoleColor FoodColor = ConsoleColor.Red;
        // счетчик сьеденных яблочек
        public static int Counter = 0;
        // задерка между кадрами(передвижением змейки)
        public static int FrameMS = 200;
        public static SnakeGame.Movement CurrentMovement = SnakeGame.Movement.Right;
        static void Main(string[] args)
        {
            // устанавливаем размер окна
            Console.SetWindowSize(WindowWidth * WindowMultiplier, WindowHeight * WindowMultiplier);
            // устанавливаем размер буффера
            Console.SetBufferSize(WindowWidth * WindowMultiplier, WindowHeight * WindowMultiplier);
            Console.CursorVisible = false;
            StartGame();
            Console.ReadKey();
        }
        // метод отрисовки барьера
        // почему-то работает коряво(думаю из-за настроек консоли)
        // поэтому если у вас коряво, то просто поиграйтесь с значениями которые я укажу
        public static void DrawBarrier()
        {
            // создаем горизонтальные барьеры
            for (int i = 0; i < WindowWidth; i++)
            {
                new Graphic.Pixel(i, 1, Color, WindowMultiplier).Draw();
                new Graphic.Pixel(i, WindowHeight-1, Color, WindowMultiplier).Draw();
            }
            // создаем вертикальные барьеры
            for (int i = 1; i < WindowHeight-1; i++)
            {
                new Graphic.Pixel(0, i, Color, WindowMultiplier).Draw();
                // вот с этой -1 попробуйте поиграться и настроить под себя если что-то не так
                new Graphic.Pixel(WindowWidth-1, i, Color, WindowMultiplier).Draw();
            }
        }
        public static void ShowCount(int counter)
        {
            string CounterString = $"Ваш счет: {Counter}"; 
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(WindowWidth * WindowMultiplier / 2 - CounterString.Length / 2, 1);
            Console.Write(CounterString);
        }
        
        // функция принятия направления
        public static SnakeGame.Movement TakeDirection(SnakeGame.Movement movement)
        {
            // если ничего не нажато
            if (!Console.KeyAvailable)
                return movement;
            else
            {
                SnakeGame.Movement NextMovement = movement;
                // берем нажатую клавишу
                ConsoleKey key = Console.ReadKey(true).Key;
                // игрок не может повернуть назад если он двигается вперед поэтому мы делаем на это проверку
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            if (movement != SnakeGame.Movement.Down) NextMovement = SnakeGame.Movement.Up;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (movement != SnakeGame.Movement.Up) NextMovement = SnakeGame.Movement.Down;
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (movement != SnakeGame.Movement.Right) NextMovement = SnakeGame.Movement.Left;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (movement != SnakeGame.Movement.Left) NextMovement = SnakeGame.Movement.Right;
                            break;
                        }
                }
                return NextMovement;
            }
        }
        public static void StartGame()
        {
            Console.Clear();
            // создание барьера
            DrawBarrier();
            // вывод надписи счета по центру сверху
            ShowCount(Counter);
            // вызываем функцию запуска игры
            // создание змейки
            // в конструкторе змейки уже вызван метод draw поэтому мы просто создаем экземпляр
            var Snake = new Snake.Snake(WindowHeight / 2, 5, SnakeColor, BodyColor, WindowMultiplier);
            // создаем экземпляр еды который сразу же отрисовывается
            var Food = new SnakeGame.Food(WindowWidth, WindowHeight, FoodColor, WindowMultiplier, Snake);
            // создаем таймер
            Stopwatch watcher = new Stopwatch();
            // устанавливаем изначачальное направление так как при рестарте почему-то без этой строки змейка шла вверх
            CurrentMovement = SnakeGame.Movement.Right;
            while (true)
            {
                // тут мы берем движение игрока
                // запускаем таймер
                watcher.Restart();
                // это строка не дает змейке за один фрейм два раза переместиться куда-то чтобы змейка не вошла в себя
                SnakeGame.Movement OldMovement = CurrentMovement;
                // пока таймер меньше чем надо для кадра мы берем движение
                // если оно превысит то просто продвигаем змею в старом направлении
                while (watcher.ElapsedMilliseconds < FrameMS)
                {
                    if (CurrentMovement == OldMovement) CurrentMovement = TakeDirection(CurrentMovement);
                }
                // если координаты головы равны координатам яблока
                if (Snake.Head.X == Food.x && Snake.Head.Y == Food.y)
                {
                    // повышаем счетчик
                    ++Counter;
                    ShowCount(Counter);
                    // создаем новую еду
                    Food = new SnakeGame.Food(WindowWidth, WindowHeight, FoodColor, WindowMultiplier, Snake);
                    Snake.Move(CurrentMovement, true);
                }
                else Snake.Move(CurrentMovement);
                // отдел с поражением
                // проверяем зашла ли змейка на координаты барьера
                // у координат y такая не разбериха с проверкой из-за моей консоли(почему-то она оставляет всегда снизу одну пустую строку) + добавления строки с счетом
                foreach (var i in Snake.Body)
                {
                    if (Snake.Head.X == i.X && Snake.Head.Y == i.Y)
                    {
                        // если зашла то очищаем ее
                        Snake.Clear();
                        // вызываем функцию с выводом текста
                        EndGame();
                        break;
                    }
                }
                if (Snake.Head.X == 0 || Snake.Head.X == WindowWidth - 1 || Snake.Head.Y == 1 || Snake.Head.Y == WindowHeight-2)
                {
                    // если зашла то очищаем ее
                    Snake.Clear();
                    // вызываем функцию с выводом текста
                    EndGame();
                }
                if (Counter == WindowWidth*WindowHeight - (WindowWidth-2)*(WindowHeight-2))
                {
                    Snake.Clear();
                    WinGame();
                }
            }
        }
        // вывод текста при поражении с принятием клавиш
        public static void EndGame()
        {
            // надпись при конце игры
            string[] EndGameString = { "Вы проиграли :(", $"Ваш счет: {Counter}", "Начать заново(Y/N)?" };
            // строка для очистки места для надписи
            string Clearing = "                   ";
            // выводим текст при поражении
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(WindowWidth * WindowMultiplier / 2 - EndGameString[0].Length / 2, WindowHeight * WindowMultiplier / 2 - EndGameString[0].Length / 2 + i);
                Console.Write(Clearing);
                Console.SetCursorPosition(WindowWidth * WindowMultiplier / 2 - EndGameString[0].Length / 2, WindowHeight * WindowMultiplier / 2 - EndGameString[0].Length / 2 + i);
                Console.Write(EndGameString[i]);
            }
            // обнуляем счетчик
            Counter = 0;
            // запрашиваем нажатие
            while (true)
            {
                // берем у него нажатие
                ConsoleKey key = Console.ReadKey(true).Key;
                // если Y запускаем заново игру
                if (key == ConsoleKey.Y) StartGame();
                // если N закрываем окно
                else if (key == ConsoleKey.N) Process.GetCurrentProcess().Kill();
            }
        }
        public static void WinGame()
        {
            // надпись при конце игры
            string[] EndGameString = { "Вы Выйграли :)", $"Ваш счет: {Counter}", "Начать заново(Y/N)?" };
            // строка для очистки места для надписи
            string Clearing = "                   ";
            // выводим текст при поражении
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(WindowWidth * WindowMultiplier / 2 - EndGameString[0].Length / 2, WindowHeight * WindowMultiplier / 2 - EndGameString[0].Length / 2 + i);
                Console.Write(Clearing);
                Console.SetCursorPosition(WindowWidth * WindowMultiplier / 2 - EndGameString[0].Length / 2, WindowHeight * WindowMultiplier / 2 - EndGameString[0].Length / 2 + i);
                Console.Write(EndGameString[i]);
            }
            // обнуляем счетчик
            Counter = 0;
            // запрашиваем нажатие
            while (true)
            {
                // берем у него нажатие
                ConsoleKey key = Console.ReadKey(true).Key;
                // если Y запускаем заново игру
                if (key == ConsoleKey.Y) StartGame();
                // если N закрываем окно
                else if (key == ConsoleKey.N) Process.GetCurrentProcess().Kill();
            }
        }
    }
}
