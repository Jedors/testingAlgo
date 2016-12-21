using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Parametre
    {
        /// <summary>
        /// Les différents types de passage de paramètre dans une fonction (adresse ou valeur)
        /// </summary>
        public enum PassagePar
        {
            ADRESSE,
            VALEUR,
            UNKNOWN
        }

        private PassagePar passagePar;  // type de passage
        private Type type;              // type du paramètre
        private string contenu;         // contenu à faire analyser (peut-être résultat d'opération, ou fonc, var, ...)

        /// <summary>
        /// Constructeur complet par défaut
        /// </summary>
        /// <param name="type">Type du paramètre</param>
        /// <param name="passagePar">Type de passage</param>
        /// <param name="contenu">Contenu à faire analyser</param>
        public Parametre(Type type, PassagePar passagePar,string contenu)
        {
            
            this.type = new Type(type); // Instanciation d'une copie -> Ne doit pas être modifiable de l'extérieur
            this.passagePar = passagePar;
            this.contenu = contenu;

        }

        /// <summary>
        /// Surcharge de ToString() pour l'affichage d'un paramètre
        /// </summary>
        /// <returns>Contenu du paramètre formaté</returns>
        public override string ToString()
        {
            return "\nContenu : " + contenu + "\ntype : " + type + "\nPassé par :" +  passagePar;
        }
    }
}
