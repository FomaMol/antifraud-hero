using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Common.SequentialBuilder
{
    public class BuilderStepSequence
    {
        private readonly List<BaseBuildStep> _steps = new();
        private readonly IObjectResolver _container;

        public BuilderStepSequence(IObjectResolver container)
        {
            _container = container;
        }
        
        public void AddStep<T>() where T : BaseBuildStep
        {
            _steps.Add(_container.Resolve<T>());
        }

        public async UniTask Launch(CancellationToken cancellationToken)
        {
            foreach (var step in _steps)
            {
                await step.Run(cancellationToken);
            }

            Debug.Log("[BuilderStepSequence] Build complete.");
        }
    }
}