using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    interface ILoginService
    {
        event Action<UserEntity> LoginCompleted;
        event Action<string> LoginRejected;

        void BeginLogin(string userName, string userPassword);

    }
}
