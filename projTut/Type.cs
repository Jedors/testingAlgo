using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Type
    {
        public enum types
        {
            ENTIER,
            REEL,
            BOOLEAN,
            UNKNOWN
        }
        private types _type;

        public Type(Type type) : this(type.type) { }

        public Type(types type)
        {
            _type = type;
        }

        public Type() : this(types.UNKNOWN)
        {

        }

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

    }
}
