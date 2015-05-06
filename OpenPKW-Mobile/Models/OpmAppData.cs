using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Dane aplikacji Open PKW Mobile - wspólne dla wszystkich stron
    /// </summary>
    public class OpmAppData
    {
        /// <summary>
        /// Komisje wyborcze, wybrane przez użytkownika, dla których wysyła dane
        /// </summary>
        public IEnumerable<ElectoralCommission> SelectedCommisions { get; set; }

        /// <summary>
        /// Komisja wyborcza, aktualnie wybrana przez użytkownika
        /// </summary>
        public CommissionEntity CurrentCommision { get; set; }

        /// <summary>
        /// Lista kandydatów na urząd prezydenta
        /// </summary>
        public CandidateEntity[] Candidates { get; set; }

        /// <summary>
        /// Podsumowanie głosów z urny wyborczej
        /// </summary>
        public ElectionEntity Election { get; set; }

        /// <summary>
        /// Zdjęcia protokołów
        /// </summary>
        public IEnumerable<PhotoEntity> ProtocolPhotos { get; set; }

#if DEBUG
        public OpmAppData()
        {
            CommissionEntity commission = new CommissionEntity()
            {
                Type = "Obwodowa Komisja Wyborcza",
                Title = "Nr 5 w Łodzi",
                Id = "106101-5",
                Place = "Laboratorium Produkcji Ogrodniczej w Zespole Szkół Rzemiosła im. Jana Kilińskiego",
                Address = "ul. Liściasta 181, 91-220 Łódź"
            };

            CurrentCommision = commission;

            ProtocolPhotos = new List<PhotoEntity>
            {
                new PhotoEntity()
                {
                    Name = "Photo1",
                    Image = getSampleImage(200, 200, Colors.Red)
                },
                new PhotoEntity()
                {
                    Name = "Photo2",
                    Image = getSampleImage(200, 200, Colors.Green)
                },
                new PhotoEntity()
                {
                    Name = "Photo3",
                    Image = getSampleImage(200, 200, Colors.Blue)
                },
                new PhotoEntity()
                {
                    Name = "Photo4",
                    Image = getSampleImage(200, 200, Colors.Brown)
                },
                new PhotoEntity()
                {
                    Name = "Photo5",
                    Image = getSampleImage(200, 200, Colors.Purple)
                },
                new PhotoEntity()
                {
                    Name = "Photo6",
                    Image = getSampleImage(200, 200, Colors.Orange)
                }
            };

        }

        private ImageSource getSampleImage(int width, int height, Color color)
        {
            Canvas canvas = new Canvas();
            canvas.Background = new SolidColorBrush(color);
            canvas.Width = width;
            canvas.Height = height;

            WriteableBitmap bmp = new WriteableBitmap(canvas, null);

            return bmp;
        }
#endif
    }
}
