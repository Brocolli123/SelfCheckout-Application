using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Self_Checkout_Simulator
{
    public partial class UserInterface : Form
    {
        // Attributes
        SelfCheckout selfCheckout;
        BarcodeScanner barcodeScanner;
        BaggingAreaScale baggingAreaScale;
        LooseItemScale looseItemScale;
        ScannedProducts scannedProducts;

        // Constructor
        public UserInterface()
        {
            InitializeComponent();

            // NOTE: This is where we set up all the objects,
            // and create the various relationships between them.

            baggingAreaScale = new BaggingAreaScale();
            scannedProducts = new ScannedProducts();
            barcodeScanner = new BarcodeScanner();
            looseItemScale = new LooseItemScale();
            selfCheckout = new SelfCheckout(baggingAreaScale, scannedProducts, looseItemScale);
            barcodeScanner.LinkToSelfCheckout(selfCheckout);
            baggingAreaScale.LinkToSelfCheckout(selfCheckout);
            looseItemScale.LinkToSelfCheckout(selfCheckout);

            btnUserScansBarcodeProduct.Enabled = true;
            btnUserSelectsLooseProduct.Enabled = true;
            btnUserWeighsLooseProduct.Enabled = false;
            btnUserChooseToPay.Enabled = false;
            btnUserPutsProductInBaggingAreaCorrect.Enabled = false;
            btnUserPutsProductInBaggingAreaIncorrect.Enabled = false;
            btnAdminOverridesWeight.Enabled = false;
            AdminRemoveProduct.Enabled = false;
            RemoveProduct.Enabled = false;

            UpdateDisplay();
        }

        // Operations
        private void UserScansProduct(object sender, EventArgs e)       //Packaged product
        {
            barcodeScanner.BarcodeDetected();
            string price = selfCheckout.GetCurrentProduct().CalculatePrice().ToString("c2");
            lbBasket.Items.Add(selfCheckout.GetCurrentProduct().GetName() + price);

            btnUserScansBarcodeProduct.Enabled = false;
            btnUserSelectsLooseProduct.Enabled = false;
            btnUserWeighsLooseProduct.Enabled = false;
            btnUserPutsProductInBaggingAreaCorrect.Enabled = true;
            btnUserPutsProductInBaggingAreaIncorrect.Enabled = true;
            btnUserChooseToPay.Enabled = false;
            RemoveProduct.Enabled = true;

            UpdateDisplay();
        }

        private void UserPutsProductInBaggingAreaCorrect(object sender, EventArgs e)
        {
            int weight = selfCheckout.GetCurrentProduct().GetWeight();
            if (selfCheckout.GetCurrentProduct().IsLooseProduct() == true)      //to double for loose product which gives half correct value        
            {
                weight *= 2;
            }
            baggingAreaScale.WeightChangeDetected(weight);    //changes weight with current product

            btnUserScansBarcodeProduct.Enabled = true;
            btnUserSelectsLooseProduct.Enabled = true;
            btnUserPutsProductInBaggingAreaCorrect.Enabled = false;
            btnUserPutsProductInBaggingAreaIncorrect.Enabled = false;
            btnUserChooseToPay.Enabled = true;

            UpdateDisplay();
        }

        private void UserPutsProductInBaggingAreaIncorrect(object sender, EventArgs e)
        {
            baggingAreaScale.WeightChangeDetected(new Random().Next(20, 100));  //change weight with the random weight given

            btnUserScansBarcodeProduct.Enabled = false;
            btnUserSelectsLooseProduct.Enabled = false;
            btnUserPutsProductInBaggingAreaCorrect.Enabled = false;
            btnUserPutsProductInBaggingAreaIncorrect.Enabled = false;
            btnAdminOverridesWeight.Enabled = true;

            UpdateDisplay();
        }

        private void UserSelectsALooseProduct(object sender, EventArgs e)
        {
            btnUserSelectsLooseProduct.Enabled = false;
            btnUserScansBarcodeProduct.Enabled = false;
            btnUserChooseToPay.Enabled = false;
            btnUserWeighsLooseProduct.Enabled = true;

            selfCheckout.LooseProductSelected();
            scannedProducts.Add(selfCheckout.GetCurrentProduct());

            UpdateDisplay();
        }

        private void UserWeighsALooseProduct(object sender, EventArgs e)
        {
            // NOTE: We are pretending to weigh a banana or whatever here.
            // To simulate this we'll use a random number, here's one for you to use.
            looseItemScale.WeightChangeDetected(new Random().Next(20, 100));        //Sets scale weight to random weight given
            string price = selfCheckout.GetCurrentProduct().CalculatePrice().ToString("c2");
            //string pricestring = price.ToString("c2");
            lbBasket.Items.Add(selfCheckout.GetCurrentProduct().GetName() + price);

            btnUserScansBarcodeProduct.Enabled = false;
            btnUserSelectsLooseProduct.Enabled = false;
            btnUserWeighsLooseProduct.Enabled = false;
            btnUserPutsProductInBaggingAreaCorrect.Enabled = true;
            btnUserPutsProductInBaggingAreaIncorrect.Enabled = true;

            UpdateDisplay();
        }

        private void AdminOverridesWeight(object sender, EventArgs e)
        {
            baggingAreaScale.OverrideWeight();      //Giving a wrong number??? or called at wrong place??

            btnUserScansBarcodeProduct.Enabled = true;
            btnUserSelectsLooseProduct.Enabled = true;
            btnUserChooseToPay.Enabled = true;
            btnAdminOverridesWeight.Enabled = false;

            UpdateDisplay();
        }

        private void UserChoosesToPay(object sender, EventArgs e)
        {
            selfCheckout.UserPaid();

            btnUserChooseToPay.Enabled = false;
            lbBasket.Items.Clear();

            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            lbBasket.Text = lbBasket.Items.ToString();
            lblScreen.Text = selfCheckout.GetPromptForUser();
            lblTotalPrice.Text = "£"+scannedProducts.CalculatePrice().ToString("0.00");
            lblBaggingAreaCurrentWeight.Text = baggingAreaScale.GetCurrentWeight().ToString("n2");
            lblBaggingAreaExpectedWeight.Text = baggingAreaScale.GetExpectedWeight().ToString("n2");

            if (scannedProducts.HasItems() == true) {
                RemoveProduct.Enabled = true;
                AdminRemoveProduct.Enabled = true;
            }
        }

        private void lblBaggingAreaExpectedWeight_Click(object sender, EventArgs e)
        {

        }

        private void lblScreen_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)      //Admin confirm remove product
        {
            AdminRemoveProduct.Enabled = false;     //Disable this
            selfCheckout.RemoveProductAdmin();
            selfCheckout.DisableRemove();

            lbBasket.Items.RemoveAt(lbBasket.Items.Count-1);  //remove from basket list

            UpdateDisplay();
        }

        private void RemoveProduct_Click(object sender, EventArgs e)        //Remove product
        {
            AdminRemoveProduct.Enabled = true;      //Enable admin confirm button
            RemoveProduct.Enabled = false;          //Disable this

            selfCheckout.EnableRemove();
            selfCheckout.GetPromptForUser();
      
            UpdateDisplay();

        }

        private void UserInterface_Load(object sender, EventArgs e)
        {

        }

        private void PercentDiscountBtn_Click(object sender, EventArgs e)
        {
            scannedProducts.DiscountPercentageOff();
            UpdateDisplay();
        }

        private void BOGOFDiscountBtn_Click(object sender, EventArgs e)
        {
            scannedProducts.DiscountBuyOneGetOneFree();
            UpdateDisplay();
        }

        private void CheapestOf3DiscountBtn_Click(object sender, EventArgs e)
        {
            scannedProducts.DiscountBuyThreeCheapestFree();
            UpdateDisplay();
        }
    }
}