using QLifeC_Datatool;
using System;
using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace UnitTest
{
    public class AddingDeletingFeatureTesting
    {
        public bool CheckIfNameIsInTitleCase(string cityName)
        {
            bool firstLetterIsUp;
            bool otherLettersAreLow=false;

            if (char.IsUpper(cityName[0])) //methode for cities with easy names - not yet for 'Bad Mergentheim' ->string an leerstelle aufteilen in array
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

        public bool CheckIfNameIsEmpty(string cityName)
        {
            bool tmp_isEmpty = false;
            if (cityName == "")
            {
                tmp_isEmpty = true;
            }
            return tmp_isEmpty;
        }
        public bool CheckIfContainsNoSymbols(string cityName)
        {
            char[] symbols = new char[] { '?', '!', '.', '°', '^', '"', '§', '$', '%', '&', '/', '(', ')', '=', '`', '´', '+', '*', '~', '}', ']', '[', '{', '#', '-', '_', ':', ',', ';', '<', '>', '}' };// ohne backslash ohne '\
            bool containsNoSymbol = true;
            for (int i = 0; i < cityName.Length; i++)
            {
                for (int c = 0; c < symbols.Length; c++)
                {
                    if (symbols[c] == cityName[i]) containsNoSymbol = false;
                }
            }
            return containsNoSymbol;
        }
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
        /*[Fact]
        public void TestDeleting()
        {
            //Arrange
            double expected = 13.0;

            //Act
            double actual = 
            //CheckIfNameIsEmpty()

            //Assert
            Assert.Equal(expected, actual);
        }*/
    }
}
