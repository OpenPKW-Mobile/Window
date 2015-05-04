using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Mocks
{
    public class DesignCommissionEntity : CommissionEntity
    {
        public DesignCommissionEntity()
        {
            this.Type = "[typ komisji wyborczej]";
            this.Title = "[numer i miasto]";
            this.Id = "[identyfikator]";
            this.Place = "[nazwa miejsca/budynku]";
            this.Address = "[adres]";
        }
    }
}
