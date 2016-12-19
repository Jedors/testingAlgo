using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Procedure
    {
        protected List<Parametre> listeParametre;
        protected List<Instruction> listeInstruction;
        protected string nom;

        public Procedure(List<Parametre> listeParametre, List<Instruction> listeInstruction, string nom)
        {
            this.listeParametre = new List<Parametre>(listeParametre);
            this.listeInstruction = new List<Instruction>(listeInstruction);
            this.nom = nom;
        }

        public Procedure(List<Parametre> listeParametre, string nom) : this(listeParametre, new List<Instruction>(), nom)
        {

        }

        public void insertInstruction(Instruction instruction)
        {
            listeInstruction.Add(new Instruction(instruction));
        }
    }
}
