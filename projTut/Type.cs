using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Type
    {
        /// <summary>
        /// Différents types de données possibles
        /// </summary>
        public enum types
        {
            ENTIER,
            REEL,
            BOOLEAN,
            UNKNOWN
        }
        private types _type; // Type de l'objet

        /// <summary>
        /// Constructeur de recopie
        /// </summary>
        /// <param name="type">Type à recopier</param>
        public Type(Type type) : this(type.type) { }

        /// <summary>
        /// Constructeur par type
        /// </summary>
        /// <param name="type">"types" du Type</param>
        public Type(types type)
        {
            _type = type;
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Type() : this(types.UNKNOWN)
        {

        }

        /// <summary>
        /// Retourne un type primitif par rapport à "types"
        /// </summary>
        /// <param name="type">type à convertir</param>
        /// <returns>Retourne le type correspondant</returns>
        public static System.Type getType(types type)
        {
            switch (type)
            {
                case types.BOOLEAN:
                    return typeof(bool);
                case types.ENTIER:
                    return typeof(int);
                case types.REEL:
                    return typeof(float);
                case types.UNKNOWN:
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Accesseur get/set
        /// </summary>
        public types type {
            get
            {
                return _type;
            }
            set
            {
                if (_type != types.UNKNOWN && _type != value)
                {
                    throw new Exception("Type déjà connu");
                }
                _type = value;
            }
        }

        /// <summary>
        /// Retourne en string la valeur du type
        /// </summary>
        /// <returns>valeur du type</returns>
        public override string ToString()
        {
            switch (type)
            {
                case types.BOOLEAN:
                    return "BOOLEAN";
                case types.ENTIER:
                    return "ENTIER";
                case types.REEL:
                    return "REEL";
                case types.UNKNOWN:
                    return "UNKNOWN";
                default:
                    return "UNKNOWN";
            }
        }

    }
}
