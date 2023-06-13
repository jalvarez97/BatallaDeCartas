using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatallaDeCartas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool bJugando = true;
            int nJugadoresSinCarta = 0;

            Console.WriteLine("Introduce el numero de jugadores:");
            while (VariablesGlobales.NumeroJugadores == 1 || VariablesGlobales.NumeroJugadores > 10)
            {
                while (!int.TryParse(Console.ReadLine(), out VariablesGlobales.NumeroJugadores))
                    Console.WriteLine(" Debes introducir un número.");

                if (VariablesGlobales.NumeroJugadores == 1 || VariablesGlobales.NumeroJugadores > 10)
                {
                    Console.WriteLine(" Numero de jugadores invalido se esperan entre 2 y 10 jugadores");
                }
            }

            Baraja baraja = new Baraja();
            baraja.Barajar();

            List<Mano> lstJugadores = new List<Mano>();
            for (int i = 1; i <= VariablesGlobales.NumeroJugadores; i++)
            {
                lstJugadores.Add(baraja.PedirMano(i));
            }

            while (bJugando)
            {

                Console.Clear();

                List<Carta> lstCartasRonda = ObtenerCartasRonda(lstJugadores);
                List<Carta> lstCartaGanadora = ObtenerCartaGanadora(lstCartasRonda);

                ComprobarEstadoRonda(lstCartasRonda, lstCartaGanadora, lstJugadores);

                //Comprobamos cuantos jugadores tienen cartas
                for (int i = 0; i < VariablesGlobales.NumeroJugadores; i++)
                {
                    if (lstJugadores[i].Cartas.Count == 0)
                        nJugadoresSinCarta++;
                }

                if (VariablesGlobales.NumeroJugadores - nJugadoresSinCarta == 1)
                {
                    bJugando = false;
                    Console.WriteLine("No quedan participantes, se acabó el juego.");
                }
                else
                {
                    Console.WriteLine("Pulsa cualquier tecla para pasar al siguiente turno. . .");
                    Console.ReadKey();
                }

                nJugadoresSinCarta = 0;
            }

        }

        static List<Carta> ObtenerCartasRonda(List<Mano> lstJugadores)
        {
            List<Carta> lstCartasRonda = new List<Carta>();
            Console.WriteLine("----------------------------------------------------");
            for (int i = 0; i < VariablesGlobales.NumeroJugadores; i++)
            {
                int nJugador = i + 1;
                Console.WriteLine(" ");
                if (lstJugadores[i].Cartas.Count != 0)
                {
                    Console.WriteLine(" Jugador " + nJugador);
                    Console.WriteLine("     Cartas en mano: " + (lstJugadores[i].Cartas.Count - 1));
                    Console.WriteLine("     Carta en juego: " + lstJugadores[i].Cartas[0]);
                    lstCartasRonda.Add(lstJugadores[i].Cartas[0]);
                    lstJugadores[i].Cartas.RemoveAt(0);
                }
                else
                {
                    Console.WriteLine(" Jugador " + nJugador + " sin cartas.");
                }
            }
            Console.WriteLine(" ");
            Console.WriteLine("----------------------------------------------------");
            return lstCartasRonda;
        }

        static List<Carta> ObtenerCartaGanadora(List<Carta> lstCartasRonda)
        {
            List<Carta> lstCartaGanadora = new List<Carta>();

            foreach (Carta oCartaGanadora in lstCartasRonda)
            {
                if (oCartaGanadora.Rango == lstCartasRonda.Max(x => x.Rango))
                {
                    lstCartaGanadora.Add(oCartaGanadora);
                }
            }
            return lstCartaGanadora;
        }

        static void ComprobarEstadoRonda(List<Carta> lstCartasRonda
                                        , List<Carta> lstCartaGanadora
                                        , List<Mano> lstJugadores)
        {
            List<Carta> lstCartasRondaEmpate = new List<Carta>();
            List<Carta> lstGanadorEmpate = new List<Carta>();
            //Si hay mas de una carta ganadora es empate
            if (lstCartaGanadora.Count() > 1)
            {
                //Informamos de los empates
                foreach (Carta oCartaEmpatada in lstCartaGanadora)
                {
                    Console.WriteLine(" ");
                    Console.Write("El jugador " + oCartaEmpatada.Jugador + " ha empatado con: " + oCartaEmpatada + " ");
                    Console.WriteLine(" ");
                }
                foreach (Carta oCartaEmpatada in lstCartaGanadora)
                {
                    if (lstJugadores[oCartaEmpatada.Jugador - 1].Cartas.Count() > 0)
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine(" Jugador " + oCartaEmpatada.Jugador);
                        Console.WriteLine("     Cartas en mano: " + (lstJugadores[oCartaEmpatada.Jugador - 1].Cartas.Count - 1));
                        Console.WriteLine("     Carta en juego: " + lstJugadores[oCartaEmpatada.Jugador - 1].Cartas[0]);
                        lstCartasRonda.Add(lstJugadores[oCartaEmpatada.Jugador - 1].Cartas[0]);
                        lstCartasRondaEmpate.Add(lstJugadores[oCartaEmpatada.Jugador - 1].Cartas[0]);
                        lstJugadores[oCartaEmpatada.Jugador - 1].Cartas.RemoveAt(0);
                    }
                    else
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine(" Jugador " + (oCartaEmpatada.Jugador - 1) + " sin cartas.");
                    }
                }
                Console.WriteLine(" ");
                Console.WriteLine("----------------------------------------------------");
                //Los que han empatado juegan un turno mas y volvemos a comprobar que haya un ganador
                //para repartir todas las cartas de la mesa al ganador                
                lstGanadorEmpate = ObtenerCartaGanadora(lstCartasRondaEmpate);
                ComprobarEstadoRonda(lstCartasRonda, lstGanadorEmpate, lstJugadores);
            }
            else
            {
                Console.WriteLine(" ");
                Console.WriteLine("El jugador " + lstCartaGanadora[0].Jugador + " ha ganado con: " + lstCartaGanadora[0]);
                //Devolvemos las cartas de la mesa al ganador
                for (int i = 0; i < lstCartasRonda.Count; i++)
                {
                    lstCartasRonda[i].Jugador = lstCartaGanadora[0].Jugador;

                    lstJugadores[lstCartaGanadora[0].Jugador - 1].Cartas.Add(lstCartasRonda[i]);
                }
            }

        }
    }
}
