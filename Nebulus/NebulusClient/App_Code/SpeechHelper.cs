﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;

namespace NebulusClient.App_Code
{
    public class SpeechHelper
    {
        static SpeechSynthesizer synth;

        public static void Speak(string Text)
        {
            if(synth == null)
                synth = new SpeechSynthesizer();
            
            synth.Rate = -2;

            synth.SpeakAsync(Text);
            
        }

        public static void MakeBeep()
        {
            System.Console.Beep();
        }

        public static async void HighFreuencyAlarm()
        {
            System.Console.Beep(3100, 1000);
            await Task.Delay(1000);
            System.Console.Beep(3100, 1000);
            await Task.Delay(1000);
            System.Console.Beep(3100, 1000);
            await Task.Delay(1000);
            System.Console.Beep(3100, 1000);
        }

        public static async void LowFreuencyAlarm()
        {
            System.Console.Beep(320, 1000);
            await Task.Delay(1000);
            System.Console.Beep(320, 1000);
            await Task.Delay(1000);
            System.Console.Beep(320, 1000);
            await Task.Delay(1000);
            System.Console.Beep(320, 1000);
        }

        internal static string GetSpeechString(string p)
        {
            string speech = string.Empty;
            HtmlAgilityPack.HtmlDocument docmuent = new HtmlAgilityPack.HtmlDocument();
            docmuent.LoadHtml(p);

            var speechSpan = docmuent.DocumentNode.DescendantsAndSelf("span").Where(span => span.Attributes.Contains("class") && span.Attributes["class"].Value.Contains("Speech"));
            if(speechSpan.Count() > 0)
            {
                foreach(var item in speechSpan)
                {
                    speech += item.InnerText + ", ";
                }
            }

            return speech;
        }
    }
}
