// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.UniDirectional;

using Core;
using Core.UniDirectional;

/// <inheritdoc cref="IPullReactable{TDataOut}"/>
public class PullReactable<TDataOut>
    : ReactableBase<IRespondReactor<TDataOut>>, IPullReactable<TDataOut>
{
    /// <inheritdoc/>
    public IResult<TDataOut> Pull(Guid respondId)
    {
        for (var i = 0; i < Reactors.Count; i++)
        {
            if (Reactors[i].Id != respondId)
            {
                continue;
            }

            var result = Reactors[i].OnRespond();

            return result ?? ResultFactory.CreateEmptyResult<TDataOut>();
        }

        return ResultFactory.CreateEmptyResult<TDataOut>();
    }
}
