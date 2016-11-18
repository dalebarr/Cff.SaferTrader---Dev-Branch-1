using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class NoteType
    {
        private readonly int id;
        private readonly string name;
        private static readonly NoteType allNoteType = new NoteType(0, "All");
        private static readonly NoteType standardNoteType = new NoteType(1, "Standard");
        private static readonly NoteType disputesNoteType = new NoteType(2, "Disputes");
        private static readonly NoteType clientActionNoteType = new NoteType(3, "Client action required");
        private static readonly NoteType oldNotesNoteType = new NoteType(4, "Old note");
        private static readonly NoteType newPermanentNoteNoteType = new NoteType(5, "New permanent note");

        private static readonly Dictionary<int, NoteType> noteTypes = InitialiseNoteTypes();

        private NoteType(int id, string name)
        {
            ArgumentChecker.ThrowIfLessThanZero(id, "id");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            this.id = id;
            this.name = name;
        }

        private static Dictionary<int, NoteType> InitialiseNoteTypes()
        {
            Dictionary<int, NoteType> knownNoteTypes = new Dictionary<int, NoteType>
                                                      {
                                                          {allNoteType.Id, allNoteType},
                                                          {standardNoteType.Id, standardNoteType},
                                                          {disputesNoteType.Id, disputesNoteType},
                                                          {clientActionNoteType.Id, clientActionNoteType},
                                                          {oldNotesNoteType.Id, oldNotesNoteType}
                                                      };
            return knownNoteTypes;
        }

        public static NoteType Parse(int selectedNoteTypeId)
        {
            NoteType noteType = allNoteType;
            if(noteTypes.ContainsKey(selectedNoteTypeId))
            {
                noteType = noteTypes[selectedNoteTypeId];
            }
            return noteType;
        }

        public static NoteType Parse(string selectedNoteType)
        {
            NoteType noteType = allNoteType;
            foreach (KeyValuePair<int, NoteType> knownNoteType in KnownTypes)
            {
                if(selectedNoteType.CompareTo(knownNoteType.Value.Name)==0)
                {
                    noteType = knownNoteType.Value;
                }
            }
            return noteType;
        }

        public static Dictionary<int, NoteType> KnownTypes
        {
            get { return noteTypes; }
        }

        public static List<ListItem> KnownTypesForFilteringNotes
        {
            get
            {
                List<ListItem> noteTypeListItems = new List<ListItem>();
                foreach (KeyValuePair<int, NoteType> type in noteTypes)
                {
                    noteTypeListItems.Add(new ListItem(type.Value.Name, type.Key.ToString()));
                }
                return noteTypeListItems;
            }
        }

        public static Dictionary<int, string> KnownTypesForNewNotes
        {
            get
            {
                return new Dictionary<int, string>
                    {
                        {standardNoteType.Id, standardNoteType.Name},
                        {disputesNoteType.Id, disputesNoteType.Name},
                        {clientActionNoteType.Id, clientActionNoteType.Name}
                    };
            }
        }

        public string Name
        {
            get { return name; }
        }

        public int Id
        {
            get { return id; }
        }

        public static NoteType OldNote
        {
            get { return oldNotesNoteType; }
        }

        public static NoteType Standard
        {
            get { return standardNoteType; }
        }

        public static NoteType Disputes
        {
            get { return disputesNoteType; }
        }

        public static NoteType ClientActionRequired
        {
            get { return clientActionNoteType; }
        }

        public static NoteType AllNotes
        {
            get { return allNoteType; }
        }

        public static NoteType NewPermanentNote
        {
            get { return newPermanentNoteNoteType; }
        }
    }
}