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
    public partial class NodeImage : ScatterCustom
    {

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // liste de choix de l'image
        ListeImages imageChoice;

        // Image du ImageNode
        Brush currentImage;
        Point currentSize;
        Point tempSize = new Point(300, 200);
        Point previousSize = new Point(300, 200);
        string tempPath = "NONE";
        string currentPath = "NONE";


        /// <summary>
        /// Defalut Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public NodeImage(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {

            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            base.SetTypeOfNode("Image");

            imageChoice = new ListeImages(this);

            currentImage = new SolidColorBrush(Colors.Gray);
            currentSize = new Point(300, 200);

            mise_a_echelle();
            base.TypeScatter.Background = currentImage;

            base.CanScale = true;
            base.SizeChanged += new SizeChangedEventHandler(OnNodeImageSizeChanged);

            

            // modification de la barre des taches
            SurfaceButton btnImageChoice = new SurfaceButton();
            btnImageChoice.Width = 75;
            btnImageChoice.Height = 75;
            ImageBrush imgBckg = new ImageBrush();
            imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_images.png", UriKind.Relative));
            btnImageChoice.Background = imgBckg;
            base.grdButtonH.Children.Add(btnImageChoice);
            btnImageChoice.Margin = new Thickness(-75, 0, 75, 0);
            btnImageChoice.Click += new RoutedEventHandler(OnImageChoiceSelection);

        }





        //
        //   EVENT de selection du menu
        //
        /// <summary>
        /// Appelé lors de la selection du Menu de choix d'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImageChoiceSelection(object sender, RoutedEventArgs e)
        {
            if (!base.isEditing)
            {
                base.Height = 275;
                base.Width = 375;
                base.TypeScatter.Width = (currentSize.X > 300) ? 300 : currentSize.X;
                base.TypeScatter.Height = (currentSize.Y > 200) ? 200 : currentSize.Y;

                base.AddonGrid.Items.Add(imageChoice);
                imageChoice.InitListView();

                base.AddonGrid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                CanScale = false;
                base.isEditing = true;
            }
        }


        //
        //  EVENT de changement de taille
        //
        /// <summary>
        /// Evenement de changement de taille réadapté aux images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNodeImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isActive())
            {
                
                double ecartX = currentSize.X - 300;
                double ecartY = currentSize.Y - 200;
                double ecartXparY = ecartX / ecartY;

                if (currentSize.X <= 300 && currentSize.Y <= 200)
                {
                    base.Width = previousSize.X;
                    base.Height = previousSize.Y;
                    this.TypeScatter.Width = currentSize.X;
                    this.TypeScatter.Height = currentSize.Y;
                }
                else if (currentSize.X > 300 && currentSize.Y > 200)
                {
                    if (ecartX > ecartY)
                    {
                        double newHeight = (base.Width - 300) / ecartX * ecartY;
                        base.Height = newHeight + 200;
                    }
                    else if (ecartY > ecartX)
                    {
                        double newWidth = (base.Height - 200) / ecartY * ecartX;
                        base.Width = newWidth + 300;
                    }
                    else // si ecartX == ecartY
                    {

                    }
                }
                else if (currentSize.X <= 300 && currentSize.Y > 200)
                {
                    base.Width = previousSize.X;
                }
                else if (currentSize.X > 300 && currentSize.Y <= 200)
                {
                    base.Height = previousSize.Y;
                }
                else
                {
                    // pas normal !!! 
                }



                // si la dimensiondu Node est trop grande
                if (base.Width > currentSize.X + 75)
                {
                    base.Width = currentSize.X + 75;
                }
                if (base.Height > currentSize.Y + 75)
                {
                    base.Height = currentSize.Y + 75;
                }

                // si la dimension du Node est trop petite
                if (base.Width <= 375)
                {
                    base.Width = 375;
                }
                if (base.Height <= 275)
                {
                    base.Height = 275;
                }

                this.MainGrid.Width = base.Width;
                this.MainGrid.Height = base.Height;
                if (currentSize.X > 300) this.TypeScatter.Width = base.Width - 75;
                if (currentSize.Y > 200) this.TypeScatter.Height = base.Height - 75;


                this.grdButtonV.Margin = new Thickness(base.Width - 75, 0, 0, base.Height - 225);
                if (base.isTextAnnotationOpened)
                {
                    base.grdButtonH.Margin = new Thickness(0, base.Height + 25, base.Width - 375, -100);
                }
                else
                {
                    base.grdButtonH.Margin = new Thickness(0, base.Height - 75, base.Width - 375, 0);
                }

                this.grdBGColor.Width = this.TypeScatter.Width;
                this.grdBGColor.Height = this.TypeScatter.Height;

                previousSize = new Point(base.Width, base.Height);
            
            }

        }




        //
        //  GESTION IMAGE PAR LISTE D'IMAGE
        //
        /// <summary>
        /// Appelé lors de la fermeture de la liste d'image
        /// </summary>
        public void onCloseImagesList()
        {
            base.AddonGrid.Items.Remove(imageChoice);

            base.Width = 375;
            base.Height = 275;
            base.MainGrid.Width = 375;
            base.MainGrid.Height = 275;
            base.TypeScatter.Width = (currentSize.X > 300) ? 300 : currentSize.X;
            base.TypeScatter.Height = (currentSize.Y > 200) ? 200 : currentSize.Y;
            base.TypeScatter.Background = currentImage;
            
            CanScale = true;
            isEditing = false;

            tempSize = new Point(300, 200);
        }

        /// <summary>
        /// Appelé lors de l'appui sur une image de la liste
        /// </summary>
        /// <param name="newPath"></param>
        /// <param name="dimension"></param>
        /// <param name="path"></param>
        public void onChoice(Brush newPath, Point dimension, string path)
        {
            base.TypeScatter.Background = newPath;
            tempSize = dimension;
            base.TypeScatter.Width = (tempSize.X > 300) ? 300 : tempSize.X;
            base.TypeScatter.Height = (tempSize.Y > 200) ? 200 : tempSize.Y;

            base.AddonGrid.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            tempPath = path;

        }
        /// <summary>
        /// Appéle lors de la validation du choix de l'image
        /// </summary>
        public void onValidateChoice()
        {
            currentImage = base.TypeScatter.Background;
            currentSize = tempSize;
            mise_a_echelle();

            currentPath = tempPath;


            // le fichier a été modifié
            Surface.Modification(true);

            onCloseImagesList();
        }


        //
        //  FONCTION de mise a echelle du Node
        //
        /// <summary>
        /// Met le NodeImage à l'echelle de l'image qu'il contient
        /// </summary>
        void mise_a_echelle()
        {
            // taille du Node
            base.Height = 200;
            base.Width = 300;

            //// taille maximum du Node
            // reglage sur les X
            if (currentSize.X > 300)
            {
                base.MainGrid.Width = base.Width;
            }
            else
            {
                base.MainGrid.Width = currentSize.X;
            }
            // reglage sur les Y
            if (currentSize.Y > 200)
            {
                base.MainGrid.Height = base.Height;
            }
            else
            {
                base.MainGrid.Height = currentSize.Y;
            }

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            
            previousSize = new Point(base.Width, base.Height);



        }



        //
        //  GESTION chargement image
        //
        /// <summary>
        /// Charge l'image passé en paramètre avec les caractéristique de l'image
        /// </summary>
        /// <param name="newPath"></param>
        /// <param name="dimension"></param>
        /// <param name="path"></param>
        public void LoadImage(Brush newPath, Point dimension, string path)
        {
            base.MainGrid.Background = newPath;
            currentSize = dimension;
            base.MainGrid.Width = (tempSize.X > 300) ? 300 : tempSize.X;
            base.MainGrid.Height = (tempSize.Y > 200) ? 200 : tempSize.Y;

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            
            currentPath = path;

            currentImage = base.MainGrid.Background;
            mise_a_echelle();
        }


        //
        //
        //
        /// <summary>
        /// Retourne le chemin vers l'image
        /// </summary>
        /// <returns></returns>
        public string GetImagePath()
        {
            return currentPath;
        }
        /// <summary>
        /// Retourne l'image
        /// </summary>
        /// <returns></returns>
        public Brush GetImage()
        {
            return currentImage;
        }
        /// <summary>
        /// Retourne la taille maximale de l'image du NodeImage
        /// </summary>
        /// <returns></returns>
        public Point GetSize()
        {
            return currentSize;
        }


    }
}
