using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBrowsCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create OpenFileDialog 
                Microsoft.Win32.OpenFileDialog OfdTxtfile = new Microsoft.Win32.OpenFileDialog();
                OfdTxtfile.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = OfdTxtfile.ShowDialog();

                //Checking the result of Browsing
                if (result == true)
                {
                    string filename = OfdTxtfile.FileName;
                    TxtCsv.Text = filename;
                    //
                    Int64 Timestamp;
                    //Use RowNumber to Ignore the lines which are not usable
                    int Gazex, Gazey,RowNumber=0;
                    //Opens a text file, reads all lines of the file, and then closes the file.
                    string[] lines = System.IO.File.ReadAllLines(OfdTxtfile.FileName);
                    //Create List of Points which contain Gazex,Gazey
                    PointCollection collection = new PointCollection();
                    //Read each line and Draw the Lines
                    foreach(string line in lines)
                    {
                        RowNumber++;
                        //If beacuse The first three numbers are not Usable
                        if(RowNumber>6 )
                        {
                            var temp = line.Split('\t');
                            //If Timestamp is not null
                            if (temp[7].Length>0)
                            {
                                Timestamp = Int64.Parse(temp[7]);
                                //If Gazex and Gazey is not null
                                if (temp[23].Length > 0 && temp[24].Length > 0)
                                {
                                    Gazex = Int32.Parse(temp[23]);
                                    Gazey = Int32.Parse(temp[24]);
                                    //If Gazex and Gazey is inside the Image
                                    if (Gazex >= 0 && Gazex <= 1280 && Gazey >= 0 && Gazey <= 1024)
                                    {
                                        Point TP = new Point { X = Gazex, Y = Gazey };
                                        collection.Add(TP);
                                    }

                                }

                            }

                        }
                    }
                    DrawLine(collection);
                     
                }
                else
                {
                    MessageBox.Show("Please Select the File again");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnBrowsImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create OpenFileDialog 
                Microsoft.Win32.OpenFileDialog OfdTxtfile = new Microsoft.Win32.OpenFileDialog();
                OfdTxtfile.Filter = "JPEG Files: (*.JPG;*.JPEG;*.JPE;*.JFIF)|*.JPG;*.JPEG;*.JPE;";
                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = OfdTxtfile.ShowDialog();
                //Checking the result of Browsing and Show the Image
                if (result == true)
                {
                    string Imagefilepath = OfdTxtfile.FileName;
                    TxtImg.Text = Imagefilepath;
                    BitmapImage bitImg = new BitmapImage(new Uri(TxtImg.Text, UriKind.RelativeOrAbsolute));
                    ImgMain.Source = bitImg;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //function to create line and show in the image
        private void DrawLine(PointCollection collection)
        {
            Polyline newline = new Polyline();
            newline.Points = collection;
            newline.Stroke = new SolidColorBrush(Colors.Yellow);
            newline.StrokeThickness = 2;
            CurrentImage.Children.Add(newline);
        }

      
    }
}
