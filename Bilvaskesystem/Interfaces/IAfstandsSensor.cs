using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilvaskesystem.Interfaces
{
    public class AfstandChangedEventArgs : EventArgs
    {
        public int Afstand { get; set; }
    }
    public interface IAfstandsSensor
    {
        event EventHandler<AfstandChangedEventArgs> NyAfstandEvent;
    }
}
