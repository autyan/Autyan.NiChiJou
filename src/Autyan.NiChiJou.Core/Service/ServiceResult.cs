using System;
using System.Collections.Generic;
using Autyan.NiChiJou.Core.Extension;

namespace Autyan.NiChiJou.Core.Service
{
    public class ServiceResult<T> : ServiceResult
    {
        private ServiceResult()
        {

        }

        public T Data { get; protected set; }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T>
        {
            Succeed = true,
            Data = data
        };

        public new static ServiceResult<T> Failed() => new ServiceResult<T>();

        public new static ServiceResult<T> Failed(string message)
        {
            var result = Failed();
            result.ErrorMessages.Add(message);
            return result;
        }

        public new static ServiceResult<T> Failed(IEnumerable<string> messages)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            return result;
        }

        public new static ServiceResult<T> Failed(int errorCode)
        {
            var result = Failed();
            result.ErrorCode = errorCode;
            return result;
        }

        public new static ServiceResult<T> Failed(string message, int errorCode)
        {
            var result = Failed();
            result.ErrorMessages.Add(message);
            result.ErrorCode = errorCode;
            return result;
        }

        public new static ServiceResult<T> Failed(IEnumerable<string> messages, int errorCode)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            result.ErrorCode = errorCode;
            return result;
        }

        public static ServiceResult<T> Failed(IEnumerable<string> messages, T data)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            result.Data = data;
            return result;
        }

        public static ServiceResult<T> Failed(int errorCode, T data)
        {
            var result = Failed();
            result.ErrorCode = errorCode;
            result.Data = data;
            return result;
        }

        public static ServiceResult<T> Failed(string message, int errorCode, T data)
        {
            var result = Failed();
            result.ErrorMessages.Add(message);
            result.ErrorCode = errorCode;
            result.Data = data;
            return result;
        }

        public new static ServiceResult<T> Failed(Enum value)
        {
            var result = Failed();
            result.ErrorCode = Convert.ToInt32(value);
            var hasMessage = value.TryGetDescription(out var message);
            if (hasMessage)
            {
                result.ErrorMessages.Add(message);
            }
            return result;
        }

        public static ServiceResult<T> Failed(IEnumerable<string> messages, int errorCode, T data)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            result.ErrorCode = errorCode;
            result.Data = data;
            return result;
        }

        public new static ServiceResult<T> FailedFrom(ServiceResult source) => Failed(source.Messages, source.ErrorCode);

        public void WarpData(T data)
        {
            Data = data;
        }
    }

    public class ServiceResult
    {
        protected readonly List<string> ErrorMessages = new List<string>();

        public bool Succeed { get; protected set; }

        public IEnumerable<string> Messages => ErrorMessages;

        public int ErrorCode { get; protected set; }

        protected ServiceResult()
        {

        }

        private static readonly ServiceResult SuccessResult = new ServiceResult
        {
            Succeed = true
        };

        public static ServiceResult Success() => SuccessResult;

        public static ServiceResult Failed()
        {
            return new ServiceResult();
        }

        public static ServiceResult Failed(string message)
        {
            var result = Failed();
            result.ErrorMessages.Add(message);
            return result;
        }

        public static ServiceResult Failed(IEnumerable<string> messages)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            return result;
        }

        public static ServiceResult Failed(int errorCode)
        {
            var result = Failed();
            result.ErrorCode = errorCode;
            return result;
        }

        public static ServiceResult Failed(string message, int errorCode)
        {
            var result = Failed();
            result.ErrorMessages.Add(message);
            result.ErrorCode = errorCode;
            return result;
        }

        public static ServiceResult Failed(IEnumerable<string> messages, int errorCode)
        {
            var result = Failed();
            result.ErrorMessages.AddRange(messages);
            result.ErrorCode = errorCode;
            return result;
        }

        public static ServiceResult Failed(Enum value)
        {
            var result = Failed();
            result.ErrorCode = Convert.ToInt32(value);
            var hasMessage = value.TryGetDescription(out var message);
            if (hasMessage)
            {
                result.ErrorMessages.Add(message);
            }
            return result;
        }

        public static ServiceResult FailedFrom(ServiceResult source) => Failed(source.Messages, source.ErrorCode);

        public void AddMessage(string message)
        {
            ErrorMessages.Add(message);
        }

        public void SetErrorCode(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
