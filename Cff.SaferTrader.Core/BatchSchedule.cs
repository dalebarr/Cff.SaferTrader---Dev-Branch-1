using System;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class BatchSchedule
    {
        private readonly string createdBy;
        private readonly IDate date;
        private readonly string header;
        private readonly string modifiedBy;
        private readonly IDate modifiedDate;
        private readonly int number;
        private readonly IDate released;
        private readonly string statusDescription;
        private readonly IDate dateFinished;
        private readonly int status;
        private readonly string note;
        private readonly BatchScheduleFinanceInfo scheduleFinanceInfo;
        private readonly ClientAttribute clientAttribute;

        public BatchSchedule(int number, string statusDescription, short status, IDate date, IDate released, IDate modifiedDate,
            IDate dateFinished, string header, string note, string createdBy, string modifiedBy,
            BatchScheduleFinanceInfo scheduleFinanceInfo, ClientAttribute clientAttribute)
        {
            this.number = number;
            this.clientAttribute = clientAttribute;
            this.scheduleFinanceInfo = scheduleFinanceInfo;
            this.note = note;
            this.status = status;
            this.dateFinished = dateFinished;
            this.date = date;
            this.released = released;
            this.statusDescription = statusDescription;
            this.createdBy = createdBy;
            this.modifiedDate = modifiedDate;
            this.modifiedBy = modifiedBy;
            this.header = header;
        }

        public void Display(IScheduleTabView view)
        {
            scheduleFinanceInfo.Display(view);
            
            if (IsBatchInProcessing)
            {
                view.ShowScheduleIsInProcessing(this);
            }
            else
            {
                view.DisplaySchedule(this);
                if (string.IsNullOrEmpty(note))
                {
                    view.HideNote();
                }
                else
                {
                    view.ShowNote();
                }
            }
        }

        public ClientAttribute ClientAttribute
        {
            get { return clientAttribute; }
        }

        public BatchScheduleFinanceInfo ScheduleFinanceInfo
        {
            get { return scheduleFinanceInfo; }
        }

        public string Note
        {
            get { return note; }
        }

        public bool IsBatchInProcessing
        {
            get
            {
                if (Status == 10)
                {
                    return true;
                }
                return false;
            }
        }

        public int Status
        {
            get { return status; }
        }

        public IDate DateFinished
        {
            get { return dateFinished; }
        }

        public string Header
        {
            get { return header; }
        }

        public int Number
        {
            get { return number; }
        }

        public IDate Date
        {
            get { return date; }
        }

        public IDate Released
        {
            get { return released; }
        }

        public string StatusDescription
        {
            get { return statusDescription; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
        }

        public IDate ModifiedDate
        {
            get { return modifiedDate; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }
    }
}