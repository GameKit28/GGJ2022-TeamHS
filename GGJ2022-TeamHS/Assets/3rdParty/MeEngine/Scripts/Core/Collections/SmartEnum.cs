using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MeEngine
{
    /// <summary>
    /// A framework for creating enum-like classes.
    /// </summary>
    public abstract class SmartEnumEntry
    {
        protected string name;
        protected int value;

        protected SmartEnumEntry(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }

        //Converts this "enum" to its integer value
        public static explicit operator int(SmartEnumEntry a)
        {
            return a.value;
        }

        //Converts this "enum" to its string name
        public static explicit operator string(SmartEnumEntry a)
        {
            return a.name;
        }
    }

    /// <summary>
    /// A Smart Enum that allows for the listing of all internal "emum" values.
    /// </summary>
    public abstract class SmartEnumType<T> : SmartEnumEntry where T : SmartEnumType<T>
    {
        //Stores all the possible values of each "enum" type.
        private static readonly Dictionary<System.Type, List<T>> allTypeDict = new Dictionary<System.Type, List<T>>();

        protected SmartEnumType(int value, string name) : base(value, name) {
            
            List<T> typeList;
            if(allTypeDict.TryGetValue(typeof(T), out typeList))
            {
                typeList.Add(this as T);
            }
            else
            {
                typeList = new List<T>();
                typeList.Add(this as T);
                allTypeDict.Add(typeof(T), typeList);
            }
        }

        /// <summary>
        /// Returns all possible values of this Enum type.
        /// </summary>
        public static List<T> ListAll()
        {
            List<T> returnList;
            if(allTypeDict.TryGetValue(typeof(T), out returnList)){
                return returnList;
            }else{
                //Note. In some cases ListAll is called before the static initializers of the respective Enum are called.
                //In this case we need to manually initialize them, then the allTypeDict will be populated.
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
                return allTypeDict[typeof(T)];
            }
        }

        protected T FromInt(int value)
        {
            foreach (T instance in ListAll())
            {
                if (instance.value == value)
                {
                    return instance;
                }
            }
            throw new ArgumentException("No " + typeof(T).ToString() + " matches the provided value of " + value + ".");
        }

        protected T FromString(string name)
        {
            foreach (T instance in ListAll())
            {
                if (instance.name == name)
                {
                    return instance;
                }
            }
            throw new ArgumentException("No " + typeof(T).ToString() + " matches the provided name " + name + ".");
        }
    }

    public sealed class PrimaryColors : SmartEnumType<PrimaryColors>
    {
        public static readonly PrimaryColors
            Red = new PrimaryColors(0, "Red"),
            Yellow = new PrimaryColors(1, "Yellow"),
            Blue = new PrimaryColors(2, "Blue");

        private PrimaryColors(int value, string name) : base(value, name) { }
    }

    public class Foo
    {
        PrimaryColors myColor = PrimaryColors.Red;

        void bar()
        {
            foreach (PrimaryColors color in PrimaryColors.ListAll())
            {

            }
        }
    }
}
