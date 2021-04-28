using System;
using System.IO;
using System.Linq;

namespace Bankrupt
{
    class Program
    {
        private static string[] ReadText()
        {
            string[] textLines;
            try
            {
                //Build Path
                string path = Directory.GetCurrentDirectory() + "/gameConfig.txt";
                //Development Path
                //string path = @"~/../../../../gameConfig.txt";
                textLines = File.ReadAllLines(path);
                return textLines;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }
        }

        private static Property[] PopulateBoard(string[] textLines)
        {
            Property[] board = new Property[textLines.Length];
            for (int i = 0; i < textLines.Length; i++)
            {
                string[] PriceAndRent = textLines[i].Split(null);
                board[i] = new Property(Convert.ToInt32(PriceAndRent[0]), Convert.ToInt32(PriceAndRent[PriceAndRent.Length - 1]));
            }
            return board;
        }

        private static IPlayerActions GetTypeOfPlayer(int type)
        {
            switch (type) {
                case 0:
                    Impulsive impulsivePlayer = new Impulsive(); ;
                    return impulsivePlayer;
                case 1:
                    Picky pickyPlayer = new Picky();
                    return pickyPlayer;
                case 2:
                    Cautious cautiousPlayer = new Cautious();
                    return cautiousPlayer;
                default:
                    RandomP randomPlayer = new RandomP();
                    return randomPlayer;
            }
        }

        private static IPlayerActions[] InitializePlayers()
        {
            Random rnd = new Random();
            IPlayerActions[] players = new IPlayerActions[4];
            int[] playerTypes = { 0, 1, 2, 3 };
            for (int i = 0; i < 4; i++)
            {
                int randomNumber = rnd.Next(playerTypes.Length);
                players[i] = GetTypeOfPlayer(playerTypes[randomNumber]);
                playerTypes = playerTypes.Where(val => val != playerTypes[randomNumber]).ToArray();
            }
            return players;
        }

        private static int RollDice()
        {
            Random rnd = new Random();
            return rnd.Next(1, 7);
        }

        private static void MainGameLoop(Property[] properties)
        {
            int randomVictory = 0;
            int impulsiveVictory = 0;
            int pickyVictory = 0;
            int cautiousVictory = 0;
            int[] endTurns = new int[300];

            for (int x = 0; x < 300; x++)
            {
                IPlayerActions[] players = InitializePlayers();

                for (int i = 0; i < 1000; i++)
                {
                    foreach (IPlayerActions player in players)
                    {
                        if (!player.GetGameOverStatus())
                        {
                            player.SetPosition(RollDice(), properties.Length);
                            player.Action(properties[player.GetPosition()]);
                        }
                    }

                    if (CheckEndGame(players, i) != TypeOfPlayer.None)
                    {
                        switch (CheckEndGame(players, i))
                        {
                            case TypeOfPlayer.Aleatorio:
                                randomVictory++;
                                break;
                            case TypeOfPlayer.Cauteloso:
                                cautiousVictory++;
                                break;
                            case TypeOfPlayer.Exigente:
                                pickyVictory++;
                                break;
                            default:
                                impulsiveVictory++;
                                break;
                        }
                        endTurns[x] = i;
                        break;
                    }
                }
                properties = ResetProperties(properties);
                Array.Clear(players, 0, players.Length);
            }
            Estatistics(randomVictory, impulsiveVictory, pickyVictory, cautiousVictory, endTurns);
        }

        private static TypeOfPlayer CheckEndGame(IPlayerActions[] players, int turn)
        {
            if (turn >= 999)
            {
                TypeOfPlayer winnerType = players[0].GetTypeOfPlayer();
                for (int i = 1; i < players.Length; i++)
                {
                    if (players[i].GetCoins() > players[i - 1].GetCoins())
                    {
                        winnerType = players[i].GetTypeOfPlayer();
                    }
                }
                return winnerType;
            }
            else
            {
                players = players.Where(val => !val.GetGameOverStatus()).ToArray();
                if (players.Length == 1)
                {
                    return players[0].GetTypeOfPlayer();
                }
            }
            return TypeOfPlayer.None;
        }

        private static Property[] ResetProperties(Property[] properties)
        {
            foreach (Property property in properties)
            {
                property.Owner = null;
            }
            return properties;
        }

        private static void Estatistics(int randomVictory, int impulsiveVictory, int pickyVictory, int cautiousVictory, int[] endTurns)
        {
            float randomPecentage = (randomVictory * 100) / 300;
            float impusivePecentage = (impulsiveVictory * 100) / 300;
            float pickyPecentage = (pickyVictory * 100) / 300;
            float cautiousPecentage = (cautiousVictory * 100) / 300;

            int timeout = 0;
            int average = 0;
            int shorterTurn = endTurns[0];
            for (int j = 0; j < endTurns.Length; j++)
            {
                if (endTurns[j] == 999)
                    timeout++;
                average += endTurns[j];
                if (endTurns[j] < shorterTurn)
                    shorterTurn = endTurns[j];
            }
            average = average / 300;
            Console.WriteLine("Random Victory: " + randomVictory + " A total of " + randomPecentage + "% Victories");
            Console.WriteLine("Picky Victory: " + pickyVictory + " A total of " + pickyPecentage + "% Victories");
            Console.WriteLine("Cautious Victory: " + cautiousVictory + " A total of " + cautiousPecentage + "% Victories");
            Console.WriteLine("Impusive Victory: " + impulsiveVictory + " A total of " + impusivePecentage + "% Victories");
            Console.WriteLine("Average Turns: " + average);
            Console.WriteLine("Timeout Turns: " + timeout);
            Console.WriteLine("Shorter Turn: " + shorterTurn);

            string[] linesToBeWritten =
            {
                "Vitorias do jogador aleatorio: " + randomVictory.ToString() + " Totalizando " + randomPecentage.ToString() +"% Vitorias.",
                "Vitorias do jogador exigente: " + pickyVictory.ToString() + " Totalizando " + pickyPecentage.ToString() +"% Vitorias.",
                "Vitorias do jogador cauteloso: " + cautiousVictory.ToString() + " Totalizando " + cautiousPecentage.ToString() +"% Vitorias.",
                "Vitorias do jogador impulsivo: " + impulsiveVictory.ToString() + " Totalizando " + impusivePecentage.ToString() +"% Vitorias.",
                "A media de turnos foi de: " + average.ToString() + " turnos.",
                "O numero de partidas que terminaram na ultima rodada foram de: " + timeout.ToString() + " partidas.",
                "A partida com o turno mais curto foi de: " + shorterTurn.ToString() + " turnos."
            };
            // Build PATH
            string path = Directory.GetCurrentDirectory() + "/result.txt";
            // Development PATH
            //string path = @"~/../../../../result.txt";
            File.WriteAllLines(path, linesToBeWritten);
        }

        static void Main(string[] args)
        {
            string[] textLines = ReadText();
            Property[] properties = PopulateBoard(textLines);
            MainGameLoop(properties);
            Console.ReadKey();
        }
    }
}
