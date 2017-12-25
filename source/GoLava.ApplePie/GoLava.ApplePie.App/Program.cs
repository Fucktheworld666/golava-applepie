using System;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients.Portal;
using GoLava.ApplePie.Contracts;

namespace GoLava.ApplePie.App
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).Wait();

        static async Task MainAsync(string[] args)
        {
            var client = new PortalClient(new PortalUrlProvider());

            Console.WriteLine("Enter Apple Account Name:");
            var accountName = Console.ReadLine();

            Console.WriteLine("Enter Apple Password:");
            var password = Console.ReadLine();

            var context = await client.LogonAsync(accountName, password);
            Console.WriteLine("Logon: {0}", context.Authentication);

            if (context.Authentication == Authentication.Success)
            {
                var teams = await client.GetTeamsAsync(context);
                Console.WriteLine("Teams count: {0}", teams.Count);

                foreach (var team in teams)
                {
                    var devices = await client.GetDevicesAsync(context, team.TeamId);
                    Console.WriteLine("Team {0}: devices count: {1}", team.TeamId, devices.Count);

                    var addedDevices = await client.AddDeviceAsync(context, team.TeamId, "1111111111111111111111111111111111111112", "test", Contracts.Portal.DeviceClass.iPhone);
                    Console.WriteLine("Team {0}: added devices count: {1}", team.TeamId, addedDevices.Count);

                    var newDevice = addedDevices[0];

                    var successfulDisabled = await client.DisableDeviceAsync(context, team.TeamId, newDevice);
                    if (successfulDisabled) 
                    {
                        Console.WriteLine("Successfully disabled device.");
                        var enabledDevice = await client.EnableDeviceAsync(context, team.TeamId, newDevice);
                        if (enabledDevice != null)
                            Console.WriteLine("Successfully enabled device.");

                        await client.DisableDeviceAsync(context, team.TeamId, newDevice);

                        var changedDevice = await client.ChangeDeviceNameAsync(context, team.TeamId, newDevice, "foobar");
                        if (changedDevice != null)
                            Console.WriteLine("Successfully changed device name to {0}.", changedDevice.Name);
                    }
                }
            }
        }
    }
}
