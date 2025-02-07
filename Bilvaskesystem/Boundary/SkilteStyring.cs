using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Interfaces;

namespace Bilvaskesystem.Boundary
{
    public class SkilteStyring : ISkilteStyring
    {
        public void turnOffSkilte()
        {
            Console.Clear();
        }
        public void KoerTilbage()
        {
            turnOffSkilte();
            Console.WriteLine($"Kør Tilbage");
        }

        public void KoerFrem()
        {
            turnOffSkilte();
            Console.WriteLine($"Kør Frem");
        }

        public void Stop()
        {
            turnOffSkilte();
            Console.WriteLine($"Stop");
        }


    }
}
