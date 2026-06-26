using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Extensions
{
    public static class UniTaskExtensions
    {
        public static async void OnComplete(this UniTask task, Action onComplete, CancellationToken token = default)
        {
            await task;
            
            if (token.IsCancellationRequested) return;

            onComplete?.Invoke();
        }
    }
}