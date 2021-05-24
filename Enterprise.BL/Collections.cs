using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.BL.Collections
{
    /* Collection: a type whose purpose is to group data together.Let you treat lots of objects as one single object.
     * Array characteristics: fixed size and ordered.
     * Arrays are zero-based index.Index of last element = number of elements - 1.[] to identify items.For each to enumerate.
     * It use of zero based-index returns to historical reasons: made memory management easier and better for performance when computers were slow.
     * Use DataBank website to explore different databases of informations.
     * Arrays are alawys reference types.
     * Arrays start full of null/default values.
     * List<T>: starts empty then add values.search with FindIndex.Insert and delete can be inefficient.
     * 
     */

    public class ArrayOprations
    {
        public string[] DaysOfWeek => new[] { "Saturday", "SunDay", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        public void EnumerateOverArray()
        {
            foreach (string day in DaysOfWeek)
            {
                // do action
            }
        }

        public void LookupElement()
        {
            int iday = int.Parse(Console.ReadLine());
            string chosenDay = DaysOfWeek[iday];
            // do action with the data
        }

    }

    /* Dictonaries:
     * Lookup items with a key.Great for unordered data.
     * foreach is only for reading a collection.
     * use for to modify a collection but work backwards.
     */
    public class UserDictonary<TKey, TValue> where TKey : class where TValue : class
    {
        private List<KeyValuePair<TKey, TValue>> _keyValues;
        public UserDictonary()
        {
            _keyValues = new List<KeyValuePair<TKey, TValue>>();
        }
        public void Add(TKey key, TValue value)
        {
            if (_keyValues.FindIndex(x => x.Key == key) >= 0)
                throw new ArgumentException("An item the same key already exist");
            _keyValues.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
        public bool ContainsKey(TKey key) => _keyValues.FindIndex(x => x.Key == key) >= 0;
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            if (this.ContainsKey(key))
            {
                value = this._keyValues[this._keyValues.FindIndex(x => x.Key == key)].Value;
                return true;
            }
            return false;
        }

        public IEnumerable<TValue> Values => this._keyValues.Select(x => x.Value);
        public IEnumerable<TKey> Keys => this._keyValues.Select(x => x.Key);
    }
    /* Collections of collections
     * Allows partitioning data
     * Chained Lookups: T[][]
     * Multidiensional arrays : single array with two indices, require a completely regular grid. 3*3
     * Jagged arrays: array of arrays ,not  multidimensional arras 3*7
     * Linq:complete framework for querying data sources(collections,databases,etc.).
     */
    public class CollectionOfCollection
    {
        private int[][] _jaggedBoard = { new int[3], new int[5], new int[6] }; //Jagged Array
        private int[,] _multBoard = new int[3, 3]; //Multidiensional Array
    }
}
