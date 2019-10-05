using System;
using System.Collections.Generic;
using System.Linq;

namespace Self_Checkout_Simulator
{
    class ScannedProducts
    {
        // Attributes
        private List<Product> products = new List<Product>();
        double discountPercentage = 1;


        // Operations
        public List<Product> GetProducts()
        {
            return products;
        }

        public int CalculateWeight()
        {
            int weight = 0;
            foreach (Product p in products)
            {
                weight += p.GetWeight();
            }
            return weight;
        }

        public double CalculatePrice()
        {
            double price = 0;
            foreach (Product p in products)
            {
                if (!p.IsFree())
                {
                    if (p.IsLooseProduct() == true)
                    {
                        price += p.CalculatePrice() / 2;
                    }
                    else
                    {
                        price += p.CalculatePrice();
                    }
                }
            }
            price = price * discountPercentage;
            return price;
        }

        public void Reset()
        {
            products.Clear();       //Empty list
        }

        public void Add(Product p)
        {
            products.Add(p);        //Add p to products
        }

        public void Remove(Product p)
        {
            products.Remove(p);
        }

        public bool HasItems()
        {
            if (products.Count <= 0)        //if count is above 0, HasItems
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DiscountPercentageOff()
        {
            try
            {
                discountPercentage = double.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter percentage discount (0-100)", "Discount Percentage", "0.00")) / 100;
                if (discountPercentage > 1 || discountPercentage < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a number between 0 and 100");
                    DiscountPercentageOff();
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Please enter a number between 0 and 100");
                DiscountPercentageOff();
            }
        }

        public void DiscountBuyOneGetOneFree()
        {
            Product recentProduct = products[products.Count - 1];
            Product otherProduct = products[products.Count - 2];

            if (recentProduct.CalculatePrice() < otherProduct.CalculatePrice())
            {
                products[products.Count - 2].MakeFree();
            }
            else
            {
                products[products.Count - 1].MakeFree();
            }
        }

        public void DiscountBuyThreeCheapestFree()
        {
            Product recentProduct = products[products.Count - 1];
            Product otherProduct = products[products.Count - 2];
            Product thirdProduct = products[products.Count - 3];

            if ((recentProduct.CalculatePrice() < otherProduct.CalculatePrice()) && (otherProduct.CalculatePrice() < thirdProduct.CalculatePrice()))
            {
                products[products.Count - 2].MakeFree();
            }
            else
            {
                if (thirdProduct.CalculatePrice() < recentProduct.CalculatePrice())
                {
                    products[products.Count - 3].MakeFree();
                }
                else
                {
                    products[products.Count - 1].MakeFree();
                }
            }
        }

    }
}