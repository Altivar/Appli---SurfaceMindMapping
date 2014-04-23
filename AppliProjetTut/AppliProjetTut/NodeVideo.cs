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

        SurfaceButton btnPlayPause;

        // Liste de videos
        ListeVideo listeVideo;

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

            listeVideo = new ListeVideo(this);
            CanScale = false;
            base.isEditing = false;
            base.TypeScatter.Background = new SolidColorBrush(Colors.DarkRed);

            videoElement = new MediaElement();
            videoElement.LoadedBehavior = MediaState.Manual;
            this.TypeScatter.Children.Add(videoElement);


            base.CanScale = true;
            base.SizeChanged += new SizeChangedEventHandler(OnNodeVideoSizeChanged);
            base.Width = 375;
            base.Height = 275;
            base.MaxHeight = 275;
            base.MaxWidth = 375;


            // modification de la barre des taches
            SurfaceButton btnVideoChoice = new SurfaceButton();
            btnVideoChoice.Width = 75;
            btnVideoChoice.Height = 75;
            ImageBrush imgBckg = new ImageBrush();
            imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_videos.gif", UriKind.Relative));
            btnVideoChoice.Background = imgBckg;
            base.grdButtonH.Children.Add(btnVideoChoice);
            btnVideoChoice.Margin = new Thickness(-75, 0, 75, 0);
            btnVideoChoice.Click += new RoutedEventHandler(OnVideoChoiceSelection);

            btnPlayPause = new SurfaceButton();
            btnPlayPause.Width = 75;
            btnPlayPause.Height = 75;
            ImageBrush imgBckg1 = new ImageBrush();
            imgBckg1.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_play.gif", UriKind.Relative));
            btnPlayPause.Background = imgBckg1;
            base.grdButtonH.Children.Add(btnPlayPause);
            btnPlayPause.Margin = new Thickness(0, 0, 0, 0);
            btnPlayPause.Click += new RoutedEventHandler(OnPlayPauseClick);

            SurfaceButton btnStop = new SurfaceButton();
            btnStop.Width = 75;
            btnStop.Height = 75;
            ImageBrush imgBckg2 = new ImageBrush();
            imgBckg2.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_stop.gif", UriKind.Relative));
            btnStop.Background = imgBckg2;
            base.grdButtonH.Children.Add(btnStop);
            btnStop.Margin = new Thickness(75, 0, -75, 0);
            btnStop.Click += new RoutedEventHandler(OnStopClick);

        }

        


        // EVENTS
        void OnVideoChoiceSelection(object sender, RoutedEventArgs e)
        {

            if (!base.isEditing)
            {
                base.AddonGrid.Items.Add(listeVideo);
                listeVideo.InitListView();

                base.Width = 375;
                base.Height = 275;
                base.MainGrid.Width = 375;
                base.MainGrid.Height = 275;
                base.TypeScatter.Width = 300;
                base.TypeScatter.Height = 200;

                base.AddonGrid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                base.isEditing = true;

                if (isOnPlay)
                {
                    videoElement.Pause();
                    ImageBrush imgBckg = new ImageBrush();
                    imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_play.gif", UriKind.Relative));
                    btnPlayPause.Background = imgBckg;
                    isOnPlay = false;
                }
            }

        }


        void OnNodeVideoSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isActive())
            {

                if (base.Width < 375 || base.Height < 275)
                {
                    base.Width = 375;
                    base.Height = 275;
                }

                base.MainGrid.Width = base.Width;
                base.MainGrid.Height = base.Height;
                base.TypeScatter.Height = base.Height - 75;
                base.TypeScatter.Width = base.Width - 75;

                this.grdButtonV.Margin = new Thickness(base.TypeScatter.Width, 0, 0, base.Height - this.grdButtonV.Height);
                if (base.isTextAnnotationOpened)
                {
                    base.grdButtonH.Margin = new Thickness(0, base.Height + 25, base.Width - 375, -100);
                    base.TextGrid.Margin = new Thickness(0, this.Height - 75, 75, -100);
                }
                else
                {
                    base.grdButtonH.Margin = new Thickness(0, base.Height - 75, base.Width - 375, 0);
                }

                base.textAnnotation.setWidth(this.TypeScatter.Width, isTextAnnotationOpened);
                base.TextGrid.Width = this.TypeScatter.Width;


            }

        }


        void OnPlayPauseClick(object sender, RoutedEventArgs e)
        {
            if (currentPath == "NONE")
                return;

            if (isEditing)
                return;

            if (isOnPlay)
            {
                videoElement.Pause();
                ImageBrush imgBckg = new ImageBrush();
                imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_play.gif", UriKind.Relative));
                btnPlayPause.Background = imgBckg;
                isOnPlay = false;
            }
            else
            {
                videoElement.Play();
                ImageBrush imgBckg = new ImageBrush();
                imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_pause.png", UriKind.Relative));
                btnPlayPause.Background = imgBckg;
                isOnPlay = true;
            }
        }

        void OnStopClick(object sender, RoutedEventArgs e)
        {
            if (currentPath == "NONE")
                return;
            
            if (isOnPlay)
            {
                videoElement.Pause();
                ImageBrush imgBckg = new ImageBrush();
                imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_play.gif", UriKind.Relative));
                btnPlayPause.Background = imgBckg;
                isOnPlay = false;
            }
            videoElement.Stop();
        }

        public void onCloseVideosList()
        {
            base.AddonGrid.Items.Remove(listeVideo);
            base.isEditing = false;
        }
        // END EVENTS

        public void SetVideoPath(string path)
        {

            if (path == "NONE")
                return;

            string videoPath = ".\\Resources\\Videos\\" + path;
            Uri uriPath = new Uri(videoPath, UriKind.Relative);
            videoElement.Source = uriPath;

            currentPath = path;

            base.MaxWidth = 800;
            base.MaxHeight = 700;

            base.AddonGrid.Items.Remove(listeVideo);
            isEditing = false;
            
        }

        
    }

}
