using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Web.UserControls.GenericGridViewControls
{
    public class WebDataSelectionBase
    {
        public WebDataSelectionBase(WebDataProxy webData);

        protected int CountCore { get; }
        protected internal bool IsLockSelection { get; }
        protected bool IsStoreSelected { get; set; }
        protected Dictionary<object, bool> Selected { get; }
        protected bool SelectionChangedOnLock { get; }
        protected WebDataProxy WebData { get; }

        protected void AddRow(object keyValue);
        protected internal void BeginSelection();
        protected internal void CancelSelection();
        protected bool CanSelectRow(int visibleIndex);
        protected internal void EndSelection();
        protected virtual void FireSelectionChanged();
        protected object GetListSourceRowKeyValue(int listSourceRowIndex);
        protected object GetRowKeyValue(int visibleIndex);
        //protected WebRowType GetRowType(int visibleIndex);
        protected internal virtual List<object> GetSelectedValues(string[] fieldNames, int visibleStartIndex, int visibleRowCountOnPage);
        protected bool IsListSourceRowSelected(int listSourceRowIndex);
        protected bool IsRowSelectedByKeyCore(object keyValue);
        protected bool IsRowSelectedCore(int visibleIndex);
        //protected void LoadStateCore(TypedBinaryReader reader);
        protected virtual void OnSelectionChanged();
        protected void RemoveRow(object keyValue);
        protected internal void SaveState(TypedBinaryWriter writer);
        protected virtual void SelectAllCore();
        protected void SetSelectionCore(int visibleIndex, bool selected);
        protected virtual void SetSelectionCore(object keyValue, bool selected);
        protected void UnselectAllCore();
    }
}