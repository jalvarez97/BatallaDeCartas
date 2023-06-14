using System;
using System.Collections.Generic;
using System.Linq;

namespace BatallaDeCartas
{
    internal class Mano
    {
        public List<Carta> Cartas = new List<Carta>();
        int nJugadoresSinCarta = 0;

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

        public List<Mano> RepartirManosJugadores(Baraja baraja)
        {
            List<Mano> lstJugadores = new List<Mano>();
            for (int i = 1; i <= VariablesGlobales.NumeroJugadores; i++)
            {
                lstJugadores.Add(baraja.PedirMano(i));
            }
            return lstJugadores;
        }

        public bool ComprobarCartasJugadores(List <Mano> lstJugadores)
        {
            bool bJugando = true;
            //Comprobamos cuantos jugadores tienen cartas
            for (int i = 0; i < VariablesGlobales.NumeroJugadores; i++)
            {
                if (lstJugadores[i].Cartas.Count == 0)
                    nJugadoresSinCarta++;
            }
            //Si del total de jugadores solo hay uno con cartas finalizamos partida
            if (VariablesGlobales.NumeroJugadores - nJugadoresSinCarta == 1)
            {
                Console.WriteLine("No quedan participantes, se acabó el juego.");
                bJugando = false;
            }
            else
            {
                Console.WriteLine("Pulsa cualquier tecla para pasar al siguiente turno. . .");
                Console.WriteLine("Pulsa ESC para salir. . .");
            }
            nJugadoresSinCarta = 0;
            return bJugando;
        }

        public List<Carta> ObtenerCartasRonda(List<Mano> lstJugadores)
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

        public List<Carta> ObtenerManoCartaGanadora(List<Carta> lstCartasRonda)
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

        public void AsignarCartasGanador(List<Carta> lstCartasRonda
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
                lstGanadorEmpate = ObtenerManoCartaGanadora(lstCartasRondaEmpate);
                AsignarCartasGanador(lstCartasRonda, lstGanadorEmpate, lstJugadores);
            }
            else
            {                
                Console.WriteLine("El jugador " + lstCartaGanadora[0].Jugador + " ha ganado con: " + lstCartaGanadora[0]);
                Console.WriteLine(" ");
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

