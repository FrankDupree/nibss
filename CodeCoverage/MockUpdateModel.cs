using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeCoverage
{
    class MockUpdateModel : IUpdateModel
    {
        ModelStateDictionary IUpdateModel.ModelState => throw new NotImplementedException();

         Task<bool> IUpdateModel.TryUpdateModelAsync<TModel>(TModel model)
        {
            return null;
        }

        async Task<bool> IUpdateModel.TryUpdateModelAsync<TModel>(TModel model, string prefix)
        {
            return await Task.FromResult(true);
        }

         Task<bool> IUpdateModel.TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions)
        {
            return null;
        }


        bool IUpdateModel.TryValidateModel(object model)
        {
            return true;
        }

        bool IUpdateModel.TryValidateModel(object model, string prefix)
        {
            return true;
        }
    }
}
