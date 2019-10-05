using System;
using System.Collections.Generic;
using System.Linq;

namespace Self_Checkout_Simulator
{
    class LooseItemScale
    {
        //Attributes
        bool enabled;
        SelfCheckout selfCheckout;

        //Operations
        public void Enable()
        {
            enabled = true;
        }

        public bool IsEnabled()
        {
            return enabled;
        }

        public void LinkToSelfCheckout(SelfCheckout sc)
        {
            this.selfCheckout = sc;
        }

        public void WeightChangeDetected(int weight)
        {
            selfCheckout.LooseItemAreaWeightChanged(weight);
            enabled = false;
        }
    }
}