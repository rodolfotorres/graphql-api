using GraphQLApi.Schema;
using System;
using System.Linq;

namespace graphql_api.console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-schema"))
            {
                Console.WriteLine(new TeamSchema().Describe());
                return;
            }
        }
    }
}
