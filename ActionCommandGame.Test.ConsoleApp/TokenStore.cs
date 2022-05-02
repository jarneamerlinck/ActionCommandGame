﻿using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Test.ConsoleApp
{
    public class TokenStore: ITokenStore
    {
        private string Token { get; set; }
        public Task ClearTokenAsync()
        {
            Token = "";
            return Task.CompletedTask;
        }

        public Task<string> GetTokenAsync()
        {
            return Task.FromResult(Token);
        }

        public Task SaveTokenAsync(string token)
        {
            Token = token;

            return Task.CompletedTask;
        }
    }
}