using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GroceryTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> items;
        ObservableCollection<ItemToBuy> shopping;
        string search = "";
        private bool storeData;
        bool del_event = false;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            items = GenerateItemsData(50);
            shopping = GenerateShoppingListData(50);

            if (items != null )
            {
                Lbx_items.ItemsSource = items;
            }
            else
            {
                items = new ObservableCollection<Item>();
            }
            if (shopping == null)
            {
                shopping = new ObservableCollection<ItemToBuy>();
            }
            Lbx_Shopping.DataContext = shopping;

        }

        private ObservableCollection<ItemToBuy> GenerateShoppingListData(int count)
        {
            var shopInput = Storage.ReadXml<ObservableCollection<ItemToBuy>>("ShoppingListData.xml");
            return shopInput;
        }

        private ObservableCollection<Item> GenerateItemsData(int cnt)
        {
            var lstInput = Storage.ReadXml<ObservableCollection<Item>>("InputData1.xml");
            return lstInput;            
        }

        private void Tbx_Search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            search = Tbx_Search.Text.ToLower();
            if (search == "")
            {
                Lbx_items.ItemsSource = items;
            }
            else
            {
                var results = from s in items where s.itemName.ToLower().Contains(search) select s;
                Lbx_items.ItemsSource = results;
            }
         }

        private void Btn_Add_Item_Click(object sender, RoutedEventArgs e)
        {
            var results = new Item { itemName = "Please edit", itemQuantity = 0, itemExpDate = DateTime.Today.Date};
            items.Add(results);
            Lbx_items.SelectedItem = results;
            Lbx_items.ScrollIntoView(results);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Tbx_Name.Text != "" && Tbx_Quan.Text != "")
            {
                storeData = true;
            }
            else
            {
                MessageBox.Show("Please enter item name and quantity to save");
            }
        }

        private void Btn_Add_To_Shopping_List_Click(object sender, RoutedEventArgs e)
        {
            if (Lbx_items.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item to add");
            }
            else
            {
                string selectedItem = ((Item)Lbx_items.SelectedItem).itemName.ToString();
                ItemToBuy newItem = new ItemToBuy { itemName = selectedItem };
                shopping.Add(newItem);
                MessageBox.Show(newItem.itemName + " added to shopping list");
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(storeData)
                Storage.WriteXml<ObservableCollection<Item>>(items, "InputData1.xml");
                Storage.WriteXml<ObservableCollection<ItemToBuy>>(shopping, "ShoppingListData.xml");
        }

        private void Btn_Del_Item_Click(object sender, RoutedEventArgs e)
        {
            if (Lbx_items.SelectedItem == null)
            {
                MessageBox.Show("Please select an item first to be deleted", "Error");
                return;
            }
            else
            {
                del_event = true;
                items.Remove(Lbx_items.SelectedItem as Item);
                Tbx_Search.Text = "";
                MessageBox.Show("deleted from grocery list");
            }
        }

        private void Tbx_Quan_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!del_event)
            {
                    if (Tbx_Quan.Text != "" && Convert.ToInt32(Tbx_Quan.Text) < 0)
                    {
                        MessageBox.Show("Quantity cannot be less than 0");
                        Tbx_Quan.Focus();
                    }
            }
        }

        private void Tbx_Name_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!del_event)
            {
                if (Tbx_Name.Text == "")
                {
                     MessageBox.Show("Please enter item name");
                     Tbx_Name.Focus();
                }
            }
        }

        private void Lbx_Shopping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedShopItem = ((ItemToBuy)Lbx_Shopping.SelectedItem).itemName.ToString();
            Item found_item = (from s in items where s.itemName.Contains(selectedShopItem) select s).First<Item>();
            if (found_item != null)
            {
                Lbx_items.SelectedItem = found_item;
            }
        }

        private void Btn_Del_Item_Shopping_List_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
