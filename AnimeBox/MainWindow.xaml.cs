using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Threading;



namespace AnimeBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
 
    public partial class MainWindow : Window
    {

        OpenFileDialog ofd = new OpenFileDialog();
        OpenFileDialog openimage = new OpenFileDialog();
        DispatcherTimer timer = new DispatcherTimer();
        ObservableCollection<Songdata> Listsong = new ObservableCollection<Songdata>();
        ReadMp3 mp3 = new ReadMp3();
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(600);
            timer.Tick += new EventHandler(timer_Tick);
        }


        #region //窗体功能
        public void minwin()
        {
            this.WindowState = WindowState.Minimized;
        }

        public void closewin()
        {
            this.Close();
        }



        private void saveListsong()
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Listsong.txt", append: false);
            for (int i = 0; i < Listsong.Count; i++)
            {
                sw.WriteLine(Listsong[i].Source);
                sw.WriteLine(Listsong[i].Name);
                sw.WriteLine(Listsong[i].Title);
                sw.WriteLine(Listsong[i].Artist);
                sw.WriteLine(Listsong[i].Album);
                sw.WriteLine(Listsong[i].Year);
            }
            sw.Close();
        }

        private void readerListsongs()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Listsong.txt"))
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Listsong.txt");
                while (sr.EndOfStream == false)
                {
                    string source = sr.ReadLine();
                    string name = sr.ReadLine();
                    string title = sr.ReadLine();
                    string artist = sr.ReadLine();
                    string album = sr.ReadLine();
                    string year = sr.ReadLine();
                    Listsong.Add(new Songdata { Source = source,Name = name,Title = title,Artist = artist,Album = album,Year = year});
                }
                sr.Close();
            }
        }

        private void saveimage()
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "background.txt", append: false);
            sw.WriteLine(openimage.FileName);
            sw.Close();
        }

        private void readerimage()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "background.txt"))
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "background.txt");
                BitmapImage images01 = new BitmapImage(new Uri(sr.ReadLine()));
                background.Source = images01;
                sr.Close();
            }
        }

        //-------------------------------------------------------分割线------------------------------------------------------------------------------
        #endregion


        #region  // 音乐功能


        public void music(string i)
        {
            if (listsongs.Items.Count != 0)
            {
                media.Source = new Uri(i);
                btngo.Visibility = Visibility.Hidden;
                btnpause.Visibility = Visibility.Visible;
                media.Play();
            }
        }
        public void play()
        {
            if (listsongs.Items.Count != 0)
            {
                if (media.HasAudio == false)
                {
                    music(Listsong[listsongs.SelectedIndex].Source);
                }
                btngo.Visibility = Visibility.Hidden;
                btnpause.Visibility = Visibility.Visible;
                media.Play();
            }
        }
        public void pause()
        {
            btnpause.Visibility = Visibility.Hidden;
            btngo.Visibility = Visibility.Visible;
            media.Pause();
        }

        public void stop()
        {
            btnpause.Visibility = Visibility.Hidden;
            btngo.Visibility = Visibility.Visible;
            media.Stop();
            lbname.Content = "AnimeMusic";
        }

        private void openmedia()
        {
            if (mycd.Visibility == Visibility.Visible)
            {
                lbname.Content = System.IO.Path.GetFileName(Listsong[datacd.SelectedIndex].Source);
            }
            else
            {
                lbname.Content = System.IO.Path.GetFileName(Listsong[listsongs.SelectedIndex].Source);
            }
        }

        public void add()
        {
            ofd.Filter = "音乐文件|*.mp3;*wma";
            ofd.Multiselect = true;
            ofd.Title = "添加歌曲";
            if (ofd.ShowDialog() == true)
            {
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    string[] name = ofd.FileNames[i].Split('\\');
                    mp3.RenderMp3(ofd.FileNames[i]);
                    Listsong.Add(new Songdata { Source = ofd.FileNames[i], Name = name[name.Length - 1], Title = mp3.title, Artist = mp3.artist, Album = mp3.album, Year = mp3.year });
                }
            }
        }

        public void openbackground()
        {
            openimage.Filter = "图片|*.jpg;*.png";
            openimage.Title = "设置背景图片";
            if (openimage.ShowDialog() == true)
            {
                BitmapImage images01 = new BitmapImage(new Uri(openimage.FileName));
                background.Source = images01;
                saveimage();
            }
        }

        public void next()
        {
            if (listsongs.SelectedIndex == listsongs.Items.Count - 1)
            {
                listsongs.SelectedIndex = 0;
                music(Listsong[listsongs.SelectedIndex].Source);
            }
            else
            {
                listsongs.SelectedIndex++;
                music(Listsong[listsongs.SelectedIndex].Source);
            }
        }

        public void back()
        {
            if (listsongs.SelectedIndex == 0)
            {
                listsongs.SelectedIndex = listsongs.Items.Count - 1;
                music(Listsong[listsongs.SelectedIndex].Source);
            }
            else
            {
                listsongs.SelectedIndex--;
                music(Listsong[listsongs.SelectedIndex].Source);
            }
        }

        public void doubleclick()
        {
            if (datacd.Visibility == Visibility.Visible)
            {
                if (datasongs.SelectedCells.Count > 0)
                {
                    music(Listsong[datacd.SelectedIndex].Source);
                }
            }
            else
            {
                if (datasongs.SelectedCells.Count > 0)
                {
                    music(Listsong[datasongs.SelectedIndex].Source);
                }
            }
        }

        public void offg1()
        {
            if (g1.Visibility == Visibility.Visible)
            {
                g1.Visibility = Visibility.Hidden;
            }
            else
            {
                g1.Visibility = Visibility.Visible;
            }
        }

        public void clear()
        {
            Listsong.Clear();
            stop();
        }

        private void delete()
        {
            if (listsongs.SelectedIndex != -1)
            {
                if (media.HasAudio == true)
                {
                    Listsong.RemoveAt(listsongs.SelectedIndex);
                    music(Listsong[listsongs.SelectedIndex].Source);
                }
                else
                    Listsong.RemoveAt(listsongs.SelectedIndex);
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (media.HasAudio==true)
            {
                musicsli.ValueChanged -= musicsli_ValueChanged;
                musicsli.Value = (media.Position.TotalSeconds / media.NaturalDuration.TimeSpan.TotalSeconds) * 10;
                musicsli.ValueChanged += musicsli_ValueChanged;
                musictime.Content = string.Format("{1:D2}:{2:D2}", media.Position.Hours, media.Position.Minutes, media.Position.Seconds);
            }
        }

        private void musicsli_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (media.HasAudio == true)
            {
                media.Position = TimeSpan.FromSeconds((media.NaturalDuration.TimeSpan.TotalSeconds * musicsli.Value) / 10);
            }
        }

        public void volume()
        {
            if (btnvolume.Visibility == Visibility.Visible)
            {
                media.IsMuted = !media.IsMuted;
                btnvolume.Visibility = Visibility.Hidden;
                btnoffvolume.Visibility = Visibility.Visible;
            }
            else
            {
                media.IsMuted = !media.IsMuted;
                btnvolume.Visibility = Visibility.Visible;
                btnoffvolume.Visibility = Visibility.Hidden;
            }
        }


        //  ------------------------------------------------------------分割线------------------------------------------------------------------------
        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveListsong();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            readerimage();
            readerListsongs();
            lbcount.Content = Listsong.Count + " 首";
            datasongs.ItemsSource = Listsong;
            datacd.ItemsSource = Listsong;
            listsongs.ItemsSource = Listsong;
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            openmedia();
            timer.Start();
        }

        private void btnpause_Click(object sender, RoutedEventArgs e)
        {
            pause();
        }

        private void btngo_Click(object sender, RoutedEventArgs e)
        {
            play();
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            add();
        }

        private void btnfor_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            next();
        }
        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            back();
        }
        private void listsongs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            doubleclick();
        }
        private void btnvolume_Click(object sender, RoutedEventArgs e)
        {
            volume();
        }

        private void btnoffvolume_Click(object sender, RoutedEventArgs e)
        {
            volume();
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            next();
        }

        private void offg1_Click(object sender, RoutedEventArgs e)
        {
            offg1();
        }

        private void btnmin_Click(object sender, RoutedEventArgs e)
        {
            minwin();
        }

        private void btnbackground_Click(object sender, RoutedEventArgs e)
        {
            openbackground();
        }

        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            closewin();
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            delete();
        }

        private void btnclear_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void btnmysongs_Click(object sender, RoutedEventArgs e)
        {
            if (mysongs.Visibility == Visibility.Hidden)
            {
                mysongs.Visibility = Visibility.Visible;
                g1.Visibility = Visibility.Hidden;
                mycd.Visibility = Visibility.Hidden;
            }
            else
            {
                mysongs.Visibility = Visibility.Hidden;
                g1.Visibility = Visibility.Visible;
            }
        }

        private void btnmycd_Click(object sender, RoutedEventArgs e)
        {
            if (mycd.Visibility == Visibility.Hidden)
            {
                mycd.Visibility = Visibility.Visible;
                g1.Visibility = Visibility.Hidden;
                mysongs.Visibility = Visibility.Hidden;
            }
            else
            {
                mycd.Visibility = Visibility.Hidden;
                g1.Visibility = Visibility.Visible;
            }
        }

        private void btnhome_Click(object sender, RoutedEventArgs e)
        {
            mysongs.Visibility = Visibility.Hidden;
            mycd.Visibility = Visibility.Hidden;
            g1.Visibility = Visibility.Hidden;
        }

        private void datasongs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            doubleclick();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void datasongs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            listsongs.SelectedIndex = datasongs.SelectedIndex;
            lbcount.Content = Listsong.Count + " 首";
        }

        private void listsongs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            datasongs.SelectedIndex = listsongs.SelectedIndex;
            lbcount.Content = Listsong.Count + " 首";
        }

      

      

      

 






































    }
}






























