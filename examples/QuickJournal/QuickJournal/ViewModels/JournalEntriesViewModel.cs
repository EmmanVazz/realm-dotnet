using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Realms;
using Xamarin.Forms;

namespace QuickJournal
{
    public class JournalEntriesViewModel : INotifyPropertyChanged
    {
        // TODO: add UI for changing that.
        private const string AuthorName = "Me";

        private Realm _realm;

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));  
		}

		public IList<JournalEntry> Entries { get; private set; }

		public JournalView RealmObject { get; set; }

        public ICommand AddEntryCommand { get; private set; }

        public ICommand DeleteEntryCommand { get; private set; }

        public INavigation Navigation { get; set; }

        public JournalEntriesViewModel()
        {
            _realm = Realm.GetInstance();
			Entries = new List<JournalEntry>();

			RealmObject = _realm.All<JournalView>().FirstOrDefault();
			if (RealmObject == null)
			{
				var transaction = _realm.BeginWrite();
				RealmObject = _realm.Add(new JournalView());
				transaction.Commit();
			}

			RealmObject.PropertyChanged += RealmObject_PropertyChanged;
			ProcessChildren();
            AddEntryCommand = new Command(AddEntry);
            DeleteEntryCommand = new Command<JournalEntry>(DeleteEntry);
        }

		private async void AddEntry()
		{
			
			await _realm.WriteAsync((realm) =>
			{
				var entry = realm.Add(new JournalEntry
				{
					Metadata = new EntryMetadata
					{
						Date = DateTimeOffset.Now,
						Author = AuthorName
					}
				});

				realm.All<JournalView>().First().AddEntry(entry);
			});

			var transaction = _realm.BeginWrite();
			var page = new JournalEntryDetailsPage(new JournalEntryDetailsViewModel(_realm.All<JournalEntry>().Last(), transaction));

			await Navigation.PushAsync(page);
		}

		internal void EditEntry(JournalEntry entry)
		{
			var transaction = _realm.BeginWrite();

			var page = new JournalEntryDetailsPage(new JournalEntryDetailsViewModel(entry, transaction));

			Navigation.PushAsync(page);
		}

		void RealmObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("Property Changed {0}", e.PropertyName));
			if (e.PropertyName == nameof(JournalView.Children))
				ProcessChildren();
		}

		public void ProcessChildren()
		{
			Entries.Clear();
			foreach (var entry in RealmObject.Children)
			{
				Entries.Add(entry);
			}
			OnPropertyChanged(nameof(Entries));
		}

		private void DeleteEntry(JournalEntry entry)
        {
            _realm.Write(() => _realm.Remove(entry));
        }
    }
}

