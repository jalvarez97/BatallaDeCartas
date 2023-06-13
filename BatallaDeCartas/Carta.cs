using System;
using static BatallaDeCartas.VariablesGlobales;

namespace BatallaDeCartas
{
    internal class Carta
    {
        public string Valor { get; set; }
        public Figura Palo { get; set; }
        public int Rango { get; set; }

        public int Jugador { get; set; }

        public Carta(string valor, Figura figuras, int rango, int jugador)
        {
            Valor = valor;
            Palo = figuras;
            Rango = rango;
            Jugador = jugador;
        }

        public Carta(string valor, Figura figuras, int rango)
        {
            Valor = valor;
            Palo = figuras;
            Rango = rango;
        }

        public override string ToString()
        {
            string sFiguraSimbolo = null;
            switch (Palo)
            {
                case Figura.Diamantes:
                    sFiguraSimbolo = "♦";
                    break;
                case Figura.Corazones:
                    sFiguraSimbolo = "♥";
                    break;
                case Figura.Picas:
                    sFiguraSimbolo = "♠";
                    break;
                case Figura.Trebol:
                    sFiguraSimbolo = "♣";
                    break;
            }
            //return String.Format($"{Valor} {Palo} {sFiguraSimbolo}");
            return String.Format($"{Valor} {sFiguraSimbolo}");
        }

        public static bool operator <(Carta a, Carta b)
        {
            return a.Rango < b.Rango;
        }

        public static bool operator >(Carta a, Carta b)
        {
            return a.Rango > b.Rango;
        }
    }
}
