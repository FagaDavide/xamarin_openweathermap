using System.ComponentModel;
using Xamarin.Essentials;
using Nito.Mvvm.CalculatedProperties; // one true <3 to  StephenCleary@github 

namespace Xweather.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly PropertyHelper Property;

        public bool IsNotConnected { get; set; }
        public BaseViewModel()
        {
            Property = new PropertyHelper(RaisePropertyChanged);
            Connectivity.ConnectivityChanged += ConnectivityConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        void ConnectivityConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
            if (IsNotConnected)
                App.Current.MainPage.DisplayAlert("-=[ Info ]=-", "Connectivity lost, Xweather need Internet to work", "ok");
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
