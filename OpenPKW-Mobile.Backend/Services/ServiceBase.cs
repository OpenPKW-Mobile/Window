using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Services
{
    public abstract class ServiceBase
    {
        StringDictionary _params = new StringDictionary();

        /// <summary>
        /// Pobiera listę parametrów.
        /// </summary>
        /// <returns></returns>
        public virtual string GetMenu()
        {
            var @params = this._params;
            StringBuilder sb = new StringBuilder();

            var keys = @params.Keys
                .OfType<string>()
                .OrderBy(key => key);

            foreach(string key in keys)
            {
                string line = String.Format("\t{0}: {1}", key, @params[key] ?? "(null)");
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Dodaje parametr do listy.
        /// Nie można dodać parametru z zewnątrz.
        /// </summary>
        /// <param name="name">Nazwa parametru</param>
        /// <param name="value">Wartość domyślna parametru</param>
        protected void AddParam(string name, string value)
        {
            var @params = this._params;

            name = name.ToLower();
            if (@params.ContainsKey(name))
                @params[name] = value;
            else @params.Add(name, value);
        }

        /// <summary>
        /// Ustawia wartość parametru.
        /// </summary>
        /// <param name="name">Nazwa parametru</param>
        /// <param name="value">Wartość parametru</param>
        public void SetParam(string name, string value)
        {
            var @params = this._params;

            name = name.ToLower();
            if (@params.ContainsKey(name))
                @params[name] = value;
        }

        /// <summary>
        /// Pobiera wartość parametru.
        /// </summary>
        /// <param name="name">Nazwa parametru</param>
        /// <returns>Wartość parametru, jęsli istnieje, w przeciwnym przypadku NULL</returns>
        public string GetParam(string name)
        {
            var @params = this._params;

            name = name.ToLower();
            if (@params.ContainsKey(name))
                return @params[name];
            else return null;
        }
    }
}
