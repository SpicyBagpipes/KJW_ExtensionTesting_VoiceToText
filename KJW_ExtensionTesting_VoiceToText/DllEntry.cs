using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Text;

namespace KJW_ExtensionTesting_VoiceToText
{
    public class Extension
    {
        public static ExtensionCallback callback;
        public delegate int ExtensionCallback([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPStr)] string data);

        /// <summary>
        /// Gets called when callback() is used.
        /// </summary>
        /// <param name="name">Extension name, unique name etc.</param>
        /// <param name="func">Name of the function it sends the result to. Not needed really</param>
        /// <param name="data">Arguments to pass through</param>
		#if WIN64
			[DllExport("RVExtensionRegisterCallback", CallingConvention = CallingConvention.Winapi)]
		#else
			[DllExport("_RVExtensionRegisterCallback@4", CallingConvention = CallingConvention.Winapi)]
		#endif
	        public static void RVExtensionRegisterCallback([MarshalAs(UnmanagedType.FunctionPtr)] ExtensionCallback func)
			{
	            callback = func;
		    }

        /// <summary>
        /// Gets called when arma starts up and loads all extension.
        /// It's perfect to load in static objects in a seperate thread so that the extension doesn't needs any seperate initalization
        /// </summary>
        /// <param name="output">The string builder object that contains the result of the function</param>
        /// <param name="outputSize">The maximum size of bytes that can be returned</param>
		#if WIN64
			[DllExport("RVExtensionVersion", CallingConvention = CallingConvention.Winapi)]
		#else
	        [DllExport("_RVExtensionVersion@8", CallingConvention = CallingConvention.Winapi)]
		#endif
        public static void RvExtensionVersion(StringBuilder output, int outputSize)
        {
            //Function goes here.
        }

        /// <summary>
        /// The entry point for the default callExtension command.
        /// </summary>
        /// <param name="output">The string builder object that contains the result of the function</param>
        /// <param name="outputSize">The maximum size of bytes that can be returned</param>
        /// <param name="function">The string argument that is used along with callExtension</param>
#if WIN64
			[DllExport("RVExtension", CallingConvention = CallingConvention.Winapi)]
#else
        [DllExport("_RVExtension@12", CallingConvention = CallingConvention.Winapi)]
		#endif
			public static void RvExtension(StringBuilder output, int outputSize,
			   [MarshalAs(UnmanagedType.LPStr)] string function) {
					Worker.EngineHandler(function);
				}

		/// <summary>
		/// The entry point for the callExtensionArgs command.
		/// </summary>
		/// <param name="output">The string builder object that contains the result of the function</param>
		/// <param name="outputSize">The maximum size of bytes that can be returned</param>
		/// <param name="function">The string argument that is used along with callExtension</param>
		/// <param name="args">The args passed to callExtension as a string array</param>
		/// <param name="argsCount">The size of the string array args</param>
		/// <returns>The result code</returns>
		#if WIN64
			[DllExport("RVExtensionArgs", CallingConvention = CallingConvention.Winapi)]
		#else
			[DllExport("_RVExtensionArgs@20", CallingConvention = CallingConvention.Winapi)]
		#endif
			public static int RvExtensionArgs(StringBuilder output, int outputSize,
				[MarshalAs(UnmanagedType.LPStr)] string function,
				[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 4)] string[] args, int argCount) {
					//Function goes here.
					return 0;
				}

        // Callback.
        /*
			callback ("Name", "Function", "Data").
			Name: KJW_Core_VoiceRecognised
			Function: make it name of the function the extension sends the result to. (Note: The returned function is just a STRING! So compile is needed, before using call or spawn, to execute it).
			Data: params returned. 
			All of these are just strings passed as params to the MEH.
		*/
    }
}