using System;
using System.Collections.Generic;
using System.Text;

namespace Leoxia.Collections
{
    public sealed class Cursor<T> : ICursor<T> where T : class
    {
        private readonly IEnumerator<T> _enumerator;

        internal Cursor(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
        }

        public T Next()
        {
            if (_enumerator.MoveNext())
            {
                return _enumerator.Current;
            }
            return null;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }

    public static class CursorEx
    {

        public static ICursor<T> GetCursor<T>(this IEnumerable<T> enumerable)
            where T : class
        {
            return new Cursor<T>(enumerable);
        }
    }

    public interface ICursor<T> : IDisposable
    {
        T Next();
    }
}
