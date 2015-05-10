using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Utils
{
    /*
    /// <summary>
    /// Obsługuje zadanie wykonywane w tle.
    /// Przechowuje typ wartości wyliczanej.
    /// </summary>
    /// <typeparam name="T">Typ wartości wylicznej.</typeparam>
    class BackgroundWorkerEx<T> : BackgroundWorker
        where T : struct, IConvertible
    {
        public readonly T Type;

        public BackgroundWorkerEx(T type)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T musi być typem wyliczanym");
            }

            this.Type = type;
        }
    }
    */
    class BackgroundWorkerEx<T> : BackgroundWorker
    {
        public readonly T Object;

        public BackgroundWorkerEx(T obj)
        {
            this.Object = obj;
        }
    }
}
