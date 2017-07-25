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
        // Maximum number of cards that player can get to receive <=21 points.
        const int MAX_CARDS_IN_HAND = 8;
        // Maximum amount of points, until computer will continue playing.
        const int MAX_POINTS_TO_CONTINUE_PLAYING = 17;

        static void Main(string[] args)
        {
            int suitsLength = Enum.GetValues(typeof(Suits)).Length;
            int raitingsLength = Enum.GetValues(typeof(Ratings)).Length;

            // Number of cards in a deck for this game.
            int cardsInDeck = suitsLength * raitingsLength;

            Deck Deck = new Deck();
            Deck.Cards = new Card[cardsInDeck];

            Random random = new Random();

            // Generate an ordered deck

            for (int s = 0; s < suitsLength; s++)
            {
                // Var i helps to switch to another suit when all ratings for the current suit are already in the deck
                int i = s * raitingsLength;

                for (int r = 0; r < raitingsLength; r++)
                {
                    Deck.Cards[i].Suit = (Suits)Enum.GetValues(typeof(Suits)).GetValue(s);
                    Deck.Cards[i].Rating = (Ratings)Enum.GetValues(typeof(Ratings)).GetValue(r);

                    i++;
                }
            }

            string next = "";
            int winsHuman = 0;
            int winsComp = 0;
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

                string player = "";

                do
                {
                    Console.WriteLine("Who does receive the cards first? Enter 1 if you, enter 2 if computer.");
                    player = Console.ReadLine();
                }
                while (player != "1" && player != "2");

                Deck DealHuman = new Deck();
                DealHuman.Cards = new Card[MAX_CARDS_IN_HAND];

                Deck DealComp = new Deck();
                DealComp.Cards = new Card[MAX_CARDS_IN_HAND];

                int scoreHuman = 0;
                int scoreComp = 0;
                int cartcounter = 4;
                int dealHumanLength = 2;
                int dealCompLength = 2;

                // Human receives first cards

                if (player == "1")
                {
                    for (int i = 0; i < dealHumanLength; i++)
                    {
                        DealHuman.Cards[i] = Deck.Cards[i];
                        scoreHuman += (int)DealHuman.Cards[i].Rating;

                        Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                        Console.WriteLine(DealHuman.Cards[i].Suit);
                        Console.WriteLine(DealHuman.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Your score: " + scoreHuman);
                    Console.WriteLine("----------");

                    for (int i = 0; i < dealCompLength; i++)
                    {
                        DealComp.Cards[i] = Deck.Cards[i + dealHumanLength];
                        scoreComp += (int)DealComp.Cards[i].Rating;

                        Console.WriteLine("--- Computer Card " + (i + 1) + " ---");
                        Console.WriteLine(DealComp.Cards[i].Suit);
                        Console.WriteLine(DealComp.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Computer score: " + scoreComp);
                    Console.WriteLine("----------");

                    if (scoreHuman == scoreComp && scoreHuman == 21)
                    {
                        Console.WriteLine("Dead heat!");
                        deadheat++;
                    }
                    else if (
                        scoreHuman == 21 ||
                        (DealHuman.Cards[0].Rating == Ratings.Ace && DealHuman.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("----------");
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("You won!");
                        winsHuman++;
                    }
                    else if (
                        scoreComp == 21 ||
                        (DealComp.Cards[0].Rating == Ratings.Ace && DealComp.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("Computer won!");
                        winsComp++;
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
                            DealHuman.Cards[dealHumanLength] = Deck.Cards[cartcounter];
                            cartcounter++;
                            scoreHuman += (int)DealHuman.Cards[dealHumanLength].Rating;
                            dealHumanLength++;

                            for (int i = 0; i < dealHumanLength; i++)
                            {
                                Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                                Console.WriteLine(DealHuman.Cards[i].Suit);
                                Console.WriteLine(DealHuman.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Your score: " + scoreHuman);
                            Console.WriteLine("----------");

                            if (scoreHuman >= 21)
                            {
                                Console.WriteLine("||||||||| You've received " + scoreHuman + " points. Now it is a computer turn. ||||||||| ");
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

                        while (scoreComp <= MAX_POINTS_TO_CONTINUE_PLAYING)
                        {
                            Console.WriteLine("----------");
                            Console.WriteLine("Computer has decided to receive one more card.");
                            Console.WriteLine("----------");

                            DealComp.Cards[dealCompLength] = Deck.Cards[cartcounter];
                            cartcounter++;
                            scoreComp += (int)DealComp.Cards[dealCompLength].Rating;
                            dealHumanLength++;

                            for (int i = 0; i < dealCompLength + 1; i++)
                            {
                                Console.WriteLine("--- Computer card: " + (i + 1) + " ---");
                                Console.WriteLine(DealComp.Cards[i].Suit);
                                Console.WriteLine(DealComp.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Computer score: " + scoreComp);
                            Console.WriteLine("----------");
                            dealCompLength++;
                        }

                        if (scoreHuman == scoreComp)
                        {
                            Console.WriteLine("Dead heat!");
                            deadheat++;
                        }
                        else if (
                            (scoreHuman == 21) ||
                            (scoreHuman > 21 && scoreComp > 21 && scoreHuman < scoreComp) ||
                            (scoreHuman < 21 && scoreComp < 21 && scoreHuman > scoreComp) ||
                            (scoreHuman < 21 && scoreComp > 21)
                        )
                        {
                            Console.WriteLine("You won!");
                            winsHuman++;
                        }
                        else
                        {
                            Console.WriteLine("Computer won!");
                            winsComp++;
                        }
                    }
                }

                // Computer receives first cards.
                else
                {
                    for (int i = 0; i < dealCompLength; i++)
                    {
                        DealComp.Cards[i] = Deck.Cards[i];
                        scoreComp += (int)DealComp.Cards[i].Rating;

                        Console.WriteLine("--- Computer Card " + (i + 1) + " ---");
                        Console.WriteLine(DealComp.Cards[i].Suit);
                        Console.WriteLine(DealComp.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Computer score: " + scoreComp);
                    Console.WriteLine("----------");

                    for (int i = 0; i < dealHumanLength; i++)
                    {
                        DealHuman.Cards[i] = Deck.Cards[i + dealCompLength];
                        scoreHuman += (int)DealHuman.Cards[i].Rating;

                        Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                        Console.WriteLine(DealHuman.Cards[i].Suit);
                        Console.WriteLine(DealHuman.Cards[i].Rating);
                    }

                    Console.WriteLine("----------");
                    Console.WriteLine("Your score: " + scoreHuman);
                    Console.WriteLine("----------");

                    if (scoreHuman == scoreComp && scoreHuman == 21)
                    {
                        Console.WriteLine("Dead heat!");
                        deadheat++;
                    }
                    else if (
                        scoreHuman == 21 ||
                        (DealHuman.Cards[0].Rating == Ratings.Ace && DealHuman.Cards[1].Rating == Ratings.Ace)
                    )
                    {
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("You won!");
                        winsHuman++;
                    }
                    else if (scoreComp == 21 || (DealComp.Cards[0].Rating == Ratings.Ace) && (DealComp.Cards[1].Rating == Ratings.Ace))
                    {
                        Console.WriteLine("----------");
                        Console.WriteLine("The Game is over!");
                        Console.WriteLine("----------");
                        Console.WriteLine("Computer won!");
                        winsComp++;
                    }

                    else
                    {
                        while (scoreComp <= MAX_POINTS_TO_CONTINUE_PLAYING)
                        {
                            Console.WriteLine("Computer has decided to receive one more card.");
                            Console.WriteLine("----------");

                            DealComp.Cards[dealCompLength] = Deck.Cards[cartcounter];
                            cartcounter++;
                            scoreComp += (int)DealComp.Cards[dealCompLength].Rating;

                            for (int i = 0; i < dealCompLength + 1; i++)
                            {
                                Console.WriteLine("--- Computer card " + (i + 1) + " ---");
                                Console.WriteLine(DealComp.Cards[i].Suit);
                                Console.WriteLine(DealComp.Cards[i].Rating);
                            }
                            Console.WriteLine("----------");
                            Console.WriteLine("Computer score: " + scoreComp);
                            Console.WriteLine("----------");
                            dealCompLength++;
                        }

                        Console.WriteLine("Computer has stopped receiving cards. Now it is your turn!");
                        Console.WriteLine("----------");

                        for (int i = 0; i < dealHumanLength; i++)
                        {
                            Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                            Console.WriteLine(DealHuman.Cards[i].Suit);
                            Console.WriteLine(DealHuman.Cards[i].Rating);
                        }

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
                            DealHuman.Cards[dealHumanLength] = Deck.Cards[cartcounter];
                            cartcounter++;
                            scoreHuman += (int)DealHuman.Cards[dealHumanLength].Rating;
                            dealHumanLength++;

                            for (int i = 0; i < dealHumanLength; i++)
                            {
                                Console.WriteLine("--- Your Card " + (i + 1) + " ---");
                                Console.WriteLine(DealHuman.Cards[i].Suit);
                                Console.WriteLine(DealHuman.Cards[i].Rating);
                            }

                            Console.WriteLine("----------");
                            Console.WriteLine("Your score: " + scoreHuman);
                            Console.WriteLine("----------");

                            if (scoreHuman >= 21)
                            {
                                Console.WriteLine("||||||||| You've receive " + scoreHuman + " points. ||||||||| ");
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

                        if (scoreHuman == scoreComp)
                        {
                            Console.WriteLine("Dead heat!");
                            deadheat++;
                        }
                        else if (
                            (scoreHuman == 21) ||
                            (scoreHuman > 21 && scoreComp > 21 && scoreHuman < scoreComp) ||
                            (scoreHuman < 21 && scoreComp < 21 && scoreHuman > scoreComp) ||
                            (scoreHuman < 21 && scoreComp > 21)
                        )
                        {
                            Console.WriteLine("You won!");
                            winsHuman++;
                        }
                        else
                        {
                            Console.WriteLine("Computer won!");
                            winsComp++;
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

            Console.WriteLine("You won " + winsHuman + " time(s), computer won " + winsComp + " time(s). Dead heat occurred " + deadheat + " time(s).");
            Console.WriteLine();
        }
    }

}