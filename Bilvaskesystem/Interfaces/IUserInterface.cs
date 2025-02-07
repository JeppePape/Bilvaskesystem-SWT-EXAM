using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilvaskesystem.Interfaces
{
    public class ValgAfVaskEventArgs : EventArgs
    {
        public int VaskType { get; set; }
    }
    public interface IUserInterface
    {
        event EventHandler<ValgAfVaskEventArgs> VaskValgtEvent;
    }
}
