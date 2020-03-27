using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingChargeCalculator
{
    public class ConsoleApplication
    {
        private readonly IChargeCalculator chargeCalculator;

        private enum ActionRequired { None, CalculateCharge };
        public ConsoleApplication(IChargeCalculator chargeCalculator)
        {
            this.chargeCalculator = chargeCalculator;
        }
        public void Run(string[] args)
        {
            ActionRequired actionRequired = ActionRequired.None;
            foreach (string cmd in args)
            {
                if (cmd.StartsWith("/"))
                {
                    switch (cmd.Substring(1))
                    {
                        case "c":
                            actionRequired = ActionRequired.CalculateCharge;
                            break;
                        default:
                            break;
                    }
                }
            }
            switch (actionRequired)
            {
                case ActionRequired.CalculateCharge:
                    chargeCalculator.Action();
                    break;
                default:
                    break;
            }
        }
    }
}
