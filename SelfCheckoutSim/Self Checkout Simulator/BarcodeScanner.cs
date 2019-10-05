using System;
using System.Collections.Generic;
using System.Linq;

namespace Self_Checkout_Simulator
{
    class BarcodeScanner
    {
        // Attributes
        private SelfCheckout selfCheckout;      //This correct? How does it get passed this?

        // Operations
        public void BarcodeDetected()
        {
            // NOTE: In reality BarcodeScanner would see a real barcode and convert
            //       it to a number. We have to fake this, so we grab a random
            //       barcode from the available products and pretend we saw that
            int barcode = ProductsDAO.GetRandomProductBarcode();      
            selfCheckout.BarcodeWasScanned(barcode);        //passes selfcheckout the random barcode
        }

        public void LinkToSelfCheckout(SelfCheckout selfCheckout)
        {
            this.selfCheckout = selfCheckout;   //reference self checkout
        }
    }
}