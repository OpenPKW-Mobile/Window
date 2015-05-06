using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OpenPKW_Mobile.Models
{
    public class PhotoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Nazwa zdjęcia.
        /// </summary>
        private string _name = null;
        public string Name 
        { 
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Zdjęcie protokołu.
        /// </summary>
        private ImageSource _image = null;
        public ImageSource Image 
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
                OnPropertyChanged("Image");
            }
        }

        #region Implementacja INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion Implementacja INotifyPropertyChanged
    }
}
