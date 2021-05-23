using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media;

namespace GameOfLife.Models
{
    public class Zelle : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Die Anzahl der Pixel die die Zelle hat
        /// </summary>
        public int PixelAnzahl { get; }
        /// <summary>
        /// der RelayCommand um den Status einer Zelle auf druck zu ändern
        /// </summary>
        public RelayCommand KlickButton { get; }
        /// <summary>
        /// Die Breite des SpielBrettes
        /// </summary>
        public int SpielBreite { get; }
        /// <summary>
        /// Die Höhe des SpielBrettes
        /// </summary>
        public int SpielHoehe { get; }
        /// <summary>
        /// Gibt an ob die Zelle Lebt oder Tot ist
        /// </summary>
        public bool Lebt { get; private set; }
        /// <summary>
        /// Gibt an ob die Zelle im Nächsten Zyklus lebt
        /// </summary>
        public bool WirdLeben { get; private set; }
        /// <summary>
        /// Gibt an ob der Status der Zelel Sich Verändert hat
        /// </summary>
        public bool HatVeraendert 
        { 
            get 
            { 
                return (Lebt != WirdLeben); 
            } 
        }
        /// <summary>
        /// Die Koordinaten auf dem UI
        /// </summary>
        public Point Koordinaten { get; }
        /// <summary>
        /// Die Koordinaten im Array
        /// </summary>
        public Point ArrKoordinaten { get; }
        /// <summary>
        /// Liste aller umliegenden Felder(Nachbarn)
        /// </summary>
        public IEnumerable<Point> Nachbarn { get; }

        private Brush _farbe;
        /// <summary>
        /// Die Farbe der Zelle
        /// </summary>
        public Brush Farbe
        {
            get 
            { 
                return _farbe; 
            }
            set
            {
                if ( value != _farbe)
                {
                    _farbe = value;
                    RaisePropertyChanged( nameof( Farbe ) );
                }
            }
        }
        /// <summary>
        /// Der Konstruktor der alle Variablen ihre werte zuweist
        /// </summary>
        /// <param name="koordinate">Die Array Koordinate der Zelle</param>
        /// <param name="breite">Die Breite der Map</param>
        /// <param name="hoehe">Die Höhe der Map</param>
        /// <param name="pixelAnzahl">die Anzahl der pixel von Der Zelle</param>
        /// <param name="lebt">Gibt an ob die Zelle oder nicht/param>
        public Zelle( Point koordinate, int breite, int hoehe, int pixelAnzahl, bool lebt = false )
        {
            this.ArrKoordinaten = koordinate;
            this.SpielBreite = breite;
            this.SpielHoehe = hoehe;
            this.Lebt = lebt;
            this.Nachbarn = GetNachbarKoordinaten();
            this.PixelAnzahl = pixelAnzahl;
            Koordinaten = new Point(PixelAnzahl * ArrKoordinaten.X, PixelAnzahl * ArrKoordinaten.Y);
            Farbe = Lebt ? Brushes.Green : Brushes.White;
            KlickButton = new( a => AenderStatus(), a => true );
        }
        /// <summary>
        /// Erweckt ein PropertyCHanged Event
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
        /// <summary>
        /// Setzt die Farbe der Zelle (Lebendig = Blau,Tot = weiß)
        /// </summary>
        public void SetFarbe()
        {
            if (Lebt)
            {
                Farbe = Brushes.Blue;
            }
            else 
            {
                Farbe = Brushes.White; 
            }
        }

        /// <summary>
        /// Errechnet den Nächsten Status der Zelle
        /// </summary>
        /// <param name="spielBrett"></param>
        public void GetNaechstenStatus( SpielBrett spielBrett )
        {
            int LebendeNachbarn = Nachbarn.Select( point => spielBrett[point] ).Count( zelle => zelle.Lebt );

            if ( ! Lebt)
            {
                WirdLeben = LebendeNachbarn == 3;

            }
            else
            {
                WirdLeben = (LebendeNachbarn >= 2 && LebendeNachbarn <= 3);
            }
        }
        /// <summary>
        /// wendet Den Nächsten Status an
        /// </summary>
        public void BenutzeNaechstenStatus()
        {
            if (HatVeraendert)
            { 
                Lebt = WirdLeben; 
                SetFarbe();
            }
        }
        /// <summary>
        /// Ändert den Status
        /// </summary>
        public void AenderStatus()
        {
            Lebt = !Lebt;
            WirdLeben = !Lebt;
            SetFarbe();
        }
        /// <summary>
        /// Sammelt die Koordinaten der Nachbarn
        /// </summary>
        /// <returns>Koordinaten der Nachbarn</returns>
        private IEnumerable<Point> GetNachbarKoordinaten()
        {
            for ( int x = -1; x <= 1; x++)
            {
                for ( int y = -1; y <= 1; y++ )
                {
                    if ( x == 0 && y == 0 )continue;

                   int X = ArrKoordinaten.X + x;
                   int Y  = ArrKoordinaten.Y + y;                    
                   if ( X < 0)
                   {
                       X += SpielBreite;
                   }
                   else if ( X == SpielBreite)
                   {
                       X = 0;
                   }
                   if ( Y < 0)
                   { 
                        Y += SpielHoehe;
                   }
                   else if ( Y == SpielHoehe)
                   {
                       Y = 0;
                   }
                   yield return new Point( X, Y );
                }
            }
        }
    }
}
