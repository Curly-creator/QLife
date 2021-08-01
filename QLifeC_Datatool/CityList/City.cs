namespace QLifeC_Datatool
{
    public class City
    {
        private string _Name; 
        private string _Url;
    
        Category[] _Categories;
        private int id;
        private string changetype;

        public City()
        {
            Categories = new Category[] {
            new Category("Cost of Living"),
            new Category("Healthcare"),
            new Category("Internet Access"),
            new Category("Environmental Quality"),
            new Category("Travel Connectivity"),
            new Category("Outdoors"),
            };           
        }


        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        public Category[] Categories { get => _Categories; set => _Categories = value; }
        public int Id { get => id; set => id = value; }
        public string Changetype { get => changetype; set => changetype = value; }
    }
}

