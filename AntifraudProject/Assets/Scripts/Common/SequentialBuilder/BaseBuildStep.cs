using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.SequentialBuilder
{
    public abstract class BaseBuildStep
    {
        public abstract UniTask Run(CancellationToken cancellationToken);
    }
}