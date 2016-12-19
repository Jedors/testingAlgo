using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Parametre
    {
        public enum PassagePar
        {
            ADRESSE,
            VALEUR,
            UNKNOWN
        }

        private PassagePar passagePar;
        private Type type;
    }
}
