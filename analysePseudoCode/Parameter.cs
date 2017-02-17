using System;

namespace analysePseudoCode
{
    internal class Parameter
    {
        internal TypeElement TypeParam { get; }
        private TypePassage _typePass;


        internal TypePassage TypePass
        {
            get { return _typePass; }
            set
            {
                if (_typePass != TypePassage.Unknown && _typePass != value)
                {
                    Console.Error.WriteLine("Error: Passage parameter type different from the one already known");
                    throw new ArgumentException("Passage parameter type different from the one already known");
                }
                _typePass = value;
            }
        }

        internal Parameter(TypeElement type) : this(type, TypePassage.Unknown) { }

        internal Parameter(TypeElement type, TypePassage passage)
        {
            TypeParam = type;
            _typePass = passage;
        }

        public override string ToString()
        {
            return $"{{Param: {{Type:{TypeParam}, Passage:{TypePass}}}}}";
        }
    }
}