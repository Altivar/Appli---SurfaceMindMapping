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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;

using System.Windows.Forms;

namespace AppliProjetTut
{
    /// <summary>
    /// Logique d'interaction pour ScatterCustom.xaml
    /// </summary>
    public partial class ScatterCustom : ScatterViewItem
    {

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // type du node
        string thisType;

        // etat de l'annotation (ouverte ou fermée)
        bool isTextAnnotationOpened;
        bool isAnimated;

        // Gestion de l'inactivité du Node
        public Timer ActivityTimer;
        bool m_bIsActive;
        int nbSec;
        int ActivityDuration = 5;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public ScatterCustom(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            

            CanScale = false;
            isTextAnnotationOpened = false;
            isAnimated = false;

            // création du timer
            ActivityTimer = new Timer();
            nbSec = ActivityDuration;
            ActivityTimer.Interval = 1000;
            ActivityTimer.Tick += new EventHandler(InactiveEvent);
            ActivityTimer.Start();
            // initialisation du booleen d'activation
            m_bIsActive = true;
            // gestion des events d'activation
            PreviewTouchDown += new EventHandler<TouchEventArgs>(ScatterCustom_PreviewTouchDown);
            PreviewTouchUp += new EventHandler<TouchEventArgs>(ScatterCustom_PreviewTouchUp);
            PreviewTouchMove += new EventHandler<TouchEventArgs>(ScatterCustom_PreviewTouchMove);

        }


        // lorsque la node est chargée
        protected void OnNodeLoaded(object sender, RoutedEventArgs e)
        {
            // TODO : a implementer dans les classes héritée
        }



