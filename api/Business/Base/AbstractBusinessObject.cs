using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Business.Base
{
    public class AbstractBusinessObject
    {
        protected TransactionOptions _options = new TransactionOptions()
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(30)
        };

        protected async virtual Task<OperationResult> ExecuteOperation(Func<Task> operation, TransactionOptions options = default)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options == default ? _options : options, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await operation.Invoke();
                    scope.Complete();
                    return new OperationResult() { IsSuccess = true };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult() { IsSuccess = false, Exception = ex };
            }
        }

        protected async virtual Task<OperationResult<T>> ExecuteOperation<T>(Func<Task<T>> operation, TransactionOptions options = default)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options == default ? _options : options, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await operation.Invoke();
                    scope.Complete();
                    return new OperationResult<T>() { IsSuccess = true, Result = result };
                }

            }
            catch (Exception ex)
            {
                return new OperationResult<T>() { IsSuccess = false, Exception = ex };
            }
        }
    }
}
