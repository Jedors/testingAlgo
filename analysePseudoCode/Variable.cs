using System;
using System.Globalization;

namespace analysePseudoCode
{
    internal class Variable
    {
        public string Nom { get; }
        public TypeElement Type { get; }
        private bool _contentBool; // Contenu si booleen
        private float _contentFloat; // Contenu si float
        private int _contentInt; // Contenu si int
        internal bool ContentKnown { get; private set; }

        /// <summary>
        ///     Constructeur de variable inconnu
        /// </summary>
        /// <param name="nom">nom de la variable</param>
        public Variable(string nom) : this(nom, new TypeElement()) { }

        public Variable(string nom, TypeElement type) : this(nom, type, false, false,  0.0f, 0) { }

        public Variable(string nom, bool contentBool) : this(nom, new TypeElement(TypeEnum.Boolean), contentBool) { }

        public Variable(string nom, float contentFloat) : this(nom, new TypeElement(TypeEnum.Real), contentFloat) { }

        public Variable(string nom, int contentInt) : this(nom, new TypeElement(TypeEnum.Integer), contentInt) { }

        public Variable(string nom, TypeElement type, bool contentBool) : this(nom, type, true, contentBool, 0.0f, 0)
        {
            if (type.Type == TypeEnum.Unknown)
                type.Type = TypeEnum.Boolean;
            if (type.Type != TypeEnum.Boolean)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        public Variable(string nom, TypeElement type, float contentFloat) : this(nom, type, true, false, contentFloat, 0)
        {
            if (type.Type == TypeEnum.Unknown)
                type.Type = TypeEnum.Real;
            if (type.Type != TypeEnum.Real)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        public Variable(string nom, TypeElement type, int contentInt) : this(nom, type, true, false, 0.0f, contentInt)
        {
            if (type.Type == TypeEnum.Unknown)
                type.Type = TypeEnum.Integer;
            if (type.Type != TypeEnum.Integer)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        private Variable(string nom, TypeElement type, bool contentKnown,
            bool contentBool, float contentFloat, int contentInt)
        {
            Nom = nom;
            Type = type;
            ContentKnown = contentKnown;
            _contentBool = contentBool;
            _contentFloat = contentFloat;
            _contentInt = contentInt;
        }

        internal bool SetVariable(Variable var)
        {
            if (var.Type.Type == TypeEnum.Unknown ||
                (var.Type.Type != Type.Type && Type.Type != TypeEnum.Unknown))
                throw new Exception("Error in type");
            
            bool rc = false;

            if (Type.Type == TypeEnum.Unknown)
            {
                rc = true;
                Type.Type = var.Type.Type;
            }

            if (Type.Type == TypeEnum.Boolean && !var._contentBool.Equals(_contentBool))
                rc = true;
            if (Type.Type == TypeEnum.Integer && !var._contentInt.Equals(_contentInt))
                rc = true;
            if (Type.Type == TypeEnum.Real && !var._contentFloat.Equals(_contentFloat))
                rc = true;
            if (!ContentKnown && var.ContentKnown)
                rc = true;

            ContentKnown = var.ContentKnown;
            _contentBool = var._contentBool;
            _contentFloat = var._contentFloat;
            _contentInt = var._contentInt;

            return rc;
        }

        internal void ContentUnknown()
        {
            ContentKnown = false;
        }

        internal bool GetContentBool()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Boolean)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentBool;
        }

        internal int GetContentInt()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Integer)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentInt;
        }

        internal float GetContentReal()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Real)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentFloat;
        }

        public override string ToString()
        {
            return $"Variable {Nom} {{Type: {Type}; Contenu: {(Type.Type != TypeEnum.Unknown ? (ContentKnown ? (Type.Type == TypeEnum.Boolean ? _contentBool.ToString() : (Type.Type == TypeEnum.Integer ? _contentInt.ToString() : (Type.Type == TypeEnum.Real ? _contentFloat.ToString(CultureInfo.CurrentCulture) : "inconnu"))) : "inconnu") : "inconnu")}}}";
        }
    }
}