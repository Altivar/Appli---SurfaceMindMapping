using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

using System.Windows.Media.Animation;

namespace AppliProjetTut
{
    /// <summary>
    /// Interaction logic for MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : TagVisualization
    {

        // Surface
        SurfaceWindow1 parentSurface;

        // si le menu est en animation on empeche l'interaction
        bool isAnimated = false;

        // animation ouverture
        Storyboard sbOpening;
        
        // animation ouverture/fermeture liste sauvegarde
        Storyboard sbListSauvOpening;
        Storyboard sbListSauvOpening2;
        Storyboard sbListSauvClosing;
        Storyboard sbListSauvClosing2;
        bool isListSauvOpened = false;

        // animation ouverture/fermeture texte
        Storyboard sbTextOpening;
        Storyboard sbTextClosing;
        bool isTextOpened = false;

        // interface permettant d'entrer le nom de la sauvegare
        NodeText txt;

        // liste des sauvegardes de fichiers
        ListeSauvegarde saveList;



        /// <summary>
        /// Default Constructor
        /// </summary>
        public MenuPrincipal()
        {
            InitializeComponent();

            btnSaveOpen.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnBtnSaveOpenPreviewTouchDown);
            btnSaveAs.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnBtnSaveAsPreviewTouchDown);
            btnSave.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnBtnSavePreviewTouchDown);
        }

        
        private void MenuPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize MenuPrincipal's UI based on this.VisualizedTag here
        }


        /// <summary>
        /// Lie le menu a la surface et initialise les composants
        /// </summary>
        /// <param name="surface"></param>
        public void SetSurfaceWindow(SurfaceWindow1 surface)
        {
            parentSurface = surface;
            
            // initialisation du  menu
            txt = new NodeText(surface, null);
            txt.TransformToFileSaver();
            txt.clavier.Enter.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnEnterPreviewTouchDown);
            this.grdText.Children.Add(txt);

            saveList = new ListeSauvegarde(this);
            saveList.Margin = new Thickness(0, 0, 0, 0);
            this.grdListeSauvegarde.Children.Add(saveList);


            OpeningMenu();
        }












        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        /// <summary>
        /// Ouvre le MENU
        /// </summary>
        public void OpeningMenu()
        {

            isAnimated = true;

            sbOpening = new Storyboard();

            ThicknessAnimation marginBtnSave = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 50),
                To = new Thickness(0, 115, 0, -65),
                Duration = TimeSpan.FromSeconds(0.35)
            };
            ThicknessAnimation marginBtnSave2 = new ThicknessAnimation
            {
                From = new Thickness(0, 115, 0, -65),
                To = new Thickness(0, 100, 0, -50),
                Duration = TimeSpan.FromSeconds(0.15),
                BeginTime = TimeSpan.FromSeconds(0.35)
            };
            ThicknessAnimation marginBtnSaveAs = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 50),
                To = new Thickness(0, 165, 0, -115),
                Duration = TimeSpan.FromSeconds(0.35),
                BeginTime = TimeSpan.FromSeconds(0.1)
            };
            ThicknessAnimation marginBtnSaveAs2 = new ThicknessAnimation
            {
                From = new Thickness(0, 165, 0, -115),
                To = new Thickness(0, 150, 0, -100),
                Duration = TimeSpan.FromSeconds(0.15),
                BeginTime = TimeSpan.FromSeconds(0.45)
            };
            ThicknessAnimation marginBtnOpen = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 50),
                To = new Thickness(0, 215, 0, -165),
                Duration = TimeSpan.FromSeconds(0.35),
                BeginTime = TimeSpan.FromSeconds(0.2)
            };
            ThicknessAnimation marginBtnOpen2 = new ThicknessAnimation
            {
                From = new Thickness(0, 215, 0, -165),
                To = new Thickness(0, 200, 0, -150),
                Duration = TimeSpan.FromSeconds(0.15),
                BeginTime = TimeSpan.FromSeconds(0.55)
            };
            ThicknessAnimation marginBtnLanguage = new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 50),
                To = new Thickness(0, 265, 0, -215),
                Duration = TimeSpan.FromSeconds(0.35),
                BeginTime = TimeSpan.FromSeconds(0.3)
            };
            ThicknessAnimation marginBtnLanguage2 = new ThicknessAnimation
            {
                From = new Thickness(0, 265, 0, -215),
                To = new Thickness(0, 250, 0, -200),
                Duration = TimeSpan.FromSeconds(0.15),
                BeginTime = TimeSpan.FromSeconds(0.65)
            };

            Storyboard.SetTargetProperty(marginBtnSave, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnSave, btnSave);
            Storyboard.SetTargetProperty(marginBtnSaveAs, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnSaveAs, btnSaveAs);
            Storyboard.SetTargetProperty(marginBtnOpen, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnOpen, btnSaveOpen);
            Storyboard.SetTargetProperty(marginBtnLanguage, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnLanguage, btnLanguage);

            Storyboard.SetTargetProperty(marginBtnSave2, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnSave2, btnSave);
            Storyboard.SetTargetProperty(marginBtnSaveAs2, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnSaveAs2, btnSaveAs);
            Storyboard.SetTargetProperty(marginBtnOpen2, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnOpen2, btnSaveOpen);
            Storyboard.SetTargetProperty(marginBtnLanguage2, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginBtnLanguage2, btnLanguage);

            sbOpening.Children.Add(marginBtnSave);
            sbOpening.Children.Add(marginBtnSaveAs);
            sbOpening.Children.Add(marginBtnOpen);
            sbOpening.Children.Add(marginBtnLanguage);

            sbOpening.Children.Add(marginBtnSave2);
            sbOpening.Children.Add(marginBtnSaveAs2);
            sbOpening.Children.Add(marginBtnOpen2);
            sbOpening.Children.Add(marginBtnLanguage2);

            sbOpening.Completed += new EventHandler(MainMenuOpenedCompleted);

            sbOpening.Begin();


        }
        // lorsque le menu est ouvert
        void MainMenuOpenedCompleted(object sender, EventArgs e)
        {
            this.grdListeSauvegarde.Margin = new Thickness(0, 200, 0, -150);

            isAnimated = false;
        }














        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        /// <summary>
        /// Appui sur la grid "Open"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBtnSaveOpenPreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (isAnimated)
                return;

            if (isListSauvOpened)
                CloseListeSauvegarde();
            else
                OpenListeSauvegarde();
        }
        
        /// <summary>
        /// Premiere animation d'ouverture de la grid "Open"
        /// </summary>
        public void OpenListeSauvegarde()
        {

            isAnimated = true;

            sbListSauvOpening = new Storyboard();

            ThicknessAnimation marginListeSauvegarde = new ThicknessAnimation
            {
                From = this.grdListeSauvegarde.Margin,
                To = new Thickness(100, 200, -200, -150),
                Duration = TimeSpan.FromSeconds(0.25)
            };
            DoubleAnimation widthListeSauvegarde = new DoubleAnimation
            {
                From = this.grdListeSauvegarde.Width,
                To = 200,
                Duration = TimeSpan.FromSeconds(0.25)
            };
            
            Storyboard.SetTargetProperty(marginListeSauvegarde, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginListeSauvegarde, grdListeSauvegarde);
            Storyboard.SetTargetProperty(widthListeSauvegarde, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthListeSauvegarde, grdListeSauvegarde);
            
            sbListSauvOpening.Children.Add(marginListeSauvegarde);
            sbListSauvOpening.Children.Add(widthListeSauvegarde);
            
            sbListSauvOpening.Completed += new EventHandler(OnSBListSauvOpeningCompleted);

            sbListSauvOpening.Begin();
        }
        // lorsque la premiere animation d'ouverture de la grid "Open" est terminé
        void OnSBListSauvOpeningCompleted(object sender, EventArgs e)
        {

            this.grdListeSauvegarde.Margin = new Thickness(100, 200, -200, -150);
            this.grdListeSauvegarde.Width = 200;

            sbListSauvOpening2 = new Storyboard();

            double hauteur = 200;

            ThicknessAnimation marginListeSauvegarde2 = new ThicknessAnimation
            {
                From = this.grdListeSauvegarde.Margin,
                To = new Thickness(100, 200, -200, -100 - hauteur),
                Duration = TimeSpan.FromSeconds(0.25)
            };
            DoubleAnimation heightListeSauvegarde = new DoubleAnimation
            {
                From = this.grdListeSauvegarde.Height,
                To = hauteur,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            Storyboard.SetTargetProperty(marginListeSauvegarde2, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginListeSauvegarde2, grdListeSauvegarde);
            Storyboard.SetTargetProperty(heightListeSauvegarde, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightListeSauvegarde, grdListeSauvegarde);

            sbListSauvOpening2.Children.Add(marginListeSauvegarde2);
            sbListSauvOpening2.Children.Add(heightListeSauvegarde);

            sbListSauvOpening2.Completed += new EventHandler(OnSBListSauvOpening2Completed);

            sbListSauvOpening2.Begin();
        }
        // lorsque l'animation d'ouverture est terminée
        void OnSBListSauvOpening2Completed(object sender, EventArgs e)
        {
            isAnimated = false;
            isListSauvOpened = true;
        }







        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        /// <summary>
        /// Animation de fermeture de la grid "Open"
        /// </summary>
        public void CloseListeSauvegarde()
        {

            isAnimated = true;

            sbListSauvClosing = new Storyboard();

            ThicknessAnimation marginListeSauvegarde = new ThicknessAnimation
            {
                From = this.grdListeSauvegarde.Margin,
                To = new Thickness(100, 200, -200, -150),
                Duration = TimeSpan.FromSeconds(0.25)
            };
            DoubleAnimation heightListeSauvegarde = new DoubleAnimation
            {
                From = this.grdListeSauvegarde.Height,
                To = 50,
                Duration = TimeSpan.FromSeconds(0.25)
            };


            Storyboard.SetTargetProperty(marginListeSauvegarde, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginListeSauvegarde, grdListeSauvegarde);
            Storyboard.SetTargetProperty(heightListeSauvegarde, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightListeSauvegarde, grdListeSauvegarde);

            sbListSauvClosing.Children.Add(marginListeSauvegarde);
            sbListSauvClosing.Children.Add(heightListeSauvegarde);

            sbListSauvClosing.Completed += new EventHandler(OnSBListSauvClosingCompleted);

            sbListSauvClosing.Begin();
        }
        // lorsque la premiere animation de la fermeture de la grid "Open" est terminée
        void OnSBListSauvClosingCompleted(object sender, EventArgs e)
        {
            
            sbListSauvClosing2 = new Storyboard();

            this.grdListeSauvegarde.Height = 50;
            this.grdListeSauvegarde.Margin = new Thickness(100, 200, -200, -150);

            ThicknessAnimation marginListeSauvegarde = new ThicknessAnimation
            {
                From = this.grdListeSauvegarde.Margin,
                To = new Thickness(0, 200, 0, -150),
                Duration = TimeSpan.FromSeconds(0.25)
            };
            DoubleAnimation widthListeSauvegarde = new DoubleAnimation
            {
                From = this.grdListeSauvegarde.Width,
                To = 100,
                Duration = TimeSpan.FromSeconds(0.25)
            };


            Storyboard.SetTargetProperty(marginListeSauvegarde, new PropertyPath(Grid.MarginProperty));
            Storyboard.SetTarget(marginListeSauvegarde, grdListeSauvegarde);
            Storyboard.SetTargetProperty(widthListeSauvegarde, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthListeSauvegarde, grdListeSauvegarde);

            sbListSauvClosing2.Children.Add(marginListeSauvegarde);
            sbListSauvClosing2.Children.Add(widthListeSauvegarde);

            sbListSauvClosing2.Completed += new EventHandler(OnSBListSauvClosing2Completed);

            sbListSauvClosing2.Begin();
        }
        // lorsque l'animation de fermeture est terminée
        void OnSBListSauvClosing2Completed(object sender, EventArgs e)
        {
            isAnimated = false;
            isListSauvOpened = false;
        }










        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        void OnBtnSaveAsPreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (isAnimated)
                return;

            if (isListSauvOpened)
            {
                CloseListeSauvegarde();
            }

            if (isTextOpened)
                ClosingText();
            else
                OpeningText();
        }
        void OnBtnSavePreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (isAnimated)
                return;

            

            if (isListSauvOpened)
            {
                CloseListeSauvegarde();
            }

            if (isTextOpened)
            {
                ClosingText();
            }
            else
            {
                if (parentSurface.GetFileName() != "<>")
                {
                    parentSurface.SaveUnderFileName(parentSurface.GetFileName());
                    return;
                }
                OpeningText();
            }


        }
        public void OpeningText()
        {
            isAnimated = true;

            sbTextOpening = new Storyboard();

            DoubleAnimation widthText = new DoubleAnimation
            {
                From = 0,
                To = 300,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            DoubleAnimation heightText = new DoubleAnimation
            {
                From = 0,
                To = 125,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTargetProperty(widthText, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthText, grdText);
            Storyboard.SetTargetProperty(heightText, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightText, grdText);

            sbTextOpening.Children.Add(widthText);
            sbTextOpening.Children.Add(heightText);

            sbTextOpening.Completed += new EventHandler(OnSBTextOpeningCompleted);

            sbTextOpening.Begin();

        }
        // lorsque l'animation d'ouverture est terminée
        void OnSBTextOpeningCompleted(object sender, EventArgs e)
        {
            isAnimated = false;
            isTextOpened = true;
        }


        public void ClosingText()
        {
            isAnimated = true;

            sbTextClosing = new Storyboard();

            DoubleAnimation widthText = new DoubleAnimation
            {
                From = 300,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            DoubleAnimation heightText = new DoubleAnimation
            {
                From = 125,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTargetProperty(widthText, new PropertyPath(Grid.WidthProperty));
            Storyboard.SetTarget(widthText, grdText);
            Storyboard.SetTargetProperty(heightText, new PropertyPath(Grid.HeightProperty));
            Storyboard.SetTarget(heightText, grdText);

            sbTextClosing.Children.Add(widthText);
            sbTextClosing.Children.Add(heightText);

            sbTextClosing.Completed += new EventHandler(OnSBTextClosingCompleted);

            sbTextClosing.Begin();
        }
        // lorsque l'animation de fermeture du texte est terminée
        void OnSBTextClosingCompleted(object sender, EventArgs e)
        {
            isAnimated = false;
            isTextOpened = false;
        }






        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        void OnEnterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            // on recupère le nom du fichier de sauvegarde
            string newFileName = txt.GetText();
            string[] strTab = newFileName.Split("\n|".ToCharArray());
            newFileName = strTab[0];

            // on ferme le texte en le reinitialisant
            ClosingText();
            txt.STextBox.Clear();
            txt.STextBox.AppendText("|");


            parentSurface.SaveUnderFileName(newFileName);
        }







        ////                                                       ////
        ///////////////////////////////////////////////////////////////
        ////                                                       ////
        public void OnFileSelection(string fileName)
        {
            parentSurface.OpenFile(fileName);

            CloseListeSauvegarde();
        }



        ///
        /// Fin CLASSE
        ///

    }
}
