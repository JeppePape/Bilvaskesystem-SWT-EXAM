using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Interfaces;

namespace Bilvaskesystem.Boundary
{
    public class AfstandsSensor : IAfstandsSensor
    {
        public event EventHandler<AfstandChangedEventArgs> NyAfstandEvent;
        private int _GammelAfstand;
        public void NyAfstand(int nyAfstand)
        {
            if (nyAfstand != _GammelAfstand && nyAfstand < 1000)
            {
                AfstandChanged(new AfstandChangedEventArgs { Afstand = nyAfstand });
                _GammelAfstand = nyAfstand;
            }
            if (nyAfstand == -1337)
            {
                AfstandChanged(new AfstandChangedEventArgs { Afstand = 1000 });
                _GammelAfstand = nyAfstand;
            }
        }



        protected virtual void AfstandChanged(AfstandChangedEventArgs e)
        {
            NyAfstandEvent?.Invoke(this, e);
        }
    }
}
