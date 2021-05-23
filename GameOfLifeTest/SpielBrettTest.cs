using NUnit.Framework;
using GameOfLife;
using GameOfLife.Models;
using System.Linq;

namespace GameOfLifeTest
{
    public class SpielBrettTest
    {
        [Test]
        public void Konstruktor()
        {
            var breite = 10;
            var hoehe = 10;
            var pixelAnzahlProZelle = 5;
            var spielBrett = new SpielBrett(breite, hoehe, pixelAnzahlProZelle);
            Assert.AreEqual(breite, spielBrett.Breite );
            Assert.AreEqual(hoehe, spielBrett.Hoehe );
            Assert.AreEqual(pixelAnzahlProZelle, spielBrett.PixelAnzahlProZelle );

            Assert.AreEqual(breite * hoehe, spielBrett.Zellen.Count );
        }

        [Test]
        public void ZellenZufälligEinfügen()
        {
            var breite = 10;
            var hoehe = 10;
            var pixelAnzahlProZelle = 5;
            var spielBrett = new SpielBrett(breite, hoehe, pixelAnzahlProZelle);
            spielBrett.ZellenZufälligEinfügen();
            var livingCells = spielBrett.Zellen.Where( s => s.Lebt ).Count();
            Assert.Greater( livingCells, 10 );
        }

        [Test]
        public void ToggleCell()
        {
            var breite = 10;
            var hoehe = 10;
            var pixelAnzahlProZelle = 5;
            var spielBrett = new SpielBrett(breite, hoehe, pixelAnzahlProZelle);
            bool initialState = spielBrett[new( 1, 1 )].Lebt;
            spielBrett.AenderZelle( new( 1, 1 ) );
            bool toggledState = spielBrett[new( 1, 1 )].Lebt;

            Assert.AreEqual( initialState, toggledState );
        }

        [Test]
        public void NaechsteZug()
        {
            var breite = 10;
            var hoehe = 10;
            var pixelAnzahlProZelle = 5;
            SpielBrett spielBrett = new SpielBrett(breite, hoehe, pixelAnzahlProZelle);
            spielBrett.ZellenZufälligEinfügen();

            spielBrett.NaechsterZug();
            SpielBrett spielBrett2 = new SpielBrett(breite, hoehe, pixelAnzahlProZelle);
            Assert.AreNotEqual(spielBrett, spielBrett2);
        }
    }
}