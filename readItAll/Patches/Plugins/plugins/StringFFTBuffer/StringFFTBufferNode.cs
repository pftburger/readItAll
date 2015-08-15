#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "FFTBuffer", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
	public class StringFFTBufferNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultString = "hello c#", IsSingle = true)]
		IDiffSpread<string> FInput;

		[Input("Update", IsSingle = true)]
		ISpread<bool> FUpdate;
		
		[Input("Frame")]
		ISpread<int> FFrame;
		
		[Output("Output")]
		ISpread<ISpread<double>> FOutput; 

		[Output("FrameCount")]
		ISpread<int> FFrameCount;
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		double[][] Temp;
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;

			if(FInput.IsChanged || FUpdate[0]) {
				// split string
				string[] lines = FInput[0].Split('#');
				Temp = new double[lines.Length][];
				for(int i = 0; i < lines.Length; i++)
				{
					string[] vars = lines[i].Split(';');
					Temp[i] = new double[vars.Length];
					for(int j = 0; j < vars.Length; j++)
					{
						double res = 0;
						if(double.TryParse(vars[j], out res))
							Temp[i][j] = res; 
					}
				}
			}
			
			// write output
			FOutput.SliceCount = FFrame.SliceCount;
			for(int i = 0; i < FFrame.SliceCount; i++) {
				if(FFrame[i] < 0 || FFrame[i] > Temp.Length) {
					FFrame[i] = 0;			
				}
				FOutput[i].AssignFrom(Temp[FFrame[i]]);
			}
			
			FFrameCount.SliceCount = 1;
			FFrameCount[0] = Temp.Length;

			//FLogger.Log(LogType.Debug, "Logging to Renderer (TTY)");
		}
	}
}
