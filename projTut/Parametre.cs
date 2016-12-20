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
        private string contenu;

        public Parametre(Type type, PassagePar passagePar,string contenu)
        {
            
            this.type = type;
            this.passagePar = passagePar;
            this.contenu = contenu;

        }

        public override string ToString()
        {
            return "\nContenu : " + contenu + "\ntype : " + type + "\nPassé par :" +  passagePar;
        }
    }
}
