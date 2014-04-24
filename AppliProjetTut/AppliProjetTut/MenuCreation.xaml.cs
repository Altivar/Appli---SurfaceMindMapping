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

using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace AppliProjetTut
{
    /// <summary>
    /// Logique d'interaction pour MenuCreation.xaml
    /// </summary>
    public partial class MenuCreation : ScatterViewItem
    {


        // surface window parent
        SurfaceWindow1 parentWindow;

        Storyboard sb;
        Storyboard sbclose;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public MenuCreation(SurfaceWindow1 parent)
        {
            InitializeComponent();

            parentWindow = parent;



            this.ShowsActivationEffects = false;
            this.BorderBrush = System.Windows.Media.Brushes.Transparent;





            mCircleText.PreviewTouchDown += new EventHandler<TouchEventArgs>(TextButtonClick);
            mCircleImage.PreviewTouchDown += new EventHandler<TouchEventArgs>(ImageButtonClick);
            mCircleVideo.PreviewTouchDown += new EventHandler<TouchEventArgs>(VideoButtonClick);

            mCircleCross.PreviewTouchDown += new EventHandler<TouchEventArgs>(CloseMenuClick);

            
            ellText.PreviewTouchDown += new EventHandler<TouchEventArgs>(TextButtonClick);
            ellImage.PreviewTouchDown += new EventHandler<TouchEventArgs>(ImageButtonClick);
            ellVideo.PreviewTouchDown += new EventHandler<TouchEventArgs>(VideoButtonClick);

            sb = new Storyboard();
            sbclose = new Storyboard();

            DoubleAnimation textAnimOpening = new DoubleAnimation
            {
                From = 0,
                To = 40,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(0.4)
            };
            DoubleAnimation imageAnimOpening = new DoubleAnimation
            {
                From = 0,
                To = 40,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(0.6)
            };
            DoubleAnimation videoAnimOpening = new DoubleAnimation
            {
                From = 0,
                To = 40,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(0.8)
            };

            DoubleAnimation textAnimClosing = new DoubleAnimation
            {
                From = 40,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(4.6)
            };
            DoubleAnimation imageAnimClosing = new DoubleAnimation
            {
                From = 40,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(4.6)
            };
            DoubleAnimation videoAnimClosing = new DoubleAnimation
            {
                From = 40,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
                BeginTime = TimeSpan.FromSeconds(4.6)
            };

            Storyboard.SetTargetProperty(textAnimOpening, new PropertyPath(Ellipse.WidthProperty));
            Storyboard.SetTarget(textAnimOpening, ellText);
            Storyboard.SetTargetProperty(imageAnimOpening, new PropertyPath(Ellipse.WidthProperty));
            Storyboard.SetTarget(imageAnimOpening, ellImage);
            Storyboard.SetTargetProperty(videoAnimOpening, new PropertyPath(Ellipse.WidthProperty));
            Storyboard.SetTarget(videoAnimOpening, ellVideo);

            Storyboard.SetTargetProperty(textAnimClosing, new PropertyPath(Ellipse.HeightProperty));
            Storyboard.SetTarget(textAnimClosing, ellText);
            Storyboard.SetTargetProperty(imageAnimClosing, new PropertyPath(Ellipse.HeightProperty));
            Storyboard.SetTarget(imageAnimClosing, ellImage);
            Storyboard.SetTargetProperty(videoAnimClosing, new PropertyPath(Ellipse.HeightProperty));
            Storyboard.SetTarget(videoAnimClosing, ellVideo);

            sb.Children.Add(textAnimOpening);
            sb.Children.Add(imageAnimOpening);
            sb.Children.Add(videoAnimOpening);
            sb.Children.Add(textAnimClosing);
            sb.Children.Add(imageAnimClosing);
            sb.Children.Add(videoAnimClosing);

            sb.Begin();

        }



        private void ScatterViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
            ssc = this.Template.FindName("shadow", this) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
        }





        /// <summary>
        /// Ferme le menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMenuClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Close");
        }

        /// <summary>
        /// Crée un NodeText
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Text");
        }

        /// <summary>
        /// Crée un NodeImage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Image");
        }

        private void VideoButtonClick(object sender, RoutedEventArgs e)
        {
            parentWindow.MenuIsClicked(this, "Video");
        }


        


    }
}
