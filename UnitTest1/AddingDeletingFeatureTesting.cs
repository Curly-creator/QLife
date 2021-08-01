using QLifeC_Datatool;
using System;
using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace UnitTest
{
    public class AddingDeletingFeatureTesting
    {
        //Initializing all three Checking-Methods. Which are to be tested.
        public bool CheckIfNameIsInTitleCase(string cityName)
        {
            if (cityName.Contains(". ")) //Bsp: Mt. Cook
            {
                string[] splittedName = cityName.Split(". ");
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                }
                return true;
            }
            else if (cityName.Contains(' ')) //Bsp: Bad Neustadt 
            {
                string[] splittedName = cityName.Split(' ');
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                    //else return true;
                }
                return true;
            }
            else if (cityName.Contains('-')) //Bsp: Bad-Neustadt
            {
                string[] splittedName = cityName.Split('-');
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                }
                return true;
            }
            else if (cityName.Contains("(")) //Bsp: Halle (Saale)
            {
                cityName = cityName.Replace("(", "");
                cityName = cityName.Replace(")", "");
                if (!CheckIfNameIsInTitleCase(cityName)) return false;
                else return true;
            }
            else
            {
                bool firstLetterIsUp;
                bool otherLettersAreLow = false;
                if (cityName.Length == 1) otherLettersAreLow = true;

                if (char.IsUpper(cityName[0]))
                    firstLetterIsUp = true;
                else firstLetterIsUp = false;

                for (int i = 1; i < cityName.Length; i++)
                {
                    if (char.IsLower(cityName[i])) otherLettersAreLow = true;
                    else
                    {
                        otherLettersAreLow = false;
                        break;
                    }
                }
                if (firstLetterIsUp && otherLettersAreLow) return true;
                else return false;
            }
        }

        public bool CheckIfNameIsEmpty(string cityName)
        {
            bool tmp_isEmpty = false;
            if (cityName == "")
            {
                tmp_isEmpty = true;
            }
            else tmp_isEmpty = false;
            return tmp_isEmpty;
        }

        public bool CheckIfContainsNoSymbols(string cityName)
        {
            bool containsNoSymbol = true;
            //accepting: '-' '()' ',' '.' 
            char[] symbols = new char[] { '?', '!', '°', '^', '"', '§', '$', '%', '&', '/', '=', '`', '´', '+', '*', '~', '}', ']', '[', '{', '#', '_', ':', ';', '<', '>', '}' };

            for (int i = 0; i < cityName.Length; i++)
            {
                for (int c = 0; c < symbols.Length; c++)
                {
                    if (symbols[c] == cityName[i])
                    {
                        containsNoSymbol = false;
                    }
                }
            }
            return containsNoSymbol;
        }

        //4 unit tests:
        [Fact]
        public void TestTitleCaseMethod()
        {
            //Arrange
            bool expected = false;

            //Act
            bool actual = CheckIfNameIsInTitleCase("berlin");

            //Assert:
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestTitleCaseMethod2()
        {
            //Arrange
            bool expected = false;

            //Act
            bool actual = CheckIfNameIsInTitleCase("BERLIN");

            //Assert:
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestNameIsEmptyMethod()
        {
            //Arrange
            bool expected = true;

            //Act
            bool actual = CheckIfNameIsEmpty("");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestContainsNoSymbolsMethod()
        {
            //Arrange
            bool expected = false;

            //Act
            bool actual = CheckIfContainsNoSymbols("Berl?n");

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
