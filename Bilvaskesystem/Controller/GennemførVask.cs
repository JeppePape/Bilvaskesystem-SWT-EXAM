using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilvaskesystem.Boundary;
using Bilvaskesystem.Interfaces;

namespace Bilvaskesystem.Controller
{
    public class GennemfoerVask
    {
        private enum VaskeState
        {
            Ledig,
            KlarTilVask,
            Vask,
            Udkoersel
        };

        private VaskeState state;

        public string GetCurrentState()
        {
            switch (state)
            {
                case VaskeState.Ledig:
                    return "Ledig";
                case VaskeState.KlarTilVask:
                    return "KlarTilVask";
                case VaskeState.Vask:
                    return "Vask";
                case VaskeState.Udkoersel:
                    return "Udkoersel";
                default:
                    return "Unknown";
            }
        }

        // Her mangler medlemsdata/fields/properties
        private IAfstandsSensor _afstandsSensor;
        private IVaskeSekvens vaskeSekvens;
        private IUserInterface _userInterface;
        private ISkilteStyring skilte;
        private IPortStyring port;

        // Her mangler constructor med initialisering osv.
        public GennemfoerVask(IAfstandsSensor afstandssensor,IUserInterface userInterface,
            IVaskeSekvens vaskesekvens,IPortStyring portStyring, ISkilteStyring skilteStyring)
        {
            _afstandsSensor = afstandssensor;
            _userInterface = userInterface;
            vaskeSekvens = vaskesekvens;
            port = portStyring;
            skilte = skilteStyring;

            userInterface.VaskValgtEvent += HandleValgAfVask;
            vaskeSekvens.VaskAfsluttetEvent += HandleVaskAfsluttet;
            _afstandsSensor.NyAfstandEvent += HandleNyAfstand;

            state = VaskeState.Ledig;
        }

        // Her er to eventhandlere
        // Handler for valg af vask

        private void HandleValgAfVask(object source, ValgAfVaskEventArgs e)
        {
            switch (state)
            {
                // Kun i denne tilstand skal der reageres
                case VaskeState.KlarTilVask:
                    port.LukPort();
                    vaskeSekvens.Start(e.VaskType);
                    state = VaskeState.Vask;
                    break;
            }
        }

        // Handler for vask afsluttet
        private void HandleVaskAfsluttet(object source, VaskAfsluttetEventArgs e)
        {
            switch (state)
            {
                // Kun i denne tilstand skal der reageres
                case VaskeState.Vask:
                    port.AabenPort();
                    skilte.KoerTilbage();
                    state = VaskeState.Udkoersel;
                    break;
            }
        }

        // Her mangler mindst een handler, ud fra dit eget design

        private void HandleNyAfstand(object source, AfstandChangedEventArgs e)
        {
            switch (state)
            {
                // Kun i denne tilstand skal der reageres
                case VaskeState.Ledig:
                    if (e.Afstand >= 0 && e.Afstand < 150)
                    {
                        skilte.KoerTilbage();
                    }
                    if (e.Afstand >= 150 && e.Afstand <= 200)
                    {
                        skilte.Stop();
                        state = VaskeState.KlarTilVask;
                        break;
                    }
                    if (e.Afstand > 200 && e.Afstand <= 1000)
                    {
                        skilte.KoerFrem();
                    }
                    if (e.Afstand == -1337)
                    {
                        skilte.KoerFrem();
                    }
                    break;

                case VaskeState.Udkoersel:
                    if (e.Afstand >= 0 && e.Afstand > 999)
                    {
                        skilte.KoerTilbage();
                    }

                    if (e.Afstand >= 1000)
                    {
                        skilte.KoerFrem();
                        state = VaskeState.Ledig;
                    }

                    break;
            }

        }
    }
}
