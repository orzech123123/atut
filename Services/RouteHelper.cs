namespace Atut.Services
{
    public class RouteHelper
    {
        public void Update(string action, string controller)
        {
            Action = action;
            Controller = controller;
        }

        public string Action { get; private set; }
        public string Controller { get; private set; }
    }
}
