using System.ComponentModel;
using System.Threading;

namespace GameOfLife.Models
{
    public class Spiel : INotifyPropertyChanged
    {
        /// <summary>
        /// Das Relay Command um das Spiel über den Gebindeten Knopf zu starten
        /// </summary>
        public RelayCommand SpielStarten { get; }
        /// <summary>
        /// Das Relay Command um das Spiel über den Gebindeten Knopf zu Stoppen
        /// </summary>
        public RelayCommand SpielStoppen { get; }
        /// <summary>
        /// Das Relay Command um das Spiel über den Gebindeten Knopf zu Erstellen
        /// </summary>
        public RelayCommand SpielBrettErstellen { get; }
        /// <summary>
        /// Das Relay Command um das Spiel über den Gebindeten Knopf mit zufälligen Zellen zu nefüllen
        /// </summary>
        public RelayCommand SpielBrettZufälligFüllen { get; }
        /// <summary><
        ///  Überprüft ob das Brett eine angegebene Breite hat
        /// </summary>
        private bool HatBreite
        {
            get
            {
                return Breite != default;
            }

        }
        /// <summary>
        /// Überprüft ob das Brett eine angegebene Hoehe hat
        /// </summary>
        private bool HatHoehe
        {
            get
            {
                return (Hoehe != default);
            }
        }
        /// <summary>
        /// Überprüft ob das Brett eine angegebene Pixel Anzahl hat
        /// </summary>
        private bool HatPixelAnzahl
        {
            get
            {
                return (PixelAnzahl > 0);
            }
        }

        private int _verzoegerung = 1000;
        /// <summary>
        /// Die Verzögerung Zwischen den Generationen
        /// </summary>
        public int Verzoegerung
        {
            get { return _verzoegerung; }
            set
            {
                if (value != _verzoegerung)
                {
                    _verzoegerung = value;
                    RaisePropertyChanged(nameof(Verzoegerung));
                }
            }
        }
        private int _hoehe = 40;
        /// <summary>
        /// Die Höhe des Spielbretts
        /// </summary>
        public int Hoehe
        {
            get { return _hoehe; }
            set
            {
                if (value != _hoehe)
                {
                    _hoehe = value;
                    RaisePropertyChanged(nameof(Hoehe));
                }
            }
        }

        private int _breite = 40;
        /// <summary>
        /// Die Breite des Spielbretts
        /// </summary>
        public int Breite
        {
            get { return _breite; }
            set
            {
                if (value != _breite)
                {
                    _breite = value;
                    RaisePropertyChanged(nameof(Breite));
                }
            }
        }
        private SpielBrett _spielBrett;
        /// <summary>
        /// Das SpielBrett
        /// </summary>
        public SpielBrett SpielBrett
        {
            get
            {
                return _spielBrett;
            }
            set
            {
                if (value != _spielBrett)
                {
                    _spielBrett = value;
                    RaisePropertyChanged(nameof(SpielBrett));
                }
            }
        }
        private int _pixelAnzahl = 10;
        /// <summary>
        /// Die Pixel anzahl Pro Zelle
        /// </summary>
        public int PixelAnzahl
        {
            get
            {
                return _pixelAnzahl;
            }
            set
            {
                if (value != _pixelAnzahl)
                {
                    _pixelAnzahl = value;
                    RaisePropertyChanged(nameof(PixelAnzahl));
                }
            }
        }
        /// <summary>
        /// Gibt an ob Die Regler Editierbar sind
        /// </summary>
        public bool IstEditierbar
        {
            get
            {
                return (Laeuft == false);
            }
        }
        private bool _laeuft = default(bool);
        /// <summary>
        /// Gibt an Ob das Spiel Gerade im Gange Ist
        /// </summary>
        public bool Laeuft
        {
            get { return _laeuft; }
            set
            {
                if (value != _laeuft)
                {
                    _laeuft = value;
                }
            }
        }



        /// <summary>
        /// Der Konstruktor der Die RelayCommand Erstellt
        /// </summary>
        public Spiel()
        {
            SpielStarten = new RelayCommand((object a) => SpielStartenSchleife(), f => !Laeuft);
            SpielStoppen = new RelayCommand((object a) => SpielSchleifeStoppen(), f => Laeuft);
            SpielBrettErstellen = new RelayCommand((object a) => SpielBrett = new SpielBrett(Breite, Hoehe, PixelAnzahl), f => (HatBreite && HatHoehe && HatPixelAnzahl) && !Laeuft);
            SpielBrettZufälligFüllen = new RelayCommand((object a) => SpielBrett.ZellenZufälligEinfügen(), (object f) => !Laeuft);
        }
        /// <summary>
        /// Startet eine Schleife in der das Spiel Läuft
        /// </summary>
        public void SpielStartenSchleife()
        {
            Laeuft = true;
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (Laeuft)
                {
                    SpielBrett.NaechsterZug();
                    Thread.Sleep(Verzoegerung);
                }
            });
        }
        /// <summary>
        /// Stoppt die Schleife in der das Spiel Läuft
        /// </summary>
        public void SpielSchleifeStoppen()
        {
            Laeuft = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, (new PropertyChangedEventArgs(propertyName)));
        }

    }
}
