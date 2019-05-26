using FLS;
using FLS.Rules;
using System;
using System.Collections.Generic;
using System.Text;


namespace ControlUnit
{
    public class HeatControl
    {
        public double Control()
        {
            var water = new LinguisticVariable("Water");
            var cold = water.MembershipFunctions.AddTrapezoid("Cold", 0, 0, 20, 40);
            var warm = water.MembershipFunctions.AddTriangle("Warm", 30, 50, 70);
            var hot = water.MembershipFunctions.AddTrapezoid("Hot", 50, 80, 100, 100);

            var power = new LinguisticVariable("Power");
            var low = power.MembershipFunctions.AddTriangle("Low", 0, 25, 50);
            var high = power.MembershipFunctions.AddTriangle("High", 25, 50, 75);

            IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

            var rule1 = Rule.If(water.Is(cold).Or(water.Is(warm))).Then(power.Is(high));
            var rule2 = Rule.If(water.Is(hot)).Then(power.Is(low));
            fuzzyEngine.Rules.Add(rule1, rule2);

            var result = fuzzyEngine.Defuzzify(new { water = 60 });
            return result;

        }
    }
}

