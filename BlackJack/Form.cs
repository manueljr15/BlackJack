using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class MainMenu : Form
    {
        public MainMenu(){
            InitializeComponent();
        }
        //Actions when start button is clicked
        private void buttonStart_Click(object sender, EventArgs e){
            StartGame newGame = new StartGame();
            this.Hide();
            newGame.Show();
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
            Environment.Exit(0);
        }

    }
}
