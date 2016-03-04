using SharkData.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace SharkData.ViewModels
{
    public class SharkViewModel
    {
        private ObservableCollection<Shark> _sharks = new ObservableCollection<Shark>();
        private DataRetriever _dataRetriever = new DataRetriever("http://sharkdata.azurewebsites.net/api");

        public ObservableCollection<Shark> Sharks { get { return _sharks; } }

        public SharkViewModel()
        {
            _sharks.CollectionChanged += _sharks_CollectionChanged;

            RefreshSharks();
        }

        private void _sharks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= Shark_PropertyChanged;
            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += Shark_PropertyChanged;
        }

        private async void Shark_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                await _dataRetriever.ModifyShark((Shark)sender);
        }

        public async void RefreshSharks()
        {
            _sharks.Clear();
            foreach (Shark shark in await _dataRetriever.GetShark())
                _sharks.Add(shark);
        }
    }
}
