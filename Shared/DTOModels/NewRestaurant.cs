using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOModels
{
    public class NewRestaurant: Restaurant
    {
        public string ImageBase64 { get; set; }
        public string ImageFileExtensionIncludingDot { get; set; }
    }
}
