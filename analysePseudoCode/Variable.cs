using System;
using System.Globalization;

namespace analysePseudoCode
{
    /// <summary>
    /// Variable
    /// </summary>
    internal class Variable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Type of the variable
        /// </summary>
        public TypeElement Type { get; }
        /// <summary>
        /// Content if type is boolean
        /// </summary>
        private bool _contentBool;
        /// <summary>
        /// Content if type is float
        /// </summary>
        private float _contentFloat;
        /// <summary>
        /// Content if type is integer
        /// </summary>
        private int _contentInt;
        /// <summary>
        /// Define if the content of the variable is known
        /// </summary>
        internal bool ContentKnown { get; private set; }

        /// <summary>
        /// Constructor of the variable if only having a type
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Variable type</param>
        public Variable(string name, TypeElement type) : this(name, type, false, false,  0.0f, 0) { }

        /// <summary>
        /// Constructor of a boolean variable by having content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="contentBool">Boolean content</param>
        public Variable(string name, bool contentBool) : this(name, new TypeElement(TypeEnum.Boolean), contentBool) { }

        /// <summary>
        /// Constructor of a real variable by having content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="contentFloat">Float content</param>
        public Variable(string name, float contentFloat) : this(name, new TypeElement(TypeEnum.Real), contentFloat) { }

        /// <summary>
        /// Constructor of an integer variable by having content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="contentInt">Integer content</param>
        public Variable(string name, int contentInt) : this(name, new TypeElement(TypeEnum.Integer), contentInt) { }

        /// <summary>
        /// Constructor of a variable with type already existing and boolean content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="contentBool">Boolean content</param>
        public Variable(string name, TypeElement type, bool contentBool) : this(name, type, true, contentBool, 0.0f, 0)
        {
            if (type.Type == TypeEnum.Unknown)
                try
                {
                    type.Type = TypeEnum.Boolean;
                }
                catch (ArgumentException e)
                {
                    Program.PrintError(e.Message);
                }
            if (type.Type != TypeEnum.Boolean)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        /// <summary>
        /// Constructor of a variable with type already existing and float content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="contentFloat">Float content</param>
        public Variable(string name, TypeElement type, float contentFloat) : this(name, type, true, false, contentFloat, 0)
        {
            if (type.Type == TypeEnum.Unknown)
                try
                {
                    type.Type = TypeEnum.Real;
                }
                catch (ArgumentException e)
                {
                    Program.PrintError(e.Message);
                }
            if (type.Type != TypeEnum.Real)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        /// <summary>
        /// Constructor of a variable with type already existing and integer content
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="contentInt">Integer content</param>
        public Variable(string name, TypeElement type, int contentInt) : this(name, type, true, false, 0.0f, contentInt)
        {
            if (type.Type == TypeEnum.Unknown)
                try
                {
                    type.Type = TypeEnum.Integer;
                }
                catch (ArgumentException e)
                {
                    Program.PrintError(e.Message);
                }
            if (type.Type != TypeEnum.Integer)
            {
                Console.Error.WriteLine("Error: Variable type different from content type.");
                throw new Exception("Type Error from variable to content");
            }
        }

        /// <summary>
        /// Full constructor
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="contentKnown">f the content is known</param>
        /// <param name="contentBool">Boolean content</param>
        /// <param name="contentFloat">Float content</param>
        /// <param name="contentInt">Integer content</param>
        private Variable(string name, TypeElement type, bool contentKnown,
            bool contentBool, float contentFloat, int contentInt)
        {
            Name = name;
            Type = type;
            ContentKnown = contentKnown;
            _contentBool = contentBool;
            _contentFloat = contentFloat;
            _contentInt = contentInt;
        }

        /// <summary>
        /// Define content or type of a variable
        /// </summary>
        /// <param name="var">variable to attribute</param>
        /// <returns>If updated or not changed</returns>
        internal bool SetVariable(Variable var)
        {
            if (var.Type.Type == TypeEnum.Unknown ||
                (var.Type.Type != Type.Type && Type.Type != TypeEnum.Unknown))
                throw new Exception("Error in type");
            
            bool rc = false;

            if (Type.Type == TypeEnum.Unknown)
            {
                rc = true;
                try
                {
                    Type.Type = var.Type.Type;
                }
                catch (ArgumentException e)
                {
                    Program.PrintError(e.Message);
                }
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

        /// <summary>
        /// Set the content of the variable unknown
        /// </summary>
        internal void ContentUnknown()
        {
            ContentKnown = false;
        }

        /// <summary>
        /// Get the content of the variable if type is boolean
        /// </summary>
        /// <returns>Boolean content</returns>
        internal bool GetContentBool()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Boolean)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentBool;
        }

        /// <summary>
        /// Get the content of the variable if type is integer
        /// </summary>
        /// <returns>Integer content</returns>
        internal int GetContentInt()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Integer)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentInt;
        }

        /// <summary>
        /// Get the content of the variable if type is float
        /// </summary>
        /// <returns>Float content</returns>
        internal float GetContentReal()
        {
            if (!ContentKnown || Type.Type != TypeEnum.Real)
            {
                Console.Error.WriteLine("Error: Not concording type, or unknown content.");
                throw new Exception("Not the good type of content or content unknown.");
            }

            return _contentFloat;
        }

        /// <summary>
        /// Basic ToString, simply format properly the thing
        /// </summary>
        /// <returns>Beautiful formating of the variable</returns>
        public override string ToString()
        {
            return $"Variable {Name} {{Type: {Type}; Contenu: {(Type.Type != TypeEnum.Unknown ? (ContentKnown ? (Type.Type == TypeEnum.Boolean ? _contentBool.ToString() : (Type.Type == TypeEnum.Integer ? _contentInt.ToString() : (Type.Type == TypeEnum.Real ? _contentFloat.ToString(CultureInfo.CurrentCulture) : "inconnu"))) : "inconnu") : "inconnu")}}}";
        }
    }
}