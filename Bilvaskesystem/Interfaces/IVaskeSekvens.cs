using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilvaskesystem.Interfaces
{
    public class VaskAfsluttetEventArgs : EventArgs
    {
    }
    public interface IVaskeSekvens
    {
        void Start(int vasktype);
        event EventHandler<VaskAfsluttetEventArgs> VaskAfsluttetEvent;
    }
}
