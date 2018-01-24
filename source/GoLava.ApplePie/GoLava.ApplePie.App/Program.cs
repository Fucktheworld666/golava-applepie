using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GoLava.ApplePie.App
{
    class Program
    {
        static readonly Random Random = new Random();

        static void Main(string[] args) => MainAsync(args).Wait();

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            var services = new ServiceCollection();
            services.AddApplePie();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        static async Task MainAsync(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var appliePieClient = serviceProvider.GetService<ApplePieClient>();

            var appleDeveloperClient = serviceProvider.GetService<IAppleDeveloperClient>();

            Console.WriteLine("Enter Apple Account Name:");
            var accountName = Console.ReadLine();

            Console.WriteLine("Enter Apple Password:");
            var password = ReadPassword();

            var context = await appleDeveloperClient.LogonWithCredentialsAsync(accountName, password);
            Console.WriteLine("Logon: {0}", context.Authentication);

            while (context.Authentication == Authentication.TwoStepSelectTrustedDevice)
            {
                Console.WriteLine("Two-Step Authentication, select trusted device:");
                var trustedDevices = context.LogonAuth.TrustedDevices;
                for (var i = 0; i < trustedDevices.Count; i++) {
                    var trustedDevice = trustedDevices[i];
                    Console.WriteLine("{0}) {1} ({2})", i+1, trustedDevice.Name, trustedDevice.Type);
                }

                if (int.TryParse(Console.ReadLine(), out int index) && index-1 >= 0 && index-1 < trustedDevices.Count)
                {
                    context = await appleDeveloperClient.AcquireTwoStepCodeAsync(context, trustedDevices[index-1]);
                    while (context.Authentication == Authentication.TwoStepCode)
                    {
                        var verifiableDevice = context.LogonAuth.VerifiableDevice;
                        var securityCode = context.LogonAuth.SecurityCode;
                        Console.WriteLine("Enter auth {0} digest code for {1}:", securityCode.Length, verifiableDevice.Name);

                        var code = Console.ReadLine();
                        context = await appleDeveloperClient.LogonWithTwoStepCodeAsync(context, code);
                    }
                }
                else 
                {
                    Console.WriteLine("Failed to select device.");
                    break;
                }
            }

            if (context.Authentication == Authentication.Success)
            {
                var teams = await appleDeveloperClient.GetTeamsAsync(context);
                Console.WriteLine("Teams count: {0}", teams.Count);

                foreach (var team in teams)
                {
                    var certificates = await appleDeveloperClient.GetCertificateRequestsAsync(context, team.TeamId);
                    Console.WriteLine("Team {0}: certificates count: {1}", team.TeamId, certificates.Count);

                    foreach (var certificate in certificates)
                    {
                        var cert = await appleDeveloperClient.DownloadCertificateAsync(context, certificate);
                        Console.WriteLine("\tCertificate: {0}", cert.Subject);
                    }



                    var applications = await appleDeveloperClient.GetApplicationsAsync(context, team.TeamId);
                    Console.WriteLine("Team {0}: applications count: {1}", team.TeamId, applications.Count);

                    foreach (var application in applications) 
                    {
                        var c = await appliePieClient.Certificates.CreateCertificateAsync(
                            context, application, CertificateTypeDisplayId.DevelopmentPush, password);


                        var applicationDetails = await appleDeveloperClient.GetApplicationDetails(context, application);
                        Console.WriteLine("\tApplication: {0}", applicationDetails.Name);

                        if (!applicationDetails.IsWildCard && applicationDetails.CanEdit.HasValue && applicationDetails.CanEdit.Value)
                        {
                            // change value of HomeKit
                            var changedApplicationDetails = await appleDeveloperClient.UpdateApplicationFeatureAsync(
                                context, applicationDetails, f => f.HomeKit, !applicationDetails.Features.HomeKit);

                            // change it back
                            changedApplicationDetails = await appleDeveloperClient.UpdateApplicationFeatureAsync(
                                context, changedApplicationDetails, f => f.HomeKit, !changedApplicationDetails.Features.HomeKit);
                        }
                    }

                    var newApplication = await appleDeveloperClient.AddApplicationAsync(context, team.TeamId, new NewApplication
                    {
                        Passbook = true,
                        HealthKit = true,
                        Type = ApplicationType.Explicit,
                        Identifier = "ich.du.er.so",
                        Name = "golava test"
                    });

                    await appleDeveloperClient.DeleteApplicationAsync(context, newApplication);

                    var devices = await appleDeveloperClient.GetDevicesAsync(context, team.TeamId);
                    Console.WriteLine("Team {0}: devices count: {1}", team.TeamId, devices.Count);

                    /*
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
                    }*/
                }
            }
        }

        static SecureString ReadPassword()
        {
            var lengths = new Stack<int>();

            var secureString = new SecureString();
            while (true)
            {
                var i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (secureString.Length > 0)
                    {
                        secureString.RemoveAt(secureString.Length - 1);

                        // remove the number of stars from the current position
                        var r = lengths.Pop();
                        while (r-- > 0)
                            Console.Write("\b \b");
                    }
                }
                else
                {
                    secureString.AppendChar(i.KeyChar);

                    // randomize the number of * to show
                    var r = Random.Next(1, 4); // return numbers between 1 and 3, but not 4 
                    lengths.Push(r);
                    while (r-- > 0)
                        Console.Write("*");
                }
            }
            return secureString;
        }
    }
}
