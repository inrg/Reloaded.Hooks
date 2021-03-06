﻿using System;

namespace Reloaded.Hooks.Definitions
{
    public interface IHook<TFunction>
    {
        /// <summary>
        /// Returns true if the hook is enabled and currently functional, else false.
        /// </summary>
        bool IsHookEnabled { get; }

        /// <summary>
        /// Returns true if the hook has been activated.
        /// The hook may only be activated once.
        /// </summary>
        bool IsHookActivated { get; }

        /// <summary>
        /// Allows you to call the original function that was hooked.
        /// </summary>
        TFunction OriginalFunction { get; }

        /// <summary>
        /// The address to call if you wish to call the <see cref="OriginalFunction"/>.
        /// </summary>
        IntPtr OriginalFunctionAddress { get; }

        /// <summary>
        /// The address of the wrapper used to call the <see cref="OriginalFunction"/>.
        /// If the <see cref="OriginalFunction"/> is CDECL, this is equal to <see cref="OriginalFunctionAddress"/>.
        /// </summary>
        IntPtr OriginalFunctionWrapperAddress { get; }

        /// <summary>
        /// The reverse function wrapper that allows us to call the C# function
        /// as if it were to be of another calling convention.
        /// </summary>
        IReverseWrapper<TFunction> ReverseWrapper { get; }

        /// <summary>
        /// Performs a one time activation of the hook, making the necessary memory writes to permanently commit the hook.
        /// </summary>
        /// <remarks>
        ///     This function should be called after instantiation as soon as possible,
        ///     preferably in the same line as instantiation.
        ///
        ///     This class exists such that we don't run into concurrency issues on
        ///     attaching to other processes whereby the following happens:
        ///
        ///     A. Original process calls a function that was just hooked.
        ///     B. Create function has not yet returned, and OriginalFunction is unassigned.
        ///     C. Hook tried to call OriginalFunction. NullReferenceException.
        /// </remarks>
        IHook<TFunction> Activate();

        /// <summary>
        /// Temporarily disables the hook, causing all functions re-routed to your own function to be re-routed back to the original function instead.
        /// </summary>
        /// <remarks>This is implemented in such a fashion that the hook shall never touch C# code.</remarks>
        void Disable();

        /// <summary>
        /// Re-enables the hook if it has been disabled, causing all functions to be once again re-routed to your own function.
        /// </summary>
        void Enable();

    }
}