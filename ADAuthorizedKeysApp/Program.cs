using System;
using System.DirectoryServices;

namespace ADAuthorizedKeysApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            var username = args[0];
            var splittedUsername = username.Split('\\');
            if (splittedUsername.Length > 1)
                username = splittedUsername[1];

            splittedUsername = username.Split('@');
            if (splittedUsername.Length > 1)
                username = splittedUsername[0];

            var searcher = new DirectorySearcher();
            searcher.Filter = $"(&(objectClass=user)(sAMAccountName={username}))";
            searcher.PropertiesToLoad.Add("sshPublicKeys");

            var result = searcher.FindOne();
            if (result != null)
            {
                foreach (string sshPublicKey in result.Properties["sshPublicKeys"])
                {
                    Console.Write(sshPublicKey);
                }
            }
        }
    }
}
