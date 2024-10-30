using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.General
{
    public class ErrorResponseModel
    {
        public required int StatusCode { get; set; }
        public required string Error { get; set; }
        public required string Details { get; set; }
    }
}
