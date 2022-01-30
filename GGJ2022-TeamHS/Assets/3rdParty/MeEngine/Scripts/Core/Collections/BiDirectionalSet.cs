using System;
using System.Collections.Generic;

namespace MeEngine.Collections
{
    /// <summary>
    /// Represents a collection of items that are tied to other items. Easily retrieve all items tied to a specific item.
    /// Example: Characters can be friends. Return all friends of the specified character.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BiDirectionalSet<T>
    {
        Dictionary<T, HashSet<T>> tieDict = new Dictionary<T, HashSet<T>>();

        public BiDirectionalSet() { }

        /// <summary>
        /// Add an entry to the Bi-Directional Dictionary
        /// </summary>
        public void AddTie(T item1, T item2)
        {
            HashSet<T> set;
            if(!tieDict.TryGetValue(item1, out set))
            {
                set = new HashSet<T>();
                tieDict.Add(item1, set);
            }
            set.Add(item2);

            if(!tieDict.TryGetValue(item2, out set))
            {
                set = new HashSet<T>();
                tieDict.Add(item2, set);
            }
            set.Add(item1);
        }

        /// <summary>
        /// Severs the connection between two items.
        /// </summary>
        public void RemoveTie(T item1, T item2)
        {
            tieDict[item1].Remove(item2);
            tieDict[item2].Remove(item1);
        }

        /// <summary>
        /// Removes an item from the collection along with all ties to that item.
        /// </summary>
        public void RemoveItem(T item)
        {
            HashSet<T> removalSet;
            if (tieDict.TryGetValue(item, out removalSet))
            {
                tieDict.Remove(item);
                foreach (T item2 in removalSet)
                {
                    tieDict[item2].Remove(item);
                }
            }
        }

        /// <summary>
        /// Get the set of all items tied to the specified item.
        /// </summary>
        public bool TryGetTies(T item, out HashSet<T> set)
        {
            return tieDict.TryGetValue(item, out set);
        }

        /// <summary>
        /// Get the set of all items tied to the specified item.
        /// </summary>
        public HashSet<T> GetTies(T item)
        {
            return tieDict[item];
        }
    }

    /// <summary>
    /// Represents a collection of items that are tied to other items of a second type. Easily retrieve all other items tied to a specific item.
    /// Example: Events and Listeners. Get all Listeners listening to a specific event. OR Get all events a specific listener is listening for.
    /// </summary>
    public sealed class BiDirectionalSet<TypeA, TypeB>
    {
        Dictionary<TypeA, HashSet<TypeB>> AToBDict = new Dictionary<TypeA, HashSet<TypeB>>();
        Dictionary<TypeB, HashSet<TypeA>> BToADict = new Dictionary<TypeB, HashSet<TypeA>>();

        public BiDirectionalSet() { }

        /// <summary>
        /// Add an entry to the Bi-Directional Dictionary
        /// </summary>
        public void AddTie(TypeA item1, TypeB item2)
        {
            HashSet<TypeB> BSet;
            if(!AToBDict.TryGetValue(item1, out BSet))
            {
                BSet = new HashSet<TypeB>();
                AToBDict.Add(item1, BSet);
            }
            BSet.Add(item2);

            HashSet<TypeA> ASet;
            if(!BToADict.TryGetValue(item2, out ASet))
            {
                ASet = new HashSet<TypeA>();
                BToADict.Add(item2, ASet);
            }
            ASet.Add(item1);
        }

        /// <summary>
        /// Removes the connection between two items.
        /// </summary>
        public void RemoveTie(TypeA item1, TypeB item2)
        {
            AToBDict[item1].Remove(item2);
            BToADict[item2].Remove(item1);
        }

        /// <summary>
        /// Removes an item from the set along with all ties to that item.
        /// </summary>
        public void RemoveItem(TypeA item1)
        {
            HashSet<TypeB> removalSet;
            if (AToBDict.TryGetValue(item1, out removalSet))
            {
                AToBDict.Remove(item1);
                foreach(TypeB item2 in removalSet)
                {
                    BToADict.Remove(item2);
                }
            }
        }

        /// <summary>
        /// Removes an item from the set along with all ties to that item.
        /// </summary>
        public void RemoveItem(TypeB item2)
        {
            HashSet<TypeA> removalSet;
            if (BToADict.TryGetValue(item2, out removalSet))
            {
                BToADict.Remove(item2);
                foreach (TypeA item1 in removalSet)
                {
                    AToBDict.Remove(item1);
                }
            }
        }

        /// <summary>
        /// Get the set of all items tied to the specified item.
        /// </summary>
        public bool TryGetSet(TypeA keyA, out HashSet<TypeB> setB)
        {
            return AToBDict.TryGetValue(keyA, out setB);
        }

        /// <summary>
        /// Get the set of all items tied to the specified item.
        /// </summary>
        public bool TryGetSet(TypeB keyB, out HashSet<TypeA> setA)
        {
            return BToADict.TryGetValue(keyB, out setA);
        }
    }
}
