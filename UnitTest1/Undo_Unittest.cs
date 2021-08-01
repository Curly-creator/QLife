using QLifeC_Datatool;
using Xunit;

namespace UnitTest
{
    public class Undo_Unittest
    {
        readonly CityList cityList = new CityList();
        public ChangeCityStack changeCityStack = new ChangeCityStack();
        readonly string[] cityNames = new string[]
        {
            "Berlin", "Stuttgart", "München"
        };
        readonly string[] changetype = new string[]
{
            "Undo_Add", "Undo_Delete", "Undo_Edit", "update"
};

        internal void FillCityList()
        {
            for (int i = 0; i < cityNames.Length; i++)
            {
                City city = new City { Name = cityNames[i], Index=i };
                cityList.Add(city);
            }
            changeCityStack.cityList.AddRange(cityList);
        }
        internal void AddToStack(string changetype, int id)
        {
            City test = new City { Changetype = changetype, Index = id };
            changeCityStack.Push(test);
        }
        internal bool CheckUndoAdd(CityList cities)
        {
            foreach (var city in cities)
            {
                if (city.Index == 0) return false;
            }
            return true;
        }
        internal bool CheckUndoDelete(CityList cities)
        {
            foreach (var city in cities)
            {
                if (city.Index == 3) return true;
            }
            return false;
        }

        [Fact]
        public void Undo_Add()
        {
            //Arrange
            FillCityList();
            AddToStack(changetype[0], 0);
            //Assert:
            Assert.True(CheckUndoAdd(changeCityStack.Undo(0)));
        }
        [Fact]
        public void Undo_Delete()
        {
            //Arrange
            FillCityList();
            AddToStack(changetype[1], 3);
            //Assert:
            Assert.True(CheckUndoDelete(changeCityStack.Undo(0)));
        }
    }
}
