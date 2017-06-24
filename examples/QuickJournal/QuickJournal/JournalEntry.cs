using System;
using System.Collections.Generic;
using Realms;

namespace QuickJournal
{
    public class JournalEntry : RealmObject
    {
        public string Title { get; set; }
        public string BodyText { get; set; }

        public EntryMetadata Metadata { get; set; }

        // If we remove that and use Metadata.Date in the binding, exception is thrown when deleting item. See #883.
        public DateTimeOffset Date => Metadata.Date;
    }

	public class JournalView : RealmObject
	{
		public IList<JournalEntry> Entries { get; }
		public IList<JournalEntry> Children
		{
			get
			{
				return Entries;
			}
		}

		public void AddEntry(JournalEntry entry)
		{
			Entries.Add(entry);
			RaisePropertyChanged(nameof(Children));
		}
	}
}