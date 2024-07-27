using BackEndProject.ViewModels;

namespace BackEndProject.Interfaces
{
    public interface ILayoutService
    {
        IDictionary<string, string> GetSettings();
        IEnumerable<BasketVM> GetBasket();
    }
}