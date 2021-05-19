using System.Collections.Generic;

namespace BusinessLogicLayer.Models.Response
{
    public class AllVariablesResponseModel
    {
        public List<AllVariablesModel> allVariablesModels { get; set; }

        public AllVariablesResponseModel()
        {
            allVariablesModels = new List<AllVariablesModel>();
        }
    }
}
