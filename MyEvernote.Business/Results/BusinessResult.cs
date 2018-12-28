using MyEvernote.Entities.Messages;
using System.Collections.Generic;

namespace MyEvernote.Business.Results
{
    public class BusinessResult<T> where T : class
    {
        public List<ErrorMessage> Errors { get; set; }
        public T Result { get; set; }

        public BusinessResult()
        {
            Errors = new List<ErrorMessage>();
        }

        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessage() { Code = code, Message = message });
        }
    }
}
