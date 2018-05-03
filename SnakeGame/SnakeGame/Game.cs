using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    public class Game
    {
        private readonly int _fieldX;
        private readonly int _fieldY;
        private readonly Snake _snake;
        private Vector2D _food;
        private int _foodAnimationCounter = 0;
        private Random _rnd = new Random();

        public Game(int fieldX, int fieldY)
        {
            if (fieldX < 5 || fieldY < 5)
            {
                throw new Exception("Minimum field size is 5x5");
            }

            _fieldX = fieldX;
            _fieldY = fieldY;

            _snake = new Snake(3, fieldX, fieldY);
        }

        private void NewFood()
        {
            List<Vector2D> tail = _snake.Tail;
            while (true)
            {
                Vector2D newFood = new Vector2D
                {
                    X = _rnd.Next(_fieldX - 1),
                    Y = _rnd.Next(_fieldY - 1)
                };
                if (tail.Contains(newFood))
                {
                    continue;
                }
                _food.X = newFood.X;
                _food.Y = newFood.Y;
                break;
            }
        }

        public void Tick()
        {
            _snake.Move();
            if (_snake.CheckFood(_food))
            {
                NewFood();
            }
        }

        public void Render()
        {
            List<Vector2D> tail = _snake.Tail;

            foreach (Vector2D c in tail)
            {
                Console.SetCursorPosition(c.X * 2 + 10, c.Y + 5);
                Console.Write("x");
            }

            _foodAnimationCounter++;
            if (_foodAnimationCounter > 4)
            {
                _foodAnimationCounter = 0;
            }
            else if (_foodAnimationCounter > 1 && tail.Count < _fieldX * _fieldY)
            {
                Console.SetCursorPosition(_food.X * 2 + 10, _food.Y + 5);
                Console.Write("o");
            }
        }

        public bool ControllCallback(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    _snake.ChangeDir(0, 1);
                    break;
                case ConsoleKey.UpArrow:
                    _snake.ChangeDir(0, -1);
                    break;
                case ConsoleKey.RightArrow:
                    _snake.ChangeDir(1, 0);
                    break;
                case ConsoleKey.LeftArrow:
                    _snake.ChangeDir(-1, 0);
                    break;
                default:
                    return true;
            }

            return false;
        }
    }

    public class Snake
    {
        private readonly int _fieldX;
        private readonly int _fieldY;
        private Vector2D _direction;
        private readonly List<Vector2D> _tail = new List<Vector2D>(); // Массив хвоста

        public Snake(int length, int fieldX, int fieldY) // Конструктор
        {
            _fieldX = fieldX;
            _fieldY = fieldY;
            _direction.X = 1;
            _direction.Y = 0;

            if (length >= fieldX)
            {
                throw new Exception("Snake length is greater than Field X");
            }

            for (int i = 0; i < length; i++)
            {
                _tail.Add(new Vector2D { X = i, Y = 0 });
            }
        }

        public List<Vector2D> Tail // Возворащает массив хвоста
        {
            get { return _tail; }
        }

        public void ChangeDir(int x, int y) // Меняет вектор движения, нет движения по диагонали
        {
            if (-1 <= x && x <= 1 && -1 <= y && y <= 1 && Math.Abs(x) != Math.Abs(y))
            {
                _direction.X = x;
                _direction.Y = y;
            }
            else
            {
                throw new Exception("Invalid direction");
            }
        }

        public void Move() // Двигает змею
        {
            AddTail();
            _tail.RemoveAt(_tail.Count - 1); // Удаляем последний блок в хвосту
        }

        public void AddTail() // Добавляет один блок впереди змеи
        {
            Vector2D head = _tail.First();
            int newX = head.X += _direction.X;
            int newY = head.Y += _direction.Y;

            if (newX >= _fieldX)
            {
                newX = 0;
            }
            else if (newX < 0)
            {
                newX = _fieldX - 1;
            }

            if (newY < 0)
            {
                newY = _fieldY - 1;
            }
            if (newY >= _fieldY)
            {
                newY = 0;
            }

            Vector2D newHead = new Vector2D { X = newX, Y = newY };
            _tail.Insert(0, newHead);

            // Проверка на пересечение с хвостом
            int matchResult = _tail.FindLastIndex(o => o.X == newHead.X && o.Y == newHead.Y);
            if (matchResult > 0)
            {
                // Если есть пересечение, то отсекаем хвост с места пересечения
                _tail.RemoveRange(matchResult, _tail.Count - matchResult - 1);
            }
        }

        public bool CheckFood(Vector2D food) // Проверка на пересечение с едой
        {
            if (_tail.Contains(food))
            {
                AddTail();
                return true;
            }
            return false;
        }
    }

    public struct Vector2D
    {
        public int X;
        public int Y;
    }
}
