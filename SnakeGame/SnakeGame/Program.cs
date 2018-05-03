using System;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        private static Game _game;
        private static bool _exit;
        private static bool _isDrawing;

        static void InitProgram()
        {
            _game = new Game(20, 20);
            Console.CursorVisible = false;
            Console.WindowWidth = 60;
            Console.WindowHeight = 40;
            Console.SetBufferSize(60, 40);
        }

        static public void Tick(Object stateInfo)
        {
            if (_isDrawing)
            {
                return;
            }

            _isDrawing = true;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo cki = Console.ReadKey(false);
                _exit = _game.ControllCallback(cki.Key);
            }

            Console.Clear();
            _game.Tick();
            _game.Render();
            DrawScreen();

            _isDrawing = false;
        }

        static void Main()
        {
            InitProgram();
            TimerCallback callback = Tick;
            new Timer(callback, null, 0, 100);

            // loop here forever
            while (!_exit)
            {
            }
        }

        static void DrawScreen()
        {
            Console.SetCursorPosition(27, 1);
            Console.Write("SNAKE");

            Console.SetCursorPosition(10, 4);
            for (int i = 40; i > 0; --i)
            {
                Console.Write("═");
            }

            Console.SetCursorPosition(9, 4);
            Console.WriteLine("╔");
            Console.SetCursorPosition(50, 4);
            Console.WriteLine("╗");

            Console.SetCursorPosition(10, 26);
            for (int i = 40; i > 0; --i)
            {
                Console.Write("═");
            }

            Console.SetCursorPosition(9, 26);
            Console.WriteLine("╚");
            Console.SetCursorPosition(50, 26);
            Console.WriteLine("╝");
        }
    }
}
