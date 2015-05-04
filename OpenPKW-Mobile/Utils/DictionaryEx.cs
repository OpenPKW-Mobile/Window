using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Utils
{
    /// <summary>
    /// Bezpieczna wersja słownika.
    /// </summary>
    /// <typeparam name="TKey">Typ klucza.</typeparam>
    /// <typeparam name="TValue">Typ wartości.</typeparam>
    public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return base[key];
                else return default(TValue);
            }
            set
            {
                if (base.ContainsKey(key))
                    base[key] = value;
                else base.Add(key, value);
            }
        }
    }
}
