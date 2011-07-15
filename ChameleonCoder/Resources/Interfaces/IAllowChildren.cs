﻿namespace ChameleonCoder.Resources.Interfaces
{
    public interface IAllowChildren : IResource
    {
        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        Collections.ResourceCollection children { get; }
    }
}