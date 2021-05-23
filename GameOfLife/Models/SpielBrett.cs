using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace GameOfLife.Models
{
    public class SpielBrett
    {
        /// <summary>
        /// Das Spielbrett als Koordinaten System in einem 2 Dimensionalen Array
        /// </summary>
        private readonly Zelle[,] Zellenarr;
        public ObservableCollection<Zelle> Zellen { get; }
        /// <summary>
        /// Breite des Spielbrettes
        /// </summary>
        public int Breite { get; }
        /// <summary>
        /// Höhe des Spielbrettes
        /// </summary>
        public int Hoehe { get; }
        /// <summary>
        /// Pixel anzahl Pro Zelle
        /// </summary>
        public int PixelAnzahlProZelle { get; }
        /// <summary>
        /// Gibt die Zelle an dem Angegebenen Punkt zurück
        /// </summary>
        /// <param name="point">Koodrinaten der Zelle</param>
        /// <returns></returns>
        public Zelle this[Point point] {
            get 
            { 
                return Zellenarr[point.X, point.Y]; 
            }
        }

        /// <summary>
        /// Der Konstruktor vom Spielbrett der auch Gleichzeitig die Zellen Initialisiert
        /// </summary>
        /// <param name="breite">Breite des Spielbrettes</param>
        /// <param name="hoehe">Höhe des Spielbrettes</param>
        /// <param name="pixelAnzahlProZelle">Pixel anzahl Pro Zelle</param>
        public SpielBrett( int breite, int hoehe, int pixelAnzahlProZelle)
        {
            this.Breite = breite;
            this.Hoehe = hoehe;
            this.PixelAnzahlProZelle = pixelAnzahlProZelle;
            this.Zellenarr = new Zelle[this.Breite, this.Hoehe];
            InitialisiereZellen();
            this.Zellen = new ObservableCollection<Zelle>( Zellenarr.OfType<Zelle>() );
        }



        /// <summary>
        /// Ändert den Status Der Genau an dem Punkt liegt
        /// </summary>
        /// <param name="point">Koordinaten der Zelle von der der Status geändert werden soll</param>
        public void AenderZelle(Point point)
        { 
            this[point].AenderStatus(); 
        }
        /// <summary>
        /// Initialisiert die Zellen
        /// </summary>
        private void InitialisiereZellen()
        {
            for ( int x = 0; x < Breite; x++ )
                for ( int y = 0; y < Hoehe; y++ )
                    Zellenarr[x, y] = new Zelle( new Point( x, y ), Breite, Hoehe, PixelAnzahlProZelle);
        }


        /// <summary>
        /// Berechnet die Nächste Generation der Zellen und wendet Sie danach an
        /// </summary>
        public void NaechsterZug()
        {
            NaechsteGeneration();
            BenutzteNaechsteGeneration();
        }
        /// <summary>
        /// Sammelt Die Information der Nächste Generation aus den Zellen
        /// </summary>
        private void NaechsteGeneration()
        {
            foreach ( Zelle zelle in Zellenarr) 
            { 
                zelle.GetNaechstenStatus( this );
            }
        }
        /// <summary>
        /// Verwendet die Nächste Generation
        /// </summary>
        private void BenutzteNaechsteGeneration()
        {
            foreach (Zelle zelle in Zellenarr) 
            { 
                zelle.BenutzeNaechstenStatus();
            }
        }
        /// <summary>
        /// Fügt "Zufällig" lebende Zellen hinzu
        /// </summary>
        public void ZellenZufälligEinfügen()
        {
            var random = new Random();

            for (int x = 0; x < Breite; x++)
            { 
                for (int y = 0; y < Hoehe; y++)
                {
                    if (random.Next(0, 9) < 3)
                    {
                        this[new Point( x, y)].AenderStatus();
                    }
                }
            }
        }
    }
}
