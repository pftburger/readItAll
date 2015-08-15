#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "GetArray", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueGetArrayNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		ISpread<ISpread<double>> FInput;

		[Input("Rows")]
		ISpread<int> FRows;
		
		[Input("Columns")]
		ISpread<int> FColumns;
		
		[Output("Output")]
		ISpread<ISpread<double>> FOutput;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = FColumns.SliceCount;
			for (int i = 0; i < FColumns.SliceCount; i++)
			{
				FOutput[i].SliceCount = FRows.SliceCount;
				for (int j = 0; j < FRows.SliceCount; j++)
				{
					FOutput[i][j] = FInput[FColumns[i]][FRows[j]];
				}
			}

			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
