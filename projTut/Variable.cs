using System;

namespace projTut
{
    internal class Variable
    {
        public string Nom { get; }
        public TypeElement Type { get; }
        private bool _contentBool; // Contenu si booleen
        private float _contentFloat; // Contenu si float
        private int _contentInt; // Contenu si int
        private bool _contentKnown; // Si le contenu est connu ou non

        /// <summary>
        ///     Constructeur de variable inconnu
        /// </summary>
        /// <param name="nom">nom de la variable</param>
        public Variable(string nom)
        {
            Nom = nom;
            _contentKnown = false;
            Type = new TypeElement();
        }

        /// <summary>
        ///     Contructeur de booleen
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentBool">Contenu booleen</param>
        public Variable(string nom, bool contentBool)
        {
            Nom = nom;
            Type = new TypeElement(TypeEnum.Boolean);
            _contentKnown = true;
            _contentBool = contentBool;
        }

        /// <summary>
        ///     Contructeur de entier
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentInt">Contenu entier</param>
        public Variable(string nom, int contentInt)
        {
            Nom = nom;
            Type = new TypeElement(TypeEnum.Entier);
            _contentKnown = true;
            _contentInt = contentInt;
        }

        /// <summary>
        ///     Contructeur de décimal
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="contentFloat">Contenu décimal</param>
        public Variable(string nom, float contentFloat)
        {
            Nom = nom;
            Type = new TypeElement(TypeEnum.Reel);
            _contentKnown = true;
            _contentFloat = contentFloat;
        }

        /// <summary>
        ///     Contructeur de tye connu mais contenu inconnu
        /// </summary>
        /// <param name="nom">Nom de la variable</param>
        /// <param name="type">type de la variable</param>
        public Variable(string nom, TypeElement type)
        {
            Nom = nom;
            Type = new TypeElement(type);
            _contentKnown = false;
        }

        /// <summary>
        ///     Constructeur de recopie
        /// </summary>
        /// <param name="var">Variable à copier</param>
        public Variable(Variable var)
        {
            Nom = var.Nom;
            Type = new TypeElement(var.Type);
            _contentKnown = var._contentKnown;
            _contentBool = var._contentBool;
            _contentInt = var._contentInt;
            _contentFloat = var._contentFloat;
        }

        /// <summary>
        ///     Défini si le contenu est connu ou non
        /// </summary>
        /// <returns>Boolean true si connu</returns>
        public bool IsKnown()
        {
            return _contentKnown;
        }

        /// <summary>
        ///     Getteur de contenu, vérifie si int, lance erreur si non
        /// </summary>
        /// <returns>Contenu int</returns>
        public int GetInt()
        {
            if (IsKnown())
            {
                if (Type.Type == TypeEnum.Entier)
                    return _contentInt;
                throw new Exception("Erreur de type");
            }
            throw new Exception("Valeur non connu");
        }

        /// <summary>
        ///     Getteur de contenu, vérifie si booleen, lance erreur si non
        /// </summary>
        /// <returns>Contenu booleen</returns>
        public bool GetBool()
        {
            if (IsKnown())
            {
                if (Type.Type == TypeEnum.Boolean)
                    return _contentBool;
                throw new Exception("Erreur de type");
            }
            throw new Exception("Valeur non connu");
        }

        /// <summary>
        ///     Getteur de contenu, vérifie si decimal, lance erreur si non
        /// </summary>
        /// <returns>Contenu float</returns>
        public float GetFloat()
        {
            if (IsKnown())
            {
                if (Type.Type == TypeEnum.Reel)
                    return _contentFloat;
                throw new Exception("Erreur de type");
            }
            throw new Exception("Valeur non connu");
        }

        /// <summary>
        ///     attribue le contenu à la variable de celle passée en paramètre
        /// </summary>
        /// <param name="var">Variable à copier/assigner</param>
        public void SetVar(Variable var)
        {
            if (_contentKnown == var.IsKnown() && Type.Type == var.Type.Type)
            {
                _contentBool = var._contentBool;
                _contentFloat = var._contentFloat;
                _contentInt = var._contentInt;
                _contentKnown = true;
            }
        }
    }
}