using System;

namespace analysePseudoCode
{
    internal class TypeElement
    {
        private TypeEnum _type;
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

        public TypeElement() : this(TypeEnum.Unknown) { }

        public TypeElement(TypeEnum type)
        {
            _type = type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}