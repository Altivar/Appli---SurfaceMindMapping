using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Surface.Presentation.Controls;

using System.IO;

namespace SurfaceMindMapping
{
    /// <summary>
    /// Logique d'interaction pour ListeSauvegarde.xaml
    /// </summary>
    public partial class ListeSauvegarde : ScatterViewItem
    {

        // lien entre la liste et la liste des fichiers
        MenuPrincipal MenuParent;

        // liste de bouton
        List<string> listButton = new List<string>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public ListeSauvegarde(MenuPrincipal parent)
        {
            InitializeComponent();

            MenuParent = parent;

            InitSaveList();
        }

        /// <summary>
        /// Initialise les liste de Sauvegarde
        /// </summary>
        public double InitSaveList()
        {

            listButton.Clear();
            ButtonListGrid.Children.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves\\");
            
            DirectoryInfo[] DirSaves = dirInfo.GetDirectories();
            foreach (DirectoryInfo dir in DirSaves)
            {   
                string nomSauvegarde = dir.FullName;
                char[] separator = { '\\' };
                string[] nomPartition = nomSauvegarde.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                listButton.Add(nomPartition.Last());
             }



            // scroll viewer
            this.listButtonSV.Height = (listButton.Count * 50 > 210) ? 210 : listButton.Count * 50;
            // grid du scroll viewer
            this.ButtonListGrid.Height = listButton.Count * 50;
            // main grid
            this.ButtonsGrid.Height = (listButton.Count * 50 > 210) ? 210 : listButton.Count * 50;
            // object
            this.Height = (listButton.Count * 50 > 210) ? 210 : listButton.Count * 50;


            for (int i = 0; i < listButton.Count; i++)
            {
                SurfaceButton btn = new SurfaceButton();
                btn.Width = 200;
                btn.Height = 50;
                btn.Content = listButton.ElementAt(i);
                btn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                btn.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                btn.Foreground = Brushes.White;
                btn.Margin = new Thickness(0, i * 50, 0, (listButton.Count - (i+1)) * 50);
                btn.Opacity = 0.6;
                if (i % 2 == 0)
                {
                    btn.Background = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    btn.Background = new SolidColorBrush(Colors.DodgerBlue);
                }

                btn.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnFileButtonPreviewTouchUp);
                ButtonListGrid.Children.Add(btn);
            }

            return ButtonListGrid.Height;
        }

        /// <summary>
        /// Lors de la sélection d'un bouton : ouvre le fichier correspondant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFileButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            SurfaceButton senderBtn = (SurfaceButton)sender;
            if (senderBtn == null)
                return;

            
            string fileContent = (string)senderBtn.Content;
            if (fileContent == null)
                return;
            
            MenuParent.OnFileSelection(fileContent);

        }

    }
}
