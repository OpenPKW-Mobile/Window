using Microsoft.Xna.Framework.Media;
using OpenPKW_Mobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenPKW_Mobile.Mocks
{
    public class DesignPhotoModels
    {
        public List<PhotoModel> Photos { get; set; }

        public Size GridRatio
        {
            get
            {
                return new Size(3, 2);
            }
        }

        public DesignPhotoModels()
        {
            Photos = new List<PhotoModel>
            {
                new PhotoModel()
                {
                    Name = "Photo1",
                    Image = getSampleImage(200, 200, Colors.Red)
                },
                new PhotoModel()
                {
                    Name = "Photo2",
                    Image = getSampleImage(200, 200, Colors.Green)
                },
                new PhotoModel()
                {
                    Name = "Photo3",
                    Image = getSampleImage(200, 200, Colors.Blue)
                },
                new PhotoModel()
                {
                    Name = "Photo4",
                    Image = getSampleImage(200, 200, Colors.Brown)
                },
                new PhotoModel()
                {
                    Name = "Photo5",
                    Image = getSampleImage(200, 200, Colors.Purple)
                },
                new PhotoModel()
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
    }
}
