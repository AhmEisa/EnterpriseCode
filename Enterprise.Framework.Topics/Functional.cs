using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
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

        public static ImmutableTypes.Option<int> Parse(string s)
        {
            int result;
            if (int.TryParse(s, out result)) return Some(result);
            else return None;
        }
        public static ImmutableTypes.Option<string> Lookup(this NameValueCollection @this, string key) => @this[key];
        public static ImmutableTypes.Option<T> Lookup<K, T>(this Dictionary<K, T> dict, K key)
        {
            T value;
            if (dict.TryGetValue(key, out value)) return Some(value);
            else return None;
        }

    }

    public class AppConfig
    {
        NameValueCollection source;
        public AppConfig() : this(ConfigurationManager.AppSettings) { }
        public AppConfig(NameValueCollection source) { this.source = source; }
        public ImmutableTypes.Option<string> Get(string name) => this.source.Lookup(name);
        public string Get(string name, string defaultValue) => this.source.Lookup(name).Match(() => defaultValue, (value) => value);
    }
    //public static class Functional
    //{
    //    public static R Connect<R>(string connStr, Func<IDbConnection, R> func)
    //    {
    //        using (var conn = new SqlConnection(connStr))
    //        {
    //            conn.Open();
    //            return func(conn);
    //        }
    //    }
    //}
}

namespace Enterprise.Framework.Topics.ImmutableTypes
{
    public class Age
    {
        private int Value { get; }
        public static Option<Age> Of(int value)
        {
            if (IsValid(value)) return new Age(value);
            else return new Option<Age>();
        }
        private Age(int value)
        {
            if (!IsValid(value)) throw new ArgumentNullException($"{value} is not a valid age.");
            this.Value = value;
        }

        private static bool IsValid(int value) => value >= 0 || value < 120;

        public static bool operator <(Age l, Age r) => l.Value < r.Value;
        public static bool operator >(Age l, Age r) => l.Value > r.Value;
        public static bool operator <(Age l, int r) => l < new Age(r);
        public static bool operator >(Age l, int r) => l > new Age(r);
    }
    public struct None { internal static readonly None Default = new None(); }
    public struct Some<T> { internal T Value { get; } internal Some(T value) { if (value == null) throw new ArgumentNullException(); Value = value; } }

    //Use it in your data objects to model the fact that a property may not be set, and in your
    //functions to indicate the possibility that a suitable value may not be returned.
    public struct Option<T>
    {
        readonly bool isSome;
        readonly T value;

        private Option(T value) { this.isSome = true; this.value = value; }
        public static implicit operator Option<T>(None _) => new Option<T>();
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);
        public static implicit operator Option<T>(T value) => value == null ? new Option<T>() : new Option<T>(value);//None : Some(value);
        public R Match<R>(Func<R> None, Func<T, R> Some) => isSome ? Some(value) : None();
    }
}
