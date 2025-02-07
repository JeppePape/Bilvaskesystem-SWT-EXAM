using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Interfaces;

namespace Bilvaskesystem.Boundary
{
    public class UserInterface : IUserInterface
    {


        public event EventHandler<ValgAfVaskEventArgs> VaskValgtEvent;

        public void VælgVask(int vaskType)
        {
            VaskValgt(new ValgAfVaskEventArgs { VaskType = vaskType });
        }

        public void VaskValgt(ValgAfVaskEventArgs e)
        {
            VaskValgtEvent?.Invoke(this, e);
        }
    }

}


