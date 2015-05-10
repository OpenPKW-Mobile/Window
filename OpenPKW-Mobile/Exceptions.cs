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

    /// <summary>
    /// Rodzaj wyjątku używany w procesie odzyskiwania hasła przez użytkownika.
    /// </summary>
    public class RemindException : ApplicationException
    {
        public enum ErrorReason
        {
            AnonymousNotAllowed,
            EmailRequired,
            IncorrectNameOrEmail            
        }

        private ErrorReason _reason;
        private Dictionary<ErrorReason, string> _messages;

        public RemindException(ErrorReason reason)
        {
            this._reason = reason;
            this._messages = new Dictionary<ErrorReason, string>()
            {
                { ErrorReason.AnonymousNotAllowed, "Aby odzyskać hasło do systemu, musisz podać nazwę użytkownika oraz adres mailowy." },
                { ErrorReason.EmailRequired, "Powinieneś podać adres e-mail, które otrzymałeś od administratora systemu." },
                { ErrorReason.IncorrectNameOrEmail, "Prawdopodobnie popełniłeś błąd wprowadzając nazwę użytkownika lub adres mailowy." }                
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

    /// <summary>
    /// Rodzaj wyjątku używany w procesie przesyłania danych wyborów.
    /// </summary>
    public class VotingException : ApplicationException
    {
        public enum ErrorReason
        {
            NobodyCandidate,
            CannotLoadCandidates,
            CannotSendResults,
        }

        private ErrorReason _reason;
        private Dictionary<ErrorReason, string> _messages;

        public VotingException(ErrorReason reason)
        {
            this._reason = reason;
            this._messages = new Dictionary<ErrorReason, string>()
            {
                { ErrorReason.NobodyCandidate, "System nie posiada wiedzy o kandydatach w tych wyborach." },
                { ErrorReason.CannotLoadCandidates, "Nie udało się uzyskać informacji o liście osób kandydujących w wyborach." },
                { ErrorReason.CannotSendResults, "Teraz nie można wysłać danych o wynikach wyborów. Poczekaj chwilę i spróbuj ponownie." },
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

    /// <summary>
    /// Rodzaj wyjątku używany w procesie przesyłania zdjęć protokołów.
    /// </summary>
    public class PhotoException : ApplicationException
    {
        public enum ErrorReason
        {
            InvalidBitmap,
            CannotUploadFile
        }

        private ErrorReason _reason;
        private Dictionary<ErrorReason, string> _messages;

        public PhotoException(ErrorReason reason)
        {
            this._reason = reason;
            this._messages = new Dictionary<ErrorReason, string>()
            {
                { ErrorReason.InvalidBitmap, "Można przesłać do zewnętrznego magazynu wyłącznie zdjęcia." },
                { ErrorReason.CannotUploadFile, "Teraz nie można wysłać zdjęcia protokołu. Poczekaj chwilę i spróbuj ponownie." },
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
