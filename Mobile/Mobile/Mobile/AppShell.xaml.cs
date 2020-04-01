﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Main", typeof(Views.Main));
        }
    }
}
