using System.Collections.Generic;
using System.Linq;

namespace Atut.Services
{
    public class RequestModelService
    {
        private readonly IList<object> _models = new List<object>();

        public void AddModel(object model)
        {
            var ofTypeModel = _models.SingleOrDefault(m => m.GetType() == model.GetType());
            if (ofTypeModel != null)
            {
                _models.Remove(ofTypeModel);
            }

            _models.Add(model);
        }

        public T GetModel<T>()
        {
            return _models.OfType<T>().SingleOrDefault();
        }
    }
}