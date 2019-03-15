using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace DataAccessLayers.Common
{
    [Serializable]
    public class SmartCollectionBase : CollectionBase
    {
        public string lastSortProperty = "";
        public const string nullGuid = "{0EAD0442-D81E-421e-A912-D2176B2B8B5D}";
        public const string lowercaseTag = "_[Lower]_";
        public ArrayList removed;
        public Hashtable propertyLookupTables;

        public SmartCollectionBase()
        {
        }

        public SmartCollectionBase(int capacity)
          : base(capacity)
        {
        }

        public ArrayList Removed
        {
            get
            {
                if (this.removed == null)
                    this.removed = new ArrayList();
                return this.removed;
            }
        }

        public bool HasRemovedItems
        {
            get
            {
                return this.removed != null && this.removed.Count > 0;
            }
        }

        protected Hashtable PropertyLookupTables
        {
            get
            {
                if (this.propertyLookupTables == null)
                    this.propertyLookupTables = new Hashtable(4);
                return this.propertyLookupTables;
            }
        }

        protected internal virtual void Add(object value)
        {
            this.List.Add(value);
            this.AddObject(value);
            this.lastSortProperty = "";
        }

        protected internal virtual void Remove(object value)
        {
            this.List.Remove(value);
            if (this.propertyLookupTables != null)
            {
                foreach (string key in (IEnumerable)this.propertyLookupTables.Keys)
                {
                    object propertyValue = this.GetPropertyValue(key, value);
                    Hashtable propertyLookupTable = (Hashtable)this.propertyLookupTables[(object)key];
                    ArrayList arrayList = (ArrayList)propertyLookupTable[propertyValue];
                    arrayList.Remove(value);
                    if (arrayList.Count == 0)
                        propertyLookupTable.Remove(propertyValue);
                }
            }
            this.Removed.Add(value);
            this.lastSortProperty = "";
        }

        protected internal virtual void Insert(int index, object value)
        {
            this.List.Insert(index, value);
            this.AddObject(value);
        }

        public new virtual void Clear()
        {
            foreach (object obj in (IEnumerable)this.List)
                this.Removed.Add(obj);
            if (this.propertyLookupTables != null)
            {
                foreach (string key in (IEnumerable)this.propertyLookupTables.Keys)
                    ((Hashtable)this.propertyLookupTables[(object)key]).Clear();
            }
            base.Clear();
        }

        public void ClearLookupTables()
        {
            if (this.propertyLookupTables == null)
                return;
            this.propertyLookupTables.Clear();
            this.propertyLookupTables = (Hashtable)null;
        }

        public void ClearLookupTable(string lookupPropertyToClear)
        {
            if (this.propertyLookupTables == null || !this.propertyLookupTables.ContainsKey((object)lookupPropertyToClear))
                return;
            this.propertyLookupTables.Remove((object)lookupPropertyToClear);
        }

        public virtual object FindByProperty(string property, object propertyValue)
        {
            return this.FindByProperty(property, propertyValue, false);
        }

        public virtual object FindByProperty(string property, object propertyValue, bool ignoreCase)
        {
            return this.GetAllByProperty(property, propertyValue, ignoreCase)?[0];
        }

        public virtual ArrayList GetAllByProperty(string property, object propertyValue)
        {
            return this.GetAllByProperty(property, propertyValue, false);
        }

        public virtual ArrayList GetAllByProperty(string property, object propertyValue, bool ignoreCase)
        {
            string str = ignoreCase ? property + "_[Lower]_" : property;
            object index = propertyValue;
            if (ignoreCase && propertyValue is string)
                index = (object)((string)propertyValue).ToLower();
            Hashtable lookupTable = (Hashtable)this.PropertyLookupTables[(object)str];
            if (lookupTable == null)
            {
                lookupTable = new Hashtable(this.Count);
                foreach (object obj in (IEnumerable)this.List)
                    this.AddObjectToLookup(lookupTable, property, obj, ignoreCase);
                this.propertyLookupTables[(object)str] = (object)lookupTable;
            }
            if (index == null)
                index = (object)new Guid("{0EAD0442-D81E-421e-A912-D2176B2B8B5D}");
            return (ArrayList)lookupTable[index] ?? (ArrayList)null;
        }

        public Hashtable GetLookupTableByPropertyString(string property)
        {
            if (this.propertyLookupTables != null && this.propertyLookupTables.ContainsKey((object)property))
                return (Hashtable)this.propertyLookupTables[(object)property];
            return (Hashtable)null;
        }

        public void PopulateLookupTableByPropertyString(string property)
        {
            this.FindByProperty(property, (object)null);
        }

        private void AddObject(object value)
        {
            if (this.propertyLookupTables != null)
            {
                foreach (string key in (IEnumerable)this.propertyLookupTables.Keys)
                {
                    string propertyName = key.Replace("_[Lower]_", "");
                    this.AddObjectToLookup((Hashtable)this.propertyLookupTables[(object)key], propertyName, value);
                }
            }
            if (this.removed == null || !this.Removed.Contains(value))
                return;
            this.Removed.Remove(value);
        }

        private void AddObjectToLookup(Hashtable lookupTable, string propertyName, object value)
        {
            this.AddObjectToLookup(lookupTable, propertyName, value, false);
        }

        private void AddObjectToLookup(Hashtable lookupTable, string propertyName, object value, bool makeLowercase)
        {
            object index = this.GetPropertyValue(propertyName, value);
            if (index == null)
                index = (object)new Guid("{0EAD0442-D81E-421e-A912-D2176B2B8B5D}");
            else if (makeLowercase && index is string)
                index = (object)((string)index).ToLower();
            ArrayList arrayList = (ArrayList)lookupTable[index];
            if (arrayList == null)
                lookupTable[index] = (object)new ArrayList(1)
        {
          value
        };
            else if (arrayList.IndexOf(value) < 0)
                arrayList.Add(value);
        }

        private object GetPropertyValue(string property, object value)
        {
            object obj = value;
            string[] strArray = property.Split(".".ToCharArray());
            if (strArray.Length == 0)
                return (object)null;
            for (int index = 0; index < strArray.Length; ++index)
            {
                PropertyInfo property1 = obj.GetType().GetProperty(strArray[index]);
                if (property1 == (PropertyInfo)null)
                    throw new ApplicationException("The property requested is not supported the object.");
                obj = property1.GetGetMethod().Invoke(obj, BindingFlags.GetProperty, (Binder)null, new object[0], (CultureInfo)null);
                if (obj == null)
                    return (object)null;
            }
            return obj;
        }
    }
}