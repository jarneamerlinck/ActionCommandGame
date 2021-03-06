using System.Diagnostics.SymbolStore;
using ActionCommandGame.Api.Authentication.Model;

using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Test.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;


//config and registration
var serviceCollection = new ServiceCollection();

serviceCollection.AddApi("https://localhost:7237");

serviceCollection.AddScoped<ITokenStore, TokenStore>();


//running and start
Console.WriteLine("Press key to start");
Console.ReadLine();

var serviceProvider = serviceCollection.BuildServiceProvider();

var identityApi = serviceProvider.GetRequiredService<IIdentityApi>();
var tokenStore = serviceProvider.GetRequiredService<ITokenStore>();
var playerApi = serviceProvider.GetRequiredService<IPlayerApi>();
var itemApi = serviceProvider.GetRequiredService<IItemApi>();
var gameApi = serviceProvider.GetRequiredService<IGameApi>();
var existingPlayerToken = "";


var loginRequest = new UserSignInRequest { Email = "bavo.ketels@vives.be", Password = "Test123$"};


var loginResult = await identityApi.SignInAsync(loginRequest);

if (loginResult.Success && loginResult.Token is not null)
{
    existingPlayerToken = loginResult.Token;
    await tokenStore.SaveTokenAsync(loginResult.Token);
    Console.WriteLine($"Logged in token is: {await tokenStore.GetTokenAsync()}");
}
else if (loginResult.Errors is not null)
{
    
    foreach (var error in loginResult.Errors)
    {
        Console.WriteLine($"[{error}]");
    }
}

Console.WriteLine("Register");
Console.ReadLine();


//Register
var email = "j@knightofzero.com";
var password = "New@Password2";

var registerRequest = new UserRegistrationRequest { Email = email, Password = password};
var registerResult = await identityApi.RegisterAsync(registerRequest);

if (registerResult.Success && registerResult.Token is not null)
{
    await tokenStore.SaveTokenAsync(registerResult.Token);
    Console.WriteLine($"Registered as {email}");
}
else if (registerResult.Errors is not null)
{

    foreach (var error in registerResult.Errors)
    {
        Console.WriteLine($"[{error}]");
    }
}

Console.ReadLine();

//login with newly made user
var loginRequest2 = new UserSignInRequest { Email = email, Password = password };


var loginResult2 = await identityApi.SignInAsync(loginRequest2);

if (loginResult2.Success && loginResult2.Token is not null)
{
    await tokenStore.SaveTokenAsync(loginResult2.Token);
    Console.WriteLine($"Logged in token is: {await tokenStore.GetTokenAsync()}");
}
else if (loginResult2.Errors is not null)
{

    foreach (var error in loginResult2.Errors)
    {
        Console.WriteLine($"[{error}]");
    }
}

Console.ReadLine();
Console.Clear();
Console.WriteLine("Show players of existing user");

await tokenStore.SaveTokenAsync(existingPlayerToken);
var playerResult = await playerApi.Find(new PlayerFilter
{
    FilterUserPlayers = false
});

if (playerResult.IsSuccess && playerResult.Data is not null)
{
    foreach (var player in playerResult.Data)
    {
        Console.WriteLine($"{player.Name} has {player.Money} Euro");
    }
    
}
else 
{

    foreach (var error in playerResult.Messages)
    {
        Console.WriteLine($"[{error}]");
    }
}

Console.ReadLine();

Console.Clear();
Console.WriteLine("Login out");

await tokenStore.SaveTokenAsync("");

try
{
    playerResult = await playerApi.Find(new PlayerFilter
    {
        FilterUserPlayers = false
    });
}


catch (Exception e)
{
    Console.WriteLine(e);

}



Console.ReadLine();

Console.Clear();
Console.WriteLine("Login and select player and buy something");
await tokenStore.SaveTokenAsync(existingPlayerToken);
var playersResult = await playerApi.Find(new PlayerFilter
{
    FilterUserPlayers = false
});
if (!playerResult.IsSuccess || playerResult.Data is null)
{
    Console.WriteLine("Error");
}
else
{
    var player = playerResult.Data[2];
    Console.WriteLine($"{player.Id}:{player.Name} has {player.Money}");
    var itemListResult = await itemApi.FindAsync();
    if (!itemListResult.IsSuccess || itemListResult.Data is null)
    {
        Console.Write("Error");
    }
    else
    {
        var itemList = itemListResult.Data;
        foreach (var item in itemList)
        {
            Console.WriteLine($"{item.Id}:{item.Name} costs {item.Price}");

        }
        Console.WriteLine($"{player.Id}:{player.Name} has {player.Money}");
        await gameApi.BuyAsync(player.Id, itemList[1].Id);
        Console.WriteLine("player has bought item 1");

        playersResult = await playerApi.Find(new PlayerFilter
        {
            FilterUserPlayers = false
        });
        player = playerResult.Data[2];
        Console.WriteLine($"{player.Id}:{player.Name} has {player.Money}");
        Console.WriteLine(player.ImageLocation);

    }
    

}

Console.ReadLine();
