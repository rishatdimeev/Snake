using System;
using NUnit.Framework;

namespace SnakeGame
{
    [TestFixture]
    class Tests
    {
        [Test]
        public void GameSmallFieldSizeTest()
        {
            Assert.Catch(() => { new Game(4, 4); });
        }

        [Test]
        public void GameMinusFieldSizeTest()
        {
            Assert.Catch(() => { new Game(-1, 0); });
        }

        [Test]
        public void GameNormalFieldSizeTest()
        {
            Assert.DoesNotThrow(() => { new Game(15, 15); });
        }

        [Test]
        public void GameSmallestFieldSizeGameplayTest()
        {
            Game game = new Game(5, 5);
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    game.Tick();
                }
            });
        }

        [Test]
        public void GameCallbackExitTest()
        {
            Game game = new Game(10, 10);

            Assert.IsTrue(game.ControllCallback(ConsoleKey.Escape));
            Assert.IsTrue(game.ControllCallback(ConsoleKey.Enter));
        }

        [Test]
        public void GameCallbackArrowsTest()
        {
            Game game = new Game(10, 10);

            Assert.IsFalse(game.ControllCallback(ConsoleKey.DownArrow));
            Assert.IsFalse(game.ControllCallback(ConsoleKey.UpArrow));
            Assert.IsFalse(game.ControllCallback(ConsoleKey.LeftArrow));
            Assert.IsFalse(game.ControllCallback(ConsoleKey.RightArrow));
        }
    }
}
