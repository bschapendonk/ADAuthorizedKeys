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
            var split = username.Split('\\');
            if (split.Length > 1)
                username = split[1];

            split = username.Split('@');
            if (split.Length > 1)
                username = split[0];

            // https://docs.microsoft.com/nl-nl/windows/win32/adschema/a-samaccountname
            // SAM-Account-Name attribute
            // This attribute must be 20 characters or less to support earlier clients, and cannot contain any of these characters:
            //  "/ \ [ ] : ; | = , + * ? < >

            if (username.Length > 20)
                return;

            if (username.IndexOfAny(new char[] { '/', '\\', '[', ']', ':', ';', '|', '=', ',', '+', '*', '?', '<', '>' }) != -1)
                return;

            using (var searcher = new DirectorySearcher())
            {
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
}
