using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VowelWords
{
    class Program
    {
        // vowel letters
        static string vowelRegExp = @"^[aeiouy]+$";

        // correct format with directory is "folder/file", so file must be without type
        const string fileNameRegExp = @"^(?!-)[a-z0-9-]+(?<!-)(/(?!-)[a-z0-9-]+(?<!-))*$";
        static string path;
        static string fileContent;
        static int existVowelWordsCount;

        // get words https://word.tips/vowel-words/
        static List<string> allVowelWords = new List<string>() { "euouae", "ayaya", "aiyee", "yoyo", "euoi", "yay", "aye",
            "ayu", "eye", "oye", "uey", "yae", "yea", "you", "aia", "aua", "aue", "eau", "ay", "oy", "ya", "ye",
            "yi", "yo", "yu", "aa", "ae", "ai", "ea", "ee", "eo", "io", "oe", "oi", "oo", "ou" };

        static void Main(string[] args)
        {
            bool isExit;
            do
            {
                // clear console
                Console.Clear();

                // get and check is correct file path
                GetPath();

                // read file text
                using (var reader = File.OpenText(path))
                {
                    fileContent = reader.ReadToEnd();
                }

                // get type of check
                TypeOfWordsCheck();

                Console.WriteLine($"Count of vowel word is - {existVowelWordsCount}");

                // rerun
                YesOrNo("Try again?", out isExit); // string to bool
            } while (isExit);
        }

        static void YesOrNo(string message, out bool result)
        {
            bool isCorrectCheck = false;
            result = false;

            while (!isCorrectCheck)
            {
                Console.Write($"{message} (Type yes or no) - ");
                string checkStr = Console.ReadLine().ToLower(); // get answer

                if (String.Equals(checkStr, "yes") || String.Equals(checkStr, "no")) // compare answer
                {
                    result = String.Equals(checkStr, "yes"); // get result
                    isCorrectCheck = true;
                }
                else
                    Console.Write("Please type yes or no. ");
            }
        }

        static void GetPath()
        {
            bool isCorrectPath = false;

            while (!isCorrectPath)
            {
                Console.Write("Type path without file format. Path = ");
                path = Console.ReadLine(); // get path

                // check input path with regex
                if (!Regex.IsMatch(path, fileNameRegExp))
                {
                    Console.WriteLine("Path have incorrect format. Type name of file without format.");
                    continue;
                }

                path += ".txt"; // add format

                // check file exist
                if (!File.Exists(path))
                {
                    Console.WriteLine("File doesn`t exist. Check your file path and retry again.");
                    continue;
                }

                isCorrectPath = true;
            }
        }

        static void TypeOfWordsCheck()
        {
            YesOrNo("Do you want check real words?", out bool typeOfCheck); // string to bool

            // get punctuation
            var punctuation = fileContent.Where(Char.IsPunctuation).Distinct().ToArray();
            // remove tab/new line
            fileContent = Regex.Replace(fileContent, @"\t|\n|\r", String.Empty);

            if (typeOfCheck)             
                existVowelWordsCount = fileContent.ToLower().Split() // real vowel words;
                    .Select(x => x.Trim(punctuation))  // get all words and 
                    .Where(y => allVowelWords.Any(z => String.Equals(z, y))).Count(); // intersect 2 collection
            else
                existVowelWordsCount = fileContent.ToLower().Split() // random vowel letters
                    .Where(x => Regex.IsMatch(x, vowelRegExp)).Count(); // check with regex
        } 
    }
}
