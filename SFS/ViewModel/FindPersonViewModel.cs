using GalaSoft.MvvmLight;
using SMFS.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using SMFS.Model;

namespace SMFS.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FindPersonViewModel : ViewModelBase
    {
        private bool _showHidden;
        private string _filter;
        private ICollectionView _people;
        private bool _busy;
        private Person _selected;

        public bool ShowHidden
        {
            get => _showHidden;
            set
            {
                if (Set(() => ShowHidden, ref _showHidden, value))
                {
                    UpdateFilter();
                }
            }
        }

        public string Filter
        {
            get => _filter;
            set {
                if (Set(() => Filter, ref _filter, value))
                {
                    UpdateFilter();
                } }
        }

        public ICollectionView People
        {
            get => _people;
            set { Set(()=>People, ref _people, value); }
        }

        public bool Busy
        {
            get => _busy;
            set { Set(()=>Busy, ref _busy, value); }
        }

        public Person Selected
        {
            get => _selected;
            set { Set(()=>Selected, ref _selected, value); }
        }

        public async Task LoadData(IDataService dataService)
        {
            Busy = true;
            People = CollectionViewSource.GetDefaultView(await dataService.LoadAll());
            People.Filter = DoFilter;
            Busy = false;
        }

        private bool DoFilter(object obj)
        {
            var person = (Person) obj;
            if (person == null) return false;
            return person.InFilter(ShowHidden, Filter);

        }

        private void UpdateFilter()
        {
            People.Refresh();
        }
    }
}