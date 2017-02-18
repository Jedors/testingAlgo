using System;

namespace projTut
{
    internal class TypeElement
    {
        #region Attributues
        private TypeEnum _type; // Type of the object
        #endregion

        #region Constructors

        /// <summary>
        /// Cloning constructor
        /// </summary>
        /// <param name="type">Type à recopier</param>
        public TypeElement(TypeElement type) : this(type.Type)
        {
            
        }
        
        /// <summary>
        /// Constructor by type
        /// </summary>
        /// <param name="type">TypeEnum of the Type</param>
        public TypeElement(TypeEnum type)
        {
            _type = type;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeElement() : this(TypeEnum.Unknown)
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// Property get/set
        /// </summary>
        public TypeEnum Type {
            get
            {
                return _type;
            }
            set
            {
                if (_type != TypeEnum.Unknown && _type != value)
                    throw new Exception("Type déjà connu");
                _type = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return a primitive type corresponding to the type
        /// </summary>
        /// <param name="type">type to convert</param>
        /// <returns>Return the corresponding type</returns>
        public static Type GetType(TypeEnum type)
        {
            switch (type)
            {
                case TypeEnum.Boolean:
                    return typeof(bool);
                case TypeEnum.Entier:
                    return typeof(int);
                case TypeEnum.Reel:
                    return typeof(float);
                case TypeEnum.Unknown:
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Return as a string the type
        /// </summary>
        /// <returns>value of the type</returns>
        public override string ToString()
        {
            return Type.ToString();
        }
        #endregion
    }
}
