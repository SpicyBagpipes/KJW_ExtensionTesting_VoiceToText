using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;

namespace KJW_ExtensionTesting_VoiceToText
{
    public static class Worker
    {
        private static SpeechRecognizer? speechRecognizer; // Declare the variable outside the if/else blocks.
        public static async void EngineHandler(string type) { 

            if (type == "construct")
            {
                //Construct the speech recognition engine.
                await new MediaCapture().InitializeAsync(new()
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio,
                    MediaCategory = MediaCategory.Speech,
                });

                //Get speech recogniser language
                var language = SpeechRecognizer.SystemSpeechLanguage;

                //Create new recognizer.
                using var speechRecognizer = new SpeechRecognizer(language);
                await speechRecognizer.CompileConstraintsAsync();

                //Set up recognized event.
                speechRecognizer.ContinuousRecognitionSession.ResultGenerated += (sender, args) =>
                {
                    Extension.callback("KJW_VoiceToText", "Recognized", args.Result.Text);
                };
            }
            else
            {
                //Destroy the speech recognition engine.
                if (speechRecognizer != null)
                {
                    await speechRecognizer.ContinuousRecognitionSession.StopAsync();

                    //Dispose of the SpeechRecognizer instance.
                    speechRecognizer.Dispose();
                    speechRecognizer = null;

                    //Callback with recognizer has stopped.
                    Extension.callback("KJW_VoiceToText", "Stopped", "");
                }
            }
        }
    }
}
