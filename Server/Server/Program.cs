// See https://aka.ms/new-console-template for more information

using MagicOnion.Hosting;
using Microsoft.Extensions.Hosting;

await MagicOnionHost.CreateDefaultBuilder()
                    .UseMagicOnion()
                    .RunConsoleAsync();
