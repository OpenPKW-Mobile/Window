using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Utils
{
    /// <summary>
    /// Zakres wartości wskazanego typu.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> where T : IComparable<T>
    {
        /// <summary>
        /// Minimalna wartość zakresu.
        /// </summary>
        public T Minimum { get; set; }

        /// <summary>
        /// Maksymalna wartość zakresu.
        /// </summary>
        public T Maximum { get; set; }

        /// <summary>
        /// Prezentuje zakres w czytelnym formacie tekstowym.
        /// </summary>
        /// <returns>Tekst reprezentujący zakres.</returns>
        public override string ToString() { return String.Format("[{0} - {1}]", Minimum, Maximum); }

        /// <summary>
        /// Określa, czy zakres jest prawidłowy
        /// </summary>
        /// <returns>TRUE jeśli zakres jest prawidłowy, w przeciwnym wypadku FALSE.</returns>
        public Boolean IsValid() { return Minimum.CompareTo(Maximum) <= 0; }

        /// <summary>
        /// Określa, czy wartość jest w środku zakresu.
        /// </summary>
        /// <param name="value">Testowana wartość.</param>
        /// <returns>TRUE, jeśli wartość jest w środku zakresu, w przeciwnym wypadku FALSE</returns>
        public Boolean ContainsValue(T value)
        {
            return (Minimum.CompareTo(value) <= 0) && (value.CompareTo(Maximum) <= 0);
        }

        /// <summary>
        /// Określa, czy zakres jest w granicach innego zakresu.
        /// </summary>
        /// <param name="Range">Testowy zakres zewnętrzny.</param>
        /// <returns>TRUE, jeśli zakres zawiera się, w przeciwnym wypadku FALSE</returns>
        public Boolean IsInsideRange(Range<T> Range)
        {
            return this.IsValid() && Range.IsValid() && Range.ContainsValue(this.Minimum) && Range.ContainsValue(this.Maximum);
        }

        /// <summary>
        /// Określi, czy inny zakres jest w granicach tego zakresu
        /// </summary>
        /// <param name="Range">Testowy zakres zewnętrzny.</param>
        /// <returns>TRUE, jeśli zakres zewnętrzny zawiera się, w przeciwnym wypadku FALSE</returns>
        public Boolean ContainsRange(Range<T> Range)
        {
            return this.IsValid() && Range.IsValid() && this.ContainsValue(Range.Minimum) && this.ContainsValue(Range.Maximum);
        }
    }
}
