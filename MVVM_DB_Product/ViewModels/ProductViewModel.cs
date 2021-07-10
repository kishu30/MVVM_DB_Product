using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;   
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVM_DB_Product.Commands;
using MVVM_DB_Product.Models;

namespace MVVM_DB_Product.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        //default class member is private
        ObservableCollection<Product> productlist = null;


        NorthwindEntities db;


        #region ctor
        public ProductViewModel()
        {
            db = new NorthwindEntities();
            productlist = new ObservableCollection<Product>(); // initilized product in obserbable collection


            loadCommand = new RelayCommand(LoadMethod, canexecute);

            searchCommand = new RelayCommand(SearchMethod, canSearch);

            clearCommand = new RelayCommand(ClearMethod, canexecute);

            addCommand = new RelayCommand(Addmethod, canAdd);

            updateCommand = new RelayCommand(UpdateMethod, canUpdate);

            deleteCommand = new RelayCommand(Deletemethod, canDelete);


        }

        #endregion



        #region inotifyproperty
        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



        #region property

        private Product productobj = new Product();

        public string UI_Product_Id
        {
            get { return productobj.ProductID.ToString(); }
            set
            {
                if (value != string.Empty)
                {
                    productobj.ProductID = Convert.ToInt32(value);
                    OnPropertyChanged("UI_Product_Id");

                }
            }


        }



        public string UI_ProductName
        {
            get { return productobj.ProductName; }
            set
            {
                productobj.ProductName = value;
                OnPropertyChanged("UI_ProductName");
            }
        }


        public string UI_UnitPrice
        {
            get { return productobj.UnitPrice.ToString(); }

            set
            {
                if ( value != string.Empty)
                {
                    productobj.UnitPrice = Convert.ToDecimal(value);


                }

                else
                {
                    productobj.UnitPrice = null;
                }

                OnPropertyChanged("UI_UnitPrice");
            }
        }


        public string UI_Discontinued
        {
            get { return productobj.Discontinued.ToString(); }
            set
            {
                if (value != null)
                {
                    productobj.Discontinued = Convert.ToBoolean(value);
                }
                else
                {
                    productobj.Discontinued = Convert.ToBoolean(false);
                }
                OnPropertyChanged("UI_Discontinued");
            }
        }





       


       public ObservableCollection<Product> ProductList
        {
            get { return productlist; }
            set
            {
                productlist = value;
                OnPropertyChanged("ProductList");
            }
        }
        #endregion




        #region commands

        private ICommand loadCommand;

        public ICommand LoadCommand
        {
            get { return loadCommand; }
            set { loadCommand = value; }
        }


        private ICommand searchCommand;

        public ICommand SearchCommand
        {
            get { return searchCommand; }
            set { searchCommand = value; }
        }


        private ICommand clearCommand;

        public ICommand ClearCommand
        {
            get { return clearCommand; }
            set { clearCommand = value; }
        }

        private ICommand addCommand;

        public ICommand AddCommand
        {
            get { return addCommand; }
            set { addCommand = value; }
        }


        private ICommand updateCommand;

        public ICommand UpdateCommand
        {
            get { return updateCommand; }
            set { updateCommand = value; }
        }



        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }






        #endregion




        #region methods
        public bool canexecute(object obj)
        {
            return true;
        }


        public void LoadMethod(object obj)
        {
            //IEnumerable<Product> products = (from p in db.Products select p).ToList();
            //foreach (Product p in products)
            //{
            //    productlist.Add(p);
            //}
            List<Product> products = (from p in db.Products select p).ToList();

            ProductList = new ObservableCollection<Product>(products);

            //if (ProductList.Count > 0)
            //{
            //    this.UI_Product_Id = ProductList[0].ProductID.ToString();
            //    this.UI_ProductName = ProductList[0].ProductName;
            //    this.UI_UnitPrice = ProductList[0].UnitPrice.ToString();
            //    this.UI_Discontinued = ProductList[0].Discontinued.ToString();
            //}


        }


        public void SearchMethod(object obj)
        {
            int pid = Convert.ToInt32(UI_Product_Id);

            Product product = db.Products.Where(p => p.ProductID == pid ).SingleOrDefault();


            if (product != null)
            {
               

                UI_ProductName = product.ProductName;
                UI_UnitPrice = product.UnitPrice.ToString();
                UI_Discontinued = product.Discontinued.ToString();



            }
            else
            {
                MessageBox.Show("not found");
            }



        }


        public void ClearMethod(object obj)
        {
            UI_Product_Id = 0.ToString();
           
            UI_ProductName = string.Empty;
            UI_UnitPrice = string.Empty;
            UI_Discontinued = Convert.ToString(false);

            productlist.Clear();



        }


        public void Addmethod(object obj)
        {
            Product product = new Product();

            product.ProductID = Convert.ToInt32(UI_Product_Id);
            product.ProductName = UI_ProductName;
            product.UnitPrice = Convert.ToDecimal(UI_UnitPrice);
            product.Discontinued = Convert.ToBoolean(UI_Discontinued);
            db.Products.Add(product);
            LoadMethod(product);
            db.SaveChanges();

        }
        public void UpdateMethod(object obj)
        {
            Product product = new Product(); // asssskkkkkkkk
            product = db.Products.Where(p => p.ProductID.ToString() == UI_Product_Id).SingleOrDefault();
            if (product != null)
            {

                product.ProductName = UI_ProductName;
                product.UnitPrice = Convert.ToDecimal(UI_UnitPrice);
                
                product.Discontinued = Convert.ToBoolean(UI_Discontinued);
                db.SaveChanges();
                LoadMethod(product);

            }
            else
            {
                MessageBox.Show("No record is there");
            }
            

        }

        public void Deletemethod(object obj)

            
        {
            int pidi = Convert.ToInt32(UI_Product_Id);
            Product product = db.Products.Where(p => p.ProductID == pidi ).SingleOrDefault();

            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                LoadMethod(product);


            }
            else
            {
                MessageBox.Show("No Record Found!");
            }


        }






        public bool canAdd(object obj)
        {
            if (Convert.ToInt32(UI_Product_Id) > 0 && UI_ProductName != null)
            {
                return true;
            }

            return false;
        }


        public bool canDelete(object obj)
        {
            if (Convert.ToInt32(UI_Product_Id) > 0)
            {
                return true;
            }
            return false;
        }

        public bool canUpdate(object obj)
        {
            if (Convert.ToInt32(UI_Product_Id) > 0 && UI_ProductName != null)
            {
                return true;
            }
            return false;
        }


        public bool canSearch(object obj)
        {
            if (Convert.ToInt32(UI_Product_Id) > 0)
            {
                return true;
            }
            return false;
        }

        #endregion




    }
}
