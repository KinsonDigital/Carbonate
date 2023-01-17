// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.BiDirectional;

using Core;
using Core.BiDirectional;

/// <inheritdoc cref="IPullReactable{TDataIn,TDataOut}"/>
public class PullReactable<TDataIn, TDataOut>
    : ReactableBase<IRespondReactor<TDataIn, TDataOut>>, IPullReactable<TDataIn, TDataOut>
{
    /// <inheritdoc/>
    public IResult<TDataOut> Pull(in TDataIn data, Guid respondId)
    {
        for (var i = 0; i < Reactors.Count; i++)
        {
            if (Reactors[i].Id != respondId)
            {
                continue;
            }

            var result = Reactors[i].OnRespond(data);

            return result ?? ResultFactory.CreateEmptyResult<TDataOut>();
        }

        return ResultFactory.CreateEmptyResult<TDataOut>();
    }
}
