using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public static ImmutableTypes.None None => ImmutableTypes.None.Default;
        public static ImmutableTypes.Some<T> Some<T>(T value) => new ImmutableTypes.Some<T>(value);
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
    public static class Functional
    {
        public static R Connect<R>(string connStr, Func<IDbConnection, R> func)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                return func(conn);
            }
        }



    }


}

namespace Enterprise.Framework.Topics.ImmutableTypes
{
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
    public struct None { internal static readonly None Default = new None(); }
    public struct Some<T> { internal T Value { get; } internal Some(T value) { if (value == null) throw new ArgumentNullException(); Value = value; } }
    public struct Option<T>
    {
        readonly bool isSome;
        readonly T value;

        private Option(T value) { this.isSome = true; this.value = value; }
        public static implicit operator Option<T>(None _) => new Option<T>();
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);
        public static implicit operator Option<T>(T value) => value == null ? new Option<T>(): new Option<T>(value);//None : Some(value);
        public R Match<R>(Func<R> None, Func<T, R> Some) => isSome ? Some(value) : None();
    }
}
