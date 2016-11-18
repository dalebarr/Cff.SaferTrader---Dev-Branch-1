using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Core
{
    public class BatchType
    {
        private static readonly BatchType AllBatchType = new BatchType(0, "All", true);
        private static readonly BatchType HeldBatchType = new BatchType(8, "Held", true);
        private static readonly BatchType ReleasedBatchType = new BatchType(9, "Released", true);
        private static readonly BatchType StartedBatchType = new BatchType(10, "Started", false);
        private static readonly Dictionary<int, BatchType> BatchTypes = InitialiseBatchTypes();
        private readonly int id;
        private readonly string name;
        private readonly bool isDateRangeDependant;

        private BatchType(int id, string name, bool isDateRangeDependant)
        {
            ArgumentChecker.ThrowIfLessThanZero(id, "id");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            this.id = id;
            this.name = name;
            this.isDateRangeDependant = isDateRangeDependant;
        }

        private static Dictionary<int, BatchType> InitialiseBatchTypes()
        {
            var knownBatchTypes = new Dictionary<int, BatchType>
                                      {
                                          {AllBatchType.Id, AllBatchType},
                                          {HeldBatchType.Id, HeldBatchType},
                                          {ReleasedBatchType.Id, ReleasedBatchType},
                                          {StartedBatchType.Id, StartedBatchType}
                                      };
            return knownBatchTypes;
        }

        public static BatchType Parse(int selectedBatchTypeId)
        {
            BatchType batchType;
            if (BatchTypes.ContainsKey(selectedBatchTypeId))
            {
                batchType = BatchTypes[selectedBatchTypeId];
            }
            else
            {
                throw new InvalidOperationException("Specified Batch Type ID does not exist");
            }
            return batchType;
        }

        public static Dictionary<int, BatchType> KnownTypes
        {
            get { return BatchTypes; }
        }

        public static List<ListItem> KnownTypesAsListItems
        {
            get
            {
                var batchTypeListItems = new List<ListItem>();
                foreach (var type in BatchTypes)
                {
                    batchTypeListItems.Add(new ListItem(type.Value.Name, type.Key.ToString()));
                }
                return batchTypeListItems;
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

        public bool IsDateRangeDependant
        {
            get { return isDateRangeDependant; }
        }
    }
}