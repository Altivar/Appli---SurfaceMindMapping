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

        // etat de l'edition du node (si une interface est ouverte)
        public bool isEditing = false;

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // type du node
        string thisType;

        // etat de l'annotation (ouverte ou fermée)
        public bool isTextAnnotationOpened;
        bool isAnimated;

        // Gestion de l'inactivité du Node
        public Timer ActivityTimer;
        bool m_bIsActive;
        int nbSec;
        int ActivityDuration = 15;
        Storyboard AnimStoryboard;

        // texte d'annotation
        public NodeText textAnnotation;

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

            if (nbSec == 0 && m_bIsActive)
            {
                // on desactive toues les interfaces actives du Node
                isEditing = false;

                switch (thisType)
                { 
                    case "Text":
                        NodeText txt = (NodeText)this;
                        try { txt.AjoutTexte("Close"); }
                        catch { };
                        try { txt.ClosePalette(); }
                        catch { };
                        break;
                    case "Image":
                        NodeImage img = (NodeImage)this;
                        try { img.onCloseImagesList(); }
                        catch { };
                        break;
                    case "Video":
                        NodeVideo vid = (NodeVideo)this;
                        try { vid.onCloseVideosList(); }
                        catch { };
                        break;
                }

                m_bIsActive = false;
                double durationAnimation = 0.25;
                // typescatter
                DoubleAnimation widthTSAnimation = new DoubleAnimation
                {
                    From = this.TypeScatter.Width,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };
                DoubleAnimation heightTSAnimation = new DoubleAnimation
                {
                    From = this.TypeScatter.Height,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };
                ThicknessAnimation marginTSAnimation = new ThicknessAnimation
                {
                    From = this.TypeScatter.Margin,
                    To = new Thickness(0,0,0,0),
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };


                // maingrid
                DoubleAnimation widthMGAnimation = new DoubleAnimation
                {
                    From = this.MainGrid.Width,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };
                DoubleAnimation heightMGAnimation = new DoubleAnimation
                {
                    From = this.MainGrid.Height,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };


                // background color grid
                ThicknessAnimation marginBGAnimation = new ThicknessAnimation
                {
                    From = this.grdBGColor.Margin,
                    To = new Thickness(0, 0, 0, 0),
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };


                // base
                DoubleAnimation widthBAAnimation = new DoubleAnimation
                {
                    From = this.ActualWidth,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };
                DoubleAnimation heightBAAnimation = new DoubleAnimation
                {
                    From = this.ActualHeight,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(durationAnimation)
                };


                Storyboard.SetTargetProperty(widthTSAnimation, new PropertyPath(Grid.WidthProperty));
                Storyboard.SetTarget(widthTSAnimation, this.TypeScatter);
                Storyboard.SetTargetProperty(heightTSAnimation, new PropertyPath(Grid.HeightProperty));
                Storyboard.SetTarget(heightTSAnimation, this.TypeScatter);
                Storyboard.SetTargetProperty(marginTSAnimation, new PropertyPath(Grid.MarginProperty));
                Storyboard.SetTarget(marginTSAnimation, this.TypeScatter);

                Storyboard.SetTargetProperty(widthMGAnimation, new PropertyPath(Grid.WidthProperty));
                Storyboard.SetTarget(widthMGAnimation, this.MainGrid);
                Storyboard.SetTargetProperty(heightMGAnimation, new PropertyPath(Grid.HeightProperty));
                Storyboard.SetTarget(heightMGAnimation, this.MainGrid);

                Storyboard.SetTargetProperty(marginBGAnimation, new PropertyPath(Grid.MarginProperty));
                Storyboard.SetTarget(marginBGAnimation, this.grdBGColor);

                Storyboard.SetTargetProperty(widthBAAnimation, new PropertyPath(ScatterViewItem.WidthProperty));
                Storyboard.SetTarget(widthBAAnimation, this);
                Storyboard.SetTargetProperty(heightBAAnimation, new PropertyPath(ScatterViewItem.HeightProperty));
                Storyboard.SetTarget(heightBAAnimation, this);

                AnimStoryboard = new Storyboard();
                AnimStoryboard.Children.Add(widthTSAnimation);
                AnimStoryboard.Children.Add(heightTSAnimation);
                AnimStoryboard.Children.Add(marginTSAnimation);

                AnimStoryboard.Children.Add(widthMGAnimation);
                AnimStoryboard.Children.Add(heightMGAnimation);

                AnimStoryboard.Children.Add(marginBGAnimation);

                AnimStoryboard.Children.Add(widthBAAnimation);
                AnimStoryboard.Children.Add(heightBAAnimation);


                

                AnimStoryboard.Completed += new EventHandler(AnimStoryboardCloseCompleted);
                AnimStoryboard.Begin();


            }
        }
        void AnimStoryboardCloseCompleted(object sender, EventArgs e)
        {
            double Xpos = this.ActualCenter.X;
            double Ypos = this.ActualCenter.Y;
            if (this.ActualCenter.X < 25)
            {
                Xpos = 25;
            }
            else if (this.ActualCenter.X > this.Surface.ActualWidth - 25)
            {
                Xpos = this.Surface.ActualWidth - 25;
            }

            if (this.ActualCenter.Y < 25)
            {
                Ypos = 25;
            }
            else if (this.ActualCenter.Y > this.Surface.ActualHeight - 25)
            {
                Ypos = this.Surface.ActualHeight - 25;
            }
            this.Center = new Point(Xpos, Ypos);

            AnimStoryboard.Stop();
            this.TypeScatter.Width = 100;
            this.TypeScatter.Height = 100;
            this.TypeScatter.Margin = new Thickness(0, 0, 0, 0);
            this.MainGrid.Width = 100;
            this.MainGrid.Height = 100;
            this.grdBGColor.Margin = new Thickness(0, 0, 0, 0);
            this.Width = 100;
            this.Height = 100;
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
            double durationAnimation = 0.25;
            // typescatter
            DoubleAnimation widthTSAnimation = new DoubleAnimation
            {
                From = this.TypeScatter.Width,
                To = 300,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };
            DoubleAnimation heightTSAnimation = new DoubleAnimation
            {
                From = this.TypeScatter.Height,
                To = 200,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };
            ThicknessAnimation marginTSAnimation = new ThicknessAnimation
            {
                From = this.TypeScatter.Margin,
                To = new Thickness(0, 0, 75, 75),
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };
            // maingrid
            DoubleAnimation widthMGAnimation = new DoubleAnimation
            {
                From = this.MainGrid.Width,
                To = 375,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };
            DoubleAnimation heightMGAnimation = new DoubleAnimation
            {
                From = this.MainGrid.Height,
                To = 275,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };

            // background color grid
            ThicknessAnimation marginBGAnimation = new ThicknessAnimation
            {
                From = this.grdBGColor.Margin,
                To = new Thickness(0, 0, 75, 75),
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };


            // base
            DoubleAnimation widthBAAnimation = new DoubleAnimation
            {
                From = this.ActualWidth,
                To = 375,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };
            DoubleAnimation heightBAAnimation = new DoubleAnimation
            {
                From = this.ActualHeight,
                To = 275,
                Duration = TimeSpan.FromSeconds(durationAnimation)
            };

            Storyboard.SetTargetProperty(widthTSAnimation, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthTSAnimation, this.TypeScatter);
            Storyboard.SetTargetProperty(heightTSAnimation, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightTSAnimation, this.TypeScatter);
            Storyboard.SetTargetProperty(marginTSAnimation, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginTSAnimation, this.TypeScatter);

            Storyboard.SetTargetProperty(widthMGAnimation, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthMGAnimation, this.MainGrid);
            Storyboard.SetTargetProperty(heightMGAnimation, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightMGAnimation, this.MainGrid);
            
            Storyboard.SetTargetProperty(marginBGAnimation, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBGAnimation, this.grdBGColor);

            Storyboard.SetTargetProperty(widthBAAnimation, new PropertyPath(ScatterViewItem.WidthProperty));
            Storyboard.SetTarget(widthBAAnimation, this);
            Storyboard.SetTargetProperty(heightBAAnimation, new PropertyPath(ScatterViewItem.HeightProperty));
            Storyboard.SetTarget(heightBAAnimation, this);

            AnimStoryboard = new Storyboard();
            AnimStoryboard.Children.Add(widthTSAnimation);
            AnimStoryboard.Children.Add(heightTSAnimation);
            AnimStoryboard.Children.Add(marginTSAnimation);

            AnimStoryboard.Children.Add(widthMGAnimation);
            AnimStoryboard.Children.Add(heightMGAnimation);

            AnimStoryboard.Children.Add(marginBGAnimation);
            
            AnimStoryboard.Children.Add(widthBAAnimation);
            AnimStoryboard.Children.Add(heightBAAnimation);

            

            AnimStoryboard.Completed += new EventHandler(AnimStoryboardOpenCompleted);
            AnimStoryboard.Begin();



            
        }
        void AnimStoryboardOpenCompleted(object sender, EventArgs e)
        {
            AnimStoryboard.Stop();
            this.TypeScatter.Width = 300;
            this.TypeScatter.Height = 200;
            this.TypeScatter.Margin = new Thickness(0, 0, 75, 75);
            this.MainGrid.Width = 375;
            this.MainGrid.Height = 275;
            this.grdBGColor.Margin = new Thickness(0, 0, 75, 75);
            this.Width = 375;
            this.Height = 275;

            if (isTextAnnotationOpened)
            {
                this.grdButtonH.Margin = new Thickness(0, 300, 75, -100);
            }
            else
            {
                this.grdButtonH.Margin = new Thickness(0, 200, 75, 0);
            }
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
                if (thisType == "Text")
                {
                    NodeText txt = (NodeText)this;
                    if (txt != null)
                    {
                        txt.isLockChanged(false);
                    }
                }
            }
            else
            {
                ImageBrush img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_lock.gif", UriKind.Relative));
                btnCadenas.Background = img;
                // on désactive le timer
                ActivityTimer.Stop();
                if (thisType == "Text")
                {
                    NodeText txt = (NodeText)this;
                    if (txt != null)
                    {
                        txt.isLockChanged(true);
                    }
                }
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

            Thickness borderTextIn = new Thickness(0, 0, 75, this.Height-175);
            Thickness borderTextOut = new Thickness(0, this.Height-75, 75, -100);
            ThicknessAnimation AnimText = new ThicknessAnimation
            {
                From = (isTextAnnotationOpened) ? borderTextOut : borderTextIn,

                To = (isTextAnnotationOpened) ? borderTextIn : borderTextOut, 

                AccelerationRatio = 0.5,

                FillBehavior = FillBehavior.Stop,

                DecelerationRatio = 0.5,

                Duration = System.Windows.Duration.Automatic

            };

            Thickness borderTextButtonIn = new Thickness(0, 100, 0, 0);
            Thickness borderTextButtonOut = new Thickness((this.textAnnotation.ActualWidth - 300) / 2, 175, (this.textAnnotation.ActualWidth - 300) / 2, -75);
            ThicknessAnimation AnimButtonText = new ThicknessAnimation
            {
                From = (isTextAnnotationOpened) ? borderTextButtonOut : borderTextButtonIn,

                To = (isTextAnnotationOpened) ? borderTextButtonIn : borderTextButtonOut,

                AccelerationRatio = 0.5,

                FillBehavior = FillBehavior.Stop,

                DecelerationRatio = 0.5,

                Duration = System.Windows.Duration.Automatic

            };

            Thickness borderNodeButtonIn = new Thickness(0, this.Height - 75, this.Width - 375, 0);
            Thickness borderNodeButtonOut = this.grdButtonH.Margin = new Thickness(0, this.Height + 25, this.Width - 375, -100);
            ThicknessAnimation AnimButtonNode = new ThicknessAnimation
            {
                From = (isTextAnnotationOpened) ? borderNodeButtonOut : borderNodeButtonIn,

                To = (isTextAnnotationOpened) ? borderNodeButtonIn : borderNodeButtonOut,

                AccelerationRatio = 0.5,

                FillBehavior = FillBehavior.Stop,

                DecelerationRatio = 0.5,

                Duration = System.Windows.Duration.Automatic
            };


            Storyboard.SetTarget(AnimText, TextGrid);
            Storyboard.SetTargetProperty(AnimText, new PropertyPath(MarginProperty));

            Storyboard.SetTarget(AnimButtonText, textAnnotation.grdButtonH);
            Storyboard.SetTargetProperty(AnimButtonText, new PropertyPath(MarginProperty));

            Storyboard.SetTarget(AnimButtonNode, this.grdButtonH);
            Storyboard.SetTargetProperty(AnimButtonNode, new PropertyPath(MarginProperty));

            sb.Children.Add(AnimText);
            sb.Children.Add(AnimButtonText);
            sb.Children.Add(AnimButtonNode);

            sb.Completed += new EventHandler(sb_Completed);
            isAnimated = true;

            sb.Begin();

            isTextAnnotationOpened = !isTextAnnotationOpened;

        }
        /// <summary>
        /// Lorsque l'animation est terminée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sb_Completed(object sender, EventArgs e)
        {

            Thickness borderText;
            Thickness borderButtonText;
            Thickness borderButtonNode;

            if (isTextAnnotationOpened)
            {
                borderText = new Thickness(0, this.Height - 75, 75, -100);
                borderButtonText = new Thickness((this.textAnnotation.ActualWidth - 300) / 2, 175, (this.textAnnotation.ActualWidth - 300) / 2, -75);
                borderButtonNode = new Thickness(0, this.Height + 25, this.Width - 375, -100);
            }
            else
            { 
                borderText = new Thickness(0, 0, 75, this.Height-175);
                borderButtonText = new Thickness(0, 100, 0, 0);
                borderButtonNode = new Thickness(0, this.Height - 75, this.Width - 375, 0);
            }

            this.TextGrid.Margin = borderText;
            textAnnotation.grdButtonH.Margin = borderButtonText;
            this.grdButtonH.Margin = borderButtonNode;

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
            if (thisType != "Text")
            {
                // initialisation de la box d'annotaion
                textAnnotation = new NodeText(Surface, null);
                // reglage de la taille generale du node
                textAnnotation.MinHeight = 175;
                textAnnotation.MinWidth = 300;
                textAnnotation.MaxHeight = 175;
                textAnnotation.MaxWidth = 300;
                //
                textAnnotation.MainGrid.Children.Remove(textAnnotation.grdButtonV);
                textAnnotation.MainGrid.Height = 175;
                textAnnotation.MainGrid.Width = 300;
                textAnnotation.MainGrid.Margin = new Thickness(0, 0, 0, 0);
                // reglage de la dimension du texte
                textAnnotation.TypeScatter.Width = 300;
                textAnnotation.TypeScatter.Height = 100;
                textAnnotation.TypeScatter.Margin = new Thickness(0, 0, 0, 75);
                textAnnotation.STextBox.Height = 100;
                textAnnotation.STextBox.Width = 300;
                textAnnotation.SScrollViewer.Height = 100;
                textAnnotation.SScrollViewer.Width = 300;
                // positionnement de la barre des taches horizontale
                textAnnotation.grdButtonH.Margin = new Thickness(0, 100, 0, 0);
                textAnnotation.btnColorChoice.Margin = new Thickness(-75, 0, 75, 0);
                textAnnotation.btnEdition.Margin = new Thickness(0, 0, 0, 0);

                textAnnotation.ActivityTimer.Stop();

                this.TextGrid.Children.Add(textAnnotation);
                textAnnotation.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        /// <summary>
        /// Retourne le type du Node
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfNode()
        {
            return thisType;
        }
        /// <summary>
        /// Renvoie l'etat du Node
        /// </summary>
        /// <returns></returns>
        public bool isLocked()
        {
            return !this.CanMove;
        }

        

      
    }
}
