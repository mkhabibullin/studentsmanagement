using Shared.Contract.Utils;
using System;

namespace Shared.Contract.Consts
{
    public class QueueNames
    {
        public const string SagaOrderPayment = "withdraw-customer-credit";
        private const string rabbitUri = "queue:";
        public static Uri GetMessageUri(string key)
        {
            return new Uri(rabbitUri + key.PascalToKebabCaseMessage());
            //return new Uri(key.PascalToKebabCaseMessage());
        }
        public static string GetMessageAddress(string key)
        {
            return key.PascalToKebabCaseMessage();
        }
        public static Uri GetActivityUri(string key)
        {
            var kebabCase = key.PascalToKebabCaseActivity();
            if (kebabCase.EndsWith("-"))
            {
                kebabCase = kebabCase.Remove(kebabCase.Length - 1);
            }
            return new Uri(rabbitUri + kebabCase + '_' + "execute");
        }
    }
}
