using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = System.ValueTuple;

namespace Enterprise.Framework.Topics
{
    using static F;
    public static partial class F
    {
        public static Unit Unit() => default(Unit);
    }
    public static class ActionExt
    {
        public static Func<Unit> ToFunc(this Action action) => () => { action(); return Unit(); };
        public static Func<T, Unit> ToFunc<T>(this Action<T> action) => (t) => { action(t); return Unit(); };
    }
    public static class Instrumentation
    {
        public static void Time<T>(string op, Action act) => Time<Unit>(op, act.ToFunc());

        public static T Time<T>(string op, Func<T> f)
        {
            var sw = new Stopwatch();
            sw.Start();

            T t = f();

            sw.Stop();

            Console.WriteLine($"{op} took {sw.ElapsedMilliseconds} ");

            return t;
        }


    }
    public class Counter
    {
        private readonly string counterName;
        private int _currentValue;
        public Counter(string counterName)
        {
            this.counterName = counterName;
        }

        public void Increment() { _currentValue++; }
        public int Tally() { return _currentValue; }

        public override string ToString()
        {
            return $"{counterName} : {_currentValue}";
        }
    }
    public class Age
    {
        private int Value { get; }
        public Age(int value)
        {
            if (value <= 0 || value < 120)
                throw new ArgumentOutOfRangeException(nameof(value), "Value out of Range");

            this.Value = value;
        }
        public static bool operator <(Age l, Age r) => l.Value < r.Value;
        public static bool operator >(Age l, Age r) => l.Value > r.Value;
        public static bool operator <(Age l, int r) => l < new Age(r);
        public static bool operator >(Age l, int r) => l > new Age(r);
    }




}
