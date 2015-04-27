using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Rodzaj wyjątku używany w procesie logowania użytkownika.
    /// </summary>
    public class LoginException : ApplicationException
    {
        public enum ErrorReason
        {
            AnonymousNotAllowed,
            PasswordRequired,
            IncorrectNameOrPassword,
            SessionExpired,
        }

        private ErrorReason _reason;
        private Dictionary<ErrorReason, string> _messages;

        public LoginException(ErrorReason reason)
        {
            this._reason = reason;
            this._messages = new Dictionary<ErrorReason, string>()
            {
                { ErrorReason.AnonymousNotAllowed, "Aby zalogować się do systemu, musisz podać nazwę użytkownika oraz hasło." },
                { ErrorReason.PasswordRequired, "Powinieneś podać hasło, które otrzymałeś od administratora systemu." },
                { ErrorReason.IncorrectNameOrPassword, "Prawdopodobnie popełniłeś błąd wprowadzając nazwę użytkownika lub hasło." },
                { ErrorReason.SessionExpired, "Od Twojej ostatniej aktywności upłynęło trochę czasu, więc zaloguj się ponownie." },
            };
        }

        public override string Message
        {
            get
            {
                return this._messages[_reason];
            }
        }
    }
}
