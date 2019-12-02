using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class StartGame : Form {
        //List of type cards. 
        
        
        
        private List<Card> cards { get; set; }//cards is the complete deck
        private List<Card> playerH { get; set; }//playerH is a list with the player cards in a game
        private List<Card> dealerH { get; set; }//dealerH is a list with the dealer cards in a game

        //List that contains the pictures of cards that will be printed in the window
        private List<PictureBox> handPicture { get; set; }
        private int bet;//The bet the player entered at the beginning of the game
        
        private Label playerLbl = new Label();//Label to display the player's cards
        private Label name = new Label();//Label for the player's name
        private Label betText = new Label(); //Label for the bet
        private Label tot = new Label();//Label for the total of points in the player's game
        private Label dealerLbl = new Label();//Label for the dealer's cards
        private Label totalMLbl = new Label();//Label for the total of money of the player
        private short hitCount = 0;//Counter. Used to verify that the player can't take more than 5 cards per game
        private int totalMoney;//Total money of the player, including bets
        
        //Initialization of the deck for the game
        public void Initialize() {
            cards = new List<Card>();

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 13; j++) {
                    //Adding a new card to the list with the corresponding suit and face
                    //e.g. 4 of hearts
                    cards.Add(new Card() { Suit = (Suit)i, Face = (Face)j });
                    //For the cards 10, Q, J and K, their values is 10
                    if (j <= 8) cards[cards.Count - 1].Value = j + 1;
                    else cards[cards.Count - 1].Value = 10;
                    //Asigning the card's image from the sprite
                    cards[cards.Count -1 ].Name = SplitImage(i,j);
                }
            }
        }
        //Cards shuffle
        public void Shuffle() {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1) {//
                n--;
                int k = rng.Next(n + 1);//Generating a random index of the 52 cards
                Card card = cards[k];//With the given index, create an auxiliar card
                //Exchange of cards
                cards[k] = cards[n];
                cards[n] = card;

            }
        }
        //Printing the player's deck, needs the list of card's either from the player or dealer.
        //x parameter corresponds to the coordinate where the card will be printed
        public void PrintDeckP(List<Card> list, int x) {
            int y = 146;
            for (int i = 0; i < list.Count; i++) {
                //Create a new picture to insert in the window
                PictureBox a = new PictureBox();
                a.Location = new Point(y, x);//Location
                a.Image = list[i].Name;//The image on bottom of the stack in the player or dealer's deck
                a.Size = new Size(112, 157);//Size in the window
                a.SizeMode = PictureBoxSizeMode.Zoom;
                a.TabStop = false;
                a.Size = new Size(112, 157); 
                this.Controls.Add(a);//Adding the picture to the window
                handPicture.Add(a);//Adding the picture to the list of pictures
                y += 115;//Increasing coordinates to avoid overlap of printed cards
               
            }

        }

        //Printing the dealer's deck, same instruction than PrintDeckP
        public void PrintDeckD(List<Card> list) {
            int y = 146;
            for (int i = 0; i < list.Count; i++) {

                PictureBox a = new PictureBox();
                a.Location = new Point(y, 50);
                a.Size = new Size(112, 157);
                a.SizeMode = PictureBoxSizeMode.Zoom;
                a.TabStop = false;
                a.Size = new Size(112, 157); this.Controls.Add(a);
                handPicture.Add(a);
                //For the first card of the dealer, print the back of a card instead of the actual card
                //This keeps the dealer's deck secure
                if (i == 0) a.Image = Properties.Resources.back_deck;
                else a.Image = list[i].Name;
                y += 115;

            }

        }
        //Deleting the entire cards on the window
        public void DeleteDeck() {
            if (handPicture != null)
                foreach (PictureBox picture in handPicture) {//For each card in the list of pictures
                    this.Controls.Remove(picture);//Remove them from the window
                }
        }
        //Spliting the image of the sprite. i parameter is the suit, j is the face
        public Bitmap SplitImage(int i, int j) {
            int x = 0;//starting point from the left
            int y = 0;//starting point from the top. 
            int height = 315;//Height of the card IN THE SPRITE
            int width = 225;//Width of the card IN THE SPRITE

            switch (i) {//Depending on the suit, the y coordenate adjust to get the correct suit in the sprite
                case 0: y = 0; break;//Y coordenate of hearts
                case 1: y = 315; break;//Y coordenate of clubs
                case 2: y = 630; break;//Y coordenate of diamonds
                case 3: y = 945; break;//Y coordenate of spades
            }
            //X coordenate where the card to return is
            x = (j * width);
            Bitmap bitmap = Properties.Resources.sprite_deck;//Getting the sprite
            Bitmap img = new Bitmap(width, height);//New bitmap with the size established
            Graphics g = Graphics.FromImage(img);//New graphic object
            g.DrawImage(bitmap, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            g.Dispose();
            return img;

        }
        //Draw a card, either for the player of the dealer
        public Card DrawACard() {
            if (cards.Count <= 0) {//If there aren't cards in the deck, initialize and shuffle a new deck
                this.Initialize();
                this.Shuffle();
            }
            
            Card cardToReturn = cards[cards.Count - 1];//Getting the last card in the deck
            cards.RemoveAt(cards.Count - 1);//Removing the top of the list
            return cardToReturn;//Return the drawn card
        }
        //Printing the current stat of the game
        //points is the number of point of the player in their hand
        //bet is the current bet. totalM is the total of money of the player
        public void GameStats(string points, string bet, string totalM) {
            //Adding the name of the player to the window
            name.AutoSize = true;
            name.Location = new Point(12, 260);
            name.Name = "nameLbl";
            name.Size = new Size(35, 13);
            name.Text = "Name:"+textBox1.Text;
            this.Controls.Add(name);
            //Adding the total of money of the player to the window
            totalMLbl.AutoSize = true;
            totalMLbl.Location = new Point(12, 275);
            totalMLbl.Name = "totalMLbl";
            totalMLbl.Size = new Size(35, 13);
            totalMLbl.Text = "Total money: $" + totalM;
            this.Controls.Add(totalMLbl);
            //Adding the bet of the player to the window
            betText.AutoSize = true;
            betText.Location = new Point(12, 290);
            betText.Name = "betLbl";
            betText.Size = new Size(35, 13);
            betText.Text = "Bet: $"+ bet;
            this.Controls.Add(betText);
            //Adding the total of points of the player to the window
            tot.AutoSize = true;
            tot.Location = new Point(12, 305);
            tot.Name = "totalLbl";
            tot.Size = new Size(35, 13);
            tot.Text = "Points: "+ points;
            this.Controls.Add(tot);
            //Adding the label to differentiate the Dealer's hand from the player's hand
            dealerLbl.AutoSize = true;
            dealerLbl.Location = new System.Drawing.Point(143, 12);
            dealerLbl.Name = "dealerLbl";
            dealerLbl.Size = new System.Drawing.Size(74, 13);
            dealerLbl.TabIndex = 11;
            dealerLbl.Text = "Dealer\'s cards";
            this.Controls.Add(dealerLbl);

            //Adding the label to differenciate the player's hand from the dealer's hand
            playerLbl.AutoSize = true;
            playerLbl.Location = new System.Drawing.Point(143, 237);
            playerLbl.Name = "playerLbl";
            playerLbl.Size = new System.Drawing.Size(56, 13);
            playerLbl.TabIndex = 12;
            playerLbl.Text = "Your cards";
            this.Controls.Add(playerLbl);
        }
        //Dealer's turn
        public void GameDealer() {
            bool insurance = false;
            //Player has the option of insurance if the dealer has an Ace or 10 on the card that is shown to players
            if (dealerH[1].Face == Face.Ace || dealerH[1].Value == 10) {
                DialogResult dialogResult = MessageBox.Show("Insurance?", "Insurance", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) {//Players decide if they want to insure
                    insurance = true;
                    MessageBox.Show("Insurance accepted!");
                } else if (dialogResult == DialogResult.No) insurance = false;
            }
            
            if (dealerH[1].Face == Face.Ace || dealerH[1].Value == 10) {
                //Dealer checks if he has a Blackjack or 21
                if (dealerH[0].Value + dealerH[1].Value == 21) {
                    DeleteDeck();//Delete the cards on board
                    PrintDeckP(dealerH, 50);//Print the dealer's cards without the first hided
                    PrintDeckP(playerH, 281);//Print the players cards
                    if (insurance && playerH[0].Value + playerH[1].Value == 21) {//If player insured and dealer has 21
                        totalMoney -= bet / 2;//Player only lost half of his bet
                        MessageBox.Show("You lost $"+(bet / 2).ToString());
                        KeepGambling();//Ask the players if they want to play again
                    } else if (!insurance && playerH[0].Value + playerH[1].Value != 21) {//If player didn't insured and player don't have 21
                        totalMoney += (bet / 2) + bet;//Player wins plus half of his bet
                        MessageBox.Show("You won $"+(bet+ (bet/2)).ToString());
                        KeepGambling();//Ask the players if they want to play again
                    }
                    
                } else if(insurance) MessageBox.Show("Dealer does not have a BlackJack");//Dealer doesn't have Blackjack
            }

            GamePlayer();//Game of the player
        }
        //Player's turn
        private void GamePlayer() {
            if (playerH.Sum(card => card.Value) == 21) {//If player has 21

                string message = "Congrats, you won $" + bet.ToString();
                DialogResult dialogResult = MessageBox.Show(message, "You WON!!", MessageBoxButtons.OK);
                totalMoney += bet;//Wins 
                GameStats((playerH.Sum(card => card.Value)).ToString(), "0", totalMoney.ToString());//Print the new stats
                KeepGambling();//Ask the players if they want to play again

            } else if (playerH.Sum(card => card.Value) > 21) {//If player exceed 21

                string message = "You lost $" + bet.ToString();
                totalMoney -= bet;//Automatically lost
                GameStats((playerH.Sum(card => card.Value)).ToString(), "0", totalMoney.ToString());//Print new stats
                DeleteDeck();//Delete cards on board
                PrintDeckP(dealerH, 50);//Reveal cards of the dealer
                PrintDeckP(playerH, 281);//Printing players cards
                MessageBox.Show(message);
                KeepGambling();//Ask the players if they want to paly again

            }
        }
        //Play again
        private void KeepGambling() {
            //Ask the player if he/she wants to keep playing
            DialogResult dialogResult = MessageBox.Show("Keep gambling?", "BlackJack", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {//If yes
                if (totalMoney <= 0) {//Check that the player have enough money
                    MessageBox.Show("You have no money.");
                    MainMenu newGame = new MainMenu();
                    this.Hide();
                    newGame.Show();
                } else {
                    label2.Show();
                    textBox2.Show();
                    textBox2.Clear();
                    stayBtn.Hide();
                    hitBtn.Hide();
                    startBtn.Show();
                    textBox3.Text = totalMoney.ToString();
                    DeleteDeck();
                }
            } else if (dialogResult == DialogResult.No) {//If no
                this.Dispose();
                MessageBox.Show("You have a total of $"+totalMoney.ToString());
                MainMenu newGame = new MainMenu();//Launch the main menu window
                this.Hide();
                newGame.Show();
                //Environment.Exit(0);
            }
            
            
        }
        //Deal cards. totalM is the total money of the player
        private void DealCards(string totalM) {
            hitCount = 0;//Restart the number of times the player clicked the hit button
            //Initialize and shuffle the deck
            Initialize();
            Shuffle();
            //Hide buttons and labels from the start of the game
            startBtn.Hide();//Start button
            textBox3.Hide();//Total money text box
            textBox2.Hide();//Bet text box
            textBox1.Hide();//Name text box

            this.Controls.Remove(label3);//Remove total money's label
            label2.Hide();//Hide the bet label
            this.Controls.Remove(label1);//Remove name's label

            pictureBox1.Show();//Show the card from the deck
            stayBtn.Show();//Show the stay button
            hitBtn.Show();//Show the hit button

            playerH = new List<Card>();//New list for the player's cards
            dealerH = new List<Card>();//New list for the dealer's cards
            handPicture = new List<PictureBox>();//New list for the cards, both player and dealer
            //Player drawn his first two cards
            playerH.Add(DrawACard());
            playerH.Add(DrawACard());
            //Dealer drawn his first two cards
            dealerH.Add(DrawACard());
            dealerH.Add(DrawACard());
            //Delete if there are cards on boards and print dealer and player's cards
            DeleteDeck();
            PrintDeckD(dealerH);
            PrintDeckP(playerH, 281);

            foreach (Card card in playerH) {
                if (card.Face == Face.Ace) {//If any of the first two cards is an Ace, the value changes to 11
                    card.Value += 10;
                    break;
                }
            }
            foreach (Card card in dealerH) {
                if (card.Face == Face.Ace) {//If any of the first two cards is an Ace, the value changes to 11
                    card.Value += 10;
                    break;
                }
            }
            int points = playerH.Sum(card => card.Value);//Sum of points in the player's cards

            GameStats(points.ToString(), textBox2.Text, totalM);//Print stats
            GameDealer();//Dealer's turn. Game starts
        }
        //Action when the start button is clicked
        private void startBtn_Click(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(textBox3.Text) && 
                !String.IsNullOrEmpty(textBox2.Text) &&
                !String.IsNullOrEmpty(textBox1.Text)) {//Check that neither of the text boxes are empty

                if (int.TryParse(textBox2.Text, out bet) && 
                    int.TryParse(textBox3.Text, out totalMoney) &&
                    bet <= totalMoney &&
                    bet != 0) {//Check that the bet and money are integer and bet is lower or equal to total money and different of 0

                    DealCards(textBox3.Text);//Deal cards
                    

                } else MessageBox.Show("Insert a valid bet.");

            } else MessageBox.Show("Complete all the spaces.");

        }
        //Stay button. Player is satisfied with the sum of point of his cards
        private void stayBtn_Click(object sender, EventArgs e) {
            hitCount = 0;
            stayBtn.Hide();
            hitBtn.Hide();
            int pointsD = dealerH.Sum(card => card.Value);//Sum of points on dealer's cards
            Random rng = new Random();
            //While points of dealer are less than 18 and the random number is not 2
            while (pointsD < 18 && rng.Next(3) != 2) {
                dealerH.Add(DrawACard());//Dealer drawn a card
                pointsD = 0;
                pointsD = dealerH.Sum(card => card.Value);//New sum of points
            }

            pointsD = 0;
            pointsD = dealerH.Sum(card => card.Value);//Total of point in dealer's cards
            
            DeleteDeck();
            PrintDeckP(dealerH, 50);//Reveal the hiden card of dealer
            PrintDeckP(playerH, 281);
            if (pointsD > 21) {//If dealer has more than 21 points
                MessageBox.Show("Dealers busted!, you win $" + bet.ToString());
                totalMoney += bet;//Bet is added to the total money
            } else if (pointsD < playerH.Sum(card => card.Value)) {//If dealer has less points than player
                string message = "Congrats, you won $" + bet.ToString();
                DialogResult dialogResult = MessageBox.Show(message, "You WON!!", MessageBoxButtons.OK);
                totalMoney += bet;//Bet is added to the total money
            } else if (pointsD > playerH.Sum(card => card.Value)) {//If dealer has more points than player
                totalMoney -= bet;//Player loose his bet
                MessageBox.Show("Dealer wins, you lost $" + bet.ToString());
            } else if (pointsD == playerH.Sum(card => card.Value)) {//If it's a draw
                MessageBox.Show("Draw, no one wins or loses.");
            }
            GameStats((playerH.Sum(card => card.Value)).ToString(), "0", totalMoney.ToString());//Print stats
            KeepGambling();//Ask the players if they want to play again
        }
        //Hit button. Player want to draw a new card
        private void hitBtn_Click(object sender, EventArgs e) {
            hitCount++;
            //Player can't have more than 5 cards
            if (hitCount > 3) {//If player has clicked more than three times the hit button
                stayBtn.PerformClick();//Send to the stay button
            } else {
                playerH.Add(DrawACard());//Drawn a new card
                int points = playerH.Sum(card => card.Value);//Get the new sum of the cards in player's hand
                Controls.Remove(tot);//Remove the total of points in the label
                GameStats(points.ToString(), bet.ToString(), totalMoney.ToString());//Print the stats with new sum of points
                PrintDeckP(playerH, 281);//Print the new cards of the players
                GamePlayer();//Player's turn. Verify the possible results with the new card drawn
            }
            
        }
        public StartGame() {
            InitializeComponent();
        }
        //Stop the application if the close button of the window is clicked
        protected override void OnFormClosing(FormClosingEventArgs e) {
            Environment.Exit(0);
        }
    }
    //Enumeration for the suits 
    public enum Suit {
        Heart,
        Spade,
        Diamond,
        Club
    }
    //Enumeration for the faces
    public enum Face {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
    }
    //Card's class
    public class Card {
        public Suit Suit { get; set; }//Suit: Hearts, Clubs, Spades, Diamonds
        public Face Face { get; set; }//Face e.g. Ace, J, K, 4, 2 etc.
        public int Value { get; set; }//Value for each face. J, Q and K have a value of 10. Ace has a value either 1 or 11
        public Image Name { get; set; }//Image obteined from the sprite
    }

    




}
