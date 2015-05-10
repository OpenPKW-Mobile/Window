using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Mocks
{
    public class DesignUploadModel
    {
        public class ProgressData
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString()
            {
                return this.Text;
            }

            public static explicit operator int(ProgressData data)
            {
                return data.Value;
            }

            public override int GetHashCode()
            {
                return this.Value;
            }
        }

        public string Message { get; set; }

        public string Information { get; set; }

        public ProgressData Progress { get; set; }
        public int ProgressValue { get; set; }
        public string ProgressText { get; set; }

        public DesignUploadModel()
        {
            Message = "[tutaj będzie wyświetlony wynik operacji]";
            Information = "[tutaj będą umieszczone dodatkowe informacje dla użytkownika]";
            Progress = new ProgressData()
            {
                Value = 20,
                Text = "Strona 1   (1248 z 2345 KB)"
            };
            ProgressValue = 20;
            ProgressText = "Strona 1  (321 z 900 KB)";
        }
    }
}
