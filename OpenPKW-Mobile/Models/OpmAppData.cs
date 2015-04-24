using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Dane aplikacji Open PKW Mobile - wspólne dla wszystkich stron
    /// </summary>
    public class OpmAppData
    {
        /// <summary>
        /// Komisie wyborcze, wybrane przez użytkownika, dla których wysyła dane
        /// </summary>
        public IEnumerable<ElectoralCommision> SelectedCommisions { get; set; }
    }
}
