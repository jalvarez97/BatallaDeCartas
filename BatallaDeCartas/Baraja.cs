using System;
using System.Collections.Generic;
using static BatallaDeCartas.VariablesGlobales;

namespace BatallaDeCartas
{
    internal class Baraja
    {

        private List<Carta> Cartas = new List<Carta>();
        private int nContador = 0;

        public Baraja()
        {
            string Valor = null;

            for (int nFigura = 0; nFigura <= 3; nFigura++)
            {
                for (int Rango = 0; Rango <= 12; Rango++)
                {
                    switch (Rango)
                    {
                        case 0:
                            Valor = "As";
                            break;
                        case 10:
                            Valor = "J";
                            break;
                        case 11:
                            Valor = "Q";
                            break;
                        case 12:
                            Valor = "K";
                            break;
                        default:
                            Valor = (Rango + 1).ToString();
                            break;
                    }
                    Cartas.Add(new Carta(Valor, (Figura)nFigura, Rango + 1));
                }
            }
        }

        public void Barajar()
        {
            Random rnd = new Random();
            for (int nCartaActual = 0; nCartaActual < Cartas.Count; nCartaActual++)
            {
                int nPosicion = rnd.Next(0, Cartas.Count - 1);
                //Guardamos la carta a reemplazar
                Carta oOtraCarta = Cartas[nPosicion];
                //Insertamos la carta actual en una posicion aleatoria
                Cartas[nPosicion] = Cartas[nCartaActual];
                //Ahora insertamos la carta movida a la posicion de la carta actual
                Cartas[nCartaActual] = oOtraCarta;
            }
        }

        public Carta PedirCarta(int nJugador)
        {
            if (nContador >= Cartas.Count)
                return null;

            Carta oCarta = Cartas[nContador];
            oCarta.Jugador = nJugador;
            nContador++;
            return oCarta;
        }

        public Mano PedirMano(int nJugador)
        {
            List<Carta> lstMano = new List<Carta>();

            int nCartasRepartir = Cartas.Count / VariablesGlobales.NumeroJugadores;

            for (int i = 0; i < nCartasRepartir; i++)
            {
                Carta oCartaActual = PedirCarta(nJugador);
                if (oCartaActual != null)
                    lstMano.Add(oCartaActual);
            }

            return new Mano(lstMano);
        }

    }
}
