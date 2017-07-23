using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace Cards
{
    enum Suits
    {
        Hearts, Clubs, Spades, Diamonds
    }

    enum Ratings
    {
        Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 2, Queen = 3, King = 4, Ace = 11
    }

    struct Card
    {
        public Suits Suit;
        public Ratings Rating;
    }

    struct Deck
    {
        public Card[] Cards;
    }

    class Program
    {
        // Maximum number of cards in a deck for this game.
        const int MAX_CARDS_IN_DECK = 36;
        // Maximum number of cards that player can get to receive <=21 points.
        const int MAX_CARDS_IN_HAND = 8;
        // Maximum amount of points, until computer will continue playing.
        const int MAX_POINTS_TO_CONTINUE_PLAYING = 17;

        static void Main(string[] args)
        {

            Deck Deck = new Deck();
            Deck.Cards = new Card[MAX_CARDS_IN_DECK];

            Random random = new Random();

            int suitsLength = Enum.GetValues(typeof(Suits)).Length;
            int raitingsLength = Enum.GetValues(typeof(Ratings)).Length;

            // Generate an ordered deck

            for (int s = 0; s < suitsLength; s++)
            {
                int i = s * raitingsLength;

                for (int r = 0; r < raitingsLength; r++)
                {
                    Deck.Cards[i].Suit = (Suits)Enum.GetValues(typeof(Suits)).GetValue(s);
                    Deck.Cards[i].Rating = (Ratings)Enum.GetValues(typeof(Ratings)).GetValue(r);

                    i++;
                }
            }

            string next = "";
            int wins1 = 0;
            int wins2 = 0;
            int deadheat = 0;

            do
            {
                // Shuffle deck

                for (int i = Deck.Cards.Length - 1; i >= 1; i--)
                {
                    int j = random.Next(i + 1);
                    Card temp = Deck.Cards[j];
                    Deck.Cards[j] = Deck.Cards[i];
                    Deck.Cards[i] = temp;
                }

                // Start the game

                string player_raw = "";

                do
                {
                    Console.WriteLine("Who does receive the cards first? Enter 1 if you, enter 2 if computer.");
                    player_raw = Console.ReadLine();
                }
                while (player_raw != "1" && player_raw != "2");

                int player = int.Parse(player_raw);

                Deck Rasklad1 = new Deck();
                Rasklad1.Cards = new Card[MAX_CARDS_IN_HAND];

                Deck Rasklad2 = new Deck();
                Rasklad2.Cards = new Card[MAX_CARDS_IN_HAND];

                int score1 = 0;
                int score2 = 0;
                int cartcounter = 4;
                int rasklad1Length = 2;
                int rasklad2Length = 2;

                // Human receives first cards

                if (player == 1)
                {
                    for (int i = 0; i < rasklad1Length; i++)
                    {
                        Rasklad1.Cards[i] = Deck.Cards[i];
                        score1 += (int)Rasklad1.Cards[i].Rating;

                        Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                        Console.WriteLine(Rasklad1.Cards[i].Suit);
                        Console.WriteLine(Rasklad1.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Your score: " + score1);
                    Console.WriteLine("----------");

                    for (int i = 0; i < rasklad2Length; i++)
                    {
                        Rasklad2.Cards[i] = Deck.Cards[i + rasklad1Length];
                        score2 += (int)Rasklad2.Cards[i].Rating;

                        Console.WriteLine("--- Computer Card " + (i + 1) + " ---");
                        Console.WriteLine(Rasklad2.Cards[i].Suit);
                        Console.WriteLine(Rasklad2.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Computer score: " + score2);
                    Console.WriteLine("----------");

                    if (score1 == score2 && score1 == 21)
                    {
                        Console.WriteLine("Dead heat!");
                        deadheat++;
                    }
                    else if (
                        score1 == 21 ||
                        (Rasklad1.Cards[0].Rating == Ratings.Ace && Rasklad1.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("----------");
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("You won!");
                        wins1++;
                    }
                    else if (
                        score2 == 21 ||
                        (Rasklad2.Cards[0].Rating == Ratings.Ace && Rasklad2.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("Computer won!");
                        wins2++;
                    }
                    else
                    {
                        string answer = "";
                        do
                        {
                            Console.WriteLine("Get one more card (Y) or stop receiving cards (N)? Please type: Y/N");
                            answer = Console.ReadLine();
                        }
                        while (answer != "N" && answer != "Y" && answer != "n" && answer != "y");

                        // Aks whether to get a new card until user stops or points >= 21.
                        while (answer != "N" && answer != "n")
                        {
                            Rasklad1.Cards[rasklad1Length] = Deck.Cards[cartcounter];
                            cartcounter++;
                            score1 += (int)Rasklad1.Cards[rasklad1Length].Rating;
                            rasklad1Length++;

                            for (int i = 0; i < rasklad1Length; i++)
                            {
                                Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                                Console.WriteLine(Rasklad1.Cards[i].Suit);
                                Console.WriteLine(Rasklad1.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Your score: " + score1);
                            Console.WriteLine("----------");

                            if (score1 >= 21)
                            {
                                Console.WriteLine("||||||||| You've received " + score1 + " points. Now it is a computer turn. ||||||||| ");
                                Console.WriteLine("Press enter to continue...");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Get one more card (Y) or stop receiving cards (N)? Please type: Y/N");
                                answer = Console.ReadLine();
                            }
                        }

                        while (score2 <= MAX_POINTS_TO_CONTINUE_PLAYING)
                        {
                            Console.WriteLine("----------");
                            Console.WriteLine("Computer has decided to receive one more card.");
                            Console.WriteLine("----------");

                            Rasklad2.Cards[rasklad2Length] = Deck.Cards[cartcounter];
                            cartcounter++;
                            score2 += (int)Rasklad2.Cards[rasklad2Length].Rating;
                            rasklad1Length++;

                            for (int i = 0; i < rasklad2Length + 1; i++)
                            {
                                Console.WriteLine("--- Computer card: " + (i + 1) + " ---");
                                Console.WriteLine(Rasklad2.Cards[i].Suit);
                                Console.WriteLine(Rasklad2.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Computer score: " + score2);
                            Console.WriteLine("----------");
                            rasklad2Length++;
                        }

                        if (score1 == score2)
                        {
                            Console.WriteLine("Dead heat!");
                            deadheat++;
                        }
                        else if (
                            (score1 == 21) ||
                            (score1 > 21 && score2 > 21 && score1 < score2) ||
                            (score1 < 21 && score2 < 21 && score1 > score2) ||
                            (score1 < 21 && score2 > 21)
                        )
                        {
                            Console.WriteLine("You won!");
                            wins1++;
                        }
                        else
                        {
                            Console.WriteLine("Computer won!");
                            wins2++;
                        }
                    }
                }

                // Computer receives first cards.
                if (player == 2)
                {
                    for (int i = 0; i < rasklad2Length; i++)
                    {
                        Rasklad2.Cards[i] = Deck.Cards[i];
                        score2 += (int)Rasklad2.Cards[i].Rating;

                        Console.WriteLine("--- Computer Card " + (i + 1) + " ---");
                        Console.WriteLine(Rasklad2.Cards[i].Suit);
                        Console.WriteLine(Rasklad2.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Computer score: " + score2);
                    Console.WriteLine("----------");

                    for (int i = 0; i < rasklad1Length; i++)
                    {
                        Rasklad1.Cards[i] = Deck.Cards[i + rasklad2Length];
                        score1 += (int)Rasklad1.Cards[i].Rating;

                        Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                        Console.WriteLine(Rasklad1.Cards[i].Suit);
                        Console.WriteLine(Rasklad1.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Your score: " + score1);
                    Console.WriteLine("----------");

                    if (score1 == score2 && score1 == 21)
                    {
                        Console.WriteLine("Dead heat!");
                        deadheat++;
                    }
                    else if (
                        score1 == 21 ||
                        (Rasklad1.Cards[0].Rating == Ratings.Ace && Rasklad1.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("You won!");
                        wins1++;
                    }
                    else if (score2 == 21 || (Rasklad2.Cards[0].Rating == Ratings.Ace) && (Rasklad2.Cards[1].Rating == Ratings.Ace))
                    {
                        Console.WriteLine("----------");
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("Computer won!");
                        wins2++;
                    }

                    else
                    {
                        while (score2 <= MAX_POINTS_TO_CONTINUE_PLAYING)
                        {
                            Console.WriteLine("Computer has decided to receive one more card.");
                            Console.WriteLine("----------");

                            Rasklad2.Cards[rasklad2Length] = Deck.Cards[cartcounter];
                            cartcounter++;
                            score2 += (int)Rasklad2.Cards[rasklad2Length].Rating;

                            for (int i = 0; i < rasklad2Length + 1; i++)
                            {
                                Console.WriteLine("--- Computer card " + (i + 1) + " ---");
                                Console.WriteLine(Rasklad2.Cards[i].Suit);
                                Console.WriteLine(Rasklad2.Cards[i].Rating);
                            }
                            Console.WriteLine("----------");
                            Console.WriteLine("Computer score: " + score2);
                            Console.WriteLine("----------");
                            rasklad2Length++;
                        }

                        Console.WriteLine("Computer has stopped receiving cards. Now it is your turn!");
                        string answer = "";

                        do
                        {
                            Console.WriteLine("----------");
                            Console.WriteLine("Get one more card (Y) or stop receiving cards (N)? Please type: Y/N");
                            answer = Console.ReadLine();
                        }
                        while (answer != "N" && answer != "Y" && answer != "n" && answer != "y");

                        while (answer != "N" && answer != "n")
                        {
                            Rasklad1.Cards[rasklad1Length] = Deck.Cards[cartcounter];
                            cartcounter++;
                            score1 += (int)Rasklad1.Cards[rasklad1Length].Rating;
                            rasklad1Length++;

                            for (int i = 0; i < rasklad1Length; i++)
                            {
                                Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                                Console.WriteLine(Rasklad1.Cards[i].Suit);
                                Console.WriteLine(Rasklad1.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Your score: " + score1);
                            Console.WriteLine("----------");

                            if (score1 >= 21)
                            {
                                Console.WriteLine("||||||||| You've receive " + score1 + " points. ||||||||| ");
                                Console.WriteLine("Press enter to continue...");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Get one more card (Y) or stop receiving cards (N)? Please type: Y/N");
                                answer = Console.ReadLine();
                            }
                        }

                        if (score1 == score2)
                        {
                            Console.WriteLine("Dead heat!");
                            deadheat++;
                        }
                        else if (
                            (score1 == 21) ||
                            (score1 > 21 && score2 > 21 && score1 < score2) ||
                            (score1 < 21 && score2 < 21 && score1 > score2) ||
                            (score1 < 21 && score2 > 21)
                        )
                        {
                            Console.WriteLine("You won!");
                            wins1++;
                        }
                        else
                        {
                            Console.WriteLine("Computer won!");
                            wins2++;
                        }
                    }
                }

                do
                {
                    Console.WriteLine("----------");
                    Console.WriteLine("Do you want to start a new game? Please type: Y/N");
                    next = Console.ReadLine();
                } while (next != "N" && next != "Y" && next != "n" && next != "y");

            } while (next != "N" && next != "n");

            Console.WriteLine("You won " + wins1 + " time(s), computer won " + wins2 + " time(s). Dead heat occurred " + deadheat + " time(s).");
            Console.WriteLine();
        }
    }

}
