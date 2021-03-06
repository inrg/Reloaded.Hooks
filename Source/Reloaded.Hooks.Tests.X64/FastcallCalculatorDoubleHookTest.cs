﻿using System;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Tests.Shared;
using Xunit;

// Watch out!

namespace Reloaded.Hooks.Tests.X64
{
    public class FastcallCalculatorDoubleHookTest : IDisposable
    {
        private FastcallCalculator _calculator;
        private FastcallCalculator.AddFunction _addFunction;
        private FastcallCalculator.SubtractFunction _subtractFunction;
        private FastcallCalculator.DivideFunction _divideFunction;
        private FastcallCalculator.MultiplyFunction _multiplyFunction;

        private IHook<FastcallCalculator.AddFunction>        _addHook01;
        private IHook<FastcallCalculator.SubtractFunction>   _subHook01;
        private IHook<FastcallCalculator.DivideFunction>     _divideHook01;
        private IHook<FastcallCalculator.MultiplyFunction>   _multiplyHook01;

        private IHook<FastcallCalculator.AddFunction>        _addHook02;
        private IHook<FastcallCalculator.SubtractFunction>   _subHook02;
        private IHook<FastcallCalculator.DivideFunction>     _divideHook02;
        private IHook<FastcallCalculator.MultiplyFunction>   _multiplyHook02;

        public FastcallCalculatorDoubleHookTest()
        {
            _calculator = new FastcallCalculator();
            _addFunction = ReloadedHooks.Instance.CreateWrapper<FastcallCalculator.AddFunction>((long) _calculator.Add, out _);
            _subtractFunction = ReloadedHooks.Instance.CreateWrapper<FastcallCalculator.SubtractFunction>((long)_calculator.Subtract, out _);
            _divideFunction = ReloadedHooks.Instance.CreateWrapper<FastcallCalculator.DivideFunction>((long)_calculator.Divide, out _);
            _multiplyFunction = ReloadedHooks.Instance.CreateWrapper<FastcallCalculator.MultiplyFunction>((long)_calculator.Multiply, out _);
        }

        public void Dispose()
        {
            _calculator?.Dispose();
        }

        [Fact]
        public void TestHookAdd()
        {
            int Hookfunction01(int a, int b) { return _addHook01.OriginalFunction(a, b) + 1; }
            int Hookfunction02(int a, int b) { return _addHook02.OriginalFunction(a, b) + 1; }

            _addHook01 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.AddFunction>(Hookfunction01, (long)_calculator.Add).Activate();
            _addHook02 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.AddFunction>(Hookfunction02, (long)_calculator.Add).Activate();

            for (int x = 0; x < 100; x++)
            {
                for (int y = 1; y < 100;)
                {
                    int expected = ((x + y) + 1) + 1;
                    int result   = _addFunction(x, y);

                    Assert.Equal(expected, result);
                    y += 2;
                }
            }
        }

        [Fact]
        public void TestHookSub()
        {
            int Hookfunction01(int a, int b) { return _subHook01.OriginalFunction(a, b) - 1; }
            int Hookfunction02(int a, int b) { return _subHook02.OriginalFunction(a, b) - 1; }
            _subHook01 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.SubtractFunction>(Hookfunction01, (long)_calculator.Subtract).Activate();
            _subHook02 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.SubtractFunction>(Hookfunction02, (long)_calculator.Subtract).Activate();

            int x = 100;
            for (int y = 100; y >= 0; y--)
            {
                int expected = ((x - y) - 1) - 1;
                int result = _subtractFunction(x, y);

                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void TestHookMul()
        {
            int Hookfunction01(int a, int b) { return _multiplyHook01.OriginalFunction(a, b) * 2; }
            int Hookfunction02(int a, int b) { return _multiplyHook02.OriginalFunction(a, b) * 2; }
            _multiplyHook01 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.MultiplyFunction>(Hookfunction01, (long)_calculator.Multiply).Activate();
            _multiplyHook02 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.MultiplyFunction>(Hookfunction02, (long)_calculator.Multiply).Activate();

            int x = 100;
            for (int y = 0; y < 100; y++)
            {
                int expected = ((x * y) * 2) * 2;
                int result = _multiplyFunction(x, y);

                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void TestHookDiv()
        {
            int Hookfunction01(int a, int b) { return _divideHook01.OriginalFunction(a, b) * 2; }
            int Hookfunction02(int a, int b) { return _divideHook02.OriginalFunction(a, b) * 2; }
            _divideHook01 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.DivideFunction>(Hookfunction01, (long)_calculator.Divide).Activate();
            _divideHook02 = ReloadedHooks.Instance.CreateHook<FastcallCalculator.DivideFunction>(Hookfunction02, (long)_calculator.Divide).Activate();

            int x = 100;
            for (int y = 1; y < 100; y++)
            {
                int expected = ((x / y) * 2) * 2;
                int result = _divideFunction(x, y);

                Assert.Equal(expected, result);
            }
        }
    }
}
