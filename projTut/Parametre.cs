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
        /// Surcharge de ToString() pour l'affichage d'un paramètre
        /// </summary>
        /// <returns>Contenu du paramètre formaté</returns>
        public override string ToString()
        {
            return "\nContenu : " + _contenu + "\ntype : " + _type + "\nPassé par :" +  _passagePar;
        }
    }
}
