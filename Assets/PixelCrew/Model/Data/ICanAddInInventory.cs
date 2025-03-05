namespace PixelCrew.Model.Data
{
    public interface ICanAddInInventory
    {
        //в интерфейсах мы можем не указывать степень доступа к нашему методу
        void AddInInventory(string id, int value);
    }
}