using System;

namespace Leoxia.ReadLine
{
    public class ControlSequence : IEquatable<ControlSequence>
    {
        private readonly ConsoleModifiers _modifiers;
        private readonly ConsoleKey _key;

        public ControlSequence(ConsoleModifiers modifiers, ConsoleKey key):
            this(key)
        {
            _modifiers = modifiers;
        }

        public ControlSequence(ConsoleKeyInfo keyInfo) 
            : this(keyInfo.Modifiers, keyInfo.Key)
        {
            
        }

        public ControlSequence(ConsoleKey key)
        {
            _key = key;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ControlSequence other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _modifiers == other._modifiers && _key == other._key;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ControlSequence) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)_modifiers << 8) + (int)_key;
            }
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:Leoxia.ReadLine.ControlSequence" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(ControlSequence left, ControlSequence right)
        {
            return Equals(left, right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:Leoxia.ReadLine.ControlSequence" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(ControlSequence left, ControlSequence right)
        {
            return !Equals(left, right);
        }
    }
}