using System.Collections.Generic;

namespace BusinessLogicLayer.Models.Response
{
    public class ApiResponseModel
    {
        public List<ApiModel> apiModels { get; set; }

        public ApiResponseModel()
        {
            apiModels = new List<ApiModel>();
        }
    }
}
