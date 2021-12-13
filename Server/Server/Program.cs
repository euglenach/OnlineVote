// See https://aka.ms/new-console-template for more information

using Grpc.Core;
using MagicOnion.Hosting;
using MagicOnion.Server;
using Microsoft.Extensions.Hosting;

await MagicOnionHost.CreateDefaultBuilder()
                    .UseMagicOnion(new MagicOnionOptions(true),
                                   new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure))
                    .RunConsoleAsync();
