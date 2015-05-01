using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Strona protokołu
    /// </summary>
    public class ProtocolPage
    {
        /// <summary>
        /// Zdjęcie
        /// </summary>
        public BitmapImage Image { get; set; }
    }

    /// <summary>
    /// Dane aplikacji Open PKW Mobile - wspólne dla wszystkich stron
    /// </summary>
    public class OpmAppData
    {
        /// <summary>
        /// Inicjalizacja 
        /// </summary>
        public OpmAppData()
        {
            ProtocolPages = new Dictionary<int, IList<ProtocolPage>>();
        }

        /// <summary>
        /// Komisie wyborcze, wybrane przez użytkownika, dla których wysyła dane
        /// </summary>
        public IEnumerable<ElectoralCommission> SelectedCommisions { get; set; }

        /// <summary>
        /// Zdjęcia protokołów poszczególnych komisji (id komisji / lista zdjęć)
        /// </summary>
        public IDictionary<int, IList<ProtocolPage>> ProtocolPages { get; set; }

        /// <summary>
        /// Zdjęcie do akceptacji
        /// </summary>
        public BitmapImage ToAcceptImage { get; set; }
    }
}
