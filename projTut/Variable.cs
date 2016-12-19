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
        private bool contentKnown;
        private bool contentBool;
        private int contentInt;
        private float contentFloat;

        public Variable (string nom)
        {
            this.nom = nom;
            contentKnown = false;
            type = new Type();
        }

        public Variable (string nom, bool contentBool)
        {
            this.nom = nom;
            type = new Type(Type.types.BOOLEAN);
            contentKnown = true;
            this.contentBool = contentBool;
        }

        public Variable (string nom, int contentInt)
        {
            this.nom = nom;
            type = new Type(Type.types.ENTIER);
            contentKnown = true;
            this.contentInt = contentInt;
        }

        public Variable (string nom, float contentFloat)
        {
            this.nom = nom;
            type = new Type(Type.types.REEL);
            contentKnown = true;
            this.contentFloat = contentFloat;
        }

        public Variable (string nom, Type type)
        {
            this.nom = nom;
            this.type = new Type(type);
            contentKnown = false;
        }

        public Variable (Variable var)
        {
            nom = var.nom;
            type = new Type(var.type);
            contentKnown = var.contentKnown;
            contentBool = var.contentBool;
            contentInt = var.contentInt;
            contentFloat = var.contentFloat;
        }

        public string getNom()
        {
            return nom;
        }

        public bool isKnown()
        {
            return contentKnown;
        }

        public int getInt()
        {
            if (isKnown())
            {
                if (getType().type == Type.types.ENTIER)
                    return contentInt;
                else
                    throw new Exception("Erreur de type");
            }
            else
                throw new Exception("Valeur non connu");
        }

        public bool getBool()
        {
            if (isKnown())
            {
                if (getType().type == Type.types.BOOLEAN)
                    return contentBool;
                else
                    throw new Exception("Erreur de type");
            }
            else
                throw new Exception("Valeur non connu");
        }

        public float getFloat()
        {
            if (isKnown())
            {
                if (getType().type == Type.types.REEL)
                    return contentFloat;
                else
                    throw new Exception("Erreur de type");
            }
            else
                throw new Exception("Valeur non connu");
        }

        public Type getType()
        {
            return type;
        }
    }
}
