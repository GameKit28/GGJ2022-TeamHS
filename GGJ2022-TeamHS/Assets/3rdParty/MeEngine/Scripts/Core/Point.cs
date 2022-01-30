using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace MeEngine
{
    /// <summary>
    /// A 2D Point that stores an X and Y value. Much like a Vector2, but using int instead of float for x and y.
    /// </summary>
    public struct Point : IXmlSerializable
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        //Static Functions
        public static Point Zero { get { return new Point(0, 0); } }
        public static Point One { get { return new Point(1, 1); } }

        #region operators
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }

        public static Point operator *(Point a, int b)
        {
            return new Point(a.x * b, a.y * b);
        }

        //Equals opperations
        public static bool operator ==(Point lhs, Point rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;
            Point pObj = (Point)obj;
            return pObj.x == this.x && pObj.y == this.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                return hash;
            }
        }
        #endregion

        #region conversions
        public static explicit operator Point(Vector2 vector2)
        {
            return new Point((int)vector2.x, (int)vector2.y);
        }

        public static implicit operator Vector2(Point point)
        {
            return new Vector2(point.x, point.y);
        }

        public static explicit operator Point(string str)
        {
            char[] charactersToTrim = new char[] { '(', ')', '[', ']', '{', '}' };
            try
            {
                string[] spltStr = str.Trim(charactersToTrim).Split(',');
                return new Point(int.Parse(spltStr[0]), int.Parse(spltStr[1]));
            }
            catch (System.Exception e)
            {
                throw new System.FormatException("The string \"" + str + "\" does not match the Point format: \"(x, y)\"", e);
            }
        }
        #endregion

        #region IXmlSerializable Members
        //So we can read and write points from an XML
        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            try
            {
                char[] charactersToTrim = new char[] { '(', ')', '[', ']', '{', '}' };
                string[] spltStr = reader.ReadElementContentAsString().Trim(charactersToTrim).Split(',');
                x = int.Parse(spltStr[0]);
                y = int.Parse(spltStr[1]);
            }
            catch (System.Exception e)
            {
                throw new System.FormatException("The string \"" + reader.ReadElementContentAsString() + "\" does not match the Point format: \"(x, y)\"", e);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            //Formatted like "(x, y)"
            writer.WriteString(string.Concat("(", x.ToString(), ", ", y.ToString(), ")"));
        }
        #endregion
    }
}
