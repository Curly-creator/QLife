using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace QLifeC_Datatool
{
    public class ChangeCityStack : Stack<City>
    {
        public CityList cityList = new CityList();
        public CityList Undo(int count)
        {
            for (int i = 0; i <= count; i++)
            {
                switch (Peek().Changetype)
                {
                    case "Undo_Add": 
                        Undo_Add();
                        break;
                    case "Undo_Delete":
                        Undo_Delete();
                        break;
                    case "Undo_Edit":
                        Undo_Edit();
                        break;
                    default:
                        MessageBox.Show("Undo failed ERROR: unknown changetype");
                        break;
                }
            }
            return cityList;
        }

        public void Undo_Delete()
        {
            cityList.Add(Pop());
            
        }

        public void Undo_Add()
        {
            foreach (var city in cityList)
            {
                if (city.Index == Peek().Index)
                {
                    cityList.Remove(city);
                    Pop();
                    break;
                }
            }
        }

        public void Undo_Edit()
        {
            foreach (var city in cityList)
            {
                if (city.Index == Peek().Index)
                {
                    cityList[cityList.IndexOf(city)] = Pop();
                    break;
                }
            }
        }
    }
}
