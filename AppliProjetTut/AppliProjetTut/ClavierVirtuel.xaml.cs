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
using Microsoft.Surface.Presentation.Input;
using System.Windows.Forms;

namespace AppliProjetTut
{
    /// <summary>
    /// Logique d'interaction pour ClavierVirtuel.xaml
    /// </summary>
    public partial class ClavierVirtuel : ScatterViewItem
    {

        // Node
        private NodeText NodeParent;


        // Gestion d'un appui long sur le BackSpace
        Timer backSpaceTimer;
        bool isBackSpaceDown = false;
        int compteurBackSpace = 0;

        // booleens d'etat du clavier
        bool isCapsLock = false;
        bool isSpecialCar = false;
        bool isAccent = false;
        int numPageSpecCar = 1;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public ClavierVirtuel(NodeText parent)
        {
            InitializeComponent();

            // initialize nodeparent
            NodeParent = parent;

            this.CanScale = false;

            backSpaceTimer = new Timer();
            backSpaceTimer.Tick += new EventHandler(backSpaceTimer_Tick);
            backSpaceTimer.Interval = 100;
            backSpaceTimer.Start();


            //-- Lettres Minuscules --//
            this.A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Z.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.R.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Y.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.U.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.I.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.P.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Q.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.S.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.D.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.F.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.H.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.J.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.K.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.L.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.M.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.W.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.X.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.B.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.N.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            
            //-- Lettres Majuscules --//
            this.maj_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_Z.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_E.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_R.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_Y.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_U.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_I.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_O.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_P.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_Q.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_S.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_D.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_F.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_H.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_J.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_K.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_L.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_M.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_W.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_X.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_B.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);
            this.maj_N.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnMajLetterPreviewTouchDown);

