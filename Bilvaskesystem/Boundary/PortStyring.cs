using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Interfaces;

namespace Bilvaskesystem.Boundary
{
    public class PortStyring : IPortStyring
    {
        public void LukPort()
        {
            Console.WriteLine($"Port Lukket");
        }

        public void AabenPort()
        {
            Console.WriteLine($"Port Aaben");
        }

    }
}
