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

namespace AppliProjetTut
{
    /// <summary>
    /// Logique d'interaction pour ListeVideo.xaml
    /// </summary>
    public partial class ListeVideo : ScatterViewItem
    {


        int imgSize = 130;

        int imgBorder = 5;

        // NodeImage a laquelle il est rattaché
        NodeVideo nodeParent;

        // liste de bouton
        List<SurfaceButton> listButton = new List<SurfaceButton>();


        public ListeVideo(NodeVideo parent)
        {
            InitializeComponent();

            nodeParent = parent;

            CanScale = false;
            CanMove = false;
            CanRotate = false;

            ButtonListGrid.Height = 100;

            InitListView();
        }





        //
        //   FONCTION D'INITIALISATION DE LA LISTE DE VIDEO
        //
        /// <summary>
        /// Initialise la liste de video
        /// </summary>
        public void InitListView()
        {
            listButton.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Resources\\Videos");
            try
            {
                FileInfo[] VidFiles = dirInfo.GetFiles();
                foreach (FileInfo video in VidFiles)
                {
                    string nomVideo = video.FullName;
                    char[] separator = { '\\' };
                    string[] nomPartition = nomVideo.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    if (isVideo(nomPartition.Last()))
                    {
                        SurfaceButton btnVid = new SurfaceButton();
                        btnVid.Width = imgSize;
                        btnVid.Height = imgSize;

                        btnVid.Content = nomPartition.Last();

                        listButton.Add(btnVid);
                    }
                }

            }
            catch { };

            ButtonListGrid.Width = listButton.Count * imgSize;
            ButtonListGrid.Height = imgSize;
            for (int i = 0; i < listButton.Count; i++)
            {
                SurfaceButton btn = listButton.ElementAt(i);
                if (i == listButton.Count - 1)
                {
                    btn.Margin = new Thickness(imgSize * i + imgBorder, 0, imgSize * (listButton.Count - 1 - i) + imgBorder, 0);
                }
                else
                {
                    btn.Margin = new Thickness(imgSize * i + imgBorder, 0, imgSize * (listButton.Count - 1 - i), 0);
                }

                btn.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnVideoButtonPreviewTouchUp);
                ButtonListGrid.Children.Add(btn);
            }

        }


        /// <summary>
        /// Envoie le nom de la video au Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnVideoButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            
            SurfaceButton btnClicked = (SurfaceButton)sender;
            if(btnClicked == null)
                return;
            string newPath = (string)btnClicked.Content;
            if (newPath == null)
                return;

            string currentPath = Directory.GetCurrentDirectory() + "\\Resources\\Videos\\" + newPath;
            nodeParent.SetVideoPath(currentPath);

        }

        /// <summary>
        /// Vérifie si le fichier passé en paramètre est une video
        /// true : extension de video reconnue
        /// false : extension non reconnue
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        private bool isVideo(string nom)
        {

            char[] separator = { '.' };
            string[] nomSplitte = nom.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            switch (nomSplitte.Last())
            {
                case "avi":
                case "mp4":
                    return true;
                default:
                    return false;
            }


        }



        //
        //   INTERACTION AVEC LA LISTE DE VIDEOS
        //
        /// <summary>
        /// Annule le choix de la video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            nodeParent.onCloseVideosList();
        }




        // FIN CLASSE
        //


    }
}
