﻿using System;
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
using System.Windows.Shapes;

using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace SurfaceMindMapping
{
    /// <summary>
    /// Logique d'interaction pour LoadCircle.xaml
    /// </summary>
    public partial class LoadCircle : ScatterViewItem
    {
        //Id du cercle
        public int Id;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoadCircle()
        {
            InitializeComponent();

            this.ShowsActivationEffects = false;
            this.BorderBrush = System.Windows.Media.Brushes.Transparent;

            CanScale = false;
            CanMove = false;
        }

        /// <summary>
        /// Lorsque le chargement est terminé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScatterViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
            ssc = this.Template.FindName("shadow", this) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
            ssc.Visibility = Visibility.Hidden;
        }

        //private void resetAnim()
        //{
        //    this.mLoadCircle.Opacity = 0;

        //}
    }
}
