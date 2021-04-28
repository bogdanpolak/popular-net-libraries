namespace PopularNetLibraries.Autofac.Sample
{
    public class DataRepository : IDataRepository
    {
        public MyData GetData(int id)
        {
            return id switch
            {
                1 => new MyData { Id = 1, Name = "Code Paths" },
                2 => new MyData { Id = 2, Name = "Customize Extentions" },
                _ => null,
            };
        }
    }
}
