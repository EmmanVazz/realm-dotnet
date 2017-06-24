using System;
using Realms;

namespace QuickJournal
{
    public class JournalEntry : RealmObject
    {
        public string Title { get; set; }

		private string _bodyText { get; set; }
        public string BodyText
		{
			get
			{
				return _bodyText;
			}
			set
			{
				_bodyText = value;
				OnPropertyChanged(nameof(BodyText));
                OnPropertyChanged(nameof(Random));
			}
		}

		public string Random
		{
			get
			{
				return _bodyText;
			}
		}

        public EntryMetadata Metadata { get; set; }

        // If we remove that and use Metadata.Date in the binding, exception is thrown when deleting item. See #883.
        public DateTimeOffset Date => Metadata.Date;
    }
}