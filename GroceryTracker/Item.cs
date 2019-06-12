using System;
using System.ComponentModel;

namespace GroceryTracker
{
    public class Item
    {
        public int id { get; set; }
        public string itemName { get; set; }
        public int itemQuantity { get; set; }
        public DateTime itemExpDate { get; set; }
    }
}