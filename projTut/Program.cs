using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace projTut
{
    class Program
    {
        static int Main(string[] args)
        {
            string fileName = "", appName = Properties.Resources.appName;

            if(args.Length != 1)
            {
                Console.Error.WriteLine("Un et un seul argument requis");
                Console.Error.WriteLine("Consultez {0} --help", appName);
                return -1;
            }

            fileName = args[0];
            string[] pseudocode;

            if (!File.Exists(fileName))
            {
                Console.Error.WriteLine("{0}: Fichier inexistant", fileName);
                return -1;
            }
            else
            {
                FileIOPermission f = null;

                try
                {
                    f = new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(fileName));
                    f.Demand();
                    pseudocode = File.ReadAllLines(fileName);
                }
                catch (SecurityException e)
                {
                    Console.Error.WriteLine("{0}: {1}", appName, e.Message);
                    return -1;
                }
            }

            for (int i = 0; i < pseudocode.Length; i++)
            {
                pseudocode[i] = pseudocode[i].ToLower();
                pseudocode[i] = Regex.Replace(pseudocode[i], @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();
                pseudocode[i] = pseudocode[i].Replace(" ", string.Empty);
            }

            Parser pars = new Parser(pseudocode);
            if (!pars.isValid())
            {
                Console.Error.WriteLine("Analyse abbandonnée");
                Console.ReadKey();
                return -1;
            }

            Console.WriteLine(args[0]);
            foreach (string line in pseudocode)
            {
                Console.WriteLine(line);
            }

            Console.ReadKey();
            Console.WriteLine();
            return 0;
        }
    }
}
