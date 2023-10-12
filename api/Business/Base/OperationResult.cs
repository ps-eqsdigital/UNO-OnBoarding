using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public Exception? Exception { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Result { get; set; }
    }
}
