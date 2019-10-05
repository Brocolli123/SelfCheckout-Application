using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic;

namespace Self_Checkout_Simulator
{
    class SelfCheckout
    {
        // Attributes
        private Product currentProduct;             
        private ScannedProducts scannedProducts;        //Aggregation
        private BaggingAreaScale baggingArea;           //Association
        private LooseItemScale looseItemScale;          //Association

        private bool remove;    //for removing product
        private int mostRecentWeight;
        private Product mostRecentProduct;
        private Product[] ProductsList = new Product[30]; //to keep track of what to remove
        private int i = 0;

        // Constructor
        public SelfCheckout(BaggingAreaScale baggingArea, ScannedProducts scannedProducts, LooseItemScale looseItemScale)
        {
            this.baggingArea = baggingArea;
            this.scannedProducts = scannedProducts;
            this.looseItemScale = looseItemScale;
        }

        // Operations
        public void LooseProductSelected()
        {
            looseItemScale.Enable();
            currentProduct = ProductsDAO.GetRandomLooseProduct();   //enable scale and get the current loose product    (just finding random one for now)
        }

        public void LooseItemAreaWeightChanged(int weightOfLooseItem)       //Add changed weight to current weight
        {
            currentProduct.SetWeight(weightOfLooseItem);    //sets weight to current product weight
            scannedProducts.Add(currentProduct);    //adds current item to list
            ProductsList[i] = currentProduct;       //puts current product into list
            mostRecentProduct = ProductsList[i];        //puts most recent product
            baggingArea.SetExpectedWeight(scannedProducts.CalculateWeight());   //calculates new bagging weight
            ++i;        //adds to the index of products
        }

        public void BarcodeWasScanned(int barcode)
        {
            currentProduct = ProductsDAO.SearchUsingBarcode(barcode);        //Find product in the database and store it
            scannedProducts.Add(currentProduct);                                     //Add product to ScannedProducts
            ProductsList[i] = currentProduct;   //put current product into productslist
            mostRecentProduct = ProductsList[i];    //gets recent product from list
            baggingArea.SetExpectedWeight(scannedProducts.CalculateWeight());  //Use to SetExpectedWeight in BaggingAreaScale to new weight
            ++i;    //adds to index of products
        }

        public void BaggingAreaWeightChanged()     
        {
            currentProduct = null;      //ready to get next current product
        }

        public void UserPaid()
        {
            MessageBox.Show("User has Paid");       //Some visual feedback to paying
            scannedProducts.Reset();
            baggingArea.Reset();
        }

        public string GetPromptForUser()        //Done in order so it doesn't overwrite the message
        {
            if (scannedProducts.HasItems() && baggingArea.IsWeightOk() && currentProduct == null)
            {
                return "Scan another item or pay";
            }
            if (baggingArea.IsWeightOk() && currentProduct == null)
            {
                return "Scan Loose or Packaged Product";
            }
            if (looseItemScale.IsEnabled())
            {
                return "Place item on scale";
            }
            if (currentProduct != null && looseItemScale.IsEnabled() == false)
            {
                return "Place the item in the bagging area";
            }
            if (scannedProducts.HasItems() && baggingArea.IsWeightOk() == false)
            {
                return "Weight incorrect, needs admin override";
            }
            return "ERROR: Unknown state!";     //Defaults to error
        }

        public Product GetCurrentProduct()
        {
            return currentProduct;
        }

        public void AdminOverrideWeight(BaggingAreaScale bagArea)     //Override the weight with the inputted weight
        {
            baggingArea = bagArea;      //sets to new baggingarea to get correct weight
        }

        public void EnableRemove()
        {
            remove = true;
        }
        public void DisableRemove()
        {
            remove = false;
        }

        public void RemoveProductAdmin()
        {
            ProductsList[i] = ProductsList[i - 1];  //get product before current one
            //double price = ProductsList[i].CalculatePrice();
            scannedProducts.Remove(ProductsList[i]);    //remove it
            baggingArea.RemoveWeight(); //remove last one
            if (ProductsList[i].IsLooseProduct() == true)
            {
                baggingArea.RemoveWeight(); //has to be done twice for correct weight if it is loose
            }
            --i;    //count index back
        }

        public int GetMostRecentProductWeight()
        {
            ProductsList[i] = ProductsList[i - 1];  //get product before curent one
            mostRecentWeight = ProductsList[i].GetWeight(); //get weight of last product
            return mostRecentWeight;
        }

    }
}