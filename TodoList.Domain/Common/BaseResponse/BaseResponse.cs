

namespace TodoList.Domain.Common
{
    public class BaseResponse<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsSuccess { get; set; }
        public int ReferenceNumber { get; set; }
        public int TotalRecords { get; set; }

        // Success response method
        public static BaseResponse<T> SuccessResponse(T data, string message, int totalRecords = 0)
        {
            return new BaseResponse<T>
            {
                Data = data,
                StatusCode = 200, // OK
                ResponseMessage = message,
                IsSuccess = true,
                ReferenceNumber = new Random().Next(1, 1000),
                TotalRecords = totalRecords
            };
        }

     
        public static BaseResponse<T> FailureResponse(string errorMessage, int statusCode = 400)
        {
            return new BaseResponse<T>
            {
                Data = null,
                StatusCode = statusCode,
                ResponseMessage = errorMessage,
                IsSuccess = false,
                ReferenceNumber = new Random().Next(1, 1000),
                TotalRecords = 0
            };
        }
    }

}
