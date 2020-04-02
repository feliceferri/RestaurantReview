using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBar : ContentView
    {

        public static BindableProperty RatingProperty = BindableProperty.Create("Rating", typeof(int), typeof(RatingBar), 0, propertyChanged: OnRatingChanged);
        public static BindableProperty IsReadOnlyProperty = BindableProperty.Create("IsReadOnly", typeof(bool), typeof(RatingBar), defaultValue: false, propertyChanged: OnIsReadOnlyChanged);

        public event EventHandler<int> RatingChanged;
        public event EventHandler Tapped;

        public RatingBar()
        {

            InitializeComponent();

            // Test 1 
            // comenta esta variable y veras que no se llama 

        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set
            {
                SetValue(IsReadOnlyProperty, value);
                OnPropertyChanged("IsReadOnly");
            }
        }

        public decimal? Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set
            {
                SetValue(RatingProperty, value);

                OnPropertyChanged("Rating");
                try
                {


                    ////Cleaning value to default
                    stargray1.IsVisible = true;
                    stargray2.IsVisible = true;
                    stargray3.IsVisible = true;
                    stargray4.IsVisible = true;
                    stargray5.IsVisible = true;
                    OrangeStar1.IsVisible = false;
                    OrangeStar2.IsVisible = false;
                    OrangeStar3.IsVisible = false;
                    OrangeStar4.IsVisible = false;
                    OrangeStar5.IsVisible = false;
                    //////////////////////

                    if (value.HasValue)
                    {
                        if (value >= 1)
                        {
                            stargray1.IsVisible = false;
                            OrangeStar1.IsVisible = true;

                        }
                        if (value >= 2)
                        {
                            stargray2.IsVisible = false;
                            OrangeStar2.IsVisible = true;
                        }
                        if (value >= 3)
                        {
                            stargray3.IsVisible = false;
                            OrangeStar3.IsVisible = true;
                        }
                        if (value >= 4)
                        {
                            stargray4.IsVisible = false;
                            OrangeStar4.IsVisible = true;
                        }
                        if (value >= 5)
                        {
                            stargray5.IsVisible = false;
                            OrangeStar5.IsVisible = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    var msg = ex.Message.ToString();
                }
            }
        }


        // Events of yellow star
        private void OnOrangeStarTapped(object sender, EventArgs e)
        {

            Image lblClicked = (Image)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            var id = Convert.ToInt32(item.CommandParameter);

            // Evaluate value (Parameter rating)
            switch (id)
            {
                case 1:

                    if (OrangeStar1.IsVisible)
                    {
                        this.Rating = 1;
                    }
                    break;
                case 2:

                    if (OrangeStar2.IsVisible)
                    {
                        this.Rating = 2;
                    }
                    break;
                case 3:

                    if (OrangeStar2.IsVisible)
                    {
                        this.Rating = 3;
                    }
                    break;
                case 4:
                    if (OrangeStar2.IsVisible)
                    {
                        this.Rating = 4;
                    }
                    break;
                case 5:
                    if (OrangeStar2.IsVisible)
                    { this.Rating = 5; }
                        
                    break;

                default:
                    this.Rating = 0;
                    break;

            }

        }
        // Events to gray star 
        private void OnGreyTapped(object sender, EventArgs e)
        {

            Image lblClicked = (Image)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            var id = Convert.ToInt32(item.CommandParameter);

            // Evaluate value (Parameter rating)
            switch (id)
            {
                case 1:
                    this.Rating = 1;
                    break;
                case 2:
                    this.Rating = 2;
                    break;
                case 3:
                    this.Rating = 3;
                    break;
                case 4:
                    this.Rating = 4;
                    break;
                case 5:
                    this.Rating = 5;
                    break;

                default:
                    break;

            }
        }




        private void Ontest(object sender, EventArgs e)
        {
            var s = "";
        }

        // General 
        private void OnGrayStarTapped(object sender, EventArgs e)
        {

            this.Rating = 0;

        }
        static void OnIsReadOnlyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            var isReadOnly = (bool)newValue;

            control.OrangeStar1.IsEnabled = !isReadOnly;
            control.OrangeStar2.IsEnabled = !isReadOnly;
            control.OrangeStar3.IsEnabled = !isReadOnly;
            control.OrangeStar4.IsEnabled = !isReadOnly;
            control.OrangeStar5.IsEnabled = !isReadOnly;
            control.stargray1.IsEnabled = !isReadOnly;
            control.stargray2.IsEnabled = !isReadOnly;
            control.stargray3.IsEnabled = !isReadOnly;
            control.stargray4.IsEnabled = !isReadOnly;
            control.stargray5.IsEnabled = !isReadOnly;
        }
        static void OnRatingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Property changed implementation goes here

            var control = (RatingBar)bindable;
            //if (newValue == null) ChangeRating(control, 0);

            int newRating = Convert.ToInt32(newValue);
            control.Rating = newRating; //ChangeRating(control, newRating);

        }

        //public void OnRatingChanged(int rating)
        //{

        //    if (RatingChanged != null)
        //        RatingChanged.Invoke(this, rating);
        //}

    }
}

