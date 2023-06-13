using System;
using System.Collections.Generic;

namespace BatallaDeCartas
{
    internal class Mano
    {
        public List<Carta> Cartas = new List<Carta>();
        public Mano(List<Carta> cartas)
        {
            Cartas = cartas;
        }

        public void Mostrar()
        {
            foreach (Carta carta in Cartas)
            {
                Console.WriteLine(carta);
            }
        }
        public void BarajarMano()
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
    }
}

