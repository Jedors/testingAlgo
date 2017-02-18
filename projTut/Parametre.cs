namespace projTut
{
    internal class Parametre
    {
        /// <summary>
        /// Les différents types de passage de paramètre dans une fonction (adresse ou valeur)
        /// </summary>
        public enum PassagePar
        {
            Adresse,
            Valeur,
            Unknown
        }

        private readonly PassagePar _passagePar;  // type de passage
        private readonly TypeElement _type;              // type du paramètre
        private readonly string _contenu;         // contenu à faire analyser (peut-être résultat d'opération, ou fonc, var, ...)

        /// <summary>
        /// Constructeur complet par défaut
        /// </summary>
        /// <param name="type">Type du paramètre</param>
        /// <param name="passagePar">Type de passage</param>
        /// <param name="contenu">Contenu à faire analyser</param>
        public Parametre(TypeElement type, PassagePar passagePar,string contenu)
        {
            
            _type = new TypeElement(type); // Instanciation d'une copie -> Ne doit pas être modifiable de l'extérieur
            _passagePar = passagePar;
            _contenu = contenu;

        }

        /// <summary>
        /// Constructeur sans type donné
        /// </summary>
        /// <param name="passagePar"></param>
        /// <param name="contenu"></param>
        public Parametre(PassagePar passagePar, string contenu)
        {

            this.passagePar = passagePar;
            this.contenu = contenu;
            this.type = determineType(contenu);

        }

        private Type determineType(string contenu)
        {

            bool decimalReal = false;
            bool isReal = false;
            bool isInt = true;
            char[] tab_contenu = contenu.ToCharArray();
            List<char> chiffre = new List<char>();
            chiffre.Add('1'); chiffre.Add('2'); chiffre.Add('3'); chiffre.Add('4'); chiffre.Add('5');
            chiffre.Add('6'); chiffre.Add('7'); chiffre.Add('8'); chiffre.Add('9'); chiffre.Add('0');

            foreach (char caract in tab_contenu)
            {
                if (!chiffre.Contains(caract))
                {
                    isInt = false;
                    if (caract == '.' && decimalReal == false)
                    {
                        decimalReal = true;
                        isInt = true;
                        isReal = true;
                    }
                }
            }

            if (isInt)
            {
                if (isReal)
                {
                    return new Type(Type.types.REEL);
                }
                return new Type(Type.types.ENTIER);
            }
            else if (contenu.ToLower() == "true" || contenu.ToLower() == "false")
            {
                return new Type(Type.types.BOOLEAN);
            }
            else
            {
                return new Type(Type.types.UNKNOWN);
            }
            


        }

        /// <summary>
        /// Surcharge de ToString() pour l'affichage d'un paramètre
        /// </summary>
        /// <returns>Contenu du paramètre formaté</returns>
        public override string ToString()
        {
            return "\nContenu : " + _contenu + "\ntype : " + _type + "\nPassé par :" +  _passagePar;
        }
    }
}
