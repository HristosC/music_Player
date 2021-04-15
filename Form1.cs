using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using TagLib;
using Microsoft.VisualBasic;


namespace Music_Player_Final
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            listbox.Items.Clear();//erases all elements of listbox
            listBox1.Items.Clear();//erases all elements of listbox


            foreach (string line in Globals.songs)
            {
                Globals.counter += 1; 

            }
            for (int i = 0; i < Globals.counter; i++)
            {
                try
                {
                    var file = TagLib.File.Create(Globals.songs[i]);
                    if (file.Tag.Title == null)
                    {
                        listbox.Items.Add("Unknown Title");
                    }
                    else
                    {
                        listbox.Items.Add(file.Tag.Title);
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("A file is missing. ( A song file propably ) ");
                }
                
                
                
            }
            favorite info1 = new favorite();
            info1.Vale(listBox1);
        }

        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog mp3 = new OpenFileDialog();
            mp3.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (mp3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string mp3_name = mp3.FileName;
                Globals.songs.Add(mp3_name);
                Globals.favorites.Add("0");
                Globals.language.Add("Unknown");
                System.IO.File.WriteAllLines("songs.txt", Globals.songs);
                System.IO.File.WriteAllLines("favorite.txt", Globals.favorites);
                System.IO.File.WriteAllLines("language.txt", Globals.language);
                var file = TagLib.File.Create(Globals.songs[Globals.counter]);
                if (file.Tag.Title == null)
                {
                    listbox.Items.Add("Unkown Title");
                }
                else { listbox.Items.Add(file.Tag.Title); }
                
                Globals.counter++;
                favorite info1 = new favorite();
                info1.Vale(listBox1);
            }
        }

        private void listbox_DoubleClick(object sender, EventArgs e)
        {
            
            Globals.index = this.listbox.SelectedIndex;
            if (Globals.index >= 0)
            {
                button6.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
                button11.Visible = true;
            }
            if ((Globals.tracker != Globals.index) && (Globals.counterA > 0))
            {
                Globals.counterA = 0;
            }
            if (Globals.counterA == 0 && Globals.index != -1)
            {
                Globals.tracker = Globals.index;
                Globals.WPlayer.URL = Globals.songs[Globals.tracker];
                Globals.counterA++;
                Globals.favorites[Globals.tracker] = (Int32.Parse(Globals.favorites[Globals.tracker]) + 1).ToString() ;
                System.IO.File.WriteAllLines("favorite.txt", Globals.favorites);
                Globals.counterStop = 0;
            }
            
            if (Globals.index != -1)
            {
                Info info = new Info();
                info.ChangeLabelText(label3, label4, label5, label6, listbox,label8);
            }
            favorite info1 = new favorite();
            info1.Vale(listBox1);



        }

        private void button3_Click(object sender, EventArgs e)
        {
            Globals.WPlayer.controls.play();
            if (Globals.counterStop == 1)
            {
                Globals.favorites[Globals.tracker] = (Int32.Parse(Globals.favorites[Globals.tracker]) + 1).ToString();
                System.IO.File.WriteAllLines("favorite.txt", Globals.favorites);
                Globals.counterStop = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Globals.WPlayer.controls.pause();
            
        }

        private void listbox_Click(object sender, EventArgs e)
        {
            Globals.index = this.listbox.SelectedIndex;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Globals.WPlayer.controls.stop();
            Globals.counterStop = 1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Globals.tracker == listbox.SelectedIndex)
            {
                Globals.WPlayer.URL = null;
            }
            if (listbox.SelectedIndex == -1)
            {

            }
            else
            {
                Globals.songs.RemoveAt(listbox.SelectedIndex);
                Globals.favorites.RemoveAt(listbox.SelectedIndex);
                Globals.language.RemoveAt(listbox.SelectedIndex);
                System.IO.File.WriteAllLines("language.txt", Globals.language);
                System.IO.File.WriteAllLines("songs.txt", Globals.songs);
                System.IO.File.WriteAllLines("favorite.txt", Globals.favorites);
                listbox.Items.Remove(listbox.Items[listbox.SelectedIndex]);
                Globals.counter--;
                favorite info1 = new favorite();
                info1.Vale(listBox1);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int random = rnd.Next(Globals.counter);
            Globals.WPlayer.URL = Globals.songs[random];
            Globals.tracker = random;
            listbox.SelectedIndex = random;
            Globals.index = random;
            Globals.favorites[Globals.tracker] = (Int32.Parse(Globals.favorites[Globals.tracker]) + 1).ToString();
            System.IO.File.WriteAllLines("favorite.txt", Globals.favorites);
            Info info = new Info();
            info.ChangeLabelText(label3, label4, label5, label6, listbox,label8);

        }
        private void button6_Click(object sender, EventArgs e)
        {

            var file = TagLib.File.Create(Globals.songs[Globals.tracker]);
            Globals.WPlayer.URL = null;
            string performers = "";
            try
            {
                performers = file.Tag.Performers[0];
            }
            catch (System.IndexOutOfRangeException)
            {
                performers = "Unknown Artist";
            }
            file.Tag.Performers = null;
            file.Tag.Performers = new[] { Interaction.InputBox("Change Artist Name", "Change", "", -1, -1) };
            try
            {
                if (file.Tag.Performers[0] == null)
                {
                    file.Tag.Performers[0] = performers;
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                file.Tag.Performers = new[] { performers };
            }
            label3.Text = "Όνομα Καλλιτέχνη: " + file.Tag.Performers[0];
            try
            {
                file.Save();
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("The file is locked and the changes cannot be saved.");
            }
            Globals.WPlayer.URL = Globals.songs[Globals.tracker];
        }
        private void button7_Click(object sender, EventArgs e)
        {
            var file = TagLib.File.Create(Globals.songs[Globals.tracker]);
            Globals.WPlayer.URL = null;
            try
            {
                file.Tag.Year = uint.Parse(Interaction.InputBox("Change Release Year", "Chage", "", -1, -1));
            }
            catch (System.FormatException)
            {

            }
            label4.Text = "Release Year: " + file.Tag.Year;
            Globals.WPlayer.URL = Globals.songs[Globals.tracker];
            try
            {
                file.Save();
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("The file is locked and the changes cannot be saved.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var file = TagLib.File.Create(Globals.songs[Globals.tracker]);
            Globals.WPlayer.URL = null;
            try
            {
                file.Tag.Genres = null;
            }
            catch(System.IndexOutOfRangeException )
            {

            }
            file.Tag.Genres = new[] { Interaction.InputBox("Change Genre", "Change", "", -1, -1) } ;

            try
            {
                label5.Text = "Genre: " + file.Tag.Genres[0];
            }
            catch(System.IndexOutOfRangeException )
            {
                
            }
            Globals.WPlayer.URL = Globals.songs[Globals.tracker];
            try
            {
                file.Save();
            }
            catch(System.UnauthorizedAccessException )
            {
                MessageBox.Show("The file is locked and the changes cannot be saved.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string language = Globals.language[Globals.tracker];
            Globals.language[Globals.tracker] = Interaction.InputBox("Change Language", "Change", "", -1, -1);
            if (Globals.language[Globals.tracker] == "")
            {
                Globals.language[Globals.tracker] = language;
            }
            System.IO.File.WriteAllLines("language.txt", Globals.language);
            label6.Text = "Language: " + Globals.language[Globals.tracker];
        }
        private void button11_Click(object sender, EventArgs e)
        {
            var file = TagLib.File.Create(Globals.songs[Globals.tracker]);
            Globals.WPlayer.URL = null;
            string title = file.Tag.Title;
            file.Tag.Title = null;
            file.Tag.Title = Interaction.InputBox("Change Title", "Change", "", -1, -1);
            if (file.Tag.Title == null)
            {
                file.Tag.Title = title;
            }
            label8.Text = "Title: " + file.Tag.Title;
            Globals.WPlayer.URL = Globals.songs[Globals.tracker];
            try
            {
                file.Save();
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("The file is locked and the changes cannot be saved.");
            }
            try
            {
                listbox.Items[Globals.tracker] = file.Tag.Title.ToString();
            }
            catch (System.NullReferenceException)
            {
                file.Tag.Title = title;

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Globals.language.Count().ToString());
        }
    }
}
public class Globals
{
    public static List<string> songs = System.IO.File.ReadAllLines("songs.txt").ToList();  //pairnei to onoma toy mp3 poy epi8imoyme apo to to songs.txt kai to pernaei se mia lista
    public static List<string> favorites = System.IO.File.ReadAllLines("favorite.txt").ToList();//vazei to poses fores exei paixtei ena tragoydaki 
    public static List<string> language = System.IO.File.ReadAllLines("language.txt").ToList();//glwssa tragoydioy 
    public static int counter = 0;
    public static int counterA = 0;
    public static int counterStop = 0;
    public static int index = -1;
    public static WMPLib.WindowsMediaPlayer WPlayer = new WMPLib.WindowsMediaPlayer();
    public static int tracker;
    
}
class Info
{
    public void ChangeLabelText(Label l3,Label l4,Label l5,Label l6,ListBox list,Label l7)
    {
        l3.Text = "Artist:";
        l4.Text = "Release Year:";
        l5.Text = "Genre:";
        l6.Text = "Language:";
        l7.Text = "Title: ";
        var file = TagLib.File.Create(Globals.songs[Globals.index]);
        if (file.Tag.Title == null)
        {
            l7.Text = l7.Text + "Unknown Title";
        }
        else
        {
            l7.Text = l7.Text + file.Tag.Title;
        }
       
        try
        {
            l3.Text = l3.Text + " " + file.Tag.Performers[0];
        }
        catch(System.IndexOutOfRangeException )
        {
            l3.Text = l3.Text + " " + "Unknown Artist";
        }

        
        l4.Text = l4.Text + " " + file.Tag.Year;
        try
        {
            l5.Text = l5.Text + " " + file.Tag.Genres[0];
        }
        catch(System.IndexOutOfRangeException )
        {
            l5.Text = l5.Text + " " + "Unknown Genre";
        }
        l6.Text = l6.Text + " " + Globals.language[Globals.tracker];
    }
}
class favorite
{
    public void Vale(ListBox list)
    {
        String stdDetails = "{0,-50}{1,-20}";
        list.Items.Clear();
        list.Items.Add(String.Format(stdDetails, "Replays", "Title"));
        List<int> top_songs = new List<int>();
        List<string> songs = new List<string>();
        int temp_top = 0;
        string temp_songs = "";
        for (int i=0; i < Globals.counter; i++) 
        {
            top_songs.Add(Int32.Parse(Globals.favorites[i]));
            try
            {
                var file = TagLib.File.Create(Globals.songs[i]);
                if (file.Tag.Title == null)
                {
                    songs.Add("Unknown Title");
                }
                else
                {
                    songs.Add(file.Tag.Title);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("A file is missing.");
            }

        }
        for (int i = 0; i < top_songs.Count(); i++)
        {
            for(int j = 1; j < (top_songs.Count() - i); j++)
            {
                if (top_songs[j - 1] < top_songs[j])
                {
                    temp_top = top_songs[j - 1];
                    top_songs[j - 1] = top_songs[j];
                    top_songs[j] = temp_top;
                    temp_songs = songs[j - 1];
                    songs[j - 1] = songs[j];
                    songs[j] = temp_songs;
                }
            }
        }
        stdDetails = "{0,-40}{1,-20}";
        for (int i = 0; i < top_songs.Count(); i++)
        {
            list.Items.Add(String.Format(stdDetails, top_songs[i], songs[i]));
        }
    }
}


