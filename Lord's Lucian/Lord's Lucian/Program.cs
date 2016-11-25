using System;

namespace Lord_s_Lucian
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Lucian();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not load the assembly - {0}", exception);
                throw;
            }
        }
    }
}