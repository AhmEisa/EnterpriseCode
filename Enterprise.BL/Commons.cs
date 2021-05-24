using NullGuard;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

[assembly: NullGuard(ValidationFlags.All)]
namespace Enterprise.BL
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;
            if (ReferenceEquals(valueObject, null))
                return false;
            return EqualsCore(valueObject);
        }
        protected abstract bool EqualsCore(T other);
        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }
        protected abstract int GetHashCodeCore();
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return false;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }
        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }

    public struct MayBe<T> : IEquatable<MayBe<T>> where T : class
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (HasNotValue)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        public bool HasValue => _value != null;
        public bool HasNotValue => !HasValue;
        private MayBe([AllowNull]T value) { _value = value; }
        public static implicit operator MayBe<T>([AllowNull]T value)
        {
            return new MayBe<T>(value);
        }
        public static bool operator ==(MayBe<T> mayBe, T value)
        {
            if (mayBe.HasNotValue) return false;
            return mayBe.Value.Equals(value);
        }
        public static bool operator !=(MayBe<T> mayBe, T value)
        {
            return !(mayBe == value);
        }
        public static bool operator ==(MayBe<T> first, MayBe<T> second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(MayBe<T> first, MayBe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MayBe<T>)) return false;
            var other = (MayBe<T>)obj;
            return Equals(other);
        }

        public bool Equals(MayBe<T> other)
        {
            if (HasNotValue && other.HasNotValue) return false;
            if (HasNotValue || other.HasNotValue) return false;
            return _value.Equals(other._value);
        }
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        public override string ToString()
        {
            if (HasNotValue) return "No Value";

            return Value.ToString();
        }

        [return: AllowNull]
        public T Unwrap([AllowNull]T defaultValue = default(T))
        {
            if (HasValue) return Value;

            return defaultValue;
        }
    }
    public class Result
    {
        public bool IsSuccess { get; }
        public ErrorType? ErrorType { get; private set; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, ErrorType? errorType)
        {
            if (isSuccess && errorType != null)
                throw new InvalidOperationException();
            if (!isSuccess && errorType == null)
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            ErrorType = errorType;
        }

        public static Result Fail(ErrorType errorType)
        {
            return new Result(false, errorType);
        }
        public static Result<T> Fail<T>(ErrorType error)
        {
            return new Result<T>(default(T), false, error);
        }
        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure) return result;
            }
            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, ErrorType? error) : base(isSuccess, error)
        {
            _value = value;
        }
    }
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this MayBe<T> mayBe, string errorMessage) where T : class
        {
            if (mayBe.HasNotValue) return Result.Fail<T>(ErrorType.InputValidationError);
            return Result.Ok(mayBe.Value);
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsFailure) return result;
            action();
            return Result.Ok();
        }
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.IsFailure) return result;

            return func();
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure) action();
            return result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);
            return result;
        }
        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }
    }

    public static class EnumerableExtensions
    {
        // Criterion called twice(used to called once).
        public static T WithMinimum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion) where T : class where TKey : IComparable<TKey>
            => sequence.Aggregate((T)null, (best, cur) => best == null || criterion(cur).CompareTo(criterion(best)) < 0 ? cur : best);

        // Now it is best
        public static T WithMinimum2<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criterion) where T : class where TKey : IComparable<TKey>
            => sequence.Select(painter => Tuple.Create(painter, criterion(painter)))
                       .Aggregate((Tuple<T, TKey>)null, (best, cur) => best == null || cur.Item2.CompareTo(best.Item2) < 0 ? cur : best)
                       .Item1;

        public static IEnumerable<T> Flatten<T>(this IEnumerable<Option<T>> sequence) => sequence.SelectMany(x => x.AsEnumerable());
        public static IEnumerable<TResult> Flatten<T, TResult>(this IEnumerable<T> sequence, Func<T, Option<TResult>> map) => sequence.Select(map).Flatten();
    }

    public static class DictionaryExtensions
    {
        public static void WithValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> execute)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                execute(value);
        }
    }

    public abstract class Option<T>
    {
        public abstract void Do(Action<T> action);
        public abstract Option<TNew> Map<TNew>(Func<T, TNew> mapping);
        public abstract T Reduce(Func<T> whenNone);
        public abstract T Reduce(T whenNone);
        public abstract IEnumerable<T> AsEnumerable();
    }
    public static class Option
    {
        public static Option<T> Some<T>(T value) => new SomeImpl<T>(value);
        public static Option<T> None<T>() => new NoneImpl<T>();
        private class SomeImpl<T> : Option<T>
        {
            private T Content { get; }
            public SomeImpl(T content)
            {
                this.Content = content;
            }
            public override IEnumerable<T> AsEnumerable() => new[] { this.Content };

            public override void Do(Action<T> action) => action(this.Content);

            public override Option<TNew> Map<TNew>(Func<T, TNew> mapping) => new SomeImpl<TNew>(mapping(this.Content));

            public override T Reduce(Func<T> whenNone) => this.Content;

            public override T Reduce(T whenNone) => this.Content;
        }
        private class NoneImpl<T> : Option<T>
        {
            public override IEnumerable<T> AsEnumerable() => Enumerable.Empty<T>();

            public override void Do(Action<T> action) { }

            public override Option<TNew> Map<TNew>(Func<T, TNew> mapping) => new NoneImpl<TNew>();

            public override T Reduce(Func<T> whenNone) => whenNone();

            public override T Reduce(T whenNone) => whenNone;
        }
    }

    public class NumberFormatter
    {
        public static string FormatNumber(int value)
        {
            if (value == 0) return "(Unknown)";
            int valueRounded = RoundValue4(value);
            return $"{valueRounded:### ### ### ###}".Trim();
        }

        private static int RoundValue4(int value)
        {
            int accuracy = Math.Max((int)GetHeighestPowerOfTen(value) / 10_0001, 1);
            return RoundToNearest(value, accuracy);
        }

        private static int RoundToNearest(int value, int accuracy)
        {
            throw new NotImplementedException();
        }

        private static int GetHeighestPowerOfTen(int value)
        {
            throw new NotImplementedException();
        }
    }

    public class CsvReader
    {
        private readonly string _filePath;

        public CsvReader(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> ReadItems<T>(Func<string, T> lineConverter)
        {
            var itemsList = new List<T>();

            using (StreamReader reader = new StreamReader(_filePath))
            {
                reader.ReadLine();// skip the header
                string line = string.Empty;
                while (string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    T itemValue = lineConverter(line);
                    itemsList.Add(itemValue);
                }
            }
            return itemsList;
        }
    }
    public class DynamicTypeCreation
    {
        public static T CreateType<T>(string configurationName)
        {
            string typeName = ConfigurationManager.AppSettings[configurationName];
            Type typeToCreate = Type.GetType(typeName);
            object typeAsObject = Activator.CreateInstance(typeToCreate);
            T createdObject = (T)typeAsObject;
            return createdObject;
        }
    }
}

namespace Enterprise.BL.Data
{
    /* Try to modify only one object per transaction
     * use query to find target Id
     * Load full domain object by Id
     * Modify domain object
     * Save changes to the database.
     */
    interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        T Find(int id);
        void Add(T obj);
        void Delete(T obj);
        void SaveChanges();
    }

    interface IReadOnlyRepository<TModel> : IDisposable
    {
        IQueryable<TModel> GetAll();
        TModel Find(int id);
    }

}

namespace Enterprise.BL.ViewModels
{
    class ProfessorViewModel
    {

    }
}
