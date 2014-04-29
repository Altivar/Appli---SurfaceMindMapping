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
using System.Windows.Forms;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

using System.IO;
using System.Xml;

namespace AppliProjetTut
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        //
        //  GESTION DU FICHIER
        //

        // nom du fichier ouvert
        string nomFichier = "<>";
        bool isModified = false;
        

        //
        ////

        // liste de node
        List<ScatterCustom> listNode = new List<ScatterCustom>();

        // liste de menu de création de node
        List<KeyValuePair<MenuCreation, Timer>> listMenu = new List<KeyValuePair<MenuCreation, Timer>>();

        // liste de ligne inter-node (trait rose avec triangle)
        List<Line> listLine = new List<Line>();
        List<Polygon> listPoly = new List<Polygon>();

        // gestion de rattache à un parent
        List<KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>> listLigneRattache = new List<KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>>();
        // gestion des poly de rattache à un parent
        List<KeyValuePair<Polygon, ScatterCustom>> listRattache = new List<KeyValuePair<Polygon, ScatterCustom>>();

        // gestion du multi-touch
        List<KeyValuePair<int, KeyValuePair<int, Point>>> listTouch = new List<KeyValuePair<int, KeyValuePair<int, Point>>>();
        int mLimiteNbrTouch = 4;

        // Position du Node initial
        Point initP1 = new Point(1200, 600);
        Point initP2 = new Point(800, 300);
        Point initP3 = new Point(400, 600);

        // timer
        Timer timeRefresh = new Timer();
        
        //Liste des Cercle Chargeur
        List<KeyValuePair<LoadCircle, Timer>> listLoadCircle = new List<KeyValuePair<LoadCircle, Timer>>();


        // tag du menu principal
        MenuPrincipal menuPrincipal;






        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // si le dossier de sauvegarde n'existe pas : on le crée
            DirectoryInfo SavesDir = new DirectoryInfo(".\\Saves\\");
            if (!SavesDir.Exists)
                SavesDir.Create();

            // ajout de Nodes
            AddNode(null, initP1, "Text");
            AddNode(null, initP2, "Image");
            AddNode(null, initP3, "Video");
            Modification(false);

            PreviewTouchMove += new EventHandler<TouchEventArgs>(OnPreviewTouchMove);
            PreviewTouchDown += new EventHandler<TouchEventArgs>(OnPreviewTouchDown);
            PreviewTouchUp += new EventHandler<TouchEventArgs>(OnPreviewTouchUp);

            timeRefresh.Interval = 30;
            timeRefresh.Tick += new EventHandler(TimerRefresh);
            timeRefresh.Start();

            

        }





        /// <summary>
        /// Timer de l'actualisation d'affichage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerRefresh(object sender, EventArgs e)
        {
            RefreshImage();
        }

        /// <summary>
        /// Timer d'expiration du menu de choix de type de Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChoiceMenuTimeExpired(object sender, EventArgs e)
        {
            //
            // SUPPRIME LE MENU EXPIRE
            //
            Timer menuTimer = (Timer)sender;
            if (menuTimer == null)
                return;

            for (int i = 0; i < listMenu.Count; i++)
            {
                if (listMenu.ElementAt(i).Value == menuTimer)
                {
                    this.MainScatterView.Items.Remove(listMenu.ElementAt(i).Key);
                    listMenu.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Timer des cercle de chargement (évite des bugs)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void circleTimeOut(object sender, EventArgs e)
        {
            Timer circleTimer = (Timer)sender;
            for (int i = 0; i < listLoadCircle.Count; i++)
            {
                if (listLoadCircle.ElementAt(i).Value == circleTimer)
                {
                    this.MainScatterView.Items.Remove(listLoadCircle.ElementAt(i).Key);
                    for (int j = 0; j < listTouch.Count; j++)
                    {
                        if (listLoadCircle.ElementAt(i).Key.Id == listTouch.ElementAt(j).Key)
                        {
                            listTouch.RemoveAt(j);
                            break;
                        }
                    }
                    listLoadCircle.RemoveAt(i);
                    return;
                }
            }
        }


        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        




        //
        //  EVENEMENT
        //
        /// <summary>
        /// Preview Touch Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTouchDown(object sender, TouchEventArgs e)
        {

            if (e.TouchDevice.GetIsFingerRecognized())
            {
                
                // Verification pour le menu principal si le touch est dessus
                if (menuPrincipal != null)
                {
                    if (menuPrincipal.AreAnyTouchesOver)
                    {
                        return;
                    }
                }

                //Pour chaque node, verification de la position du touch
                for (int i = 0; i < listNode.Count; i++)
                {
                    if (listNode.ElementAt(i).AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                    {
                        return;
                    }
                }

                //Pour chaque node, verification de la position du touch
                for (int i = 0; i < listRattache.Count; i++)
                {
                    if (listRattache.ElementAt(i).Key.AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                    {
                        return;
                    }
                }


                if (listTouch.Count < mLimiteNbrTouch)
                {
                    // Timer du cercle de chargement
                    Timer circleTimer = new Timer();
                    circleTimer.Interval = 5000;    // durée de vie maximum : 5s
                    circleTimer.Tick += new EventHandler(circleTimeOut);
                    circleTimer.Start();
                    // Cercle de chargement
                    LoadCircle mLCircle = new LoadCircle();
                    mLCircle.Id = e.TouchDevice.Id;
                    mLCircle.Center = e.TouchDevice.GetPosition(this);

                    // Pair du cercle et de son timer
                    KeyValuePair<LoadCircle, Timer> myPair = new KeyValuePair<LoadCircle, Timer>(mLCircle, circleTimer);

                    listLoadCircle.Add(myPair);
                    MainScatterView.Items.Add(mLCircle);


                    // on ajoute cette instance de point avec : 
                    // son ID
                    // sa position
                    // son heure d'apparition
                    KeyValuePair<int, Point> statTouch = new KeyValuePair<int, Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
                    KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int, KeyValuePair<int, Point>>(e.TouchDevice.Id, statTouch);
                    listTouch.Add(pairTouch);
                }
            }
        }

        
        

        /// <summary>
        /// Preview Touch Move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                // met a jour la ligne de rattache si l'id du touch correspond
                bool isRattache = false;
                for (int i = 0; i < listLigneRattache.Count && !isRattache; i++)
                {
                    if (e.TouchDevice.Id == listLigneRattache.ElementAt(i).Value.Key)
                    {
                        listLigneRattache.ElementAt(i).Value.Value.X2 = e.TouchDevice.GetPosition(this).X;
                        listLigneRattache.ElementAt(i).Value.Value.Y2 = e.TouchDevice.GetPosition(this).Y;
                        isRattache = true;
                    }
                }



                for (int i = 0; i < listTouch.Count; i++)
                {
                    if (listTouch.ElementAt(i).Key == e.TouchDevice.Id)
                    {
                        double diffX = listTouch.ElementAt(i).Value.Value.X - e.TouchDevice.GetPosition(this).X;
                        double diffY = listTouch.ElementAt(i).Value.Value.Y - e.TouchDevice.GetPosition(this).Y;
                        if (diffX * diffX + diffY * diffY > 900)    // si le déplacement depuis le point de départ est plus grand que 30pxl, on reinit le timer
                        {
                            bool done = false;
                            for (int j = 0; j < listLoadCircle.Count && !done; j++)
                            {
                                if (listLoadCircle.ElementAt(j).Key.Id == e.TouchDevice.Id)
                                {
                                    // on supprime l'ancien cercle
                                    MainScatterView.Items.Remove(listLoadCircle.ElementAt(j).Key);
                                    listLoadCircle.RemoveAt(j);

                                    if (!isRattache)
                                    {
                                        // On ajoute le nouveau
                                        //

                                        // Timer du cercle de chargement
                                        Timer circleTimer = new Timer();
                                        circleTimer.Interval = 5000;    // durée de vie maximum : 5s
                                        circleTimer.Tick += new EventHandler(circleTimeOut);
                                        circleTimer.Start();
                                        // Cercle de chargement
                                        LoadCircle mLCircle = new LoadCircle();
                                        mLCircle.Id = e.TouchDevice.Id;
                                        mLCircle.Center = e.TouchDevice.GetPosition(this);

                                        // Pair du cercle et de son timer
                                        KeyValuePair<LoadCircle, Timer> myPair = new KeyValuePair<LoadCircle, Timer>(mLCircle, circleTimer);

                                        listLoadCircle.Add(myPair);
                                        MainScatterView.Items.Add(mLCircle);
                                    }

                                    done = true;
                                }
                            }
                            listTouch.RemoveAt(i);
                            if (!isRattache)
                            {
                                KeyValuePair<int, Point> statTouch = new KeyValuePair<int, Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
                                KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int, KeyValuePair<int, Point>>(e.TouchDevice.Id, statTouch);
                                listTouch.Add(pairTouch);
                            }
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Preview Touch Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                for (int i = 0; i < listLigneRattache.Count; i++)
                {
                    if (e.TouchDevice.Id == listLigneRattache.ElementAt(i).Value.Key)
                    {


                        double length = -1;
                        ScatterCustom nearestText = null;
                        for (int j = 0; j < listNode.Count; j++) // on teste la distance de chaque node
                        {
                            if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j)) // on ne test pas la node a laquelle la ligne est rattachée
                            {
                                if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j).GetParent()) // on test si la node n'a pas pour parent celle a laquelle la ligne est rattachée
                                {
                                    double diffXCarre = (listNode.ElementAt(j).ActualCenter.X - listLigneRattache.ElementAt(i).Value.Value.X2);
                                    diffXCarre *= diffXCarre;
                                    double diffYCarre = (listNode.ElementAt(j).ActualCenter.Y - listLigneRattache.ElementAt(i).Value.Value.Y2);
                                    diffYCarre *= diffYCarre;
                                    double testLenght = Math.Sqrt(diffXCarre + diffYCarre);
                                    if (length == -1 || length > testLenght) // si la node est plus proche que la précédente on la retient
                                    {
                                        length = testLenght;
                                        nearestText = listNode.ElementAt(j);
                                    }
                                }
                            }
                        }

                        if (length != -1) // si la node la plus proche est assez près
                        {
                            double dimension = (nearestText.Width > nearestText.Height) ? nearestText.Width : nearestText.Height;
                            if (length < dimension / 3 * 2)
                            {
                                listLigneRattache.ElementAt(i).Key.SetParent(nearestText);
                                Modification(true);
                            }
                        }

                        // ensuite on supprime la ligne
                        listLigneRattache.RemoveAt(i);

                    }
                }


                for (int i = 0; i < listLoadCircle.Count; i++)
                {
                    if (listLoadCircle.ElementAt(i).Key.Id == e.TouchDevice.Id)
                    {
                        MainScatterView.Items.Remove(listLoadCircle.ElementAt(i).Key);
                        listLoadCircle.RemoveAt(i);
                    }
                }

                for (int i = 0; i < listTouch.Count; i++)
                {
                    if (listTouch.ElementAt(i).Key == e.TouchDevice.Id)
                    {
                        if (e.Timestamp - listTouch.ElementAt(i).Value.Key > 2000)  // si l'appui a duré +2s
                        {
                            MenuCreation ChoiceNode = new MenuCreation(this);
                            ChoiceNode.Center = e.TouchDevice.GetCenterPosition(this);
                            ChoiceNode.Orientation = e.TouchDevice.GetOrientation(this) + 90.0;
                            MainScatterView.Items.Add(ChoiceNode);

                            Timer menuLifeTime = new Timer();
                            menuLifeTime.Interval = 5000;
                            menuLifeTime.Tick += new EventHandler(ChoiceMenuTimeExpired);
                            menuLifeTime.Start();

                            KeyValuePair<MenuCreation, Timer> myPair = new KeyValuePair<MenuCreation, Timer>(ChoiceNode, menuLifeTime);
                            listMenu.Add(myPair);
                        }
                        listTouch.RemoveAt(i);
                    }
                }
            }
        }


        //////////////////////////////////                                       //////////////////////////////////
        /////////////////                                                                         /////////////////
        /////////////////                        GESTION du MENU PRINCIPAL                        /////////////////
        /////////////////                                                                         /////////////////
        //////////////////////////////////                                       //////////////////////////////////
        /// <summary>
        /// Appelé lorsqu'on ajoute un tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            // gestion du tag
            MenuPrincipal mainMenu = (MenuPrincipal)e.TagVisualization;
            
            if (mainMenu != null)
            {
                menuPrincipal = mainMenu;
                menuPrincipal.SetSurfaceWindow(this);
            }
        }
        /// <summary>
        /// Appelé lorsqu'on enleve un tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagVisualizer_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            if (menuPrincipal == (MenuPrincipal)e.TagVisualization)
            {
                menuPrincipal = null;
            }
        }

        /// <summary>
        /// Retourne le nom du fichier
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return nomFichier;
        }

        /// <summary>
        /// Fonction d'enregistrement des fichiers
        /// </summary>
        /// <param name="saveFileName"></param>
        public void SaveUnderFileName(string saveFileName)
        {
            // on crée le nouveau dossier on annule tout en cas d'erreur
            if(!CreateFolder(saveFileName))
                return;

            if (!SaveImageFiles(saveFileName))
                return;

            if (!SaveToXMLFile(saveFileName))
                return;

            nomFichier = saveFileName;
        }

        /// <summary>
        /// Crée le dossier ou se trouvera la sauvegarde
        /// </summary>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        private bool CreateFolder(string saveFileName)
        {
            // on passe les sauvegarde pour chercher si le dossier existe déjà
            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves");
            
            DirectoryInfo[] DirFiles = dirInfo.GetDirectories();
            foreach (DirectoryInfo dir in DirFiles)
            {
                if (dir.Name == saveFileName)
                {
                    // demande de confirmation
                    string title = "Attention";
                    string message = "Ecraser le dossier de sauvegarde existant?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult res;

                    res = System.Windows.Forms.MessageBox.Show(message, title, buttons);

                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        // on supprime le dossier existant
                        dir.Delete(true);
                    }
                    else
                    {
                        // On annule la suppression
                        return false;
                    }
                }
            }

            // une fois l'ancienne suavegarde supprimée on sauve la nouvelle
            DirectoryInfo newDir = new DirectoryInfo(".\\Saves\\" + saveFileName);
            newDir.Create();

            return true;

        }


        /// <summary>
        /// Sauvegarde les images utilisées
        /// </summary>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        private bool SaveImageFiles(string saveFileName)
        {

            // si le repertoire des images n'est pas créé on le crée
            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves\\" + saveFileName + "\\Images");
            if (!dirInfo.Exists)
                dirInfo.Create();

            List<string> imagesSaved = new List<string>();
            
            for (int i = 0; i < listNode.Count; i++)
            {

                try
                {

                    // si le node n'est pas un image on passe l'iteration actuelle
                    if (listNode.ElementAt(i).GetTypeOfNode() != "Image")
                        continue;

                    // si il y a une erreur avec ce node on passe l'iteration actuelle
                    NodeImage myImage = (NodeImage)listNode.ElementAt(i);
                    if (myImage == null)
                        continue;

                    // on recupère le nom de l'image
                    string imgPath = myImage.GetImagePath();

                    // si le node n'a pas d'image on passe
                    if (imgPath == "NONE")
                        continue;

                    // si l'image de ce node a déjà été sauvegardée
                    if (imagesSaved.Contains(imgPath))
                        continue;
                    else
                        imagesSaved.Add(imgPath);

                    // on recupere l'image elle meme
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(".\\Resources\\Images\\" + imgPath, UriKind.Relative);
                    bi.EndInit();

                    // on recupere le format de l'image
                    string separator = ".";
                    string imgFormat = imgPath.Split(separator.ToCharArray()).Last();

                    // on defini où l'image sera stockée
                    string imgLocation = ".\\Saves\\" + saveFileName + "\\Images\\" + imgPath;




                    FileStream fileStr = new FileStream(imgLocation, FileMode.Create);

                    // on enregistre l'image avec l'encoder adequat
                    switch (imgFormat)
                    {
                        case "jpg":
                            JpegBitmapEncoder encoderJPG = new JpegBitmapEncoder();
                            encoderJPG.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                            encoderJPG.Save(fileStr);
                            break;
                        case "png":
                            PngBitmapEncoder encoderPNG = new PngBitmapEncoder();
                            encoderPNG.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                            encoderPNG.Save(fileStr);
                            break;
                        case "gif":
                            GifBitmapEncoder encoderGIF = new GifBitmapEncoder();
                            encoderGIF.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                            encoderGIF.Save(fileStr);
                            break;
                        case "bmp":
                            BmpBitmapEncoder encoderBMP = new BmpBitmapEncoder();
                            encoderBMP.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                            encoderBMP.Save(fileStr);
                            break;
                    }

                }
                catch
                {
                    return false;
                }


            }

            return true;
        }


        /// <summary>
        /// Crée le fichier XML de la sauvegarde
        /// </summary>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        private bool SaveToXMLFile(string saveFileName)
        {
            // on definit le chemin de la sauvegarde
            string xmlFilePath = ".\\Saves\\" + saveFileName + "\\" + saveFileName + ".xml";

            // on initialise la sauvegarde
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(docNode);


            //
            ///
            //// on debute l'ecriture du document
            XmlNode rootNode = xmlDoc.CreateElement("nodes");
            xmlDoc.AppendChild(rootNode);
            XmlNode elementNode;
            XmlAttribute typeAttribute, idAttribute;


            for(int i = 0; i < listNode.Count; i++)
            {
                
                // on crée la balise specifique au node actuel
                elementNode = xmlDoc.CreateElement("node");
                // on lui crée un ou plusieurs attributs
                typeAttribute = xmlDoc.CreateAttribute("type");
                string typeOfNode = listNode.ElementAt(i).GetTypeOfNode();
                typeAttribute.Value = typeOfNode;
                elementNode.Attributes.Append(typeAttribute);

                idAttribute = xmlDoc.CreateAttribute("id");
                idAttribute.Value = i.ToString();
                elementNode.Attributes.Append(idAttribute);
                
                // on lui ajoute des données internes
                string idParent = GetNumOfParent(listNode.ElementAt(i)).ToString();
                if (idParent != "-1")
                {
                    XmlNode parentNode = xmlDoc.CreateElement("parent");
                    XmlAttribute idParentAttr = xmlDoc.CreateAttribute("id");
                    idParentAttr.Value = idParent;
                    parentNode.Attributes.Append(idParentAttr);
                    elementNode.AppendChild(parentNode);
                }
                
                // selon le type du node on ajoute les element suivants :
                // - texte : le texte + le couleur du texte
                // - image : la source de l'image + le texte + la couelur du texte
                // - video : la source de la video + le texte + la couleur du texte
                switch (typeOfNode)
                { 
                    case "Text":

                        NodeText txt = (NodeText)listNode.ElementAt(i);
                        if(txt == null)
                            continue;
                        XmlNode textNode = xmlDoc.CreateElement("text");
                        XmlAttribute txtAttr = xmlDoc.CreateAttribute("color");
                        txtAttr.Value = txt.GetColor().ToString();
                        textNode.Attributes.Append(txtAttr);
                        if(txt.GetText() != "")
                            textNode.InnerText = txt.GetText();
                        elementNode.AppendChild(textNode);
                        break;
                    
                    case "Image":
                        NodeImage img = (NodeImage)listNode.ElementAt(i);
                        if (img == null)
                            continue;
                        if (img.GetImagePath() != "NONE")
                        {
                            XmlNode imagePathNode = xmlDoc.CreateElement("image");
                            XmlAttribute imgAttr = xmlDoc.CreateAttribute("source");
                            imgAttr.Value = ".\\Images\\" + img.GetImagePath();
                            imagePathNode.Attributes.Append(imgAttr);
                            elementNode.AppendChild(imagePathNode);
                        }
                        
                        
                        XmlNode imageTextNode = xmlDoc.CreateElement("text");
                        XmlAttribute imgTxtAttr = xmlDoc.CreateAttribute("color");
                        imgTxtAttr.Value = img.textAnnotation.GetColor().ToString();
                        imageTextNode.Attributes.Append(imgTxtAttr);
                        if (img.textAnnotation.GetText() != "")
                            imageTextNode.InnerText = img.textAnnotation.GetText();
                        elementNode.AppendChild(imageTextNode);
                        break;
                    
                    case "Video":
                        NodeVideo vid = (NodeVideo)listNode.ElementAt(i);
                        if (vid == null)
                            continue;
                        if (vid.GetVideoPath() != "NONE")
                        {
                            XmlNode imagePathNode = xmlDoc.CreateElement("video");
                            XmlAttribute imgAttr = xmlDoc.CreateAttribute("source");
                            imgAttr.Value = vid.GetVideoPath();
                            imagePathNode.Attributes.Append(imgAttr);
                            elementNode.AppendChild(imagePathNode);
                        }
                        XmlNode videoTextNode = xmlDoc.CreateElement("text");
                        XmlAttribute vidTxtAttr = xmlDoc.CreateAttribute("color");
                        vidTxtAttr.Value = vid.textAnnotation.GetColor().ToString();
                        videoTextNode.Attributes.Append(vidTxtAttr);
                        if (vid.textAnnotation.GetText() != "")
                            videoTextNode.InnerText = vid.textAnnotation.GetText();
                        elementNode.AppendChild(videoTextNode);
                        break;
                }


                // on l'ajoute au groupe de balise
                rootNode.AppendChild(elementNode);

            }

            xmlDoc.Save(xmlFilePath);
            //// on termine l'écriture du document
            ///
            //
            return true;
        }



        /// <summary>
        /// Récupère la position du parent dans la liste
        /// </summary>
        /// <param name="scatt"></param>
        /// <returns></returns>
        private int GetNumOfParent(ScatterCustom scatt)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if (scatt.GetParent() == listNode.ElementAt(i))
                {
                    return i;
                }
            }

            return -1;
        }



        /// <summary>
        /// Ouvre le fichier dont le nom est passé en paramètre
        /// </summary>
        public void OpenFile(string fileName)
        {

            // on supprime les anciens nodes
            for (int i = 0; i < listNode.Count; i++)
            {
                this.MainScatterView.Items.Remove(listNode.ElementAt(i));
            }
            listNode.Clear();



            string filePath = ".\\Saves\\" + fileName + "\\";

            List<KeyValuePair<KeyValuePair<int, int>, ScatterCustom>> listParent = new List<KeyValuePair<KeyValuePair<int, int>, ScatterCustom>>();
            
            // on crée le xmldoc qui permettra la lecture
            XmlDocument xmlDoc = new XmlDocument();
            // on charge le fichier
            xmlDoc.Load(filePath + fileName + ".xml");

            // on récupère la liste des nodes
            XmlNodeList listElementNode = xmlDoc.SelectNodes("//nodes/node");
            foreach (XmlNode nodeElmt in listElementNode)
            {
                
                string typeOfNode = nodeElmt.Attributes["type"].Value;

                XmlNode parentNode = nodeElmt.SelectSingleNode("parent");
                KeyValuePair<int, int> myPair;
                if (parentNode != null)
                {
                    myPair = new KeyValuePair<int, int>(Convert.ToInt32(nodeElmt.Attributes["id"].Value), Convert.ToInt32(parentNode.Attributes["id"].Value));
                    //listParentNode.Add(myPair);
                }
                else
                {
                    myPair = new KeyValuePair<int, int>(Convert.ToInt32(nodeElmt.Attributes["id"].Value), -1);
                }

                ScatterCustom newNode = new ScatterCustom(this, null);

                switch (typeOfNode)
                { 
                    case "Text":
                        NodeText txt = new NodeText(this, null);
                        XmlNode textNode = nodeElmt.SelectSingleNode("text");
                        if (textNode != null)
                        {
                            try
                            {
                                string txtContent = textNode.InnerText;
                                txt.LoadText(txtContent, true);
                            }
                            catch { }
                            string txtColor = textNode.Attributes["color"].Value;
                            txt.SetColor(txtColor);
                        }
                        newNode = txt;
                        this.MainScatterView.Items.Add(txt);
                        listNode.Add(txt);
                        break;
                    case "Image":
                        NodeImage img = new NodeImage(this, null);
                        XmlNode imageNode = nodeElmt.SelectSingleNode("image");
                        if (imageNode != null)
                        {
                            string imgPath = imageNode.Attributes["source"].Value.Remove(0,1);
                            imgPath = "Saves\\" + fileName + imgPath;
                            //
                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            bi.UriSource = new Uri(imgPath, UriKind.Relative);
                            bi.EndInit();
                            Point dim = new Point(bi.Width, bi.Height);
                            Brush bru = new ImageBrush(bi);
                            //
                            Point imgDim = new Point(bi.Width, bi.Height);
                            img.LoadImage(bru, imgDim, imgPath);    
                        }
                        XmlNode imgTextNode = nodeElmt.SelectSingleNode("text");
                        if (imgTextNode != null)
                        {
                            try
                            {
                                string txtContent = imgTextNode.InnerText;
                                img.textAnnotation.LoadText(txtContent, true);
                            }
                            catch { }
                            string txtColor = imgTextNode.Attributes["color"].Value;
                            img.textAnnotation.SetColor(txtColor);
                        }
                        newNode = img;
                        this.MainScatterView.Items.Add(img);
                        listNode.Add(img);
                        break;
                    case "Video":
                        NodeVideo vid = new NodeVideo(this, null);
                        XmlNode videoNode = nodeElmt.SelectSingleNode("video");
                        if (videoNode != null)
                        {
                            string vidPath = videoNode.Attributes["source"].Value;
                            vid.SetVideoPath(vidPath);
                        }
                        XmlNode vidTextNode = nodeElmt.SelectSingleNode("text");
                        if (vidTextNode != null)
                        {
                            try
                            {
                                string txtContent = vidTextNode.InnerText;
                                vid.textAnnotation.LoadText(txtContent, true);
                            }
                            catch { }
                            string txtColor = vidTextNode.Attributes["color"].Value;
                            vid.textAnnotation.SetColor(txtColor);
                        }
                        newNode = vid;
                        this.MainScatterView.Items.Add(vid);
                        listNode.Add(vid);
                        break;
                }


                KeyValuePair<KeyValuePair<int, int>, ScatterCustom> myTriple = new KeyValuePair<KeyValuePair<int, int>, ScatterCustom>(myPair, newNode);
                listParent.Add(myTriple);


            }



            //
            // Chargement des liens de parenté
            //
            for (int i = 0; i < listParent.Count; i++)
            {
                if (listParent.ElementAt(i).Key.Value != -1)
                {
                    for (int j = 0; j < listParent.Count; j++)
                    {
                        if (listParent.ElementAt(i).Key.Value == listParent.ElementAt(j).Key.Key)
                        {
                            listParent.ElementAt(i).Value.SetParent(listParent.ElementAt(j).Value);
                        }
                    }
                }
                else
                    listParent.ElementAt(i).Value.SetParent(null);
            }

        }




        



        //
        //  FONCTION DE GESTION DES NODES
        //
        /// <summary>
        /// Ajoute un NODE du type passé en paramètre
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pt"></param>
        /// <param name="typeNode"></param>
        public void AddNode(ScatterCustom parent, Point pt, String typeNode)
        {
            switch (typeNode)
            {
                case "Text":
                    NodeText text = new NodeText(this, parent);
                    text.Center = pt;
                    this.MainScatterView.Items.Add(text);
                    listNode.Add(text);
                    break;
                case "Image":
                    NodeImage image = new NodeImage(this, parent);
                    image.Center = pt;
                    this.MainScatterView.Items.Add(image);
                    listNode.Add(image);
                    break;
                case "Video":
                    NodeVideo video = new NodeVideo(this, parent);
                    video.Center = pt;
                    this.MainScatterView.Items.Add(video);
                    listNode.Add(video);
                    break;
                default:
                    return;

            }
            Modification(true);

        }
        /// <summary>
        /// Supprime le NODE passé en paramètre
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="confirmation"></param>
        public void RemoveNode(ScatterCustom parent, bool confirmation)
        {
            bool conf = confirmation;

            for (int i = 0; i < listLigneRattache.Count; i++)
            {
                if (listLigneRattache.ElementAt(i).Key == parent)
                {
                    listLigneRattache.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode.ElementAt(i).GetParent() == parent)
                {
                    if (conf)
                    {
                        // demande de confirmation
                        string title = "Warning";
                        string message = "Remove this Text will remove automatically its children. Remove?";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult res;

                        res = System.Windows.Forms.MessageBox.Show(message, title, buttons);

                        if (res == System.Windows.Forms.DialogResult.No)
                        {
                            // On annule la suppression
                            return;
                        }

                        // on separe de son parent avant d'effectuer la suppression des enfants
                        // utile dans le cas d'une boucle
                        parent.SetParent(null);

                        conf = false;


                    }
                    RemoveNode(listNode.ElementAt(i), false);
                    i--;
                }
            }
            listNode.Remove(parent);
            this.MainScatterView.Items.Remove(parent);
            if (confirmation)
            {
                Modification(true);
                RefreshImage();
            }

        }


        //
        //  REFRESH
        //
        /// <summary>
        /// Fonction de Refresh de la table appelée a chaque tick du timer
        /// </summary>
        private void RefreshImage()
        {
            // on efface toutes les lignes des liens internode
            this.LineGrid.Children.RemoveRange(0, this.LineGrid.Children.Count);

            // on efface tous les cercles de chargement verts
            listPoly.Clear();
            this.LinkParentGrid.Children.RemoveRange(0, this.LinkParentGrid.Children.Count);

            
            for (int i = 0; i < listPoly.Count; i++)
            {
                this.LineGrid.Children.Remove(listPoly.ElementAt(i));
            }
            listPoly.Clear();


            //
            //  DESSIN DES LIAISONS INTER-NODES
            //
            for (int i = 0; i < listNode.Count; i++)
            {
                Line tempLine = listNode.ElementAt(i).getLineToParent();
                if (!(tempLine.X1 == 0 && tempLine.Y1 == 0 && tempLine.X2 == 0 && tempLine.Y2 == 0))
                {
                    // on dessine la ligne
                    this.LineGrid.Children.Add(tempLine);

                    Polygon triangle = new Polygon();
                    double pourcentage = Math.Sqrt(400 / ((tempLine.X1 - tempLine.X2) * (tempLine.X1 - tempLine.X2) + (tempLine.Y1 - tempLine.Y2) * (tempLine.Y1 - tempLine.Y2)));
                    double Xplus = (tempLine.X1 + tempLine.X2) / 2 - pourcentage * (tempLine.X2 - tempLine.X1);
                    double Yplus = (tempLine.Y1 + tempLine.Y2) / 2 - pourcentage * (tempLine.Y2 - tempLine.Y1);

                    double XVect = Xplus - (tempLine.X1 + tempLine.X2) / 2;
                    double YVect = Yplus - (tempLine.Y1 + tempLine.Y2) / 2;

                    Point ptMilieu = new Point((tempLine.X1 + tempLine.X2) / 2, (tempLine.Y1 + tempLine.Y2) / 2);
                    Point ptFleche1 = new Point(Xplus + YVect, Yplus - XVect);
                    Point ptFleche2 = new Point(Xplus - YVect, Yplus + XVect);

                    // on crée le triangle a partir des points calculés précedemment
                    PointCollection triangleCollection = new PointCollection();
                    triangleCollection.Add(ptMilieu);
                    triangleCollection.Add(ptFleche1);
                    triangleCollection.Add(ptFleche2);
                    triangleCollection.Add(ptMilieu);
                    triangle.Points = triangleCollection;
                    triangle.Stroke = Brushes.PaleVioletRed;
                    triangle.Fill = Brushes.PaleVioletRed;
                    listPoly.Add(triangle);

                    this.LineGrid.Children.Add(triangle);
                }
                if (listNode.ElementAt(i).isActive())
                {
                    

                    Polygon poly = new Polygon();

                    Point pt1 = listNode.ElementAt(i).PointFromScreen(listNode.ElementAt(i).ActualCenter);
                    Point pt2 = pt1;
                    Point pt3 = pt1;
                    Point pt4 = pt1;
                    Point pt5 = pt1;
                    Point pt6 = pt1;
                    // placement du premier point de la pseudo-ellipse
                    pt1.X -= listNode.ElementAt(i).ActualWidth / 2 - 1;
                    pt1.Y -= listNode.ElementAt(i).ActualHeight / 2 - 5;
                    pt1 = listNode.ElementAt(i).PointToScreen(pt1);
                    // placement du premier point de la pseudo-ellipse
                    pt2.X -= listNode.ElementAt(i).ActualWidth / 2 - 1;
                    pt2.Y -= listNode.ElementAt(i).ActualHeight / 2 + 45;
                    pt2 = listNode.ElementAt(i).PointToScreen(pt2);
                    // placement du premier point de la pseudo-ellipse
                    pt3.X -= listNode.ElementAt(i).ActualWidth / 2 - 5;
                    pt3.Y -= listNode.ElementAt(i).ActualHeight / 2 + 50;
                    pt3 = listNode.ElementAt(i).PointToScreen(pt3);
                    // placement du premier point de la pseudo-ellipse
                    pt4.X -= listNode.ElementAt(i).ActualWidth / 2 - 55;
                    pt4.Y -= listNode.ElementAt(i).ActualHeight / 2 + 50;
                    pt4 = listNode.ElementAt(i).PointToScreen(pt4);
                    // placement du premier point de la pseudo-ellipse
                    pt5.X -= listNode.ElementAt(i).ActualWidth / 2 - 60;
                    pt5.Y -= listNode.ElementAt(i).ActualHeight / 2 + 45;
                    pt5 = listNode.ElementAt(i).PointToScreen(pt5);
                    // placement du premier point de la pseudo-ellipse
                    pt6.X -= listNode.ElementAt(i).ActualWidth / 2 - 60;
                    pt6.Y -= listNode.ElementAt(i).ActualHeight / 2 - 5;
                    pt6 = listNode.ElementAt(i).PointToScreen(pt6);

                    // création de la PointCollection qui génerera la forme
                    PointCollection polyCollection = new PointCollection();
                    polyCollection.Add(pt1);
                    polyCollection.Add(pt2);
                    polyCollection.Add(pt3);
                    polyCollection.Add(pt4);
                    polyCollection.Add(pt5);
                    polyCollection.Add(pt6);
                    polyCollection.Add(pt1);

                    // on ajoute les points au poly
                    poly.Points = polyCollection;
                    BitmapImage img1 = new BitmapImage(new Uri(".\\Resources\\Icons\\icon_chain.png", UriKind.Relative));
                    poly.Fill = new ImageBrush(img1);
                    //poly.Fill = new SolidColorBrush(Colors.Green);
                    poly.Stroke = new SolidColorBrush(Colors.White);
                    poly.StrokeThickness = 2;

                    poly.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnGreenCirclePreviewTouchDown);

                    // on dessine le poly
                    KeyValuePair<Polygon, ScatterCustom> myPair = new KeyValuePair<Polygon, ScatterCustom>(poly, listNode.ElementAt(i));
                    listRattache.Add(myPair);
                    this.LinkParentGrid.Children.Add(poly);



                    Polygon poly2 = new Polygon();

                    pt1 = listNode.ElementAt(i).PointFromScreen(listNode.ElementAt(i).ActualCenter);
                    pt2 = pt1;
                    pt3 = pt1;
                    pt4 = pt1;
                    pt5 = pt1;
                    pt6 = pt1;
                    // placement du premier point de la pseudo-ellipse
                    pt1.X -= listNode.ElementAt(i).ActualWidth / 2 - 60;
                    pt1.Y -= listNode.ElementAt(i).ActualHeight / 2 - 5;
                    pt1 = listNode.ElementAt(i).PointToScreen(pt1);
                    // placement du premier point de la pseudo-ellipse
                    pt2.X -= listNode.ElementAt(i).ActualWidth / 2 - 60;
                    pt2.Y -= listNode.ElementAt(i).ActualHeight / 2 + 45;
                    pt2 = listNode.ElementAt(i).PointToScreen(pt2);
                    // placement du premier point de la pseudo-ellipse
                    pt3.X -= listNode.ElementAt(i).ActualWidth / 2 - 65;
                    pt3.Y -= listNode.ElementAt(i).ActualHeight / 2 + 50;
                    pt3 = listNode.ElementAt(i).PointToScreen(pt3);
                    // placement du premier point de la pseudo-ellipse
                    pt4.X -= listNode.ElementAt(i).ActualWidth / 2 - 115;
                    pt4.Y -= listNode.ElementAt(i).ActualHeight / 2 + 50;
                    pt4 = listNode.ElementAt(i).PointToScreen(pt4);
                    // placement du premier point de la pseudo-ellipse
                    pt5.X -= listNode.ElementAt(i).ActualWidth / 2 - 120;
                    pt5.Y -= listNode.ElementAt(i).ActualHeight / 2 + 45;
                    pt5 = listNode.ElementAt(i).PointToScreen(pt5);
                    // placement du premier point de la pseudo-ellipse
                    pt6.X -= listNode.ElementAt(i).ActualWidth / 2 - 120;
                    pt6.Y -= listNode.ElementAt(i).ActualHeight / 2 - 5;
                    pt6 = listNode.ElementAt(i).PointToScreen(pt6);

                    // création de la PointCollection qui génerera la forme
                    PointCollection polyCollection2 = new PointCollection();
                    polyCollection2.Add(pt1);
                    polyCollection2.Add(pt2);
                    polyCollection2.Add(pt3);
                    polyCollection2.Add(pt4);
                    polyCollection2.Add(pt5);
                    polyCollection2.Add(pt6);
                    polyCollection2.Add(pt1);

                    // on ajoute les points au poly
                    poly2.Points = polyCollection2;
                    poly2.Fill = new SolidColorBrush(Colors.LightBlue);
                    //poly.Fill = new SolidColorBrush(Colors.Green);
                    poly2.Stroke = new SolidColorBrush(Colors.AliceBlue);
                    poly2.StrokeThickness = 2;

                    poly2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnBlueCirclePreviewTouchDown);

                    // on dessine le poly
                    KeyValuePair<Polygon, ScatterCustom> myPair2 = new KeyValuePair<Polygon, ScatterCustom>(poly2, listNode.ElementAt(i));
                    listRattache.Add(myPair2);
                    this.LinkParentGrid.Children.Add(poly2);


                    // dessin de la ligne servant d'icone a l'onglet bleu
                    pt1 = listNode.ElementAt(i).PointFromScreen(listNode.ElementAt(i).ActualCenter);
                    pt2 = pt1;
                    // placement du premier point de la pseudo-ellipse
                    pt1.X -= listNode.ElementAt(i).ActualWidth / 2 - 70;
                    pt1.Y -= listNode.ElementAt(i).ActualHeight / 2 + 10;
                    pt1 = listNode.ElementAt(i).PointToScreen(pt1);
                    // placement du premier point de la pseudo-ellipse
                    pt2.X -= listNode.ElementAt(i).ActualWidth / 2 - 110;
                    pt2.Y -= listNode.ElementAt(i).ActualHeight / 2 + 10;
                    pt2 = listNode.ElementAt(i).PointToScreen(pt2);

                    Line ligne = new Line();
                    ligne.Stroke = new SolidColorBrush(Colors.AliceBlue);
                    ligne.StrokeThickness = 5;
                    ligne.X1 = pt1.X;
                    ligne.X2 = pt2.X;
                    ligne.Y1 = pt1.Y;
                    ligne.Y2 = pt2.Y;

                    this.LinkParentGrid.Children.Add(ligne);

                }

            }





            //
            //  DESSIN DES LIGNES DE RATTACHE
            //
            this.LinkParentLineGrid.Children.RemoveRange(0, this.LinkParentLineGrid.Children.Count);
            this.NearParentEllipseCanvas.Children.RemoveRange(0, this.NearParentEllipseCanvas.Children.Count);
            for (int i = 0; i < listLigneRattache.Count; i++)
            {
                listLigneRattache.ElementAt(i).Value.Value.X1 = listLigneRattache.ElementAt(i).Key.GetOrigin().X;
                listLigneRattache.ElementAt(i).Value.Value.Y1 = listLigneRattache.ElementAt(i).Key.GetOrigin().Y;

                this.LinkParentLineGrid.Children.Add(listLigneRattache.ElementAt(i).Value.Value);


                double length = -1;
                ScatterCustom nearestText = null;
                for (int j = 0; j < listNode.Count; j++) // on teste la distance de chaque node
                {
                    if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j)) // on test si la node est differente de celle a laquelle la ligne est rattachée 
                    {
                        if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j).GetParent()) // on test si la node n'a pas pour parent celle a laquelle la ligne est rattachée
                        {
                            double diffXCarre = (listNode.ElementAt(j).ActualCenter.X - listLigneRattache.ElementAt(i).Value.Value.X2);
                            diffXCarre *= diffXCarre;
                            double diffYCarre = (listNode.ElementAt(j).ActualCenter.Y - listLigneRattache.ElementAt(i).Value.Value.Y2);
                            diffYCarre *= diffYCarre;
                            double testLenght = Math.Sqrt(diffXCarre + diffYCarre);
                            if (length == -1 || length > testLenght) // si la node est plus proche que la précédente on la retient
                            {
                                length = testLenght;
                                nearestText = listNode.ElementAt(j);
                            }
                        }
                    }
                }

                if (length != -1) // si la node la plus proche est assez près
                {
                    double dimension = (nearestText.Width > nearestText.Height) ? nearestText.Width : nearestText.Height;
                    if (length < dimension / 3 * 2)
                    {
                        Ellipse ell = new Ellipse();

                        ell.Width = dimension * 3 / 2 - dimension / 6;
                        ell.Height = dimension * 3 / 2 - dimension / 6;
                        ell.Fill = new SolidColorBrush(Colors.Transparent);
                        ell.Stroke = new SolidColorBrush(Colors.DarkGreen);
                        ell.StrokeThickness = 9;

                        DoubleCollection dColl = new DoubleCollection();
                        dColl.Add(10);
                        dColl.Add(5);
                        ell.StrokeDashArray = dColl;

                        this.NearParentEllipseCanvas.Children.Add(ell);
                        Canvas.SetLeft(ell, nearestText.ActualCenter.X - dimension / 3 * 2);
                        Canvas.SetTop(ell, nearestText.ActualCenter.Y - dimension / 3 * 2);
                    }
                }

            }


            


        }

        
        //
        //  FIN REFRESH



        /// <summary>
        /// Evenement Touch sur un Cercle Vert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGreenCirclePreviewTouchDown(object sender, TouchEventArgs e)
        {

            if (!e.TouchDevice.GetIsFingerRecognized())
                return;

            ScatterCustom node = null;
            for (int i = 0; i < listRattache.Count; i++)
            {
                if (listRattache.ElementAt(i).Key == (Polygon)sender)
                {
                    node = listRattache.ElementAt(i).Value;
                    node.SetParent(null);
                }
            }

            if (node != null)
            {
                
                bool isSet = false;
                for (int i = 0; i < listLigneRattache.Count && !isSet; i++)
                {
                    if (listLigneRattache.ElementAt(i).Key == node)
                    {
                        isSet = true;
                    }
                }

                if (!isSet)
                {
                    Line ligne = new Line();
                    ligne.X1 = node.GetOrigin().X;
                    ligne.Y1 = node.GetOrigin().Y;
                    ligne.X2 = e.TouchDevice.GetPosition(this).X;
                    ligne.Y2 = e.TouchDevice.GetPosition(this).Y;

                    ligne.Stroke = new SolidColorBrush(Colors.DarkGreen);
                    ligne.StrokeThickness = 6;

                    KeyValuePair<int, Line> myFirstPair = new KeyValuePair<int, Line>(e.TouchDevice.Id, ligne);
                    KeyValuePair<ScatterCustom, KeyValuePair<int, Line>> myPair = new KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>(node, myFirstPair);

                    listLigneRattache.Add(myPair);
                }

            }

        }

        void OnBlueCirclePreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (!e.TouchDevice.GetIsFingerRecognized())
                return;

            ScatterCustom node = null;
            for (int i = 0; i < listRattache.Count; i++)
            {
                if (listRattache.ElementAt(i).Key == (Polygon)sender)
                {
                    node = listRattache.ElementAt(i).Value;
                    node.AnimateClosing();
                }
            }
        }


        //
        // MENU SELECTION TYPE NODE
        //
        /// <summary>
        /// Appelé lorsqu'un "Touch" sur un cercle vert est détecté
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="choice"></param>
        public void MenuIsClicked(MenuCreation menu, String choice)
        {
            AddNode(null, menu.ActualCenter, choice);

            try
            {
                MainScatterView.Items.Remove(menu);
            }
            catch { }
        }

        





        //
        //  MODIFICATION du FICHIER
        //
        public void Modification(bool modif)
        {
            isModified = modif;
        }

        ///////////////////
            //FIN DES FONCTIONS !!!
    }
}