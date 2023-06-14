using System;
using System.Collections.Generic;

namespace BatallaDeCartas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool bJugando = true;

            Console.WriteLine("Introduce el numero de jugadores:");
            //Validaciones input
            while (VariablesGlobales.NumeroJugadores == 1 || VariablesGlobales.NumeroJugadores > 10)
            {
                while (!int.TryParse(Console.ReadLine(), out VariablesGlobales.NumeroJugadores))
                    Console.WriteLine(" Debes introducir un número.");

                if (VariablesGlobales.NumeroJugadores == 1 || VariablesGlobales.NumeroJugadores > 10)
                {
                    Console.WriteLine(" Numero de jugadores invalido se esperan entre 2 y 10 jugadores");
                }
            }

            //Generamos la baraja
            Baraja baraja = new Baraja();
            baraja.Barajar();
            //Repartirmos la mano de cada jugador
            Mano oMano = new Mano(baraja.Mazo);
            List<Mano> lstJugadores = oMano.RepartirManosJugadores(baraja);

            while (bJugando)
            {
                Console.Clear();

                List<Carta> lstCartasRonda = oMano.ObtenerCartasRonda(lstJugadores);
                List<Carta> lstCartaGanadora = oMano.ObtenerManoCartaGanadora(lstCartasRonda);

                oMano.AsignarCartasGanador(lstCartasRonda, lstCartaGanadora, lstJugadores);

                bJugando = oMano.ComprobarCartasJugadores(lstJugadores);

                //Al final de turno barajamos cada mano para evitar situaciones ciclicas
                foreach (Mano oManoBarajar in lstJugadores)
                {
                    oManoBarajar.BarajarMano();
                }

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    bJugando = false;
                    Console.Clear();
                }               
            }
        }
    }
}
