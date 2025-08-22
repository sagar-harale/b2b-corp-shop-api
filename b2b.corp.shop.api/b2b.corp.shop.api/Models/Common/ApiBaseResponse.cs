namespace b2b.corp.shop.api.Models.Common
{
    public sealed class ApiBaseResponse<T>
    {
        public ResponseContext Context { get; set; }

        public T Response { get; set; }

        public static ApiBaseResponse<T> CreateSuccess(T response, string transactionId, string message = "Success")
        {
            return new ApiBaseResponse<T>
            {
                Context = new ResponseContext(transactionId, 200, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), message),
                Response = response
            };
        }

        public static ApiBaseResponse<T> CreateError(string transactionId, long statusCode, string message)
        {
            return new ApiBaseResponse<T>
            {
                Context = new ResponseContext(transactionId, statusCode, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), message),
                Response = default
            };
        }
    }

 
    public class ResponseContext
    {
        private long _timeStamp;

        private string _transactionId;

        private long _statusCode;

        public long TimeStamp
        {
            get
            {
                return _timeStamp;
            }
            set
            {
                _timeStamp = value;
                ValidateTimeStamp(value);
            }
        }

        public string TransactionId
        {
            get
            {
                return _transactionId;
            }
            set
            {
                _transactionId = value;
                ValidateTransactionId(value);
            }
        }

        public long StatusCode
        {
            get
            {
                return _statusCode;
            }
            set
            {
                _statusCode = value;
                ValidateStatusCode(value);
            }
        }

        public string Message { get; set; }

        public ResponseContext(string transactionId, long statusCode, long timeStamp, string massage = null)
        {
            TransactionId = transactionId;
            StatusCode = statusCode;
            TimeStamp = timeStamp;
            Message = massage;
        }

        private void ValidateTimeStamp(long TimeStamp)
        {
            if (TimeStamp <= 0)
            {
                throw new ArgumentException("TimeStamp is less than or equal to zero.");
            }
        }

        private void ValidateTransactionId(string TransactionId)
        {
            if (string.IsNullOrWhiteSpace(TransactionId))
            {
                throw new ArgumentException("TransactionId is null or empty.");
            }
        }

        private void ValidateStatusCode(long StatusCode)
        {
            if (StatusCode <= 0)
            {
                throw new ArgumentException("StatusCode is less than or equal to zero.");
            }
        }
    }
}
