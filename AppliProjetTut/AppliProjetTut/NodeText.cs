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
    /// Logique d'interaction pour NodeText.xaml
    /// </summary>
    public partial class NodeText : ScatterCustom
    {

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;


        // TextBox du TextNode
        public SurfaceTextBox STextBox;
        // Nombre maximal de caractères
        int MaxLength = -1;
        // Nombre maximal de lignes
        public int MaxNbLines = 8;
        // Si le cadenas est activé

        public SurfaceScrollViewer SScrollViewer;

        // clavier virtuel
        public ClavierVirtuel clavier;
        // palette de couleur
        public PaletteCouleurs palette;

        // couleurs actuelles
        private Brush currentColor;
        private Brush tempColor;

        // boutons du menu
        public SurfaceButton btnColorChoice;
        public SurfaceButton btnEdition;


        // largeur du texte
        double textWidth = 300;


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public NodeText(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;
            clavier = new ClavierVirtuel(this);
            palette = new PaletteCouleurs(this);

            base.SetTypeOfNode("Text");

            STextBox = new SurfaceTextBox();
            STextBox.Name = "TextBoxNode";
            STextBox.IsEnabled = false;
            STextBox.TextWrapping = TextWrapping.Wrap;
            base.TypeScatter.Children.Add(STextBox);

            SScrollViewer = new SurfaceScrollViewer();
            SScrollViewer.Width = 300;
            SScrollViewer.Height = 200;
            //SScrollViewer.Content = STextBox;
            SScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            SScrollViewer.ScrollToEnd();


            currentColor = new SolidColorBrush(Colors.Black);
            TypeScatter.Background = currentColor;
            STextBox.Background = currentColor;
            STextBox.BorderBrush = new SolidColorBrush(Colors.Transparent);



            // modification de la barre des taches
            btnColorChoice = new SurfaceButton();
            btnColorChoice.Width = 75;
            btnColorChoice.Height = 75;
            ImageBrush imgBckg3 = new ImageBrush();
            imgBckg3.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_color.png", UriKind.Relative));
            btnColorChoice.Background = imgBckg3;
            base.grdButtonH.Children.Add(btnColorChoice);
            btnColorChoice.Margin = new Thickness(-75, 0, 75, 0);
            btnColorChoice.Click += new RoutedEventHandler(OnColorSelection);

            btnEdition = new SurfaceButton();
            btnEdition.Width = 75;
            btnEdition.Height = 75;
            ImageBrush imgBckg = new ImageBrush();
            imgBckg.ImageSource = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_edit.png", UriKind.Relative));
            btnEdition.Background = imgBckg;
            base.grdButtonH.Children.Add(btnEdition);
            btnEdition.Margin = new Thickness(0, 0, 0, 0);
            btnEdition.Click += new RoutedEventHandler(OnEditSelection);

            base.grdButtonH.Children.Remove(this.btnText);

        }


        //
        //   EVENT de selection du menu
        //
        /// <summary>
        /// Lorsque que le Menu du clavier est sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.AddonGrid.Items.Add(clavier);

                // on fait apparaitre le "curseur"
                STextBox.AppendText("|\r");
                clavier.CanMove = false;
                clavier.CanScale = false;
                clavier.CanRotate = false;
                isEditing = true;
            }
        }
        /// <summary>
        /// Lorsque que le Menu de la palette de couleurs est sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnColorSelection(object sender, RoutedEventArgs e)
        {
            if (!base.isEditing)
            {
                this.AddonGrid.Items.Add(palette);

                palette.CanMove = false;
                palette.CanScale = false;
                palette.CanRotate = false;
                base.isEditing = true;
            }
        }


        //
        //   INTERACTION AVEC LE CLAVIER
        //
        /// <summary>
        /// Ajoute le texte passé en paramètre
        /// </summary>
        /// <param name="str"></param>
        public void AjoutTexte(string str)
        {
            if (str.Equals("Close"))
            {
                this.AddonGrid.Items.Remove(clavier);

                // on enlève le "curseur"
                string test = STextBox.Text;

                if(test.Contains("|"))
                {
                    if (MaxLength == -1)
                    {
                        test = test.Remove(test.Length - 2);
                    }
                    else
                    {
                        test = test.Remove(test.Length - 1);
                    }
                    STextBox.Clear();
                    STextBox.AppendText(test);
                }

                base.isEditing = false;
            }
            else if (str.ToLower().Equals("backspace"))
            {
                if (MaxLength == -1)
                {
                    if (STextBox.Text.Length > 2)
                    {
                        string test = STextBox.Text;
                        test = test.Remove(test.Length - 3);
                        STextBox.Clear();
                        STextBox.AppendText(test);
                        if (STextBox.Text.Length < 1 && MaxLength != -1)
                        {
                            clavier.EnableEnterKeys(false);
                        }
                        STextBox.AppendText("|\r");

                        // le fichier a été modifié
                        Surface.Modification(true);
                    }
                }
                else
                {
                    if (STextBox.Text.Length > 1)
                    {
                        string test = STextBox.Text;
                        test = test.Remove(test.Length - 2);
                        STextBox.Clear();
                        STextBox.AppendText(test);
                        if (STextBox.Text.Length < 1 && MaxLength != -1)
                        {
                            clavier.EnableEnterKeys(false);
                        }
                        STextBox.AppendText("|");
                    }
                }
            }
            else
            {

                if (STextBox.Text.Length - 2 >= MaxLength && MaxLength != -1)
                    return;

                if (MaxLength == -1)
                {
                    string test = STextBox.Text;
                    test = test.Remove(test.Length - 2);
                    STextBox.Clear();
                    STextBox.AppendText(test);
                    STextBox.AppendText(str);
                    STextBox.AppendText("|\r");
                    clavier.EnableEnterKeys(true);
                    // le fichier a été modifié
                    Surface.Modification(true);
                }
                else
                {
                    string test = STextBox.Text;
                    test = test.Remove(test.Length - 1);
                    STextBox.Clear();
                    STextBox.AppendText(test);
                    STextBox.AppendText(str);
                    STextBox.AppendText("|");
                    clavier.EnableEnterKeys(true);
                }

            }

            if (STextBox.LineCount > MaxNbLines && isLocked())
            {
                STextBox.Width = 250;
                SScrollViewer.ScrollToBottom();
            }
            else
            {
                STextBox.Width = textWidth;
            }

        }
        


        //
        //   INTERACTION AVEC LA PALETTE
        //
        /// <summary>
        /// Change la couleur du Node
        /// </summary>
        /// <param name="color"></param>
        public void SetTempBackGroundColor(Brush color)
        {
            tempColor = color;
            STextBox.Background = tempColor;
            STextBox.BorderBrush = tempColor;
        }
        /// <summary>
        /// Ferme la palette de couleur
        /// </summary>
        public void ClosePalette()
        {
            this.AddonGrid.Items.Remove(palette);
            STextBox.Background = currentColor;
            STextBox.BorderBrush = currentColor;
            tempColor = currentColor;
            isEditing = false;
        }
        /// <summary>
        /// Valide le choix de la couleur
        /// </summary>
        public void ValidateChoice()
        {
            currentColor = tempColor;
            ClosePalette();
        }




        /// <summary>
        /// Lorsque l'etat du cadenas est changé
        /// </summary>
        /// <param name="locked"></param>
        public void isLockChanged(bool locked)
        { 
            
            // true : unlocked
            // false : locked

            if (locked)
            {
                base.TypeScatter.Children.Remove(STextBox);
                SScrollViewer.Content = STextBox;
                base.TypeScatter.Children.Add(SScrollViewer);
                SScrollViewer.ScrollToEnd();
                if(STextBox.LineCount > MaxNbLines)
                    STextBox.Width = 250;
            }
            else
            {
                base.TypeScatter.Children.Remove(SScrollViewer);
                SScrollViewer.Content = null;
                base.TypeScatter.Children.Add(STextBox);
                STextBox.Width = 300;
            }


        }


        //
        // Fonction de changement de largeur
        //
        public void setWidth(double newWidth, bool isOpened)
        {
            textWidth = newWidth;

            base.MaxWidth = newWidth;
            base.Width = newWidth;

            base.TypeScatter.Width = newWidth;
            base.MainGrid.Width = newWidth;

            this.SScrollViewer.Width = newWidth;
            this.STextBox.Width = newWidth;

            base.AddonGrid.Margin = new Thickness(newWidth / 2, 275, newWidth / 2, -100);

            if(isOpened)
                base.grdButtonH.Margin = new Thickness((newWidth - 300) / 2, 175, (newWidth - 300) / 2, -75);

        }




        //
        // autres fonctions utiles
        //
        /// <summary>
        /// Retourne la couleur
        /// </summary>
        /// <returns></returns>
        public Brush GetColor()
        {
            return currentColor;
        }
        /// <summary>
        /// Récupère la couleur passée en paramètre
        /// </summary>
        /// <param name="col"></param>
        public void SetColor(string col)
        {
            Brush color = new BrushConverter().ConvertFromString(col) as SolidColorBrush;
            SetTempBackGroundColor(color);
        }


        /// <summary>
        /// Retourne le clavier
        /// </summary>
        /// <returns></returns>
        public ClavierVirtuel GetClavier()
        {
            return clavier;
        }
        /// <summary>
        /// Retourne le texte du NodeText
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return STextBox.Text.ToString();
        }

        //
        //  prevu pour la sauvegarde de fichier
        //
        /// <summary>
        /// Transforme le NodeText en boîte de dialogue pour entrer un nom de sauvegarde
        /// </summary>
        public void TransformToFileSaver()
        {
            // on adapte la taille
            base.Height = 125;
            
            base.MainGrid.Height = 125;

            base.TypeScatter.Margin = new Thickness(0, 0, 0, 75);

            STextBox.Height = 50;
            STextBox.Margin = new Thickness(0, 0, 0, 150);
            textWidth = 300;
            STextBox.Width = 300;

            // on limite le nombre de ligne
            STextBox.MaxLines = 1;
            // nombre de caractères limité à 20
            MaxLength = 20;
            
            // on modifie la couleur de base
            base.Background = new SolidColorBrush(Colors.White);
            STextBox.Background = new SolidColorBrush(Colors.White);
            STextBox.BorderBrush = new SolidColorBrush(Colors.White);
            
            STextBox.Foreground = new SolidColorBrush(Colors.Black);

            // on desactive les caracteres speciaux du clavier
            clavier.CarSpe.IsEnabled = false;
            clavier.Accent.IsEnabled = false;
            clavier.close.IsEnabled = false;

            base.AddonGrid.Margin = new Thickness(150, 100, 150, -50);

            // on active le clavier
            base.AddonGrid.Items.Add(clavier);
            // on fait apparaitre le "curseur"
            STextBox.AppendText("azerty|");
            clavier.CanMove = false;
            clavier.CanScale = false;
            clavier.CanRotate = false;
            base.isEditing = true;

            // on stop le timer mesurant l'inactivité
            this.ActivityTimer.Stop();

            base.grdButtonV.Width = 0;
            base.Width = 300;
            base.MainGrid.Width = 300;



        }


        //
        //  CHARGEMENT du NodeText
        //
        /// <summary>
        /// Charge du texte lors de l'ouverture d'un fichier de sauvegarde
        /// </summary>
        /// <param name="str"></param>
        /// <param name="clear"></param>
        public void LoadText(string str, bool clear)
        {
            if(clear)
                STextBox.Clear();
            STextBox.AppendText(str);
        }

    }
}
