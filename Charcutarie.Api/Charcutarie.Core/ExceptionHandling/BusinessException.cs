using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Core.ExceptionHandling
{
    public class BusinessException : Exception
    {
        public readonly string[] Errors;

        public BusinessException(params string[] errors) : base(string.Join(Environment.NewLine, (errors ?? new string[0])))
        {
            this.Errors = errors;
        }
        public BusinessException(Exception innerException, params string[] errors) : base(string.Join(Environment.NewLine, (errors ?? new string[0])), innerException)
        {
            this.Errors = errors;
        }
    }
}
