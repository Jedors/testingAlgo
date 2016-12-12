using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Variable
    {
        private string nom;
        private Type type;
        private dynamic contenu;

        public Variable (string nom)
        {
            type = new Type();
        }

        public Variable (string nom, Type type) : this(nom)
        {
            this.type = new Type(type);
        }

        public Variable (string nom, Type type, dynamic contenu) : this(nom,type)
        {
            this.contenu = contenu;
        }

        public string getNom()
        {
            return nom;
        }

        public Type getType()
        {
            return type;
        }
    }
}
