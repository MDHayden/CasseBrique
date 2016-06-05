using System;

namespace CasseBrique
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (CasseBrique game = new CasseBrique())
            {
                game.Run();
            }
        }
    }
#endif
}

