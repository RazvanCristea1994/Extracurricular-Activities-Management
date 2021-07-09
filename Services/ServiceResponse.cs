using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class ServiceResponse<TResponseOk, TResponseError>
    {
        public TResponseError ResponseError { get; set; }
        public TResponseOk ResponseOk { get; set; }
    }
}
