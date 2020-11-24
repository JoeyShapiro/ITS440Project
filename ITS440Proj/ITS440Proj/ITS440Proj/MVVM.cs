using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using System.Threading.Tasks;
using System.Linq;

namespace ITS440Proj
{
    public class MVVM
    {
        public class Item
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
        class ItemViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            Item item;

            public ItemViewModel()
            {
                item = new Item();
            }
            public string Title
            {
                get
                {
                    return item.Title;
                }
                set
                {
                    if (!value.Equals(item.Title, StringComparison.Ordinal))
                    {
                        item.Title = value;
                        OnPropertyChanged("Title");
                    }
                }
            }

            void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        public class ObservableItem : INotifyPropertyChanged
        {
            [PrimaryKey, AutoIncrement]
            public int ID { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            Item item;

            // custom added
            public int Quantity { get; set; }
            public bool Got { get; set; }

            public ObservableItem()
            {
                item = new Item();
            }
            public string Title
            {
                set
                {
                    if (!value.Equals(item.Title, StringComparison.Ordinal))
                    {
                        item.Title = value;
                        OnPropertyChanged("Title");
                    }
                }
                get
                {
                    return item.Title;
                }
            }
            public string Description
            {
                set
                {
                    if (!value.Equals(item.Description, StringComparison.Ordinal))
                    {
                        item.Description = value;
                        OnPropertyChanged("Description");
                    }
                }
                get
                {
                    return item.Description;
                }
            }
            void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        public class ListViewModel
        {
            ObservableCollection<ObservableItem> items;
            public ListViewModel()
            {
                Task.Run(async () =>
                {
                    var table = await App.Database.GetItemsAsync();
                    items = new ObservableCollection<ObservableItem>(table);
                }).Wait();
            }

            public ObservableCollection<ObservableItem> Items
            {
                set
                {
                    if (value != items)
                    {
                        items = value;
                    }
                }
                get
                {
                    return items;
                }
            }

            public MyEnumerator GetEnumerator()
            {
                return new MyEnumerator(this);
            }

            public class MyEnumerator
            {
                int nIndex;
                ListViewModel collection;
                public MyEnumerator(ListViewModel coll)
                {
                    collection = coll;
                    nIndex = -1;
                }

                public bool MoveNext()
                {
                    nIndex++;
                    return (nIndex < collection.items.Count); // should be .Length but well see
                }

                public ObservableItem Current => collection.items.Where(X => X.ID == nIndex).FirstOrDefault();
            }
        }
    }
}
