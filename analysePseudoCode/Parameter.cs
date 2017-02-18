using System;

namespace analysePseudoCode
{
    /// <summary>
    /// Parameter of a function or a procedure
    /// </summary>
    internal class Parameter
    {
        /// <summary>
        /// Parameter type, with public getter
        /// </summary>
        internal TypeElement TypeParam { get; }
        /// <summary>
        /// If it is passed by adress or value
        /// </summary>
        private TypePassage _typePass;

        /// <summary>
        /// Getter, and setter to check if not weird content
        /// </summary>
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

        /// <summary>
        /// Full constructor of parameter
        /// </summary>
        /// <param name="type">Type of the parameter</param>
        /// <param name="passage">Type of passage</param>
        internal Parameter(TypeElement type, TypePassage passage)
        {
            TypeParam = type;
            _typePass = passage;
        }

        /// <summary>
        /// Basic ToString, simply format properly the thing
        /// </summary>
        /// <returns>Beautiful formating of the parameter</returns>
        public override string ToString()
        {
            return $"{{Param: {{Type:{TypeParam}, Passage:{TypePass}}}}}";
        }

        public string ToString(int i)
        {
            return $"{{Param{i}: {{Type:{TypeParam}, Passage:{TypePass}}}}}";
        }
    }
}