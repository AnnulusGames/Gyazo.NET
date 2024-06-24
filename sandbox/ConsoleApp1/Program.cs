using Gyazo;

using var client = new GyazoClient();

var response = await client.Users.GetAsync();

Console.WriteLine(response);