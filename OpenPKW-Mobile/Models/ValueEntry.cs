using OpenPKW_Mobile.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Wartość liczbowa z automatyczną walidacją
    /// </summary>
    public class ValueEntry
    {
        /// <summary>
        /// Wskazówka.
        /// Oznacza zakres, w którym powinna zawierać się wartość liczbowa.
        /// </summary>
        public class Hint : Range<int>
        {
        }

        /// <summary>
        /// Lista wskazówek dla walidacji wartości.
        /// </summary>
        public DictionaryEx<string, Hint> Hints = new DictionaryEx<string, Hint>();

        /// <summary>
        /// Wartość liczbowa.
        /// </summary>
        public readonly int Value;

        /// <summary>
        /// Czy wartość jest zaufana?
        /// </summary>
        public readonly bool IsDirty;

        /// <summary>
        /// Czy wartość jest prawidłowa? 
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !IsDirty &&
                    Hints.Values.All(hint => hint.ContainsValue(Value));
            }
        }

        /// <summary>
        /// Automatyczna konwersja na liczbę całkowitą.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static implicit operator int(ValueEntry entry)
        {
            return entry.Value;
        }

        /// <summary>
        /// Automatyczna konwersja z liczby całkowitej.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator ValueEntry(int value)
        {
            return new ValueEntry(value);
        }

        /// <summary>
        /// Automatyczna konwersja z liczby całkowitej wyrażonej tekstem.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static implicit operator ValueEntry(string text)
        {
            int value;
            if (int.TryParse(text, out value))
                return new ValueEntry(value);
            else return null;
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        public ValueEntry()
        {
            this.IsDirty = true;
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="value"></param>
        public ValueEntry(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="text"></param>
        public ValueEntry(string text)
        {
            this.Value = int.Parse(text);
        }

        /// <summary>
        /// Porównanie do innego obiektu.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ValueEntry item = obj as ValueEntry;
            if (item == null) return false;
            return this.Equals(item);
        }

        /// <summary>
        /// Porównanie do innego obiektu tej samej klasy.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected bool Equals(ValueEntry entry)
        {
            return
                this.IsDirty == entry.IsDirty &&
                this.Value == entry.Value;
        }

        /// <summary>
        /// Wartość skrótu.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value;
        }

        /// <summary>
        /// Reprezentacja obiektu w postaci tekstowej.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return IsDirty ? String.Empty : Value.ToString();
        }
    }
}
