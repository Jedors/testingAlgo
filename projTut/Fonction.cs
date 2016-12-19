using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Fonction : Procedure
    {
        private Variable retour;

        public Fonction(List<Parametre> listeParametre, List<Instruction> listeInstruction, string nom, Variable retour)
            :base(listeParametre, listeInstruction, nom)
        {
            this.retour = new Variable(retour);
        }
    }
}
