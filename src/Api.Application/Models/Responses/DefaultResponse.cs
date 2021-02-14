using System.Collections.Generic;

namespace application.Models.Responses
{

    public class SingleResponseModel<T> where T : class
    {
        public virtual string Message { get; set; }
        public virtual T Data { get; set; }
    }


    public class CollectionResponseModel <T> where T : class
    {
        public virtual int Total { get; set; }
        public virtual int TotalPage { get; set; }
        public virtual int CurrentPage { get; set; }
        public virtual int PerPage { get; set; }
        public virtual string Message { get; set; }
        public virtual IEnumerable<T> Data { get; set; }
    }

    public class ErrorResponseModel<T> where T : class
    {
        public virtual int Code { get; set; }
        public virtual string Message { get; set; }
    }
}
