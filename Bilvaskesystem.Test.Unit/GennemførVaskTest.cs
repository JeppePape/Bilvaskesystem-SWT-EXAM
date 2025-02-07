using Bilvaskesystem.Boundary;
using Bilvaskesystem.Controller;
using Bilvaskesystem.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Bilvaskesystem.Test.Unit
{
    public class Gennemf�rVaskTest
    {
        private GennemfoerVask _uut;
        private IUserInterface _userInterface;
        private IVaskeSekvens _vaskeSekvens;
        private IAfstandsSensor _afstandsSensor;
        private IPortStyring _PortStyring;
        private ISkilteStyring _SkilteStyring;

        [SetUp]
        public void Setup()
        {
            _afstandsSensor = Substitute.For<IAfstandsSensor>();
            _userInterface = Substitute.For<IUserInterface>();
            _vaskeSekvens = Substitute.For<IVaskeSekvens>();
            _PortStyring = Substitute.For<IPortStyring>();
            _SkilteStyring = Substitute.For<ISkilteStyring>();
            _uut = new GennemfoerVask(_afstandsSensor, _userInterface, _vaskeSekvens, _PortStyring, _SkilteStyring);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(50)]
        [TestCase(148)]
        [TestCase(149)]
        public void TestTilstandLedigK�rTilbage(int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            //Assert
            _SkilteStyring.Received().KoerTilbage();
            _SkilteStyring.DidNotReceive().KoerFrem();
            _SkilteStyring.DidNotReceive().Stop();

        }

        [TestCase(201)]
        [TestCase(202)]
        [TestCase(500)]
        [TestCase(999)]
        [TestCase(1000)]
        public void TestTilstandLedigK�rFrem(int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            //Assert
            _SkilteStyring.Received().KoerFrem();
            _SkilteStyring.DidNotReceive().KoerTilbage();
            _SkilteStyring.DidNotReceive().Stop();
        }
        [TestCase(150)]
        [TestCase(151)]
        [TestCase(175)]
        [TestCase(199)]
        [TestCase(200)]
        public void TestTilstandLedigStop(int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            //Assert
            //_SkilteStyring.Received().Stop();
            //_SkilteStyring.DidNotReceive().KoerTilbage();
            //_SkilteStyring.DidNotReceive().KoerFrem();
            var state = _uut.GetCurrentState();
            Assert.That(state == "KlarTilVask");

        }

        [TestCase(1, 150, 0)]
        [TestCase(2, 151, 1)]
        [TestCase(3, 175, 500)]
        [TestCase(4, 199, 998)]
        [TestCase(5, 200, 999)]
        public void TestTilstandLedigTilUdk�rselK�rTilbage(int Type, int Afstand, int Udk�rselsAfstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() {Afstand = Afstand});
            _userInterface.VaskValgtEvent += Raise.EventWith(new ValgAfVaskEventArgs() {VaskType = Type});
            _vaskeSekvens.VaskAfsluttetEvent += Raise.EventWith(new VaskAfsluttetEventArgs() { });
            _afstandsSensor.NyAfstandEvent +=
                Raise.EventWith(new AfstandChangedEventArgs() {Afstand = Udk�rselsAfstand});

            //Assert
            _PortStyring.Received().AabenPort();
            _SkilteStyring.Received(1).KoerTilbage();
            _SkilteStyring.DidNotReceive().KoerFrem();
            _SkilteStyring.Received(1).Stop();
        }

        [TestCase(1, 150, 1000)]

        public void TestTilstandLedigTilUdk�rselTilLedig(int Type, int Afstand, int Udk�rselsAfstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = Afstand });
            _userInterface.VaskValgtEvent += Raise.EventWith(new ValgAfVaskEventArgs() { VaskType = Type });
            _vaskeSekvens.VaskAfsluttetEvent += Raise.EventWith(new VaskAfsluttetEventArgs() { });
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = Udk�rselsAfstand });

            //Assert
            _PortStyring.Received().AabenPort();
            _SkilteStyring.Received(2).KoerTilbage();
            _SkilteStyring.Received(1).KoerFrem();
            _SkilteStyring.Received(1).Stop();
        }

        [TestCase(1, 150)]
        [TestCase(2, 151)]
        [TestCase(3, 175)]
        [TestCase(4, 199)]
        [TestCase(5, 200)]
        public void TestTilstandLedigTilKlarTilVaskOgVask(int Type, int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            _userInterface.VaskValgtEvent += Raise.EventWith(new ValgAfVaskEventArgs() { VaskType = Type });


            //Assert
            _PortStyring.Received().LukPort();
            _SkilteStyring.DidNotReceive().KoerTilbage();
            _SkilteStyring.DidNotReceive().KoerFrem();
            _SkilteStyring.Received(1).Stop();
        }

        [TestCase(1, 150)]
        [TestCase(2, 151)]
        [TestCase(3, 175)]
        [TestCase(4, 199)]
        [TestCase(5, 200)]
        public void TestTilstandLedigTilUdkoersel(int Type, int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            _userInterface.VaskValgtEvent += Raise.EventWith(new ValgAfVaskEventArgs() { VaskType = Type });
            _vaskeSekvens.VaskAfsluttetEvent += Raise.EventWith(new VaskAfsluttetEventArgs() { });

            //Assert
            _PortStyring.Received().AabenPort();
            _SkilteStyring.Received(1).Stop();
            _SkilteStyring.Received(1).KoerTilbage();
            _SkilteStyring.DidNotReceive().KoerFrem();
        }

        [TestCase(-1337)]
        public void TestAfstandUdenforR�kkeViddeRigtigV�rdi(int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            //Assert
            _SkilteStyring.Received().KoerFrem();
            _SkilteStyring.DidNotReceive().KoerTilbage();
            _SkilteStyring.DidNotReceive().Stop();
        }

        [TestCase(-1336)]
        [TestCase(-1335)]
        [TestCase(-13)]
        [TestCase(10000)]
        public void TestAfstandUdenforR�kkeViddeForkertV�rdi(int afstand)
        {
            //Arrange
            //Act
            _afstandsSensor.NyAfstandEvent += Raise.EventWith(new AfstandChangedEventArgs() { Afstand = afstand });
            //Assert
            _SkilteStyring.DidNotReceive().KoerFrem();
            _SkilteStyring.DidNotReceive().KoerTilbage();
            _SkilteStyring.DidNotReceive().Stop();
        }


    }
}