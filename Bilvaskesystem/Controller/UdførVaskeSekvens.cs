using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Interfaces;
using static Bilvaskesystem.Interfaces.ValgAfVaskEventArgs;

namespace Bilvaskesystem.Controller
{
    public class VaskeSekvens : IVaskeSekvens
    {
        public event EventHandler<VaskAfsluttetEventArgs> VaskAfsluttetEvent;


        public void Start(int vasktype)
        {
        }

        protected virtual void VaskAfsluttet(VaskAfsluttetEventArgs e)
        {
            VaskAfsluttetEvent?.Invoke(this, e);
        }
    }
}
