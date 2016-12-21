using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Variable
    {
        private string nom;         // Nom de la variable
        private Type type;          // Type de la variable
        private bool contentKnown;  // Si le contenu est connu ou non
        private bool contentBool;   // Contenu si booleen
        private int contentInt;     // Contenu si int
        private float contentFloat; // Contenu si float

        /// <summary>
        /// Constructeur de variable inconnu
        /// </summary>
        /// <param name="nom">nom de la variable</param>
        public Variable (string nom)
        {
            this.nom = nom;
            contentKnown = false;
            type = new Type();
        }

        /// <summary>
        /// Contructeur de booleen
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentBool">Contenu booleen</param>
        public Variable (string nom, bool contentBool)
        {
            this.nom = nom;
            type = new Type(Type.types.BOOLEAN);
            contentKnown = true;
            this.contentBool = contentBool;
        }

        /// <summary>
        /// Contructeur de entier
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentInt">Contenu entier</param>
        public Variable (string nom, int contentInt)
        {
            this.nom = nom;
            type = new Type(Type.types.ENTIER);
            contentKnown = true;
            this.contentInt = contentInt;
        }

        /// <summary>
        /// Contructeur de décimal
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentFloat">Contenu décimal</param>
        public Variable (string nom, float contentFloat)
        {
            this.nom = nom;
            type = new Type(Type.types.REEL);
            contentKnown = true;
            this.contentFloat = contentFloat;
        }

        /// <summary>
        /// Contructeur de tye connu mais contenu inconnu
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="type">type de la variable</param>
        public Variable (string nom, Type type)
        {
            this.nom = nom;
            this.type = new Type(type);
            contentKnown = false;
        }

        /// <summary>
        /// Constructeur de recopie
        /// </summary>
        /// <param name="var">Variable à copier</param>
        public Variable (Variable var)
        {
            nom = var.nom;
            type = new Type(var.type);
            contentKnown = var.contentKnown;
            contentBool = var.contentBool;
            contentInt = var.contentInt;
            contentFloat = var.contentFloat;
        }

        /// <summary>
        /// Getteur de nom
        /// </summary>
        /// <returns>nom de la variable</returns>
        public string getNom()
        {
            return nom;
        }

        /// <summary>
        /// Défini si le contenu est connu ou non
        /// </summary>
        /// <returns>Boolean true si connu</returns>
        public bool isKnown()
        {
            return contentKnown;
        }

        /// <summary>
        /// Getteur de contenu, vérifie si int, lance erreur si non
        /// </summary>
        /// <returns>Contenu int</returns>
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

        /// <summary>
        /// Getteur de contenu, vérifie si booleen, lance erreur si non
        /// </summary>
        /// <returns>Contenu booleen</returns>
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

        /// <summary>
        /// Getteur de contenu, vérifie si decimal, lance erreur si non
        /// </summary>
        /// <returns>Contenu float</returns>
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

        /// <summary>
        /// Récupère le type de la variable
        /// </summary>
        /// <returns></returns>
        public Type getType()
        {
            return type;
        }

        /// <summary>
        /// attribue le contenu à la variable de celle passée en paramètre
        /// </summary>
        /// <param name="var">Variable à copier/assigner</param>
        public void setVar(Variable var)
        {
            if (contentKnown = var.isKnown() && type.type == var.type.type)
            {
                contentBool = var.contentBool;
                contentFloat = var.contentFloat;
                contentInt = var.contentInt;
                contentKnown = true;
            }
        }
    }
}
