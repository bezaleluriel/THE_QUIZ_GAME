using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QUIZ_GAME
{
    /// <summary>
    /// Interaction logic for HighScoresWindow.xaml
    /// </summary>
    public partial class HighScoresWindow : Window
    {
        DB_Connect db;
        List<Label> userLbls;
        List<Label> scrLbls;
        public HighScoresWindow(bool gameFinished, string wonMoney)
        {


            db = new DB_Connect();
            InitializeComponent();
            if (gameFinished)
            {
                stackPanelGameFinish.Visibility = Visibility.Visible;
                moneyLbl.Content = wonMoney + " $";
            }
            userLbls = new List<Label>();
            userLbls.Add(lblUsr1);
            userLbls.Add(lblUsr2);
            userLbls.Add(lblUsr3);
            userLbls.Add(lblUsr4);
            userLbls.Add(lblUsr5);
            userLbls.Add(lblUsr6);
            userLbls.Add(lblUsr7);
            userLbls.Add(lblUsr8);
            userLbls.Add(lblUsr9);
            userLbls.Add(lblUsr10);

            scrLbls = new List<Label>();
            scrLbls.Add(lblScr1);
            scrLbls.Add(lblScr2);
            scrLbls.Add(lblScr3);
            scrLbls.Add(lblScr4);
            scrLbls.Add(lblScr5);
            scrLbls.Add(lblScr6);
            scrLbls.Add(lblScr7);
            scrLbls.Add(lblScr8);
            scrLbls.Add(lblScr9);
            scrLbls.Add(lblScr10);
            createHighScoreTable();


        }

        public void createHighScoreTable()
        {
            List<string>[] highScoreList = db.GetHighScores();
            for (int i = 0; i < highScoreList[0].Count; i++)
            {
                userLbls[i].Content = highScoreList[0].ElementAt(i);
                userLbls[i].Visibility = Visibility.Visible;
                scrLbls[i].Content = highScoreList[1].ElementAt(i);
                scrLbls[i].Visibility = Visibility.Visible;
            }
            //// Creating DataSource here as datatable having two columns
            //DataTable dt = new DataTable();
            //dt.Columns.Add("User Name", typeof(string));
            //dt.Columns.Add("Score");

            //// Adding the rows in datatable
            //for (int i = 0; i < highScoreList[0].Count; i++)
            //{
            //    var row = dt.NewRow();
            //    row["User Name"] = highScoreList[0].ElementAt(i);
            //    row["Score"] = highScoreList[0].ElementAt(i);
            //    dt.Rows.Add(row);
            //}
            //highScoresTable.AutoGenerateColumns = true;
            //highScoresTable.DataContext = dt.DefaultView;
        }

        private void btnA3_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
