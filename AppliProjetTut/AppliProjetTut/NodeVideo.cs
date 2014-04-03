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

namespace AppliProjetTut
{

    /// <summary>
    /// Noeud représentant une image
    /// </summary>
    public partial class NodeVideo : ScatterCustom
    {



        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;


        // Video
        MediaElement videoElement;
        bool isOnPlay = false;
        string currentPath = "NONE";

        // Liste de videos
        ListeVideo listeVideo;

        // Gestion d'édition
        bool isEditing;


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public NodeVideo(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            base.SetTypeOfNode("Video");


            ElementMenuItem MenuItem1 = new ElementMenuItem();
            MenuItem1.Header = "Video choice";
            MenuItem1.Click += new RoutedEventHandler(OnVideoChoiceSelection);
            base.MainMenu.Items.Add(MenuItem1);

            ElementMenuItem MenuItem2 = new ElementMenuItem();
            MenuItem2.Header = "Play/Pause";
            MenuItem2.Click += new RoutedEventHandler(OnPlayPausePreviewTouchUp);
            base.MainMenu.Items.Add(MenuItem2);

            listeVideo = new ListeVideo(this);
            CanScale = false;
            isEditing = false;
            base.MainGrid.Background = new SolidColorBrush(Colors.DarkRed);

            videoElement = new MediaElement();
            videoElement.LoadedBehavior = MediaState.Manual;
            this.TypeScatter.Children.Add(videoElement);

        }

        


        // EVENTS
        void OnVideoChoiceSelection(object sender, RoutedEventArgs e)
        {

            if (!isEditing)
            {
                base.AddonGrid.Items.Add(listeVideo);
                listeVideo.InitListView();

                double borderHeight = (base.Height - base.MainGrid.Height) / 2;
                base.AddonGrid.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
                base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                isEditing = true;
                videoElement.Pause();
                isOnPlay = false;
                base.MainGrid.Background = new SolidColorBrush(Colors.DarkRed);
            }

        }


        void OnPlayPausePreviewTouchUp(object sender, RoutedEventArgs e)
        {
            if (currentPath == "NONE")
                return;

            if (isOnPlay)
            {
                videoElement.Pause();
                base.MainGrid.Background = new SolidColorBrush(Colors.DarkRed);
                isOnPlay = false;
            }
            else
            {
                videoElement.Play();
                base.MainGrid.Background = new SolidColorBrush(Colors.DarkGreen);
                isOnPlay = true;
            }
        }

        public void onCloseVideosList()
        {
            base.MainGrid.Background = new SolidColorBrush(Colors.DarkRed);
            base.AddonGrid.Items.Remove(listeVideo);
            isEditing = false;
        }
        // END EVENTS

        public void GetVideoPath(string path)
        {

            if (path == "NONE")
                return;

            string videoPath = ".\\Resources\\Videos\\" + path;
            Uri uriPath = new Uri(videoPath, UriKind.Relative);
            videoElement.Source = uriPath;

            currentPath = path;

            base.AddonGrid.Items.Remove(listeVideo);
            isEditing = false;

        }

        
    }

}
