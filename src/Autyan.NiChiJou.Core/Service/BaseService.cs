using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Core.Service
{
    public abstract class BaseService
    {
        protected ILogger Logger { get; set; }

        protected BaseService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        protected static ServiceResult Success() => ServiceResult.Success();

        protected static ServiceResult Failed(string message) => ServiceResult.Failed(message);

        protected static ServiceResult Failed(IEnumerable<string> messages) => ServiceResult.Failed(messages);

        protected static ServiceResult Failed(int errorCode) => ServiceResult.Failed(errorCode);

        protected static ServiceResult Failed(string message, int errorCode) => ServiceResult.Failed(message, errorCode);

        protected static ServiceResult Failed(IEnumerable<string> messages, int errorCode) => ServiceResult.Failed(messages, errorCode);

        protected static ServiceResult Failed(Enum value) => ServiceResult.Failed(value);

        protected static ServiceResult FailedFrom(ServiceResult source) => ServiceResult.FailedFrom(source);

        protected static ServiceResult<T> Success<T>(T data) => ServiceResult<T>.Success(data);

        protected static ServiceResult<T> Failed<T>(string message) => ServiceResult<T>.Failed(message);

        protected static ServiceResult<T> Failed<T>(IEnumerable<string> messages) => ServiceResult<T>.Failed(messages);

        protected static ServiceResult<T> Failed<T>(int errorCode) => ServiceResult<T>.Failed(errorCode);

        protected static ServiceResult<T> Failed<T>(string message, int errorCode) => ServiceResult<T>.Failed(message, errorCode);

        protected static ServiceResult<T> Failed<T>(IEnumerable<string> messages, int errorCode) => ServiceResult<T>.Failed(messages, errorCode);

        protected static ServiceResult<T> Failed<T>(Enum value) => ServiceResult<T>.Failed(value);

        protected static ServiceResult<T> Failed<T>(IEnumerable<string> messages, T data) => ServiceResult<T>.Failed(messages, data);

        protected static ServiceResult<T> Failed<T>(int errorCode, T data) => ServiceResult<T>.Failed(errorCode, data);

        protected static ServiceResult<T> Failed<T>(string message, int errorCode, T data) =>
            ServiceResult<T>.Failed(message, errorCode, data);

        protected static ServiceResult<T> Failed<T>(IEnumerable<string> messages, int errorCode, T data) =>
            ServiceResult<T>.Failed(messages, errorCode, data);

        protected static ServiceResult<T> FailedFrom<T>(ServiceResult source) => ServiceResult<T>.FailedFrom(source);
    }
}
