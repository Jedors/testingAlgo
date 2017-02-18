using System;

namespace analysePseudoCode
{
    /// <summary>
    /// TypeEnum holder, to avoid bad usage
    /// </summary>
    internal class TypeElement
    {
        /// <summary>
        /// Type, private field
        /// </summary>
        private TypeEnum _type;
        /// <summary>
        /// Type getter/setter, set check if not different if already known
        /// </summary>
        internal TypeEnum Type
        {
            get { return _type; }
            set
            {
                if (_type != TypeEnum.Unknown && _type != value)
                {
                    Console.Error.WriteLine("Error: Type element different from type assignation");
                    throw new ArgumentException("Type element different from value");
                }
                _type = value;
            }
        }

        /// <summary>
        /// Type default constructor, if type is unknown
        /// </summary>
        public TypeElement() : this(TypeEnum.Unknown) { }

        /// <summary>
        /// Constructor with type known
        /// </summary>
        /// <param name="type">Type</param>
        public TypeElement(TypeEnum type)
        {
            _type = type;
        }

        /// <summary>
        /// Basic ToString, simply format properly the thing
        /// </summary>
        /// <returns>Beautiful formating of the type</returns>
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}