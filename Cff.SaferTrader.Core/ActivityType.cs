using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ActivityType
    {
        private readonly int id;
        private readonly string name;
        private static readonly ActivityType allNotesActivityType = new ActivityType(0, "All");
        private static readonly ActivityType phoneActivityType = new ActivityType(1, "Phone");
        private static readonly ActivityType emailActivityType = new ActivityType(2, "Email");
        private static readonly ActivityType letterActivityType = new ActivityType(3, "Letter");
        private static readonly ActivityType faxActivityType = new ActivityType(4, "Fax");
        private static readonly ActivityType oldNoteActivityType = new ActivityType(5, "Old note");

        private static readonly Dictionary<int, ActivityType> activityTypes = InitialiseActivityTypes();

        private ActivityType(int id, string name)
        {
            ArgumentChecker.ThrowIfLessThanZero(id, "id");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            this.id = id;
            this.name = name;
        }

        private static Dictionary<int, ActivityType> InitialiseActivityTypes()
        {
            Dictionary<int, ActivityType> knownActivityTypes = new Dictionary<int, ActivityType>
                                                                    {
                                                                        {allNotesActivityType.Id, allNotesActivityType},
                                                                        {phoneActivityType.Id, phoneActivityType},
                                                                        {emailActivityType.Id, emailActivityType},
                                                                        {letterActivityType.Id, letterActivityType},
                                                                        {faxActivityType.Id, faxActivityType},
                                                                        {oldNoteActivityType.Id, oldNoteActivityType}
                                                                    };
            return knownActivityTypes;
        }

        public static ActivityType Parse(int selectedActivityTypeId)
        {
            ActivityType activityType = allNotesActivityType;     
            if (activityTypes.ContainsKey(selectedActivityTypeId))
            {
                activityType = activityTypes[selectedActivityTypeId];
            }
            return activityType;
        }

        public static ActivityType Parse(string selectedActivityType)
        {
            ActivityType activityType = allNotesActivityType;
            foreach (KeyValuePair<int, ActivityType> knownActivityType in KnownTypes)
            {
                if (selectedActivityType.CompareTo(knownActivityType.Value.Name) == 0)
                {
                    activityType = knownActivityType.Value;
                }
            }
            return activityType;
        }

        public static Dictionary<int, ActivityType> KnownTypes
        {
            get { return activityTypes; }
        }
        
        public static List<ListItem> KnownTypesForFilteringNotes
        {
            get
            {
                List<ListItem> activityTypeListItems = new List<ListItem>();
                foreach(KeyValuePair<int, ActivityType> type in activityTypes)
                {
                    if (type.Key != oldNoteActivityType.Id)
                    {
                        activityTypeListItems.Add(new ListItem(type.Value.Name, type.Key.ToString()));
                    }
                }
                return activityTypeListItems;
            }
        }

        public static Dictionary<int, string> KnownTypesForNewNotes
        {
            get
            {
                return new Dictionary<int, string>
                           {
                               {phoneActivityType.Id, phoneActivityType.Name},
                               {emailActivityType.Id, emailActivityType.Name},
                               {letterActivityType.Id, letterActivityType.Name},
                               {faxActivityType.Id, faxActivityType.Name}
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

        public static ActivityType AllNotes
        {
            get { return allNotesActivityType; }
        }

        public static ActivityType Phone
        {
            get { return phoneActivityType; }
        }

        public static ActivityType Email
        {
            get { return emailActivityType; }
        }

        public static ActivityType Letter
        {
            get { return letterActivityType; }
        }

        public static ActivityType Fax
        {
            get { return faxActivityType; }
        }

        public static ActivityType OldNote
        {
            get { return oldNoteActivityType; }
        }
    }
}