        /// <summary>
        /// Timer : gère l'inactivité du Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InactiveEvent(object sender, EventArgs e)
        {
            if(nbSec > 0)
                nbSec--;

            if (nbSec == 0)
            {
                m_bIsActive = false;
                
                this.TypeScatter.Width = 100;
                this.TypeScatter.Height = 100;
                this.TypeScatter.Margin = new Thickness(0, 0, 0, 0);

                this.MainGrid.Width = 100;
                this.MainGrid.Height = 100;

                this.Width = 100;
                this.Height = 100;

            }
        }
        /// <summary>
        /// Réactive le Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScatterCustom_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (!m_bIsActive)
            {
                ReactivationNode();
            }
            m_bIsActive = true;
            nbSec = ActivityDuration;
        }
        /// <summary>
        /// Réactive le Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScatterCustom_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (!m_bIsActive)
            {
                ReactivationNode();
            }
            m_bIsActive = true;
            nbSec = ActivityDuration;
        }
        /// <summary>
        /// Réactive le Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ScatterCustom_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (!m_bIsActive)
            {
                ReactivationNode();
            }
            m_bIsActive = true;
            nbSec = ActivityDuration;
        }
        /// <summary>
        /// Anime le Node lors de la réactivation
        /// </summary>
        void ReactivationNode()
        { 
            // animation
            this.Width = 375;
            this.Height = 275;

            this.MainGrid.Width = 375;
            this.MainGrid.Height = 275;
            
            this.TypeScatter.Width = 300;
            this.TypeScatter.Height = 200;
            this.TypeScatter.Margin = new Thickness(0, 0, 75, 75);
            
        }
        /// <summary>
        /// Renvoie l'état actuel du Node
        /// </summary>
        /// <returns></returns>
        public bool isActive()
        {
            return m_bIsActive;
        }




        /// <summary>
        /// Retourne la ligne de liason entre le Node et son parent
        /// </summary>
        /// <returns></returns>
        public Line getLineToParent()
        {
            Line line = new Line();

            if (parent != null)
            {

                Point pt1 = this.ActualCenter;
                Point pt2 = parent.ActualCenter;

                line.Stroke = Brushes.PaleVioletRed;

                line.X1 = pt1.X;
                line.Y1 = pt1.Y;
                line.X2 = pt2.X;
                line.Y2 = pt2.Y;
                line.StrokeThickness = 2;

            }
            else
            {
                line.Stroke = Brushes.Transparent;
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = 0;
                line.StrokeThickness = 0;
            }

            return line;
        }







        

        // SELECTION MENU
        /// <summary>
        /// Lorsque le Menu de selection de NodeText est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAddNodeTextSelection(object sender, RoutedEventArgs e)
        {
            Point pt = ActualCenter;
            if (pt.Y < Surface.Height - 300)
            {
                pt.Y += 200;
            }
            else
            {
                pt.Y -= 200;
            }    
            Surface.AddNode(this, pt, "Text");
        }
        /// <summary>
        /// Lorsque le Menu de selection de NodeImage est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAddNodeImageSelection(object sender, RoutedEventArgs e)
        {
            Point pt = ActualCenter;
            if (pt.Y < Surface.Height - 300)
            {
                pt.Y += 200;
            }
            else
            {
                pt.Y -= 200;
            }
            Surface.AddNode(this, pt, "Image");
        }
        /// <summary>
        /// Lorsque le Menu de suppression de Node est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Surface.RemoveNode(this, true);
        }
        /// <summary>
        /// Lorsque le menu de Cadenas est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCadenas_Click(object sender, RoutedEventArgs e)
        {
            CanMove = !CanMove;
            CanRotate = CanMove;

            if (CanMove)
            {
                ImageBrush img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_unlock.png", UriKind.Relative));
                btnCadenas.Background = img;
                // on reactive le timer d'inactivité
                ActivityTimer.Start();
                nbSec = ActivityDuration;
            }
            else
            {
                ImageBrush img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_lock.gif", UriKind.Relative));
                btnCadenas.Background = img;
                // on désactive le timer
                ActivityTimer.Stop();
            }

        }
        /// <summary>
        /// Lorsque le menu de Texte est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            if(isAnimated)
                return;

            Storyboard sb = new Storyboard();

            Thickness borderTextIn = new Thickness(0, 0, 75, 100);
            Thickness borderTextOut = new Thickness(0, 275, 75, -175);

            ThicknessAnimation AnimText = new ThicknessAnimation
            {
                From = (isTextAnnotationOpened) ? borderTextOut : borderTextIn,

                To = (isTextAnnotationOpened) ? borderTextIn : borderTextOut, 

                AccelerationRatio = 0.5,

                FillBehavior = FillBehavior.Stop,

                DecelerationRatio = 0.5,

                Duration = System.Windows.Duration.Automatic

            };

            Storyboard.SetTarget(AnimText, TextGrid);
            Storyboard.SetTargetProperty(AnimText, new PropertyPath(MarginProperty));

            sb.Children.Add(AnimText);

            sb.Completed += new EventHandler(sb_Completed);
            isAnimated = true;

            sb.Begin();

            isTextAnnotationOpened = !isTextAnnotationOpened;

        }

        void sb_Completed(object sender, EventArgs e)
        {

            Thickness borderText;

            if (isTextAnnotationOpened)
            {
                borderText = new Thickness(0, 275, 75, -175);
            }
            else
            { 
                borderText = new Thickness(0, 0, 75, 100);
            }

            this.TextGrid.Margin = borderText;

            isAnimated = false;
        }
        /// <summary>
        /// Lorsque le bouton de Séparation est sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeparate_Click(object sender, RoutedEventArgs e)
        {
            parent = null;

            // le fichier a été modifié
            Surface.Modification(true);
        }
        /// <summary>
        /// Lorsque le Menu de séparation du parent est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSeparateSelection(object sender, RoutedEventArgs e)
        {
            parent = null;

            // le fichier a été modifié
            Surface.Modification(true);
        }






        // SET - GET
        /// <summary>
        /// Retourne le parent
        /// </summary>
        /// <returns></returns>
        public ScatterCustom GetParent()
        {
            return parent;
        }
        /// <summary>
        /// Rattache au Node passé en paramètre comme enfant
        /// </summary>
        /// <param name="newParent"></param>
        public void SetParent(ScatterCustom newParent)
        {
            if (newParent == null)
            {
                this.parent = null;
                // le fichier a été modifié
                Surface.Modification(true);
            }
            else if (this.parent == null)
            {
                this.parent = newParent;
                // le fichier a été modifié
                Surface.Modification(true);
            }
        }
        /// <summary>
        /// Retourne le centre du Node
        /// </summary>
        /// <returns></returns>
        public Point GetOrigin()
        {
            Point pt = this.PointFromScreen(this.ActualCenter);
            pt.X -= this.Width / 2 - 25;
            pt.Y -= this.Height / 2 + 25;
            return this.PointToScreen(pt);
        }

        /// <summary>
        /// Indique le type du Node
        /// </summary>
        /// <param name="str"></param>
        public void SetTypeOfNode(string str)
        {
            thisType = str;
        }
        /// <summary>
        /// Retourne le type du Node
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfNode()
        {
            return thisType;
        }

        

      
    }
}
