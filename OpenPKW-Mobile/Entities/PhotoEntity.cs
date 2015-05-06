using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OpenPKW_Mobile.Entities
{
    public class PhotoEntity : EntityBase
    {
        /// <summary>
        /// Nazwa zdjęcia.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Zdjęcie protokołu.
        /// </summary>
        public ImageSource Image { get; set; }

        public override string Identifier
        {
            get 
            {
                return this.Name;
            }
        }
    }
}