            //-- Chiffres --//
            this.Zero.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Un.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Deux.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Trois.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Quatre.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Cinq.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Six.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Sept.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Huit.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Neuf.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);

            //-- Fleches de direction --//
            this.ToRight.PreviewTouchDown += new EventHandler<TouchEventArgs>(ToRight_PreviewTouchDown);
            this.ToLeft.PreviewTouchDown += new EventHandler<TouchEventArgs>(ToLeft_PreviewTouchDown);

            //-- Caractères Spéciaux --//
            // Page 1
            this.quote.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Point.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.virgule.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Tab.PreviewTouchDown += new EventHandler<TouchEventArgs>(Tab_PreviewTouchDown);
            this.exclamation.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.interogation.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Euro.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Dollars.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Pourcent.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.et.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.par.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.parbis.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.moins.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.under.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.egal.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.plus.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.anti_slash.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.pointvrgl.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.dble_point.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.dbl_quotes.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.star.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.slash.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // Page 2
            this.exclamation_inv.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.interogation_inv.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Yen.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Livre.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.micro.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.circonflexe.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.smaller.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.bigger.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.crochet_ouv.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.crochet_fer.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            //this.accol_ouv.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.accol_fer.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.smaller_equal.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.bigger_equal.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.arobase.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.diez.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.vague.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.cent.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // Page 3
            this.exp_un.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.exp_deux.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.exp_trois.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.un_deux.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.un_quatre.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.trois_quatre.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.PI.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.alpha.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.beta.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.gamma.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.teta.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.sigma.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.para.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.bull.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.not.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.degree.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.copy.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.reg.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);


            //-- Accents Minuscules --//
            // ligne 1
            this.A_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.A_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.A_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.U_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.U_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.U_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // ligne 2
            this.I_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.I_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.I_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Y_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Y_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // ligne 3
            this.OE.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.AE.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.C_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O_S.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.B_All.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.N_V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            //-- Accents Majuscules --//
            // ligne 1
            this.maj_A_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_A_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_A_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_E_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_E_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_E_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_E_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_U_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_U_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_U_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // ligne 2
            this.maj_I_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_I_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_I_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_Y_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_Y_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            // ligne 3
            this.maj_OE.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_AE.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_C_C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_O_S.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.maj_N_V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);



            //-- Touches sans caractères --//
            this.Enter.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Space.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Backspace.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Backspace.PreviewTouchUp += new EventHandler<TouchEventArgs>(Backspace_PreviewTouchUp);
            this.Upper1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnUpperPreviewTouchDown);
            this.Upper2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnUpperPreviewTouchDown);
            this.CarSpe.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnCarSpePreviewTouchDown);
            this.Accent.PreviewTouchDown += new EventHandler<TouchEventArgs>(Accent_PreviewTouchDown);

            //-- Touche de fermeture --//
            this.close.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnClosePreviewTouchDown);

            MajPage();

        }

        void ToLeft_PreviewTouchDown(object sender, TouchEventArgs e)
        {

            switch (numPageSpecCar)
            { 
                case 2:
                    SepcialCarPage1();
                    break;
                case 3:
                    SepcialCarPage2();
                    break;
                default:
                    return;
            }
            numPageSpecCar--;
        }

        void ToRight_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            switch (numPageSpecCar)
            {
                case 1:
                    SepcialCarPage2();
                    break;
                case 2:
                    SepcialCarPage3();
                    break;
                default:
                    return;
            }
            numPageSpecCar++;
        }

        void OnCarSpePreviewTouchDown(object sender, TouchEventArgs e)
        {
            numPageSpecCar = 1;

            if (isSpecialCar)
            {
                isCapsLock = true;
                isSpecialCar = false;
                isAccent = false;
                MajPage();
                return;
            }

            isSpecialCar = true;
            SepcialCarPage1();
        }

        void Accent_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            isCapsLock = true;
            
            if (isAccent)
            {    
                isSpecialCar = false;
                isAccent = false;
                MajPage();
                return;
            }

            isAccent = true;
            AccentPage();
        }

        void OnUpperPreviewTouchDown(object sender, TouchEventArgs e)
        {
            isCapsLock = !isCapsLock;
            if (isAccent)
            {
                AccentPage();
            }
            else
            {
                MajPage();
            }
        }



        //
        //  TIMER POUR LE BACKSPACE
        //
        /// <summary>
        /// Temps entre chaque suppression de caractère lors de l'appui long sur le backspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backSpaceTimer_Tick(object sender, EventArgs e)
        {
            if (isBackSpaceDown)
            {
                if (compteurBackSpace < 4)
                {
                    compteurBackSpace++;
                }
                else
                {
                    NodeParent.AjoutTexte("backspace");
                }
            }
        }
        /// <summary>
        ///  reinitialise l'appui long sur le backspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Backspace_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            compteurBackSpace = 0;
            isBackSpaceDown = false;
        }



        //
        //  GESTION FERMETURE
        //
        /// <summary>
        /// Ferme le clavier virtuel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClosePreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne ferme la fenêtre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte("Close");
            }
        }
        

        
        /// <summary>
        /// Appelé lors de l'appui d'une lettre ou d'un autre caractère sur le clavier virtuel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLetterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne tape une lettre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                if (((SurfaceButton)sender).Name.ToString().ToLower() == "backspace")
                {
                    isBackSpaceDown = true;
                    NodeParent.AjoutTexte("backspace");
                    return;
                }
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }
        /// <summary>
        /// Lors de l'appui d'une touche majuscule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMajLetterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne tape une lettre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                if (((SurfaceButton)sender).Content.ToString().ToLower() == "backspace")
                {
                    isBackSpaceDown = true;
                }
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }

            // Changement Page
            MajPage();
            //
        }
        /// <summary>
        /// Appui sur la touche "Entrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Enter_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne passe à la ligne que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }
        /// <summary>
        /// Appuis sur la Barre espace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Space_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne fais un espace que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }
        /// <summary>
        /// Appui sur la touche tabulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tab_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne fait de tabulation que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                for(int i = 0; i < 4; i++)
                    NodeParent.AjoutTexte(" ");
            }
        }





        private void HideAllButtons()
        {
            this.A.Visibility = System.Windows.Visibility.Hidden;
            this.Z.Visibility = System.Windows.Visibility.Hidden;
            this.E.Visibility = System.Windows.Visibility.Hidden;
            this.R.Visibility = System.Windows.Visibility.Hidden;
            this.T.Visibility = System.Windows.Visibility.Hidden;
            this.Y.Visibility = System.Windows.Visibility.Hidden;
            this.U.Visibility = System.Windows.Visibility.Hidden;
            this.I.Visibility = System.Windows.Visibility.Hidden;
            this.O.Visibility = System.Windows.Visibility.Hidden;
            this.P.Visibility = System.Windows.Visibility.Hidden;
            this.Q.Visibility = System.Windows.Visibility.Hidden;
            this.S.Visibility = System.Windows.Visibility.Hidden;
            this.D.Visibility = System.Windows.Visibility.Hidden;
            this.F.Visibility = System.Windows.Visibility.Hidden;
            this.G.Visibility = System.Windows.Visibility.Hidden;
            this.H.Visibility = System.Windows.Visibility.Hidden;
            this.J.Visibility = System.Windows.Visibility.Hidden;
            this.K.Visibility = System.Windows.Visibility.Hidden;
            this.L.Visibility = System.Windows.Visibility.Hidden;
            this.M.Visibility = System.Windows.Visibility.Hidden;
            this.W.Visibility = System.Windows.Visibility.Hidden;
            this.X.Visibility = System.Windows.Visibility.Hidden;
            this.C.Visibility = System.Windows.Visibility.Hidden;
            this.V.Visibility = System.Windows.Visibility.Hidden;
            this.B.Visibility = System.Windows.Visibility.Hidden;
            this.N.Visibility = System.Windows.Visibility.Hidden;

            this.maj_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_Z.Visibility = System.Windows.Visibility.Hidden;
            this.maj_E.Visibility = System.Windows.Visibility.Hidden;
            this.maj_R.Visibility = System.Windows.Visibility.Hidden;
            this.maj_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_Y.Visibility = System.Windows.Visibility.Hidden;
            this.maj_U.Visibility = System.Windows.Visibility.Hidden;
            this.maj_I.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O.Visibility = System.Windows.Visibility.Hidden;
            this.maj_P.Visibility = System.Windows.Visibility.Hidden;
            this.maj_Q.Visibility = System.Windows.Visibility.Hidden;
            this.maj_S.Visibility = System.Windows.Visibility.Hidden;
            this.maj_D.Visibility = System.Windows.Visibility.Hidden;
            this.maj_F.Visibility = System.Windows.Visibility.Hidden;
            this.maj_G.Visibility = System.Windows.Visibility.Hidden;
            this.maj_H.Visibility = System.Windows.Visibility.Hidden;
            this.maj_J.Visibility = System.Windows.Visibility.Hidden;
            this.maj_K.Visibility = System.Windows.Visibility.Hidden;
            this.maj_L.Visibility = System.Windows.Visibility.Hidden;
            this.maj_M.Visibility = System.Windows.Visibility.Hidden;
            this.maj_W.Visibility = System.Windows.Visibility.Hidden;
            this.maj_X.Visibility = System.Windows.Visibility.Hidden;
            this.maj_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_V.Visibility = System.Windows.Visibility.Hidden;
            this.maj_B.Visibility = System.Windows.Visibility.Hidden;
            this.maj_N.Visibility = System.Windows.Visibility.Hidden;

            this.Zero.Visibility   = System.Windows.Visibility.Hidden;
            this.Un.Visibility     = System.Windows.Visibility.Hidden;
            this.Deux.Visibility   = System.Windows.Visibility.Hidden;
            this.Trois.Visibility  = System.Windows.Visibility.Hidden;
            this.Quatre.Visibility = System.Windows.Visibility.Hidden;
            this.Cinq.Visibility   = System.Windows.Visibility.Hidden;
            this.Six.Visibility    = System.Windows.Visibility.Hidden;
            this.Sept.Visibility   = System.Windows.Visibility.Hidden;
            this.Huit.Visibility   = System.Windows.Visibility.Hidden;
            this.Neuf.Visibility   = System.Windows.Visibility.Hidden;

            this.ToLeft.Visibility = System.Windows.Visibility.Hidden;
            this.ToRight.Visibility = System.Windows.Visibility.Hidden;

            this.quote.Visibility = System.Windows.Visibility.Hidden;
            this.virgule.Visibility = System.Windows.Visibility.Hidden;

            this.Tab.Visibility = System.Windows.Visibility.Hidden;
            this.exclamation.Visibility = System.Windows.Visibility.Hidden;
            this.interogation.Visibility = System.Windows.Visibility.Hidden;
            this.Euro.Visibility = System.Windows.Visibility.Hidden;
            this.Dollars.Visibility = System.Windows.Visibility.Hidden;
            this.Pourcent.Visibility = System.Windows.Visibility.Hidden;
            this.et.Visibility = System.Windows.Visibility.Hidden;
            this.par.Visibility = System.Windows.Visibility.Hidden;
            this.parbis.Visibility = System.Windows.Visibility.Hidden;
            this.moins.Visibility = System.Windows.Visibility.Hidden;
            this.under.Visibility = System.Windows.Visibility.Hidden;
            this.egal.Visibility = System.Windows.Visibility.Hidden;
            this.plus.Visibility = System.Windows.Visibility.Hidden;
            this.anti_slash.Visibility = System.Windows.Visibility.Hidden;
            this.pointvrgl.Visibility = System.Windows.Visibility.Hidden;
            this.dble_point.Visibility = System.Windows.Visibility.Hidden;
            this.dbl_quotes.Visibility = System.Windows.Visibility.Hidden;
            this.star.Visibility = System.Windows.Visibility.Hidden;
            this.slash.Visibility = System.Windows.Visibility.Hidden;

            this.exclamation_inv.Visibility = System.Windows.Visibility.Hidden;
            this.interogation_inv.Visibility = System.Windows.Visibility.Hidden;
            this.Yen.Visibility = System.Windows.Visibility.Hidden;
            this.Livre.Visibility = System.Windows.Visibility.Hidden;
            this.micro.Visibility = System.Windows.Visibility.Hidden;
            this.circonflexe.Visibility = System.Windows.Visibility.Hidden;
            this.smaller.Visibility = System.Windows.Visibility.Hidden;
            this.bigger.Visibility = System.Windows.Visibility.Hidden;
            this.crochet_ouv.Visibility = System.Windows.Visibility.Hidden;
            this.crochet_fer.Visibility = System.Windows.Visibility.Hidden;
            //this.accol_ouv.Visibility = System.Windows.Visibility.Hidden;
            this.accol_fer.Visibility = System.Windows.Visibility.Hidden;
            this.smaller_equal.Visibility = System.Windows.Visibility.Hidden;
            this.bigger_equal.Visibility = System.Windows.Visibility.Hidden;
            this.arobase.Visibility = System.Windows.Visibility.Hidden;
            this.diez.Visibility = System.Windows.Visibility.Hidden;
            this.vague.Visibility = System.Windows.Visibility.Hidden;
            this.cent.Visibility = System.Windows.Visibility.Hidden;

            this.exp_un.Visibility = System.Windows.Visibility.Hidden;
            this.exp_deux.Visibility = System.Windows.Visibility.Hidden;
            this.exp_trois.Visibility = System.Windows.Visibility.Hidden;
            this.un_deux.Visibility = System.Windows.Visibility.Hidden;
            this.un_quatre.Visibility = System.Windows.Visibility.Hidden;
            this.trois_quatre.Visibility = System.Windows.Visibility.Hidden;
            this.PI.Visibility = System.Windows.Visibility.Hidden;
            this.alpha.Visibility = System.Windows.Visibility.Hidden;
            this.beta.Visibility = System.Windows.Visibility.Hidden;
            this.gamma.Visibility = System.Windows.Visibility.Hidden;
            this.teta.Visibility = System.Windows.Visibility.Hidden;
            this.sigma.Visibility = System.Windows.Visibility.Hidden;
            this.para.Visibility = System.Windows.Visibility.Hidden;
            this.bull.Visibility = System.Windows.Visibility.Hidden;
            this.not.Visibility = System.Windows.Visibility.Hidden;
            this.degree.Visibility = System.Windows.Visibility.Hidden;
            this.copy.Visibility = System.Windows.Visibility.Hidden;
            this.reg.Visibility = System.Windows.Visibility.Hidden;

            this.Upper1.Visibility = System.Windows.Visibility.Hidden;
            this.Upper2.Visibility = System.Windows.Visibility.Hidden;

            this.A_A.Visibility = System.Windows.Visibility.Hidden;
            this.A_C.Visibility = System.Windows.Visibility.Hidden;
            this.A_G.Visibility = System.Windows.Visibility.Hidden;
            this.E_A.Visibility = System.Windows.Visibility.Hidden;
            this.E_C.Visibility = System.Windows.Visibility.Hidden;
            this.E_G.Visibility = System.Windows.Visibility.Hidden;
            this.E_T.Visibility = System.Windows.Visibility.Hidden;
            this.U_C.Visibility = System.Windows.Visibility.Hidden;
            this.U_G.Visibility = System.Windows.Visibility.Hidden;
            this.U_T.Visibility = System.Windows.Visibility.Hidden;
            this.I_T.Visibility = System.Windows.Visibility.Hidden;
            this.I_A.Visibility = System.Windows.Visibility.Hidden;
            this.I_C.Visibility = System.Windows.Visibility.Hidden;
            this.Y_A.Visibility = System.Windows.Visibility.Hidden;
            this.Y_T.Visibility = System.Windows.Visibility.Hidden;
            this.O_A.Visibility = System.Windows.Visibility.Hidden;
            this.O_C.Visibility = System.Windows.Visibility.Hidden;
            this.O_G.Visibility = System.Windows.Visibility.Hidden;
            this.O_T.Visibility = System.Windows.Visibility.Hidden;
            this.O_V.Visibility = System.Windows.Visibility.Hidden;
            this.OE.Visibility = System.Windows.Visibility.Hidden;
            this.AE.Visibility = System.Windows.Visibility.Hidden;
            this.C_C.Visibility = System.Windows.Visibility.Hidden;
            this.O_S.Visibility = System.Windows.Visibility.Hidden;
            this.B_All.Visibility = System.Windows.Visibility.Hidden;
            this.N_V.Visibility = System.Windows.Visibility.Hidden;

            this.maj_A_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_A_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_A_G.Visibility = System.Windows.Visibility.Hidden;
            this.maj_E_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_E_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_E_G.Visibility = System.Windows.Visibility.Hidden;
            this.maj_E_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_U_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_U_G.Visibility = System.Windows.Visibility.Hidden;
            this.maj_U_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_I_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_I_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_I_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_Y_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_Y_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_A.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_G.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_T.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_V.Visibility = System.Windows.Visibility.Hidden;
            this.maj_OE.Visibility = System.Windows.Visibility.Hidden;
            this.maj_AE.Visibility = System.Windows.Visibility.Hidden;
            this.maj_C_C.Visibility = System.Windows.Visibility.Hidden;
            this.maj_O_S.Visibility = System.Windows.Visibility.Hidden;
            this.maj_N_V.Visibility = System.Windows.Visibility.Hidden;

        }



        private void MajPage()
        {
            HideAllButtons();
            if (!isCapsLock)
            {
                this.maj_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_Z.Visibility = System.Windows.Visibility.Visible;
                this.maj_E.Visibility = System.Windows.Visibility.Visible;
                this.maj_R.Visibility = System.Windows.Visibility.Visible;
                this.maj_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_Y.Visibility = System.Windows.Visibility.Visible;
                this.maj_U.Visibility = System.Windows.Visibility.Visible;
                this.maj_I.Visibility = System.Windows.Visibility.Visible;
                this.maj_O.Visibility = System.Windows.Visibility.Visible;
                this.maj_P.Visibility = System.Windows.Visibility.Visible;
                this.maj_Q.Visibility = System.Windows.Visibility.Visible;
                this.maj_S.Visibility = System.Windows.Visibility.Visible;
                this.maj_D.Visibility = System.Windows.Visibility.Visible;
                this.maj_F.Visibility = System.Windows.Visibility.Visible;
                this.maj_G.Visibility = System.Windows.Visibility.Visible;
                this.maj_H.Visibility = System.Windows.Visibility.Visible;
                this.maj_J.Visibility = System.Windows.Visibility.Visible;
                this.maj_K.Visibility = System.Windows.Visibility.Visible;
                this.maj_L.Visibility = System.Windows.Visibility.Visible;
                this.maj_M.Visibility = System.Windows.Visibility.Visible;
                this.maj_W.Visibility = System.Windows.Visibility.Visible;
                this.maj_X.Visibility = System.Windows.Visibility.Visible;
                this.maj_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_V.Visibility = System.Windows.Visibility.Visible;
                this.maj_B.Visibility = System.Windows.Visibility.Visible;
                this.maj_N.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.A.Visibility = System.Windows.Visibility.Visible;
                this.Z.Visibility = System.Windows.Visibility.Visible;
                this.E.Visibility = System.Windows.Visibility.Visible;
                this.R.Visibility = System.Windows.Visibility.Visible;
                this.T.Visibility = System.Windows.Visibility.Visible;
                this.Y.Visibility = System.Windows.Visibility.Visible;
                this.U.Visibility = System.Windows.Visibility.Visible;
                this.I.Visibility = System.Windows.Visibility.Visible;
                this.O.Visibility = System.Windows.Visibility.Visible;
                this.P.Visibility = System.Windows.Visibility.Visible;
                this.Q.Visibility = System.Windows.Visibility.Visible;
                this.S.Visibility = System.Windows.Visibility.Visible;
                this.D.Visibility = System.Windows.Visibility.Visible;
                this.F.Visibility = System.Windows.Visibility.Visible;
                this.G.Visibility = System.Windows.Visibility.Visible;
                this.H.Visibility = System.Windows.Visibility.Visible;
                this.J.Visibility = System.Windows.Visibility.Visible;
                this.K.Visibility = System.Windows.Visibility.Visible;
                this.L.Visibility = System.Windows.Visibility.Visible;
                this.M.Visibility = System.Windows.Visibility.Visible;
                this.W.Visibility = System.Windows.Visibility.Visible;
                this.X.Visibility = System.Windows.Visibility.Visible;
                this.C.Visibility = System.Windows.Visibility.Visible;
                this.V.Visibility = System.Windows.Visibility.Visible;
                this.B.Visibility = System.Windows.Visibility.Visible;
                this.N.Visibility = System.Windows.Visibility.Visible;
            }

            this.quote.Visibility = System.Windows.Visibility.Visible;
            this.virgule.Visibility = System.Windows.Visibility.Visible;

            this.Upper1.Visibility = System.Windows.Visibility.Visible;
            this.Upper2.Visibility = System.Windows.Visibility.Visible;

            // on replace le point
            this.Point.SetValue(Grid.ColumnProperty, 24);
            this.Point.SetValue(Grid.RowProperty, 2);
            //on replace la barre espace
            this.Space.SetValue(Grid.ColumnProperty, 10);
            this.Space.SetValue(Grid.ColumnSpanProperty, 17);
            // on replace le backspace
            this.Backspace.SetValue(Grid.ColumnProperty, 30);
            this.Backspace.SetValue(Grid.ColumnSpanProperty, 6);
            // on replace Entrer
            this.Enter.SetValue(Grid.ColumnProperty, 31);
            this.Enter.SetValue(Grid.ColumnSpanProperty, 5);
            this.Enter.SetValue(Grid.RowSpanProperty, 1);
            // on replace le bouton close
            this.close.SetValue(Grid.ColumnProperty, 29);
            this.close.SetValue(Grid.ColumnSpanProperty, 7);
        }


        private void SepcialCarPage1()
        {

            HideAllButtons();

            this.Zero.Visibility = System.Windows.Visibility.Visible;
            this.Un.Visibility = System.Windows.Visibility.Visible;
            this.Deux.Visibility = System.Windows.Visibility.Visible;
            this.Trois.Visibility = System.Windows.Visibility.Visible;
            this.Quatre.Visibility = System.Windows.Visibility.Visible;
            this.Cinq.Visibility = System.Windows.Visibility.Visible;
            this.Six.Visibility = System.Windows.Visibility.Visible;
            this.Sept.Visibility = System.Windows.Visibility.Visible;
            this.Huit.Visibility = System.Windows.Visibility.Visible;
            this.Neuf.Visibility = System.Windows.Visibility.Visible;

            this.Tab.Visibility = System.Windows.Visibility.Visible;
            this.exclamation.Visibility = System.Windows.Visibility.Visible;
            this.interogation.Visibility = System.Windows.Visibility.Visible;
            this.Euro.Visibility = System.Windows.Visibility.Visible;
            this.Dollars.Visibility = System.Windows.Visibility.Visible;
            this.Pourcent.Visibility = System.Windows.Visibility.Visible;
            this.et.Visibility = System.Windows.Visibility.Visible;
            this.par.Visibility = System.Windows.Visibility.Visible;
            this.parbis.Visibility = System.Windows.Visibility.Visible;
            this.moins.Visibility = System.Windows.Visibility.Visible;
            this.under.Visibility = System.Windows.Visibility.Visible;
            this.egal.Visibility = System.Windows.Visibility.Visible;
            this.plus.Visibility = System.Windows.Visibility.Visible;
            this.anti_slash.Visibility = System.Windows.Visibility.Visible;
            this.pointvrgl.Visibility = System.Windows.Visibility.Visible;
            this.dble_point.Visibility = System.Windows.Visibility.Visible;
            this.dbl_quotes.Visibility = System.Windows.Visibility.Visible;
            this.star.Visibility = System.Windows.Visibility.Visible;
            this.slash.Visibility = System.Windows.Visibility.Visible;

            this.ToLeft.Visibility = System.Windows.Visibility.Visible;
            this.ToLeft.IsEnabled = false;
            this.ToRight.Visibility = System.Windows.Visibility.Visible;
            this.ToRight.IsEnabled = true;

            // on replace le point
            this.Point.SetValue(Grid.ColumnProperty, 28);
            this.Point.SetValue(Grid.RowProperty, 3);
            // on replace la barre espace
            this.Space.SetValue(Grid.ColumnProperty, 9);
            this.Space.SetValue(Grid.ColumnSpanProperty, 12);
            // on replace le backspace
            this.Backspace.SetValue(Grid.ColumnProperty, 32);
            this.Backspace.SetValue(Grid.ColumnSpanProperty, 4);
            // on replace Entrer
            this.Enter.SetValue(Grid.ColumnProperty, 32);
            this.Enter.SetValue(Grid.ColumnSpanProperty, 4);
            this.Enter.SetValue(Grid.RowSpanProperty, 2);
            // on replace le bouton close
            this.close.SetValue(Grid.ColumnProperty, 32);
            this.close.SetValue(Grid.ColumnSpanProperty, 4);

        }

        private void SepcialCarPage2()
        {

            HideAllButtons();

            this.Zero.Visibility = System.Windows.Visibility.Visible;
            this.Un.Visibility = System.Windows.Visibility.Visible;
            this.Deux.Visibility = System.Windows.Visibility.Visible;
            this.Trois.Visibility = System.Windows.Visibility.Visible;
            this.Quatre.Visibility = System.Windows.Visibility.Visible;
            this.Cinq.Visibility = System.Windows.Visibility.Visible;
            this.Six.Visibility = System.Windows.Visibility.Visible;
            this.Sept.Visibility = System.Windows.Visibility.Visible;
            this.Huit.Visibility = System.Windows.Visibility.Visible;
            this.Neuf.Visibility = System.Windows.Visibility.Visible;

            this.Tab.Visibility = System.Windows.Visibility.Visible;

            this.exclamation_inv.Visibility = System.Windows.Visibility.Visible;
            this.interogation_inv.Visibility = System.Windows.Visibility.Visible;
            this.Yen.Visibility = System.Windows.Visibility.Visible;
            this.Livre.Visibility = System.Windows.Visibility.Visible;
            this.micro.Visibility = System.Windows.Visibility.Visible;
            this.circonflexe.Visibility = System.Windows.Visibility.Visible;
            this.smaller.Visibility = System.Windows.Visibility.Visible;
            this.bigger.Visibility = System.Windows.Visibility.Visible;
            this.crochet_ouv.Visibility = System.Windows.Visibility.Visible;
            this.crochet_fer.Visibility = System.Windows.Visibility.Visible;
            //this.accol_ouv.Visibility = System.Windows.Visibility.Visible;
            this.accol_fer.Visibility = System.Windows.Visibility.Visible;
            this.smaller_equal.Visibility = System.Windows.Visibility.Visible;
            this.bigger_equal.Visibility = System.Windows.Visibility.Visible;
            this.arobase.Visibility = System.Windows.Visibility.Visible;
            this.diez.Visibility = System.Windows.Visibility.Visible;
            this.vague.Visibility = System.Windows.Visibility.Visible;
            this.cent.Visibility = System.Windows.Visibility.Visible;

            this.ToLeft.Visibility = System.Windows.Visibility.Visible;
            this.ToLeft.IsEnabled = true;
            this.ToRight.Visibility = System.Windows.Visibility.Visible;
            this.ToRight.IsEnabled = true;
        }

        private void SepcialCarPage3()
        {

            HideAllButtons();

            this.Zero.Visibility = System.Windows.Visibility.Visible;
            this.Un.Visibility = System.Windows.Visibility.Visible;
            this.Deux.Visibility = System.Windows.Visibility.Visible;
            this.Trois.Visibility = System.Windows.Visibility.Visible;
            this.Quatre.Visibility = System.Windows.Visibility.Visible;
            this.Cinq.Visibility = System.Windows.Visibility.Visible;
            this.Six.Visibility = System.Windows.Visibility.Visible;
            this.Sept.Visibility = System.Windows.Visibility.Visible;
            this.Huit.Visibility = System.Windows.Visibility.Visible;
            this.Neuf.Visibility = System.Windows.Visibility.Visible;

            this.Tab.Visibility = System.Windows.Visibility.Visible;

            this.exp_un.Visibility = System.Windows.Visibility.Visible;
            this.exp_deux.Visibility = System.Windows.Visibility.Visible;
            this.exp_trois.Visibility = System.Windows.Visibility.Visible;
            this.un_deux.Visibility = System.Windows.Visibility.Visible;
            this.un_quatre.Visibility = System.Windows.Visibility.Visible;
            this.trois_quatre.Visibility = System.Windows.Visibility.Visible;
            this.PI.Visibility = System.Windows.Visibility.Visible;
            this.alpha.Visibility = System.Windows.Visibility.Visible;
            this.beta.Visibility = System.Windows.Visibility.Visible;
            this.gamma.Visibility = System.Windows.Visibility.Visible;
            this.teta.Visibility = System.Windows.Visibility.Visible;
            this.sigma.Visibility = System.Windows.Visibility.Visible;
            this.para.Visibility = System.Windows.Visibility.Visible;
            this.bull.Visibility = System.Windows.Visibility.Visible;
            this.not.Visibility = System.Windows.Visibility.Visible;
            this.degree.Visibility = System.Windows.Visibility.Visible;
            this.copy.Visibility = System.Windows.Visibility.Visible;
            this.reg.Visibility = System.Windows.Visibility.Visible;

            this.ToLeft.Visibility = System.Windows.Visibility.Visible;
            this.ToLeft.IsEnabled = true;
            this.ToRight.Visibility = System.Windows.Visibility.Visible;
            this.ToRight.IsEnabled = false;
        }

        private void AccentPage()
        {

            HideAllButtons();
            if (!isCapsLock)
            {
                this.maj_A_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_A_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_A_G.Visibility = System.Windows.Visibility.Visible;
                this.maj_E_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_E_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_E_G.Visibility = System.Windows.Visibility.Visible;
                this.maj_E_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_U_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_U_G.Visibility = System.Windows.Visibility.Visible;
                this.maj_U_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_I_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_I_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_I_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_Y_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_Y_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_A.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_G.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_T.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_V.Visibility = System.Windows.Visibility.Visible;
                this.maj_OE.Visibility = System.Windows.Visibility.Visible;
                this.maj_AE.Visibility = System.Windows.Visibility.Visible;
                this.maj_C_C.Visibility = System.Windows.Visibility.Visible;
                this.maj_O_S.Visibility = System.Windows.Visibility.Visible;
                this.B_All.Visibility = System.Windows.Visibility.Visible;
                this.maj_N_V.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.A_A.Visibility = System.Windows.Visibility.Visible;
                this.A_C.Visibility = System.Windows.Visibility.Visible;
                this.A_G.Visibility = System.Windows.Visibility.Visible;
                this.E_A.Visibility = System.Windows.Visibility.Visible;
                this.E_C.Visibility = System.Windows.Visibility.Visible;
                this.E_G.Visibility = System.Windows.Visibility.Visible;
                this.E_T.Visibility = System.Windows.Visibility.Visible;
                this.U_C.Visibility = System.Windows.Visibility.Visible;
                this.U_G.Visibility = System.Windows.Visibility.Visible;
                this.U_T.Visibility = System.Windows.Visibility.Visible;
                this.I_T.Visibility = System.Windows.Visibility.Visible;
                this.I_A.Visibility = System.Windows.Visibility.Visible;
                this.I_C.Visibility = System.Windows.Visibility.Visible;
                this.Y_A.Visibility = System.Windows.Visibility.Visible;
                this.Y_T.Visibility = System.Windows.Visibility.Visible;
                this.O_A.Visibility = System.Windows.Visibility.Visible;
                this.O_C.Visibility = System.Windows.Visibility.Visible;
                this.O_G.Visibility = System.Windows.Visibility.Visible;
                this.O_T.Visibility = System.Windows.Visibility.Visible;
                this.O_V.Visibility = System.Windows.Visibility.Visible;
                this.OE.Visibility = System.Windows.Visibility.Visible;
                this.AE.Visibility = System.Windows.Visibility.Visible;
                this.C_C.Visibility = System.Windows.Visibility.Visible;
                this.O_S.Visibility = System.Windows.Visibility.Visible;
                this.B_All.Visibility = System.Windows.Visibility.Visible;
                this.N_V.Visibility = System.Windows.Visibility.Visible;
            }

            this.quote.Visibility = System.Windows.Visibility.Visible;
            this.virgule.Visibility = System.Windows.Visibility.Visible;

            this.Upper1.Visibility = System.Windows.Visibility.Visible;
            this.Upper2.Visibility = System.Windows.Visibility.Visible;

            // on replace le point
            this.Point.SetValue(Grid.ColumnProperty, 24);
            this.Point.SetValue(Grid.RowProperty, 2);
            //on replace la barre espace
            this.Space.SetValue(Grid.ColumnProperty, 10);
            this.Space.SetValue(Grid.ColumnSpanProperty, 17);
            // on replace le backspace
            this.Backspace.SetValue(Grid.ColumnProperty, 30);
            this.Backspace.SetValue(Grid.ColumnSpanProperty, 6);
            // on replace Entrer
            this.Enter.SetValue(Grid.ColumnProperty, 31);
            this.Enter.SetValue(Grid.ColumnSpanProperty, 5);
            this.Enter.SetValue(Grid.RowSpanProperty, 1);
            // on replace le bouton close
            this.close.SetValue(Grid.ColumnProperty, 29);
            this.close.SetValue(Grid.ColumnSpanProperty, 7);

        }

        //
        //  DESACTIVE les caractères SPECIAUX
        //
        public void DisableSpecialCarac()
        {
            /*dbl_quotes.IsEnabled = false;
            quote.IsEnabled = false;
            par1.IsEnabled = false;
            par1bis.IsEnabled = false;
            Tab.IsEnabled = false;
            Stars.IsEnabled = false;
            shift.IsEnabled = false;
            interogation.IsEnabled = false;

            Enter.IsEnabled = false;
            Entrer.IsEnabled = false;*/
        }
        public void EnableEnterKeys(bool enable)
        {
            Enter.IsEnabled = enable;
        }


    }
}
