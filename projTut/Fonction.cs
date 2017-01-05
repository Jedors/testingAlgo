using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projTut
{
    class Fonction : Procedure
    {
        /// <summary>
        /// Une fonction est une procédure ou la seul différence est qu'elle a une sortie
        /// </summary>
        private Variable retour;

        /// <summary>
        /// Constructeur par défaut, reprise du constructeur de procédure avec ajout de variable de retour
        /// </summary>
        /// <param name="listeParametre">liste param lors de l'appel</param>
        /// <param name="listeInstruction">liste instruction executé dans fonction</param>
        /// <param name="nom">nom de la fonction</param>
        /// <param name="retour">variable de retour</param>
        public Fonction(List<Parametre> listeParametre, List<Instruction> listeInstruction, string nom, Variable retour)
            :base(listeParametre, listeInstruction, nom)
        {
            this.retour = new Variable(retour);
        }

        public Fonction(List<Parametre> listeParametre, string nom, Variable retour)
    : base(listeParametre, nom)
        {
            this.retour = new Variable(retour);
        }

        public Fonction(string line)
            :base(line)
        {
            
        }

       



        public override string ToString()
        {
 
            return Regex.Replace(base.ToString(), "procedure", "fonction") + "blabla";
        }

    }
}
