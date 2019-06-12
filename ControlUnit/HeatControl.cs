using FLS;
using FLS.Rules;
using System;
using System.Collections.Generic;
using System.Text;


namespace ControlUnit
{
    public class HeatControl
    {
        private  IFuzzyEngine _fuzzyEngineErik;
        private readonly double[] _firArray;
        public HeatControl()
        {
            
            //inicializalom a sulyzo ombot a FIR szurohoz
            _firArray = new double[10];
            for (int i = 0; i < _firArray.Length; i++)
            {
                _firArray[i] = 1 / 10;
            }

           

        }
        public double Control(double errorInput, double derivateInput)
        {
            _fuzzyEngineErik = new FuzzyEngineFactory().Default();

            //bemeneti hiba univerzum
            var error = new LinguisticVariable("Error");
            var nagyonHuteniKell = error.MembershipFunctions.AddTriangle("Very Cool", -500, -11, -4);
            var huteniKell = error.MembershipFunctions.AddTriangle("Cool", -5, -3, -1);
            var jo = error.MembershipFunctions.AddTriangle("Good", -2, 0, 2);
            var futeniKell = error.MembershipFunctions.AddTriangle("Hot", 1, 3, 5);
            var nagyonFuteniKell = error.MembershipFunctions.AddTriangle("Very Hot", 4, 11, 500);

            //bemeneti derivalt univerzum
            var dt = new LinguisticVariable("Derivate");
            var nagyonHevul = dt.MembershipFunctions.AddTriangle("Very Cooling", -100, -0.65, -0.3);
            var hevul = dt.MembershipFunctions.AddTriangle("Cooling", -0.5, -0.3, -0.1);
            var stabil = dt.MembershipFunctions.AddTriangle("Stabil", -0.2, 0, 0.2);
            var hul = dt.MembershipFunctions.AddTriangle("Heating", 0.1, 0.3, 0.5);
            var nagyonHull = dt.MembershipFunctions.AddTriangle("Very Heating", 0.3, 0.65, 100);

            //kimeneti univerzum
            var output = new LinguisticVariable("Output");
            var outVeryCooling = output.MembershipFunctions.AddTriangle("Very Cooling", -100, -75, -50);
            var outCooling = output.MembershipFunctions.AddTriangle("Cooling", -65, -45, -25);
            var outStop = output.MembershipFunctions.AddTriangle("Stop ", -30, 0, 30);
            var outHeating = output.MembershipFunctions.AddTriangle("Heating", 25, 45, 65);
            var outVeryHeating = output.MembershipFunctions.AddTriangle("Very Heating", 50, 75, 100);

            //mamdani torvenyek hozzaadasa

            _fuzzyEngineErik.Rules.Add(
                Rule.If(error.Is(nagyonHuteniKell).And(dt.Is(nagyonHevul))).Then(output.Is(outVeryCooling)),
                Rule.If(error.Is(nagyonHuteniKell).And(dt.Is(hevul))).Then(output.Is(outVeryCooling)),
                Rule.If(error.Is(nagyonHuteniKell).And(dt.Is(stabil))).Then(output.Is(outCooling)),
                Rule.If(error.Is(nagyonHuteniKell).And(dt.Is(hul))).Then(output.Is(outCooling)),
                Rule.If(error.Is(nagyonHuteniKell).And(dt.Is(nagyonHull))).Then(output.Is(outCooling))
            );
            _fuzzyEngineErik.Rules.Add(
                Rule.If(error.Is(huteniKell).And(dt.Is(nagyonHevul))).Then(output.Is(outVeryCooling)),
                Rule.If(error.Is(huteniKell).And(dt.Is(hevul))).Then(output.Is(outCooling)),
                Rule.If(error.Is(huteniKell).And(dt.Is(stabil))).Then(output.Is(outCooling)),
                Rule.If(error.Is(huteniKell).And(dt.Is(hul))).Then(output.Is(outCooling)),
                Rule.If(error.Is(huteniKell).And(dt.Is(nagyonHull))).Then(output.Is(outStop))
            );
            _fuzzyEngineErik.Rules.Add(
                Rule.If(error.Is(jo).And(dt.Is(nagyonHevul))).Then(output.Is(outCooling)),
                Rule.If(error.Is(jo).And(dt.Is(hevul))).Then(output.Is(outStop)),
                Rule.If(error.Is(jo).And(dt.Is(stabil))).Then(output.Is(outStop)),
                Rule.If(error.Is(jo).And(dt.Is(hul))).Then(output.Is(outStop)),
                Rule.If(error.Is(jo).And(dt.Is(nagyonHull))).Then(output.Is(outHeating))
            );
            _fuzzyEngineErik.Rules.Add(
                Rule.If(error.Is(futeniKell).And(dt.Is(nagyonHevul))).Then(output.Is(outStop)),
                Rule.If(error.Is(futeniKell).And(dt.Is(hevul))).Then(output.Is(outHeating)),
                Rule.If(error.Is(futeniKell).And(dt.Is(stabil))).Then(output.Is(outHeating)),
                Rule.If(error.Is(futeniKell).And(dt.Is(hul))).Then(output.Is(outHeating)),
                Rule.If(error.Is(futeniKell).And(dt.Is(nagyonHull))).Then(output.Is(outVeryHeating))
            );
            _fuzzyEngineErik.Rules.Add(
                Rule.If(error.Is(nagyonFuteniKell).And(dt.Is(nagyonHevul))).Then(output.Is(outHeating)),
                Rule.If(error.Is(nagyonFuteniKell).And(dt.Is(hevul))).Then(output.Is(outHeating)),
                Rule.If(error.Is(nagyonFuteniKell).And(dt.Is(stabil))).Then(output.Is(outHeating)),
                Rule.If(error.Is(nagyonFuteniKell).And(dt.Is(hul))).Then(output.Is(outVeryHeating)),
                Rule.If(error.Is(nagyonFuteniKell).And(dt.Is(nagyonHull))).Then(output.Is(outVeryHeating))
            );
            var result = _fuzzyEngineErik.Defuzzify(new { Error = errorInput , Derivate = derivateInput });
            return result;
        }
        public double FirFilter(double[] incommingData)
        {
            //FIR szuro
            double result=0;
            for (int i = 0; i < incommingData.Length; i++)
            {
                result += incommingData[i] * _firArray[i];
            }

            return result;
        }
    }
}
