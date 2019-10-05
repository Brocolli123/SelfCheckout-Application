using System;
using System.Collections.Generic;
using System.Linq;

namespace Self_Checkout_Simulator
{
    class BaggingAreaScale
    {                  
        //private List<Product> products = new List<Product>(); 
        private SelfCheckout selfCheckout;
        private int weight, expected, allowedDifference;
        private int mostRecentWeight;

        // Operations
        public int GetCurrentWeight()
        {
            return weight;
        }

        public bool IsWeightOk()
        {
            int difference = expected + allowedDifference;
            if (difference == weight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetExpectedWeight()
        {
            return expected + allowedDifference;
        }

        public void SetExpectedWeight(int expectedWeight)
        {
           this.expected = expectedWeight;
        }

        public void OverrideWeight()
        {
            //weight = expected;
            allowedDifference = weight - expected;  //changes allowed weight to current
        }

        public void Reset()
        {
            allowedDifference = 0;
            weight = 0;
            SetExpectedWeight(0);
        }

        public void LinkToSelfCheckout(SelfCheckout selfCheckout)
        {
            this.selfCheckout = selfCheckout;   //reference self checkout
        }

        public void WeightChangeDetected(int change)
        {
            weight += change;       //adds to current weight
            selfCheckout.BaggingAreaWeightChanged();
        }

        public void RemoveWeight()
        {
            mostRecentWeight = selfCheckout.GetMostRecentProductWeight();  //Gets the last product's weight
            weight = expected - mostRecentWeight;       //Get correct current weight
            expected = expected - mostRecentWeight;     //Get correct expected weight
        }

    }
}