using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovableTrees
{
    internal class LocalizationManager
    {
        public static void AddTerm(string key, string term)
        {
            I2.Loc.TermData termdata = new I2.Loc.TermData();
            var sources = I2.Loc.LocalizationManager.Sources;
            termdata.Term = term;
            termdata.TermType = I2.Loc.eTermType.Text;
            termdata.Flags = new Byte[sources[0].mLanguages.Count];
            string[] languages = new string[19];
            foreach (int value in Enumerable.Range(0, sources[0].mLanguages.Count)) { languages.SetValue(termdata.Term, value); }
            termdata.Languages = (Il2CppStringArray)languages;
            sources[0].mDictionary.Add(key, termdata);
            sources[0].mTerms.Add(termdata);
            sources[0].UpdateDictionary();
        }

        public static void AddTerm(string term)
        {
            I2.Loc.TermData termdata = new I2.Loc.TermData();
            var sources = I2.Loc.LocalizationManager.Sources;
            termdata.Term = term;
            termdata.TermType = I2.Loc.eTermType.Text;
            termdata.Flags = new Byte[sources[0].mLanguages.Count];
            string[] languages = new string[19];
            foreach (int value in Enumerable.Range(0, sources[0].mLanguages.Count)) { languages.SetValue(termdata.Term, value); }
            termdata.Languages = (Il2CppStringArray)languages;
            sources[0].mDictionary.Add(termdata.Term, termdata);
            sources[0].mTerms.Add(termdata);
            sources[0].UpdateDictionary();
        }

        //enum languages
        //{
        //    English,
        //    Swedish,
        //    German,
        //    French (France),
        //    Portuguese (Brazil),
        //    Italian (Italy),
        //    Japanese,
        //    Korean,
        //    Polish,
        //    Russia,
        //    Spanish,
        //    Spanish (Mexico),
        //    Spanish (Latin Americas),
        //    Turkish,
        //    Ukrainian,
        //    Czech,
        //    Chinese (Simplified),
        //    Chinese (Traditional),
        //    Thai
        //}
    }
}
