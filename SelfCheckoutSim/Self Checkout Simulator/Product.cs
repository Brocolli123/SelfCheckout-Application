using System;
using System.Collections.Generic;
using System.Linq;

namespace Self_Checkout_Simulator
{
    abstract class Product
    {
        // Attributes
        protected int barcode;
        protected string name;
        protected int weightInGrams;
        private int mostRecentWeight;
        protected bool isFree;

        // Operations
        public string GetName()
        {
            return name;
        }

        public int GetBarcode()
        {
            return barcode;
        }

        public abstract bool IsLooseProduct();      //To be overwritten by specific product type

        public int GetWeight()
        {
            
            return weightInGrams;
        }

        public void SetWeight(int weight)
        {
            this.weightInGrams = weight;
            mostRecentWeight = weightInGrams;
        }

        public void MakeFree()
        {
            isFree = true;
        }

        public bool IsFree()
        {
            return isFree;
        }

        public abstract double CalculatePrice();
       

        // TODO: Use the class diagram for details of other operations
    }

    class PackagedProduct : Product
    {
        // Attributes
        private int priceInPence;

        // Constructor
        public PackagedProduct(int barcode, string name, int priceInPence, int weightInGrams)
        {
            this.barcode = barcode;
            this.name = name;
            this.priceInPence = priceInPence;
            this.weightInGrams = weightInGrams;
            this.isFree = false;
        }

        // Operations
        public override double CalculatePrice()
        {
            if (IsFree())
            {
                return 0;
            }
            return priceInPence / 100.00;
        }

        public override bool IsLooseProduct()
        {
            return false;
        }
    }

    class LooseProduct : Product
    {
        // Attributes
        int pencePer100g;

        // Constructor
        public LooseProduct(int barcode, string name, int pencePer100g)
        {          
            this.barcode = barcode;
            this.name = name;
            this.pencePer100g = pencePer100g;
            this.isFree = false;
        }

        // Operations
        public int GetPencePer100g()
        {
            return pencePer100g;
        }

        public override bool IsLooseProduct()
        {
            return true;
        }

        public override double CalculatePrice()
        {
            if (IsFree())
            {
                return 0;
            }
            return weightInGrams * (pencePer100g / 1000.0);
        }

    }
}