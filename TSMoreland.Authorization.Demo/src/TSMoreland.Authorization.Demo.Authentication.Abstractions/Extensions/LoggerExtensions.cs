﻿//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace TSMoreland.Authorization.Demo.Authentication.Abstractions.Extensions;

public static class LoggerExtensions
{
    /// <summary>
    /// Logs the reason for login failure 
    /// </summary>
    public static void LogFailureReason(this ILogger logger, SignInResult result)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(result, nameof(result));

        if (!result.Succeeded)
        {
            return;
        }

        switch (result)
        {
            case { IsLockedOut: true }:
                logger.LogError("Login failed because user is locked out");
                break;
            case { IsNotAllowed: true }:
                logger.LogError("Login failed because user is not allowed to login");
                break;
            case { RequiresTwoFactor: true }:
                logger.LogError("Login failed multi-factor authentication required");
                break;
        }

    }
}
