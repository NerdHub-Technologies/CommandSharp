using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandSharp.Commands
{
    public sealed class DataCalculatorCommand : Command
    {
        private static readonly CommandData data = new CommandData("datacalc", "Calculates data.", new string[] { });

        public DataCalculatorCommand() : base(data) { }

        public override bool OnInvoke(CommandInvokeParameters e)
        {
            //var TiB = DataConverter.Tebibyte.Create(2);
            var MiB = DataConverter.Mebibyte.Create(32);
            var KiB = MiB.ToKibibyte();
            var B = MiB.ToByte();
            StringBuilder b = new StringBuilder();
            b.AppendLine(MiB.ToString());
            b.AppendLine(KiB.ToString());
            b.AppendLine(B.ToString());
            Console.WriteLine(b);
            return true;
        }
    }
}
